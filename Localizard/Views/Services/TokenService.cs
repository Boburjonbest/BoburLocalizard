using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Localizard.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Localizard.Views.Services
{
    public class TokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            var secret = config["Jwt:Key"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentNullException(nameof(secret), "JWT secret key is missing or empty. ");
            }

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
               

    
       
        }


       
      

      

        //public string GenerateJwtToken(User user)
        //{
            
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user), "User cannot be null");
        //    }

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

            
        //    if (!string.IsNullOrWhiteSpace(user.Role))
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, user.Role));
        //    }


        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _config["Jwt:Issuer"],
        //        audience: _config["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddHours(1),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

           

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
