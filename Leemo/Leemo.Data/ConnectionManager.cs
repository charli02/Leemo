using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using TPSS.Common;

/// <summary>
/// Represents Leemo data project
/// </summary>
namespace Leemo.Data
{
    /// <summary>
    /// This class used for database connection related stuff.
    /// </summary>
    public class ConnectionManager
    {
        private static IConfiguration currentConfig;

        public static void SetConfig(IConfiguration configuration)
        {
            currentConfig = configuration;
        }

        /// <summary>
        /// Get a connection to the database.
        /// </summary>
        public static SqlConnection GetConnection
        {
            get
            {
                string connectionString = currentConfig.GetConnectionString(Constants.API_DbConnection);
                // Create a new connection for each query.
                SqlConnection connection = new SqlConnection(connectionString);
                return connection;
            }
        }

        /// <summary>
        /// Get connection string name from appsettings.json
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return currentConfig.GetConnectionString(Constants.API_DbConnection);
            }
        }
    }
}
