using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class SecretaryEntityConfiguration : IEntityTypeConfiguration<Secretary>
    {
        public void Configure(EntityTypeBuilder<Secretary> entity)
        {
            entity.ToTable("secretary");

            entity.HasIndex(e => e.AccountId)
                .HasName("FK_account_of_secretary");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.AccountId).HasColumnName("account_id");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Secretaries)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_account_of_secretary");
        }
    }
}
