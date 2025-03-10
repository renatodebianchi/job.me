using Domain.Entities; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Configuration; 
namespace Infra.Data.Contexts 
{ 
    public class ApplicationDbContext : DbContext 
    { 
        private readonly IConfiguration _configuration; 

        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(IConfiguration configuration, DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
            _configuration = configuration; 
        } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        { 
            optionsBuilder.UseSqlite( 
                _configuration.GetConnectionString("DefaultConnectionSqlite"), 
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName) 
            ); 
        } 
        protected override void OnModelCreating(ModelBuilder builder) 
        { 
            base.OnModelCreating(builder); 
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly); 
        } 
    } 
} 
