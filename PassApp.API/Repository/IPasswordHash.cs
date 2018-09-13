namespace PassApp.API.Repository
{
    public interface IPasswordHash
    {
        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);

        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}