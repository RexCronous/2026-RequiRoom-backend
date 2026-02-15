namespace WebApi.Services
{
    public static class UserHelpers
    {
        public static int GetCurrentUserId()
        {
            // TODO: Implement actual logic to get current user id from claims
            return 1;
        }

        // Hash password using BCrypt
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify password using BCrypt
        public static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
