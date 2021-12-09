namespace JWTAuthWebAPI.Models
{
    using Microsoft.AspNetCore.Identity;

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


}
