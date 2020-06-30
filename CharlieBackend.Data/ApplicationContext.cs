using System;
using System.Reflection;
using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CharlieBackend.Data
{
    public partial class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Mentor> Mentors { get; set; }
        public virtual DbSet<MentorOfCourse> Mentorsofcourses { get; set; }
        public virtual DbSet<MentorOfStudentGroup> Mentorsofstudentgroups { get; set; }
        public virtual DbSet<StudentGroup> Studentgroups { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentsOfGroups> Studentsofgroups { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
