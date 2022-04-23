using JWTAuth.Data.Entities;
using JWTAuthWebAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace JWTAuthWebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(ApplicationUser user);
        Task<IdentityResult> RegisterUserAsync(RegisterModel model);
        Task<AuthResponse> LoginUserAsync(LoginModel model);
    }

}
