namespace JWTAuthWebAPI.Models
{
    using Microsoft.AspNetCore.Identity;

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public AuthResponse(bool success, string message, string token = null, string refreshToken = null)
        {
            Success = success;
            Message = message;
            Token = token;
            RefreshToken = refreshToken;
        }
    }

}
