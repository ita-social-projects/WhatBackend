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
            entity.ToTable("MentorsOfCourses");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.MentorId)
                .IsRequired()
                .HasColumnName("MentorID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.Property(e => e.CourseId)
                .IsRequired()
                .HasColumnName("CourseID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.Mentor)
                .WithMany(p => p.MentorsOfCourses)
                .HasForeignKey(d => d.MentorId)
                .HasConstraintName("FK_MentorOfCourses");

            entity.HasOne(d => d.Course)
                .WithMany(p => p.MentorsOfCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_CourseOfMentors");

            entity.HasIndex(e => new { e.CourseId, e.MentorId })
                .HasName("UQ_MentorAndCourse")
                .IsUnique();
        }
    }
}
