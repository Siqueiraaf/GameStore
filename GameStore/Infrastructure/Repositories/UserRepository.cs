using System.Data;
using Dapper;
using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Infrastructure.Repositories.Interfaces;


namespace GameStore.PackingService.Infrastructure.Repositories;

public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = "SELECT * FROM Users WHERE Email = @Email";
        return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User> CreateAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users (Id, Name, Email, Password)
            VALUES (@Id, @Name, @Email, @Password)";

        await _dbConnection.ExecuteAsync(sql, user);
        return user;
    }
}
