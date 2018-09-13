namespace PassApp.API.Dtos
{
    public class UserChangePassword
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}