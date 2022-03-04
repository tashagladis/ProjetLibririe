using APILibrary.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Message : ModelBase
    {
      
        public string Text { get; set; }   

        public int RecieverId { get; set; }

        [ForeignKey("RecieverId")]
        public RegisterModel Reciever { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

    }
}
