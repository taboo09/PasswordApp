namespace PassApp.API.Repository
{
    public interface IEncryptPassword
    {
        byte[] GenerateKey();
        string EncryptString(string password, string key);
        string DecryptString(string password, string key);
    }
}