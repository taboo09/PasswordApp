using System.ComponentModel.DataAnnotations;

namespace PassApp.API.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 4, ErrorMessage="The password needs to has between 4 and 6 characters.")]
        public string Password { get; set; }
    }
}