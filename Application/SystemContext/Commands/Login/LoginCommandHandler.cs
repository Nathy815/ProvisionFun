using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Application.SystemContext.Commands.Login
{
    public class LoginCommandHandler : Util, IRequestHandler<LoginCommand, string>
    {
        private readonly MySqlContext _sqlContext;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(MySqlContext sqlContext, IConfiguration configuration)
        {
            _sqlContext = sqlContext;
            _configuration = configuration;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _user = await _sqlContext.Set<User>()
                                        .Include(u => u.Role)
                                        .Where(u => u.Email.Equals(request.Email) &&
                                                    VerifyPassword(u.Password, request.Password))
                                        .FirstOrDefaultAsync();

                // Grava as Roles
                var _roleClaims = new List<Claim>();
                _roleClaims.Add(new Claim(ClaimTypes.Role, _user.Role.Name));

                // Gera o Token
                var _tokenHandler = new JwtSecurityTokenHandler();
                var _key = Convert.FromBase64String(_configuration["Authentication:SecurityKey"]);
                var _tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Sid, _user.Id.ToString()),
                        new Claim(ClaimTypes.Name, _user.Name),
                        new Claim(ClaimTypes.Email, _user.Email),
                        new Claim(ClaimTypes.Role, _user.RoleID.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(4),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
                };

                _tokenDescriptor.Subject.AddClaims(_roleClaims);

                var _token = _tokenHandler.CreateToken(_tokenDescriptor);

                return _tokenHandler.WriteToken(_token);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
