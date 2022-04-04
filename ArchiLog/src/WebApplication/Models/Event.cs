using APILibrary.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Event: ModelBase
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Hour { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime EventDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; } 

        public string Creator { get; set; }

        public ICollection<RegisterModel> EventUsers { get; } =
          new HashSet<RegisterModel>();

        public string ImageType { get; set; }

        public string ImageBasePath { get; set; }

    }
}
