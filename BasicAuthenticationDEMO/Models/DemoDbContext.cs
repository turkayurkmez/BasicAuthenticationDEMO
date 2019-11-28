using Microsoft.EntityFrameworkCore;
using System;

namespace BasicAuthenticationDEMO.Models
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().
                HasData(new User
                {
                    UserName = "demouser",
                    Password = "demouser1",
                    Email = "demo@user.com",
                    Name = "demo",
                    LastName = "user",
                    Id = Guid.NewGuid()
                });
        }
        public DbSet<User> Users { get; set; }

    }
}