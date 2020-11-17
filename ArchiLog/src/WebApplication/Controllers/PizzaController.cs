using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Core.Attributs.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBaseAPI<Pizza, EatDbContext>
    {
        

        public PizzaController(EatDbContext context) : base(context)
        {
            
        }

        

    }
}
