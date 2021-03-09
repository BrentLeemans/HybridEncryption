using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCore.Totp;
using HybridCrypto.Business;
using HybridCrypto.Data;
using HybridCrypto.Domain;
using HybridCryptoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HybridCryptoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOptions<TokenSettings> _tokenSettings;
        private readonly HCContext _context;
        private readonly ILogger _logger;

        public AuthenticationController(SignInManager<User> signInManager, IOptions<TokenSettings> tokenSettings, HCContext context, ILogger<AuthenticationController> logger)
        {
            _userManager = signInManager.UserManager;
            _signInManager = signInManager;
            _tokenSettings = tokenSettings;
            _context = context;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"{DateTime.Now} - Modelstate invalid! - ${model}");
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Nickname = model.Nickname,
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Create the public and private RSA key of the user
                RSAWithCSPKey.AssignNewKey("RSA_userID_" + user.Id);

                //Create the public and private signature key of the user
                DigitalSignature.AssignNewKey("Signature_userID_" + user.Id);

                string key = _userManager.GenerateNewAuthenticatorKey();

                // To generate the qrcode/setup key
                var totpSetupGenerator = new TotpSetupGenerator();

                var totpSetup = totpSetupGenerator.Generate("HybridCrypto2FA", user.Email, key, 300, 300);

                user.Key = key;
                var qrCode = new { qrCodeImageUrl = totpSetup.QrCodeImage };

                _context.SaveChanges();
                return Ok(qrCode);
            }

            foreach (var error in result.Errors)
            {
                if (error.Code != "DuplicateUserName")
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            _logger.LogInformation($"{DateTime.Now} - Modelstate invalid! - ${ModelState}");
            return BadRequest(ModelState);
        }


        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogInformation($"{DateTime.Now} - {model} - 403 - Invalid username or password.");
                return StatusCode(403, "Invalid username or password.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

            if (!result.Succeeded)
            {
                string errorMessage = !result.IsLockedOut ? "Invalid username or password." : "Too many incorrect attempts. Please wait 5 minutes and try again.";
                _logger.LogInformation($"{DateTime.Now} - {model} - 403 - {errorMessage}");
                return StatusCode(403, errorMessage);
            }
            var guid = new { Guid = Guid.NewGuid()};
            user.Guid = guid.Guid;
            _context.SaveChanges();
            
            return Ok(guid);
        }

        private async Task<object> CreateJwtToken(User user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim("UserId", user.Id));


            var keyBytes = Encoding.UTF8.GetBytes(_tokenSettings.Value.Key);
            var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                _tokenSettings.Value.Issuer,
                _tokenSettings.Value.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenSettings.Value.ExpirationTimeInMinutes),
                signingCredentials: signingCredentials);

            var encryptedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encryptedToken;
        }

        [HttpPost("2FA")]
        public async Task<IActionResult> EnableAuthenticator(TwoFactorAuthenticationModel twoFactorAuthenticationModel)
        {
            var user = await _userManager.FindByEmailAsync(twoFactorAuthenticationModel.Email);
            if (user.Guid != twoFactorAuthenticationModel.Guid) return Unauthorized();
            // To validate the pin after user input (where pin is an int variable)

            var totpValidator = new TotpValidator(new TotpGenerator());
            bool isCorrectPIN = totpValidator.Validate(user.Key, twoFactorAuthenticationModel.Code);
            if (isCorrectPIN)
            {
                var token = await CreateJwtToken(user);
                return Ok(JsonSerializer.Serialize(token));
            }
            return StatusCode(403, "Wrong code was entered. Please try again.");

        }
    }
}