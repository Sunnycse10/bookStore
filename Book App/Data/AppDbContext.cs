using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Book_App.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Book_App.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

    }

    //public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    //{
    //    public AppDbContext CreateDbContext(string[] args)
    //    {
    //        IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    //        var connectionString = config.GetConnectionString("DefaultConnection");
    //        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    //        optionsBuilder.UseSqlServer(connectionString);
    //        return new AppDbContext(optionsBuilder.Options);
    //    }
    //}


}
