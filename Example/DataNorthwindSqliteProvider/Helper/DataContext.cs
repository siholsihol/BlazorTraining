using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;

namespace DataNorthwindSqliteProvider.Helper
{
    public class DataContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var resourceName = "DataNorthwindSqliteProvider.Northwind.db";
            var tempFileName = Path.GetTempFileName();
            using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
              ?? throw new FileNotFoundException($"Embedded resource {resourceName} not found.");

            using var tempStream = File.Create(tempFileName);

            // We are using a buffer to efficiently copy possibly large files.
            const int bufferSize = 8 * 1024; // 8 KB buffer (adjust as needed)
            var buffer = new byte[bufferSize];

            int bytesRead;
            while ((bytesRead = resourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                tempStream.Write(buffer, 0, bytesRead);
            }

            var conStr = _configuration.GetConnectionString($"Data Source={tempFileName}");
            var tempConnection = new SqliteConnection(conStr);
            tempConnection.Open();

            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            tempConnection.BackupDatabase(connection);

            tempConnection.Close();
            File.Delete(tempFileName);

            return connection;
        }
    }
}
