using Lun.Server.Models.Player;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Models.Data
{
    class MySQLContext : DbContext
    {
        const string DBHost = "localhost";
        const string DBDatabase = "lun";
        const string DBUser = "root";
        const string DBPassword = "156156";

        public void Run()
        {
            // Create Database if not exist
            Database.EnsureCreated();

            try
            {
                var databaseCreator = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
                databaseCreator.CreateTables();
            }
            catch
            {  }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = $@"server={DBHost};Database={DBDatabase};Uid={DBUser};Pwd={DBPassword}";            
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
            {
                options.EnableStringComparisonTranslations();
            });
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
