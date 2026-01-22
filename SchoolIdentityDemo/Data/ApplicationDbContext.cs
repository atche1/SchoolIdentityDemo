using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolIdentityDemo.Models;

namespace SchoolIdentityDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
        public DbSet<TeacherProfile> TeacherProfiles => Set<TeacherProfile>();
        public DbSet<Homework> Homeworks => Set<Homework>();
        public DbSet<HomeworkSubmission> HomeworkSubmissions => Set<HomeworkSubmission>();


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<HomeworkSubmission>()
    .HasIndex(s => new { s.HomeworkId, s.StudentId });

            builder.Entity<HomeworkSubmission>()
                .HasOne(s => s.Homework)
                .WithMany(h => h.Submissions)
                .HasForeignKey(s => s.HomeworkId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HomeworkSubmission>()
                .HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Homework>()
    .HasOne(h => h.Teacher)
    .WithMany()
    .HasForeignKey(h => h.TeacherId)
    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentProfile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<StudentProfile>(p => p.UserId)
        .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TeacherProfile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<TeacherProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
