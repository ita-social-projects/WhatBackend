using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class MentorEntityConfiguration : IEntityTypeConfiguration<Mentor>
    {
        public void Configure(EntityTypeBuilder<Mentor> entity)
        {
            entity.ToTable("mentor");

            entity.HasIndex(e => e.AccountId)
                .HasName("FK_account_of_mentor");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.AccountId).HasColumnName("account_id");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Mentors)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_account_of_mentor");
        }
    }
}
