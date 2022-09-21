using System.ComponentModel.DataAnnotations;

namespace PaycoreProject.Model
{
    public class RegisterRequest
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
