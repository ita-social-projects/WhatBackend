using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> entity)
        {
            entity.ToTable("Courses");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasColumnName("IsActive")
                .HasColumnType("BIT")
                .HasDefaultValueSql("1");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasIndex(e => e.Name)
                .HasName("UQ_NameCourses")
                .IsUnique();
        }
    }
}
