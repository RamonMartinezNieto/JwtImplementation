
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestImplementaciónJWT.Models.Common;
using TestImplementaciónJWT.Models.Request;
using TestImplementaciónJWT.Models.Response;

namespace TestImplementaciónJWT.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)   //El appSettings se genere en el startUp ya que implemente IOptions y se agrega como opción inyectada
        {
            _appSettings = appSettings.Value;
        }

        public UserResponse Auth(AuthRequest model)
        {
            UserResponse userResponse = null;

            if (model.UserName == "ramon" && model.Password == "contra")
            {
                userResponse = new UserResponse();
                userResponse.UserName = model.UserName;
                userResponse.Token = GenerateToken(model);
            }
            
            return userResponse; 
        }

        private string GenerateToken(AuthRequest model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var llave = Encoding.ASCII.GetBytes(_appSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, model.UserName),  // es mejor guardarse el ID del usuario
                        }
                    ),
                Expires = DateTime.UtcNow.AddDays(10), //expiración en 10 dias
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256)  //encriptación con la llave y HmacSha256
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); //creación del token usando la descripción.
            return tokenHandler.WriteToken(token); //devuelvo string del token.
        }
    }
}
