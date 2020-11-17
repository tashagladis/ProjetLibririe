using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class EatDbContext: DbContext
    {
        //public static readonly islogg

        public static readonly ILoggerFactory SqlLogger = LoggerFactory.Create(builder => builder.AddConsole());
        public EatDbContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<Pizza> Pizzas { get; set; }

        public DbSet<Customer> Customers { get; set; }
    }
}
