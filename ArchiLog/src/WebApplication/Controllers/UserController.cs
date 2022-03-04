﻿using APILibrary.Core.Attributs.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBaseAPI<RegisterModel, HeyYouDbContext>
    {
        public UserController(HeyYouDbContext context) : base(context)
        {

        }
    }
}