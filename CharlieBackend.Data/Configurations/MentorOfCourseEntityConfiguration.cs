using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class MentorOfCourseEntityConfiguration 
        : IEntityTypeConfiguration<MentorOfCourse>
    {
        public void Configure(EntityTypeBuilder<MentorOfCourse> entity)
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
        }
    }
}
