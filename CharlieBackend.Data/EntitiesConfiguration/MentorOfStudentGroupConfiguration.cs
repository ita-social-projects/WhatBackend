using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.EntitiesConfiguration
{
    public class MentorOfStudentGroupConfiguration : IEntityTypeConfiguration<MentorOfStudentGroup>
    {
        public void Configure(EntityTypeBuilder<MentorOfStudentGroup> builder)
        {
            builder.ToTable("mentorsofstudentgroups");

            builder.HasIndex(e => e.IdStudentGroup)
                .HasName("FK_GroupsOf");

            builder.HasIndex(e => new { e.IdMentor, e.IdStudentGroup })
                .HasName("AK_UniqueStudentGroupAndMentor")
                .IsUnique();

            builder.Property(e => e.Comments)
                .HasColumnType("varchar(1024)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            builder.HasOne(d => d.IdMentorNavigation)
                .WithMany(p => p.MentorsOfStudentGroups)
                .HasForeignKey(d => d.IdMentor)
                .HasConstraintName("FK_MentorsOf");

            builder.HasOne(d => d.IdStudentGroupNavigation)
                .WithMany(p => p.MentorsOfStudentGroups)
                .HasForeignKey(d => d.IdStudentGroup)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GroupsOf");
        }
    }
}
