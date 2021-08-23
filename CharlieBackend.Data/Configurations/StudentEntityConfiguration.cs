using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> entity)
        {
            entity.ToTable("Students");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID")
                .HasColumnType("BIGINT UNSIGNED")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.AccountId)
                .IsRequired()
                .HasColumnName("AccountID")
                .HasColumnType("BIGINT UNSIGNED");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccountStudents");

            entity.HasIndex(e => e.AccountId)
                .IsUnique()
                .HasName("UQ_AccountStudents");
        }
    }
}
