using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class FoodJournalContextFactory : IDesignTimeDbContextFactory<FoodJournalContext>
    {
        public FoodJournalContext CreateDbContext(string[] args)
        {
            Console.WriteLine("Hello");
            Console.WriteLine(args[0]);
            var connectionString = args[0];

            //var connectionString = "Server=localhost;Database=foodjournal;Uid=root;Pwd=H@milt0n;";

            // Replace with your server version and type.
            // Use 'MariaDbServerVersion' for MariaDB.
            // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
            // For common usages, see pull request #1233.
            // var serverVersion = new MySqlServerVersion(new Version(5, 6, 44));
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            // Replace 'YourDbContext' with the name of your own DbContext derived class.
            var options = new DbContextOptionsBuilder<FoodJournalContext>()
                    .UseMySql(connectionString, serverVersion)
                    // The following three options help with debugging, but should
                    // be changed or removed for production.
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();

            return new FoodJournalContext(options.Options);
        }
    }
}