namespace JWTAuthWebAPI.Models
{
    using Microsoft.AspNetCore.Identity;

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; } // Optional: Include the token in the response

        public LoginResponse(bool success, string message, string token = null)
        {
            Success = success;
            Message = message;
            Token = token;
        }
    }

}
