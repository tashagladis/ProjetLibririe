using APILibrary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Friend: ModelBase
    {
        public Friend()
        {

        }
        public string Username { get; set; }
        public ICollection<Link> Friends { get;  } =
          new HashSet<Link>();
  
        public ICollection<RegisterModel> Demands { get; } =
          new HashSet<RegisterModel>();
    }

    public class Link : ModelBase
    {
        public Link()
        {

        }
        public string Name { get; set; }
        public string ImageType { get; set; }

        public string ImageBasePath { get; set; }

    }
}
