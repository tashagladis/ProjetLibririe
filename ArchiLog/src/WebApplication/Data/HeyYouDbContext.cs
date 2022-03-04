using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Authentification;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class HeyYouDbContext :  IdentityDbContext<ApplicationUser>
    {
        public HeyYouDbContext(DbContextOptions<HeyYouDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<RegisterModel> RegisterModels { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
