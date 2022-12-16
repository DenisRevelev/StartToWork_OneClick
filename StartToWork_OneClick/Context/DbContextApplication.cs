using StartToWork_OneClick.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace StartToWork_OneClick.Context
{
    public class DbContextApplication : DbContext
    {
        private string? _connetionString;
        public DbSet<SettingsModel> settingsModels => Set<SettingsModel>();

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("ConnectionString.json");

            var config = builder.Build();

            _connetionString = config.GetConnectionString("DefaultConnection");
            return _connetionString!;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }
}
