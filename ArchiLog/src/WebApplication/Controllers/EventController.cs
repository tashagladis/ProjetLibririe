using APILibrary.Core.Attributs.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController :  ControllerBaseAPI<Event, HeyYouDbContext>
    {
        public EventController(HeyYouDbContext context) : base(context)
        {

        }

        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public override async Task<ActionResult<Event>> CreateItem([FromBody] Event item)
        {

            if (ModelState.IsValid)
            {
                item.Date = DateTime.Now;
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                item.Creator = me;


                _context.Add(item);
                await _context.SaveChangesAsync();

                return Ok(item);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("myevents")]
        public virtual async Task<ActionResult<IEnumerable<Event>>> Messages()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var me = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var results = await _context.Set<Event>()
                .Where(item => item.Creator == me)
                .ToListAsync();

            return Ok(ToJsonList(results));
        }



        [HttpGet("yourevents/{user}")]
        public virtual async Task<ActionResult<IEnumerable<Event>>> YourMessages([FromRoute] string user)
        {

            var results = await _context.Set<Event>()
                .Where(item => item.Creator == user)
                .ToListAsync();

            return Ok(ToJsonList(results));
        }

    }
}
