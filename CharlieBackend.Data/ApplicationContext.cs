﻿using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CharlieBackend.Data
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Lesson> Lessons { get; set; }

        public virtual DbSet<Mentor> Mentors { get; set; }

        public virtual DbSet<MentorOfCourse> MentorsOfCourses { get; set; }

        public virtual DbSet<MentorOfStudentGroup> MentorsOfStudentGroups { get; set; }

        public virtual DbSet<Secretary> Secretaries { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<StudentGroup> StudentGroups { get; set; }

        public virtual DbSet<StudentOfStudentGroup> StudentsOfStudentGroups { get; set; }

        public virtual DbSet<Theme> Themes { get; set; }

        public virtual DbSet<Visit> Visits { get; set; }
        
        public virtual DbSet<Schedule> Schedules { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. 
        //         See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseMySql("server=localhost;port=3306;database=soft;uid=root;
        //                pwd=charlieback", x => x.ServerVersion("8.0.20-mysql"));
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Secretary>(entity =>
            {
                entity.ToTable("secretary");

                entity.HasIndex(e => e.AccountId)
                    .HasName("FK_account_of_secretary");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Secretaries)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_account_of_secretary");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.HasIndex(e => e.Email)
                    .HasName("email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(50)")
                    .HasComment("email has been set to not null and unique")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("'1'")
                    .HasComment("is_active has been set to not null with true as a default value");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(65)")
                    .HasComment("password has been set to not null")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasComment("from enum of roles:\\\\\\\\\\\\\\\\n            1 - student\\\\\\\\\\\\\\\\n            2 - mentor\\\\\\\\\\\\\\\\n            4 - admin");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt")
                    .HasColumnType("varchar(65)")
                    .HasComment("salt has been set to not null")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("course");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(100)")
                    .HasComment("name has been set to not null and unique")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("lesson");

                entity.HasIndex(e => e.MentorId)
                    .HasName("FK_mentor_of_lesson");

                entity.HasIndex(e => e.StudentGroupId)
                    .HasName("FK_student_group_of_lesson");

                entity.HasIndex(e => e.ThemeId)
                    .HasName("FK_ThemeOfLesson_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LessonDate)
                    .HasColumnName("lesson_date")
                    .HasColumnType("datetime")
                    .HasComment("lesson_date has been set to not null");

                entity.Property(e => e.MentorId).HasColumnName("mentor_id");

                entity.Property(e => e.StudentGroupId).HasColumnName("student_group_id");

                entity.Property(e => e.ThemeId).HasColumnName("theme_id");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.Lesson)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_mentor_of_lesson");

                entity.HasOne(d => d.StudentGroup)
                    .WithMany(p => p.Lesson)
                    .HasForeignKey(d => d.StudentGroupId)
                    .HasConstraintName("FK_student_group_of_lesson");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.ThemeId)
                    .HasConstraintName("FK_theme_of_lesson");
            });

            modelBuilder.Entity<Mentor>(entity =>
            {
                entity.ToTable("mentor");

                entity.HasIndex(e => e.AccountId)
                    .HasName("FK_account_of_mentor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Mentors)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_account_of_mentor");
            });

            modelBuilder.Entity<MentorOfCourse>(entity =>
            {
                entity.ToTable("mentor_of_course");

                entity.HasIndex(e => e.MentorId)
                    .HasName("FK_mentorId");

                entity.HasIndex(e => new { e.CourseId, e.MentorId })
                    .HasName("mentorAndCourseIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.MentorId).HasColumnName("mentor_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.MentorsOfCourses)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_course_of_mentor");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.MentorsOfCourses)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_mentor_of_course");
            });

            modelBuilder.Entity<MentorOfStudentGroup>(entity =>
            {
                entity.ToTable("mentor_of_student_group");

                entity.HasIndex(e => e.StudentGroupId)
                    .HasName("FK__idx");

                entity.HasIndex(e => new { e.MentorId, e.StudentGroupId })
                    .HasName("mentorAndStudentGroupIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MentorId).HasColumnName("mentor_id");

                entity.Property(e => e.StudentGroupId).HasColumnName("student_group_id");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.MentorsOfStudentGroups)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK_mentor_of_student_group");

                entity.HasOne(d => d.StudentGroup)
                    .WithMany(p => p.MentorsOfStudentGroups)
                    .HasForeignKey(d => d.StudentGroupId)
                    .HasConstraintName("FK_student_group_of_mentor");
            });

            modelBuilder.Entity<Secretary>(entity =>
            {
                entity.ToTable("secretary");

                entity.HasIndex(e => e.AccountId)
                    .HasName("FK_account_of_secretary");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Secretaries)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_account_of_secretary");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("student");

                entity.HasIndex(e => e.AccountId)
                    .HasName("FK_account_of_student");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_account_of_student");
            });

            modelBuilder.Entity<StudentGroup>(entity =>
            {
                entity.ToTable("student_group");

                entity.HasIndex(e => e.CourseId)
                    .HasName("FK_course_of_student_group");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.FinishDate)
                    .HasColumnName("finish_date")
                    .HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(100)")
                    .HasComment("name has been set to not null and unique")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.StudentGroup)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_course_of_student_group");
            });

            modelBuilder.Entity<StudentOfStudentGroup>(entity =>
            {
                entity.ToTable("student_of_student_group");

                entity.HasIndex(e => e.StudentGroupId)
                    .HasName("FK__idx");

                entity.HasIndex(e => e.StudentId)
                    .HasName("FK_student_of_student_group_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StudentGroupId).HasColumnName("student_group_id");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.HasOne(d => d.StudentGroup)
                    .WithMany(p => p.StudentsOfStudentGroups)
                    .HasForeignKey(d => d.StudentGroupId)
                    .HasConstraintName("FK_student_group_of_student");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentsOfStudentGroups)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_student_of_student_group");
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.ToTable("theme");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(100)")
                    .HasComment("name has been set to not null and unique")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.ToTable("visit");

                entity.HasIndex(e => e.LessonId)
                    .HasName("FK_lesson_of_visit");

                entity.HasIndex(e => e.StudentId)
                    .HasName("FK_student_of_visit_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("varchar(1024)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LessonId).HasColumnName("lesson_id");

                entity.Property(e => e.Presence)
                    .HasColumnName("presence")
                    .HasComment("presence default value has been set");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.StudentMark).HasColumnName("student_mark");

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.Visits)
                    .HasForeignKey(d => d.LessonId)
                    .HasConstraintName("FK_lesson_of_visit");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Visits)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_student_of_visit");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedule");
                
                entity.HasIndex(e => e.StudentGroupId)
                    .HasName("FK_student_group_of_schedule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LessonStart).HasColumnName("lesson_start");

                entity.Property(e => e.LessonEnd).HasColumnName("lesson_end");

                entity.Property(e => e.StudentGroupId).HasColumnName("student_group_id");

                entity.Property(e => e.RepeatRate).HasColumnName("repeat_rate");

                entity.Property(e => e.DayNumber).HasColumnName("day_number");

                entity.HasOne(d => d.StudentGroup)
                    .WithMany(p => p.Schedule)
                    .HasForeignKey(d => d.StudentGroupId)
                    .HasConstraintName("FK_student_group_of_schedule");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
