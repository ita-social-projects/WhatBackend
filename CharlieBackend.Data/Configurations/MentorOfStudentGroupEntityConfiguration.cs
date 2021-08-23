using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class MentorOfStudentGroupEntityConfiguration
        : IEntityTypeConfiguration<MentorOfStudentGroup>
    {
        public void Configure(EntityTypeBuilder<MentorOfStudentGroup> entity)
        {
            entity.ToTable("MentorsOfStudentGroups");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.StudentGroupId)
                .IsRequired()
                .HasColumnName("StudentGroupID");

            entity.Property(e => e.MentorId)
                .IsRequired()
                .HasColumnName("MentorID");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.Mentor)
                .WithMany(p => p.MentorsOfStudentGroups)
                .HasForeignKey(d => d.MentorId)
                .HasConstraintName("FK_MentorOfStudentGroups");

            entity.HasOne(d => d.StudentGroup)
                .WithMany(p => p.MentorsOfStudentGroups)
                .HasForeignKey(d => d.StudentGroupId)
                .HasConstraintName("FK_StudentGroupOfMentors");

            entity.HasIndex(e => new { e.MentorId, e.StudentGroupId })
                .HasName("UQ_MentorAndStudentGroup")
                .IsUnique();
        }
    }
}
