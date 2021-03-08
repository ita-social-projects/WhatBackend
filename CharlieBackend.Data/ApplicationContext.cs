using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Interfaces;
using CharlieBackend.Data.Configurations;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CharlieBackend.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }

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
        
        public virtual DbSet<EventOccurrence> EventOccurrences { get; set; }

        public virtual DbSet<ScheduledEvent> ScheduledEvents { get; set; }

        public virtual DbSet<Attachment> Attachments { get; set; }

        public virtual DbSet<Homework> Homeworks { get; set; }

        public virtual DbSet<AttachmentOfHomework> AttachmentsOfHomework { get; set; }

        public virtual DbSet<HomeworkStudent> HomeworkStudents { get; set; }

        public virtual DbSet<AttachmentOfHomeworkStudent> AttachmentOfHomeworkStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountEntityConfiguration())
                .ApplyConfiguration(new AttachmentEntityConfiguration())
                .ApplyConfiguration(new AttachmentOfHomeworkEntityConfiguration())
                .ApplyConfiguration(new AttachmentOfHomeworkStudentEntityConfiguration())
                .ApplyConfiguration(new CourseEntityConfiguration())
                .ApplyConfiguration(new EventOccurenceEntityConfiguration())
                .ApplyConfiguration(new HomeworkEntityConfiguration())
                .ApplyConfiguration(new HomeworkStudentEntityConfiguration())
                .ApplyConfiguration(new LessonEntityConfiguration())
                .ApplyConfiguration(new MentorEntityConfiguration())
                .ApplyConfiguration(new MentorOfCourseEntityConfiguration())
                .ApplyConfiguration(new MentorOfStudentGroupEntityConfiguration())
                .ApplyConfiguration(new ScheduledEventEntityConfiguration())
                .ApplyConfiguration(new SecretaryEntityConfiguration())
                .ApplyConfiguration(new StudentEntityConfiguration())
                .ApplyConfiguration(new StudentGroupEntityConfiguration())
                .ApplyConfiguration(new StudentOfStudentGroupEntityConfiguration())
                .ApplyConfiguration(new ThemeEntityConfiguration())
                .ApplyConfiguration(new VisitEntityConfiguration());

            modelBuilder.Entity<ScheduledEvent>().Property<bool>("IsDeleted");
            modelBuilder.Entity<ScheduledEvent>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is ISoftDeletingModel).ToList();

            if (entries.Any())
            {
                UpdateSoftDeletingStatuses(entries);
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeletingStatuses(List<EntityEntry> entities)
        {
            foreach(var entry in entities)
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
        }
    }
}
