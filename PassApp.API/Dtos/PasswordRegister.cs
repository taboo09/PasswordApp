using System.ComponentModel.DataAnnotations;

namespace PassApp.API.Dtos
{
    public class PasswordRegister
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public string Hint { get; set; }
        public string SecretQuestion { get; set; }
        public string Answer { get; set; }
        public string Other { get; set; }
        
        [Required]
        public string PasswordEncrypt { get; set; }
    }
}