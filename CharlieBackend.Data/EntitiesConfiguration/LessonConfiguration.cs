using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.EntitiesConfiguration
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("lessons");

            builder.HasIndex(e => e.IdMentor)
                .HasName("FK_LessonsMentors");

            builder.HasIndex(e => e.IdStudentGroup)
                .HasName("FK_LessonsOfGroup");

            builder.HasIndex(e => e.IdTheme)
                .HasName("FK_ThemeOfLesson");

            builder.Property(e => e.LessonDate).HasColumnType("datetime");

            builder.HasOne(d => d.IdMentorNavigation)
                .WithMany(p => p.Lessons)
                .HasForeignKey(d => d.IdMentor)
                .HasConstraintName("FK_LessonsMentors");

            builder.HasOne(d => d.IdStudentGroupNavigation)
                .WithMany(p => p.Lessons)
                .HasForeignKey(d => d.IdStudentGroup)
                .HasConstraintName("FK_LessonsOfGroup");

            builder.HasOne(d => d.IdThemeNavigation)
                .WithMany(p => p.Lessons)
                .HasForeignKey(d => d.IdTheme)
                .HasConstraintName("FK_ThemeOfLesson");
        }
    }
}
