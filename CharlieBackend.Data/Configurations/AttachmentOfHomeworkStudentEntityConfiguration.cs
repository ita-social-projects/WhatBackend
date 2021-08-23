using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class AttachmentOfHomeworkStudentEntityConfiguration
        : IEntityTypeConfiguration<AttachmentOfHomeworkStudent>
    {
        public void Configure(EntityTypeBuilder<AttachmentOfHomeworkStudent> entity)
        {
            entity.ToTable("AttachmentsOfHomeworksFromStudents");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.AttachmentId)
                .IsRequired()
                .HasColumnName("AttachmentID");

            entity.Property(e => e.HomeworkStudentId)
                .IsRequired()
                .HasColumnName("HomeworkFromStudentID");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.AttachmentOfHomeworkStudents)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_AttachmentOfHomeworksFromStudents");

            entity.HasOne(d => d.HomeworkStudent)
                .WithMany(p => p.AttachmentOfHomeworkStudents)
                .HasForeignKey(d => d.HomeworkStudentId)
                .HasConstraintName("FK_HomeworkFromStudentOfAttachments");

            entity.HasIndex(e => new { e.AttachmentId, e.HomeworkStudentId })
                .HasName("UQ_HomeworkFromStudentAndAttachment")
                .IsUnique();

            entity.HasIndex(e => e.HomeworkStudentId)
                .HasName("IX_HomeworkFromStudent");

            entity.HasIndex(e => e.AttachmentId)
                .HasName("IX_Attachment");
        }
    }
}
