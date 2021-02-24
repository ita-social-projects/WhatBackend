using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class LessonEntityConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> entity)
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
        }
    }
}
