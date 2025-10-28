using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Payroll.Attendance.Application.Dto;
using Payroll.Attendance.Application.Utility;
using Payroll.Attendance.Domain.Models;

namespace Payroll.Attendance.Infrastructure;

public class CryptographyUtility : ICryptographyUtility
{
    private const int SaltSize = 16;
        private const int HashSize = 64; // SHA-512
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;
        private const char Delimiter = ';';
        private readonly JwtSettings _jwtSettings;
        private const string SaltKey = "yrdtycrydrsetu89y98uijreeweaweasz7i80u9";


        public CryptographyUtility(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings= jwtSettings.Value;
        }

        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var HashedPassword = Rfc2898DeriveBytes.Pbkdf2(password, salt,Iterations,HashAlgorithm,HashSize);
            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(HashedPassword));
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var element = passwordHash.Split(Delimiter);
            var salt = Convert.FromBase64String(element[0]);
            var hash = Convert.FromBase64String(element[1]);

            var newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithm, HashSize);
            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }

        public TokenResponse GenerateToken(UserModel user)
        {
            
            var roles = JsonSerializer.Serialize(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserData", roles)
                }),
                Issuer =_jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(1), // Token expires in 1 hour from the current UTC time
                SigningCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResponse
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = tokenDescriptor.Expires
            };
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(6)
            };
            return refreshToken;
            
        }

        
}

