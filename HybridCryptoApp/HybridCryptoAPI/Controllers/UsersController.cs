using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HybridCrypto.Data;
using HybridCrypto.Domain;

namespace HybridCryptoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HCContext _context;

        public UsersController(HCContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IList<User>>> GetUsers()
        {
            var userId = getUserIdFromToken();
            if (userId != "")
            {
                return await _context.Users.Where(u => u.Id != userId).ToListAsync();
            }
            return Unauthorized();
        }
        private string getUserIdFromToken()
        {
            string userId = "";
            try
            {
                userId = HttpContext.User.Claims.First(x => x.Type == "UserId").Value;
            }
            catch (Exception)
            {
                return userId;
            }
            return userId;
        }
    }
}