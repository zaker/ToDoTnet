using System.ComponentModel.DataAnnotations;

namespace ToDoTnet.Models
{
    public class LoginModel
    {
        [Required]
        public string User { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


    }
}
