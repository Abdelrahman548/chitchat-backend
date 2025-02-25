namespace ChitChat.Service.Helpers
{
    public static class HashingManager
    {
        public const int WORKFACTOR = 12;
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WORKFACTOR);
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
