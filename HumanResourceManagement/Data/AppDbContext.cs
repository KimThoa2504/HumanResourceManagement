using HumanResourceManagement.Models.Attendances;
using HumanResourceManagement.Models.Candidates;
using HumanResourceManagement.Models.Departments;
using HumanResourceManagement.Models.Employees;
using HumanResourceManagement.Models.LeaveRequests;
using HumanResourceManagement.Models.Performances;
using HumanResourceManagement.Models.Recruitments;
using HumanResourceManagement.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace HumanResourceManagement.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Status)
                .HasConversion<string>();


        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet <User> Users { get; set; }

        public DbSet <Employee> Employees { get; set; }

        public DbSet <Department> Departments { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<Recruitment> Recruitments { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<Performance> Performances { get; set; }  


    }
}
