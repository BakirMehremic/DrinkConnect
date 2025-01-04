using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace DrinkConnect.Utils
{
    public class TokenUtils
    {
        private readonly IConfiguration _configuration;

        public TokenUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // method to avoid possible null reference in 
        // TokenService constructor
        public SymmetricSecurityKey InitializeKey()
        {
            var signingKey = _configuration["JWT:SigningKey"];

            if (string.IsNullOrEmpty(signingKey))
            {
                throw new ArgumentException("JWT SigningKey is not configured properly.");
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }
    }
}