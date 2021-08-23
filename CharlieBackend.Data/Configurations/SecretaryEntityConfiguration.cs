using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class SecretaryEntityConfiguration : IEntityTypeConfiguration<Secretary>
    {
        public void Configure(EntityTypeBuilder<Secretary> entity)
        {
            entity.ToTable("Secretaries");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.AccountId)
                .IsRequired()
                .HasColumnName("AccountID");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Secretaries)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccountSecretaries");

            entity.HasIndex(e => e.AccountId)
                .IsUnique()
                .HasName("UQ_AccountSecretaries");
        }
    }
}
