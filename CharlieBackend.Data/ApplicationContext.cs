using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace CharlieBackend.Data
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{}

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

		public virtual DbSet<Mark> Marks { get; set; }

		public virtual DbSet<AttachmentOfHomework> AttachmentsOfHomework { get; set; }

		public virtual DbSet<HomeworkStudent> HomeworkStudents { get; set; }

		public virtual DbSet<AttachmentOfHomeworkStudent> AttachmentOfHomeworkStudents { get; set; }

		public virtual DbSet<HomeworkStudentHistory> HomeworkStudentsHistory { get; set; }

		public virtual DbSet<AttachmentOfHomeworkStudentHistory> AttachmentOfHomeworkStudentsHistory { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	
		}

	}
}
