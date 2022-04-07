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

               // var results = await _context.Set<RegisterModel>()
               //.Where(item => (item.Login == 

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

        [HttpGet("getmessages/{user}")]
        public virtual async Task<ActionResult<IEnumerable<Message>>> GetMessage([FromRoute] string user)
        {
                if (user == null)
                    return BadRequest(new { Message = $"Id ne doit pas être null" });               

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

                if (me == null)
                    return BadRequest(new { Message = $"Vous n'êtes peut-être pas connecté" });

                var results = await _context.Set<Message>()
                .Where(item =>  (item.Reciever == me && item.Sender == user)
                                 || (item.Reciever == user && item.Sender == me))
                 .ToListAsync();

                return Ok(ToJsonList(results));           
        }


        [HttpGet("messages")]
        public virtual async Task<ActionResult<IEnumerable<Message>>> Messages()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var results = await _context.Set<Message>()
                .Where(item => item.Reciever == me || item.Sender == me)
                .ToListAsync();

            return Ok(ToJsonList(results));
        }


        [HttpGet("users")]
        public virtual List<RegisterModel> ListOfUsers()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var listUsers = new List<RegisterModel>();
            // me = me.ToLower();
            var results =  _context.Set<Message>()
                .Where(item => item.Reciever == me || item.Sender == me)
                .ToList();

            List<string> users = new List<string>();

            foreach(var message in results)
            {
                //message.Sender = message.Sender;

                //Verifier si l'élément est different de celui connecté
                if (message.Sender != me)
                {
                    //Verifier si l'élément n'est pas dans la liste avant de l'ajouter
                    if (users != null)
                    {
                        if (!users.Any(s => s == message.Sender || s == message.Reciever))
                        {
                            users.Add(message.Sender);
                        }
                    }
                    else
                    {
                        users.Add(message.Sender);
                    }
                }

                if (message.Reciever != me)
                {
                    if (users != null)
                    {
                        if (!users.Any(s => s == message.Sender || s == message.Reciever))
                        {
                            users.Add(message.Reciever);
                        }
                    }
                    else 
                    {
                        users.Add(message.Reciever);
                    }
                    
                }

            }
            if(users != null)
            {
                var result = new RegisterModel();


                foreach (var user in users)
                {
                   result = _context.Set<RegisterModel>()
                  .Where(item => item.Login == user)
                  .FirstOrDefaultAsync().Result;
                   listUsers.Add(result);

                }
            }
            

            return listUsers;
        }
    }
}
