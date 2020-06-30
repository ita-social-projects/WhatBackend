using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.EntitiesConfiguration
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.ToTable("visits");

            builder.HasIndex(e => e.IdLesson)
                .HasName("FK_VisitsLessons");

            builder.HasIndex(e => new { e.IdStudent, e.IdLesson })
                .HasName("AK_UniqueLessonAndStudent")
                .IsUnique();

            builder.Property(e => e.Comments)
                .HasColumnType("varchar(1024)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.HasOne(d => d.IdLessonNavigation)
                .WithMany(p => p.Visits)
                .HasForeignKey(d => d.IdLesson)
                .HasConstraintName("FK_VisitsLessons");

            builder.HasOne(d => d.IdStudentNavigation)
                .WithMany(p => p.Visits)
                .HasForeignKey(d => d.IdStudent)
                .HasConstraintName("FK_VisitOfStudent");
        }
    }
}
