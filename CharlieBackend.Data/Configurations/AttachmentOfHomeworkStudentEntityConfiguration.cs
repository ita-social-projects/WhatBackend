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
            entity.ToTable("attachment_of_homework_student");

            entity.HasIndex(e => e.AttachmentId)
                .HasName("FK_attachment_of_homework_student_id");

            entity.HasIndex(e => e.HomeworkStudentId)
                .HasName("FK_homework_student_of_attachment_id");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.AttachmentId).HasColumnName("attachment_id");

            entity.Property(e => e.HomeworkStudentId).HasColumnName("homework_student_id");

            entity.HasOne(d => d.Attachment)
                .WithMany(p => p.AttachmentOfHomeworkStudents)
                .HasForeignKey(d => d.AttachmentId)
                .HasConstraintName("FK_attachment_of_homework_student");

            entity.HasOne(d => d.HomeworkStudent)
                .WithMany(p => p.AttachmentOfHomeworkStudents)
                .HasForeignKey(d => d.HomeworkStudentId)
                .HasConstraintName("FK_homework_ student_of_attachment");
        }
    }
}
