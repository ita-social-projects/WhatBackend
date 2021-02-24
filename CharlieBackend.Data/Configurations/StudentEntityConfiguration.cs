using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> entity)
        {
            entity.ToTable("student");

            entity.HasIndex(e => e.AccountId)
                .HasName("FK_account_of_student");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.AccountId).HasColumnName("account_id");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_account_of_student");
        }
    }
}
