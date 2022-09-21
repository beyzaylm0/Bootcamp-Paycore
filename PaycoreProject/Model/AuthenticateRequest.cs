using System.ComponentModel.DataAnnotations;

namespace PaycoreProject.Model
{
    public class AuthenticateRequest
    {
        [Required]
       
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
