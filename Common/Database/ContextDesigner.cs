using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace U.Common.Database
{
    public static class ContextDesigner
    {
        public static DbContextOptionsBuilder<T> CreateDbContextOptionsBuilder<T>(string mainFilePath) where T : DbContext
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory + mainFilePath)
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<T>();

            SetDbProviders(ref builder, configuration);

            return builder;
        }

        private static void SetDbProviders<T>(ref DbContextOptionsBuilder<T> builder, IConfigurationRoot configuration) where T : DbContext
        {
            const string dbOptionsSection = "DbOptions";
            var dbOptions = new DbOptions();
            configuration.GetSection(dbOptionsSection).Bind(dbOptions);

            if (dbOptions.Connection is null)
            {
                throw new UnsupportedDatabaseException("Database options are missing.");
            }

            switch (dbOptions.Type)
            {
                default:
                case DbType.Unknown:
                    throw new Exception("Unsupported database type selected.");
                case DbType.Npgsql:
                    builder.UseNpgsql(dbOptions.Connection);
                    break;
                case DbType.Mssql:
                    builder.UseSqlServer(dbOptions.Connection);
                    break;
            }
        }
    }
}