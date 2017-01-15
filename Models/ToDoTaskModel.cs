using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoTnet.Models
{
    public class ToDoTaskModel
    {
        public ToDoTaskModel()
        {
            new ToDoTaskModel(Guid.Empty);
        }
        public ToDoTaskModel(Guid gID)
        {
            ID = gID.ToString();
        }
        public string ID { get; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Product { get; set; }
        public string Type { get; set; }
        public int Priority { get; set; }
        public bool Done { get; set; }


    }
}
