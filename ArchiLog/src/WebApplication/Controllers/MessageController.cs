using APILibrary.Core.Attributs.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBaseAPI<Message, HeyYouDbContext>
    {
        public MessageController(HeyYouDbContext context) : base(context)
        {

        }
     
        [HttpPost("send/{receiver}")]
        public virtual async Task<ActionResult<Message>> SendMessage([FromRoute] string receiver, [FromBody] Message item)
        {
            if (ModelState.IsValid)
            {
                if(receiver == null)
                    return BadRequest(new { Message = $"Id ne doit pas être null" });

                item.Reciever = receiver;

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var sender = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (sender  == null)
                    return BadRequest(new { Message = $"Vous n'êtes peut-être pas connecté" });

                item.Sender= sender;
                item.Date = DateTime.Now;

                _context.Add(item);
                await _context.SaveChangesAsync();

                return Ok(item);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("messages")]
        public virtual async Task<ActionResult<IEnumerable<Message>>> Messages()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var results = await _context.Set<Message>()
                .Where(item => item.Reciever == me)
                .ToListAsync();

            return Ok(ToJsonList(results));
        }
     }
}
