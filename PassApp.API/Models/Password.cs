using System;
using System.ComponentModel.DataAnnotations;

namespace PassApp.API.Models
{
    public class Password
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
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