using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class MentorEntityConfiguration : IEntityTypeConfiguration<Mentor>
    {
        public void Configure(EntityTypeBuilder<Mentor> entity)
        {
            entity.ToTable("Mentors");

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
                .WithMany(p => p.Mentors)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccountMentors");

            entity.HasIndex(e => e.AccountId)
                .IsUnique()
                .HasName("UQ_AccountMentors");
        }
    }
}
