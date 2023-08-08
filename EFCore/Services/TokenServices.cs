using EFCore.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCore.Services
{
    public class TokenServices
    {
        public static string generateToken(Usuario User, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Key.Secret);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("Id" ,User.Id.ToString()), //User.Identity.Name
                    new Claim(ClaimTypes.Role, role) //User.IsInRole()
                }),
                Expires= DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenConfig);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;

        }
         
    }
}
