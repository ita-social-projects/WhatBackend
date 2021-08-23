using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class StudentGroupEntityConfiguration : IEntityTypeConfiguration<StudentGroup>
    {
        public void Configure(EntityTypeBuilder<StudentGroup> entity)
        {
            entity.ToTable("StudentGroups");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.CourseId)
                .IsRequired()
                .HasColumnName("CourseID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)");

            entity.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnName("StartDate")
                .HasColumnType("DATE");

            entity.Property(e => e.FinishDate)
                .IsRequired()
                .HasColumnName("FinishDate")
                .HasColumnType("DATE");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.Course)
                .WithMany(p => p.StudentGroup)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_CourseStudentGroups");

            entity.HasIndex(e => e.Name)
                .HasName("UQ_NameStudentGroups")
                .IsUnique();
        }
    }
}
