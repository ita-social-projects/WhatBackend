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
            entity.ToTable("mentor_of_student_group");

            entity.HasIndex(e => e.StudentGroupId)
                .HasName("FK__idx");

            entity.HasIndex(e => new { e.MentorId, e.StudentGroupId })
                .HasName("mentorAndStudentGroupIndex")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.MentorId).HasColumnName("mentor_id");

            entity.Property(e => e.StudentGroupId).HasColumnName("student_group_id");

            entity.HasOne(d => d.Mentor)
                .WithMany(p => p.MentorsOfStudentGroups)
                .HasForeignKey(d => d.MentorId)
                .HasConstraintName("FK_mentor_of_student_group");

            entity.HasOne(d => d.StudentGroup)
                .WithMany(p => p.MentorsOfStudentGroups)
                .HasForeignKey(d => d.StudentGroupId)
                .HasConstraintName("FK_student_group_of_mentor");
        }
    }
}
