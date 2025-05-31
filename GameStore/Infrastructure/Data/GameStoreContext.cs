using System.Data;
using Microsoft.Data.SqlClient;

namespace GameStore.PackingService.Infrastructure.Data;

public class GameStoreContext(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("SqlConnection");
        return new SqlConnection(connectionString);
    }
}
