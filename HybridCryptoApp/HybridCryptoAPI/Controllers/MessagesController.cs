using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HybridCrypto.Data;
using HybridCrypto.Domain;
using HybridCrypto.Business;
using System.Text;
using HybridCryptoAPI.Models;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace HybridCryptoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessagesController : ControllerBase
    {
        private readonly HCContext _context;
        private readonly ILogger _logger;

        public MessagesController(HCContext context, ILogger<MessagesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Messages/8
        [HttpGet("{senderId}")]
        public async Task<ActionResult<IEnumerable<MessageModel>>> GetMessagesFromConversation(string senderId)
        {
            string receiverId;
            try
            {
                receiverId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            }
            catch
            {
                return Unauthorized();
            }
            if (receiverId == null) return Unauthorized();
            var messages = await _context.Messages.Where(m => m.ReceiverId == receiverId).Where(m => m.SenderId == senderId).Include(m => m.EncryptedPackets).ToListAsync();
            string privateKeyReceiver = "RSA_userID_" + receiverId;
            string signatureContainer = "Signature_userID_" + senderId;

            List<MessageModel> decryptedMessages = new List<MessageModel>();

            byte[] text;
            byte[] file;

            foreach (var message in messages)
            {
                file = new byte[] { };
                text = new byte[] { };
                foreach (EncryptedPacket packet in message.EncryptedPackets)
                {
                    DecryptData(packet, ref file, ref text, privateKeyReceiver, signatureContainer);
                }

                MessageModel messageModel = new MessageModel
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Text = Encoding.UTF8.GetString(text),
                    File = file,
                    Date = message.Date
                };
                decryptedMessages.Add(messageModel);
            }

            return decryptedMessages;
        }

        // POST: api/Messages
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<Message> PostMessage(MessageModel model)
        {
            string userId;
            try
            {
                userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            }
            catch
            {
                return Unauthorized();
            }

            if (userId == null) return Unauthorized();
            model.SenderId = userId;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Message message = new Message
            {
                Date = DateTime.UtcNow,
                SenderId = model.SenderId,
                ReceiverId = model.ReceiverId,
            };
            _context.Messages.Add(message);
            _context.SaveChanges();

            byte[] messageInBytes = Encoding.UTF8.GetBytes(model.Text);
            byte[] file = model.File == null ? new byte[] { } : model.File;

            string publicKeyReceiver = "RSA_userID_" + model.ReceiverId;
            string signatureContainer = "Signature_userID_" + model.SenderId;

            EncryptedPacket encryptedMessage = null;
            EncryptedPacket encryptedFile = null;

            if (messageInBytes.Any())
            {
                encryptedMessage = HybridEncryption.EncryptData(messageInBytes, publicKeyReceiver, signatureContainer);
                encryptedMessage.isFile = false;
                encryptedMessage.MessageId = message.Id;
            }

            if (file.Any())
            {
                encryptedFile = HybridEncryption.EncryptData(file, publicKeyReceiver, signatureContainer);
                encryptedFile.isFile = true;
                encryptedFile.MessageId = message.Id;
            }

            IList<EncryptedPacket> encryptedPackets = new List<EncryptedPacket>();
            if (encryptedMessage != null)
            {
                encryptedPackets.Add(encryptedMessage);
                _context.EncryptedPackets.Add(encryptedMessage);
            }
            if (encryptedFile != null)
            {
                encryptedPackets.Add(encryptedFile);
                _context.EncryptedPackets.Add(encryptedFile);
            }

            message.EncryptedPackets = encryptedPackets;

            _context.SaveChanges();

            return NoContent();
        }

        private void DecryptData(EncryptedPacket packet, ref byte[] file, ref byte[] text, string privateKeyReceiver, string signatureContainer)
        {
            if (packet.EncryptedData.Any())
            {
                try
                {
                    if (packet.isFile)
                    {
                        file = HybridEncryption.DecryptData(packet, privateKeyReceiver, signatureContainer);
                    }
                    if (!packet.isFile)
                    {
                        text = HybridEncryption.DecryptData(packet, privateKeyReceiver, signatureContainer);
                    }
                }
                catch (CryptographicException e)
                {
                    _logger.LogInformation($"{DateTime.Now} - Packet: {packet.Id} - {e.Message}");
                    // Check in frontend if file and text is null, if so, we know something went wrong with the decryption.
                    file = null;
                    text = null;
                }
            }
        }

    }
}
