using IdentityMicroService.Database;
using IdentityMicroService.Modules.Identity.Contracts;
using IdentityMicroService.Modules.Identity.Ports;
using Dapper;
using System.IdentityModel.Tokens.Jwt;
using IdentityMicroService.Modules.Auth.Core;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityMicroService.Modules.Identity.Adapters;

public sealed class IdentityService : IIdentityService
{
    private readonly IDBConnectionFactory _connection;
    private readonly JWTSettings _jwtSettings;

    public IdentityService(IDBConnectionFactory connection, JWTSettings jwtSettings)
    {
        _connection = connection;
        _jwtSettings = jwtSettings;
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        using (var connection = await _connection.CreateConnectionAsync())
        {
            var query =
                "SELECT * FROM Users WHERE Email=@email";

            DynamicParameters param = new();
            param.Add("@email", email);

            var user = await connection.QuerySingleAsync<dynamic>(query, param);

            if (user is null)
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "user not found" }
                };

            var isPasswordValid = user.Password == password;
            if (!isPasswordValid)
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "wrong credentials" }
                };

            return CreateToken(email, user.Id);
        }
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string password)
    {
        using (var connection = await _connection.CreateConnectionAsync())
        {
            var query =
                "INSERT INTO Users (Email, Password) VALUES (@email, @password) RETURNING Id";

            DynamicParameters param = new();
            param.Add("@email", email);
            param.Add("@password", password);

            var id = await connection.QuerySingleAsync<int>(query, param);

            if (id <= 0)
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "error creating user" }
                };

            return CreateToken(email, id);
        }
    }

    private AuthenticationResult CreateToken(string email, int id)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        SecurityTokenDescriptor tokenDescriptor =
            new()
            {
                Subject = new(
                    new Claim[]
                    {
                            new(JwtRegisteredClaimNames.Sub, email),
                            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new(JwtRegisteredClaimNames.Email, email),
                            new("id", id.ToString()),
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new() { Token = tokenHandler.WriteToken(token), Success = true };
    }
}

