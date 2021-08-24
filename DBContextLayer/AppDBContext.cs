using IserviceColections_MapWhen.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IserviceColections_MapWhen.DBContextLayer
{
    public class AppDBContext : DbContext
    {
       public readonly IConfiguration _configuration;

        
        public AppDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public DbSet<Students> students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("StudentContextConnection"));
            //optionsBuilder.UseSqlServer(DataSource.Intances().GetDataSourceSever());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Students>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        }
    }
}
