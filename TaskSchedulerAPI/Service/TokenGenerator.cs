using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using TaskSchedulerAPI.Interfaces;
using TaskSchedulerAPI.Model;

namespace TaskSchedulerAPI.Service
{
    public class TokenGenerator : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly IAccountRepository _accountRepository;

        public TokenGenerator(IConfiguration config, IAccountRepository accountRepository)
        {
            _config = config;
            _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _accountRepository = accountRepository;
        }
        public string CreateToken(AppUser user)
        {
            var role = _accountRepository.GetUserRole(user.Id);
            //create the claims for the user 
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.GivenName,user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName,user.LastName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Role,role)
                
            };
           
            //create the credentials 
            SigningCredentials credentials = new SigningCredentials(_key,SecurityAlgorithms.HmacSha256Signature);

            //create the description for the token
            var token_descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _config.GetValue<string>("JWT:Issuer"),
                Audience = _config.GetValue<string>("JWT:Audience"),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(30)
            };

            var token_handler = new JwtSecurityTokenHandler();

            var token = token_handler.CreateToken(token_descriptor);

            return token_handler.WriteToken(token);
            
        }

        public string RefreshToken()
        {
            var random_number = new byte[32];
            using var rgn = RandomNumberGenerator.Create();

            rgn.GetBytes(random_number);

            return Convert.ToBase64String(random_number);
        }
    }
}
