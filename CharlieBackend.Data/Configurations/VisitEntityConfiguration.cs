using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class VisitEntityConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> entity)
        {
            entity.ToTable("Visits");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.StudentId)
                .IsRequired()
                .HasColumnName("StudentID");

            entity.Property(e => e.LessonId)
                .IsRequired()
                .HasColumnName("LessonID");

            entity.Property(e => e.StudentMark)
                .HasColumnName("StudentMark");

            entity.Property(e => e.Presence)
                .IsRequired()
                //.HasDefaultValueSql("1")
                .HasColumnName("Presence")
                .HasColumnType("BIT");

            entity.Property(e => e.Comment)
                .HasColumnName("Comment")
                .HasColumnType("VARCHAR(1024)");

            //entity.HasKey(e => e.Id)
            //    .HasName("PRIMARY");

            entity.HasOne(d => d.Lesson)
                .WithMany(p => p.Visits)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_LessonVisits");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.Visits)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_StudentVisits");

            entity.HasCheckConstraint(
                "CH_MarkVisits",
                "StudentMark >= 0 AND StudentMark <= 100");
        }
    }
}
