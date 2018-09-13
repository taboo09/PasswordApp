using System;

namespace PassApp.API.Dtos
{
    public class PasswordReturn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public string Hint { get; set; }
        public string SecretQuestion { get; set; }
        public string Answer { get; set; }
        public string Other { get; set; }
        public string PasswordEncrypt { get; set; }
    }
}