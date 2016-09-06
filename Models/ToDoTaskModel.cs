using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoTnet.Models
{
    public class ToDoTaskModel
    {

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Product { get; set; }
        public string Type { get; set; }
        public int Priority { get; set; }
        public DateTime DoneDate { get; set; }


    }
}
