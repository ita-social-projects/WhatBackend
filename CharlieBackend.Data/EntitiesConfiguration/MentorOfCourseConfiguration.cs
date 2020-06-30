using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.EntitiesConfiguration
{
    public class MentorOfCourseConfiguration : IEntityTypeConfiguration<MentorOfCourse>
    {
        public void Configure(EntityTypeBuilder<MentorOfCourse> builder)
        {
            builder.ToTable("mentorsofcourses");

            builder.HasIndex(e => e.IdMentor)
                .HasName("FK_OfMentors");

            builder.HasIndex(e => new { e.IdCourse, e.IdMentor })
                .HasName("AK_UniqueMentorAndCourse")
                .IsUnique();

            builder.Property(e => e.MentorComment)
                .HasColumnType("varchar(2048)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.HasOne(d => d.IdCourseNavigation)
                .WithMany(p => p.MentorsOfCourses)
                .HasForeignKey(d => d.IdCourse)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OfCourses");

            builder.HasOne(d => d.IdMentorNavigation)
                .WithMany(p => p.MentorsOfCourses)
                .HasForeignKey(d => d.IdMentor)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OfMentors");
        }
    }
}
