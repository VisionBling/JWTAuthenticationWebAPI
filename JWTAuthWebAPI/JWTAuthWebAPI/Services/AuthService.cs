using JWTAuth.Data.Entities;
using JWTAuthWebAPI.Data;
using JWTAuthWebAPI.Models;
using JWTAuthWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWTAuthWebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id)
                };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            // Include any additional userClaims if necessary
            claims.AddRange(userClaims);


            // Get Jwt Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User with this email already exists." });
            }
            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                foreach (var roleName in model.Roles)
                {
                    var validRole = roleName.ToLower() == "admin" || roleName.ToLower() == "user" ? roleName : "user";
                    if (!await _roleManager.RoleExistsAsync(validRole))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(validRole));
                    }
                    await _userManager.AddToRoleAsync(user, validRole);
                }
            }

            return result;
        }


        public async Task<AuthResponse> LoginUserAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new AuthResponse(false, "User not found.");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var token = await GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();
                _ = SaveRefreshToken(user.Id, refreshToken); 

                return new AuthResponse(true, "Login successful.", token, refreshToken);
            }
            else if (result.IsLockedOut)
            {
                return new AuthResponse(false, "User account is locked.");
            }
            else if (result.IsNotAllowed)
            {
                return new AuthResponse(false, "Login not allowed.");
            }
            else
            {
                return new AuthResponse(false, "Invalid login attempt.");
            }
        }


        private string GenerateRefreshToken()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[32]; // 256 bits
                randomNumberGenerator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private async Task SaveRefreshToken(string userId, string refreshToken)
        {
            // Optionally, check if a token already exists for the user and update it
            var existingToken = await _context.UserRefreshTokens.FirstOrDefaultAsync(urt => urt.UserId == userId);
            if (existingToken != null)
            {
                existingToken.RefreshToken = refreshToken;
                existingToken.Expires = DateTime.UtcNow.AddDays(7); // Set expiration to 7 days, for example
            }
            else
            {
                _context.UserRefreshTokens.Add(new UserRefreshToken
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                    Expires = DateTime.UtcNow.AddDays(7) // Set expiration to 7 days
                });
            }

            await _context.SaveChangesAsync();
        }
        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var userId = await GetUserIdFromRefreshToken(refreshToken); // Implement this method to retrieve user ID based on the refresh 
            if (userId == null) return new AuthResponse(false, "Invalid refresh token.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new AuthResponse(false, "User not found.");

            // Optionally validate or rotate the refresh token here

            var newAccessToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();  // Generate referesh token 
            _ = SaveRefreshToken(userId, newRefreshToken); // Save the new refresh token

            return new AuthResponse(true, "Token refreshed successfully.", newAccessToken, newRefreshToken);
        }
        private async Task<string> GetUserIdFromRefreshToken(string refreshToken)
        {
            var token = await _context.UserRefreshTokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken && t.Expires > DateTime.UtcNow);

            if (token == null || token.Expires <= DateTime.UtcNow)
            {
                throw new Exception("Refresh token not found or expired.");
            }

            return token.UserId;
        }


    }

}
