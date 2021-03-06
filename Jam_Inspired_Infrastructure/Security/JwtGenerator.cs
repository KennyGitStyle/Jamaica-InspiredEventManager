using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Jam_Inspired_Application.Interfaces;
using Jam_Inspired_Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Jam_Inspired_Infrastructure.Security {
    public class JwtGenerator : IJwtGenerator {
        private readonly SymmetricSecurityKey _key;
        public JwtGenerator (IConfiguration config) {
            _key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (config["TokenKey"]));
        }

        public string CreateToken (AppUser user) {
            var claims = new List<Claim> {
                new Claim (JwtRegisteredClaimNames.NameId, user.UserName)
            };

            // generate signing credentials
            var creds = new SigningCredentials (_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (claims),
                Expires = DateTime.Now.AddDays (7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler ();

            var token = tokenHandler.CreateToken (tokenDescriptor);

            return tokenHandler.WriteToken (token);
        }
    }
}