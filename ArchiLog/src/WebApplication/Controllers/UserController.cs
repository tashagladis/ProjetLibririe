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

    public class UserController : ControllerBaseAPI<RegisterModel, HeyYouDbContext>
    {
        public UserController(HeyYouDbContext context) : base(context)
        {

        }

        [HttpPost("add/{user}")]
        public virtual async Task<ActionResult<Message>> AddUser([FromRoute] string user)
        {
            if (user == null)
                return BadRequest(new { Message = $"User ne doit pas être null" });

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var login = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            if (login == null)
                return BadRequest(new { Message = $"Vous n'êtes peut-être pas connecté" });

            var userFind = _context.Set<RegisterModel>().FirstOrDefault(item => item.Login == user);

            var me = _context.Set<Friend>().FirstOrDefault(item => item.Username == login);


            if (me == null)
            {
                me = new Friend();
                
                var userToAdd = new Link();
                userToAdd.Name = userFind.Login;
                userToAdd.ImageBasePath = userFind.ImageBasePath;
                me.Friends.Add(userToAdd);
                me.Demands.Remove(userFind);
                _context.Add(me);
                await _context.SaveChangesAsync();
                return Ok(me);
            }
            else
            {
                var userToAdd = new Link();
                userToAdd.Name = userFind.Login;
                userToAdd.ImageBasePath = userFind.ImageBasePath;
                me.Friends.Add(userToAdd);
                me.Demands.Remove(userFind);
                await _context.SaveChangesAsync();

            }


     

            return Ok(me);
        }



        [HttpGet("friends")]
        public virtual async Task<ActionResult<RegisterModel>> GetFriends()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            if (me == null)
                return BadRequest(new { Message = $"Vous n'êtes peut-être pas connecté" });

            var results = _context.Set<Friend>()
                .Where(item => item.Username == me)
                .Select(item => new { friends = item.Friends })
                ;

            return Ok(results);
        }



        [HttpPost("invit/{user}")]
        public virtual async Task<ActionResult<Message>> AskUser([FromRoute] string user)
        {

            if (user == null)
                return BadRequest(new { Message = $"User ne doit pas être null" });

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var login = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            if (login == null)
                return BadRequest(new { Message = $"Vous n'êtes peut-être pas connecté" });

            var userFind = _context.Set<RegisterModel>().FirstOrDefault(item => item.Login == user);

            var me = _context.Set<RegisterModel>().FirstOrDefault(item => item.Login == login);

            var friend = _context.Set<Friend>()
                .Include(item => item.Demands)
                .FirstOrDefault(item => item.Username == user);

            if(friend == null)
            {
                friend = new Friend();
                friend.Username = user;
                friend.Demands.Add(me);
                _context.Add(friend);
                await _context.SaveChangesAsync();

                return Ok(friend);

            }   
            if (userFind != null)
            {
                friend.Demands.Add(me);

            }
            _context.Update(friend);

            await _context.SaveChangesAsync();

            return Ok(friend);
        }



        [HttpGet("invitations")]
        public virtual async Task<ActionResult<RegisterModel>> GetInvitations()
        {

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            if (me == null)
                return BadRequest(new { Message = $"Vous n'êtes peut-être pas connecté" });

            var results = _context.Set<Friend>()
                .Where(item => item.Username == me)
                .Select(item => new { invitations = item.Demands });

            return Ok(results);
        }


        [HttpGet("mydatas")]
        public virtual async Task<ActionResult<RegisterModel>> Mydatas()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var results = await _context.Set<RegisterModel>()
                .Where(item => item.Login == me)
                .ToListAsync();

            return Ok(results);
        }



        [HttpGet("yourdatas/{user}")]
        public virtual async Task<ActionResult<RegisterModel>> YourDatas([FromRoute] string user)
        {

            var results = await _context.Set<RegisterModel>()
                .Where(item => item.Login == user)
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("demandsend/{user}")]
        public virtual async Task<ActionResult<bool>> demandsend([FromRoute] string user)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var result = false;

           var results = _context.Set<Friend>()
                .Include(item => item.Demands)
                .FirstOrDefault(item => item.Username == user);
            if (results != null)
            {
                foreach (var res in results.Demands)
                {
                    if (res.Login == me)
                        result = true;
                }
            }
            return Ok(result);
        }


        [HttpGet("demandaccept/{user}")]
        public virtual async Task<ActionResult<bool>> demandaccept([FromRoute] string user)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var result = false;

            var results = _context.Set<Friend>()
                 .Include(item => item.Friends)
                 .FirstOrDefault(item => item.Username == user);
            if (results != null)
            {
                foreach (var res in results.Friends)
                {
                    if (res.Name == me)
                        result = true;
                }
            }
            return Ok(result);
        }



    }
}
