using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyFirstWebApplication.Class;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MyFirstWebApplication.Data
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<School>()
                .HasMany(s => s.Students)
                .WithOne(s => s.School)
                .HasForeignKey(s => s.SchoolId);

            modelBuilder.Entity<School>()
                .HasMany(s => s.Classrooms)
                .WithOne(c => c.School)
                .HasForeignKey(c => c.SchoolId);

            // Seed initial data
            modelBuilder.Entity<School>().HasData(
                new School(1, "MySchool") { Id = 1 }
            );
        }
    }
}