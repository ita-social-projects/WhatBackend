using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class AttachmentOfHomeworkStudentHistoryEntityConfiguration
         : IEntityTypeConfiguration<AttachmentOfHomeworkStudentHistory>
    {
        public void Configure(EntityTypeBuilder<AttachmentOfHomeworkStudentHistory> entity)
        {
            entity.ToTable("AttachmentsOfHomeworksFromStudentsHistory");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.HomeworkStudentHistoryId)
                .IsRequired()
                .HasColumnName("HomeworkFromStudentHistoryID");

            entity.Property(e => e.AttachmentId)
                .IsRequired()
                .HasColumnName("AttachmentID");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.AttachmentOfHomeworkStudentsHistory)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_AttachmentOfHomeworksFromStudentsHistory");

            entity.HasOne(d => d.HomeworkStudentHistory)
                .WithMany(p => p.AttachmentOfHomeworkStudentsHistory)
                .HasForeignKey(d => d.HomeworkStudentHistoryId)
                .HasConstraintName("FK_HomeworkFromStudentHistoryOfAttachments");

            entity.HasIndex(e => new { e.AttachmentId, e.HomeworkStudentHistoryId })
                .HasName("UQ_HomeworkFromStudentAndAttachment")
                .IsUnique();

            entity.HasIndex(e => e.HomeworkStudentHistoryId)
                .HasName("IX_HomeworkFromStudentHistory");

            entity.HasIndex(e => e.AttachmentId)
                .HasName("IX_AttachmentHistory");
        }
    }
}
