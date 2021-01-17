using MeetAndPlay.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MeetAndPlay.Core.Services
{
    public class DbContextFactory
    {
        private readonly IConfiguration _configuration;

        public DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MNPContext Create()
        {
            var options = new DbContextOptionsBuilder<MNPContext>();
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(Data.Consts.Infrastructure.MigrationAssembly));

            return new MNPContext(options.Options);
        }
    }
}