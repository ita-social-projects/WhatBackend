using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class LessonEntityConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> entity)
        {
            entity.ToTable("Lessons");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.MentorId)
                .IsRequired()
                .HasColumnName("MentorID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.Property(e => e.StudentGroupId)
                .IsRequired()
                .HasColumnName("StudentGroupID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.Property(e => e.ThemeId)
                .IsRequired()
                .HasColumnName("ThemeID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.Property(e => e.LessonDate)
                .IsRequired()
                .HasColumnName("LessonDate")
                .HasColumnType("DATETIME")
                .HasComment("Use UTC time");

            entity.HasOne(d => d.Mentor)
                .WithMany(p => p.Lesson)
                .HasForeignKey(d => d.MentorId)
                .HasConstraintName("FK_MentorLessons");

            entity.HasOne(d => d.StudentGroup)
                .WithMany(p => p.Lesson)
                .HasForeignKey(d => d.StudentGroupId)
                .HasConstraintName("FK_StudentGroupLessons");

            entity.HasOne(d => d.Theme)
                .WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ThemeId)
                .HasConstraintName("FK_ThemeLessons");
        }
    }
}
