using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NewsFeedEngine.DataAccess;
using NewsFeedEngine.Service;

namespace NewsFeedEngine.Tests
{
    public class DbGetTests
    {
        private readonly Db _context;

        public DbGetTests()
        {
            var connectionString = Config.GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;

            _context = new Db(options);
        }

        [Test]
        public void DefaultConnectionStringServerConnection()
        {
            // act
            var isConnected = _context.Database.CanConnect();

            // assert
            Assert.That(isConnected, Is.True);
        }
    }
}