using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Models;
using DrinkConnect.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DrinkConnect.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        private readonly SymmetricSecurityKey _key;

        private readonly UserManager<User> _userManager;

        private readonly TokenUtils _tokenUtils;

        public TokenService(IConfiguration configuration, UserManager<User> userManager,
        TokenUtils tokenUtils)
        {
            _configuration = configuration;
            _tokenUtils = tokenUtils;
            _key = _tokenUtils.InitializeKey();
            _userManager = userManager; 
        }

        public string CreateToken(User user){

            if (user.Email == null) throw new ArgumentNullException(nameof(user.Email), "User email is required.");
            if (user.UserName == null) throw new ArgumentNullException(nameof(user.UserName), "User username is required.");
            
            var roles = _userManager.GetRolesAsync(user).Result; 

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));  
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesc);

            // Return the JWT token as a string
            return tokenHandler.WriteToken(token);
        }

        public string CreateRefreshToken(){

            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        }

    }
}