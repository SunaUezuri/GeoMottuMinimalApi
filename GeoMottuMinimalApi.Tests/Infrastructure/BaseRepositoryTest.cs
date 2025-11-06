using GeoMottuMinimalApi.Infrastructure.Data.AppDatas;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GeoMottuMinimalApi.Tests.Infrastructure
{
    public abstract class BaseRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        protected readonly ApplicationContext Context;

        static BaseRepositoryTests()
        {
            SQLitePCL.Batteries.Init();
        }

        protected BaseRepositoryTests()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new ApplicationContext(options);

            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}