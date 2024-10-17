using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Victuz.Models;

namespace Victuz.Data
{
    public class ApplicationDbContext : IdentityDbContext<Person>
    {
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    : base(options)
        //{
        //}



        public DbSet<ActivityModel> Activities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<Person> People { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = @"Data Source=AL2;Initial Catalog=Victuz;Integrated Security=True;Trust Server Certificate=True";
            optionsBuilder.UseSqlServer(connection);
        }

    }
}
