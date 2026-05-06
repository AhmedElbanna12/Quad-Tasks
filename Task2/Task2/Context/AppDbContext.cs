using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Context
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);



            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Ahmed" },
                new User { Id = 2, Name = "Ali" },
                new User { Id = 3, Name = "Mona" }
                );

            modelBuilder.Entity<TaskItem>()
       .Property(t => t.Status)
       .HasConversion<string>();
        }


    }
}
