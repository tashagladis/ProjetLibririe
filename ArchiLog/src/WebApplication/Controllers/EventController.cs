using APILibrary.Core.Attributs.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
