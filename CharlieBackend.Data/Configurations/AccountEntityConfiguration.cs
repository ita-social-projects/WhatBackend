using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> entity)
        {
            entity.ToTable("Accounts");

            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("ID");

            entity.Property(e => e.Role)
                .HasColumnName("Role")
                .HasComment(
                "Roles:" +
                "\n 0 - NotAssigned," +
                "\n 1 - Student," +
                "\n 2 - Mentor," +
                "\n 4 - Admin," +
                "\n 8 - Secretary"
                );

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasColumnName("FirstName")
                .HasColumnType("VARCHAR(30)");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasColumnName("LastName")
                .HasColumnType("VARCHAR(30)");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType("VARCHAR(50)");

            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnName("PasswordHash")
                .HasColumnType("VARCHAR(64)")
                .HasComment("SHA265 output size is 256 bits or 64 characters");

            entity.Property(e => e.Salt)
                .IsRequired()
                .HasColumnName("Salt")
                .HasColumnType("VARCHAR(32)")
                .HasComment("Standard salt size is 128 bits or 32 characters");

            entity.Property(e => e.LastEditorID)
                .IsRequired()
                .HasColumnName("LastEditorID")
                .HasDefaultValueSql("1");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasColumnName("IsActive")
                .HasColumnType("BIT")
                .HasDefaultValueSql("1");

            entity.Property(e => e.ForgotPasswordToken)
                .HasColumnName("ForgotPasswordToken")
                .HasColumnType("VARCHAR(36)")
                .HasComment("GUID length is 36 characters");

            entity.Property(e => e.ForgotTokenGenDate)
                .HasColumnName("ForgotTokenGenDate")
                .HasColumnType("DATETIME")
                .HasComment("Use UTC time");

            entity.Property(e => e.AvatarId)
                .HasColumnName("AvatarID");

            entity.HasKey(e => e.Id)
                .HasName("PRIMARY");

            entity.HasOne(x => x.Avatar)
                .WithOne(x => x.Account)
                .HasForeignKey<Account>(x => x.AvatarId)
                .HasConstraintName("FK_AvatarAccounts");

            entity.HasIndex(e => e.Email)
                .HasName("UQ_EmailAccounts")
                .IsUnique();

            entity.HasIndex(e => e.AvatarId)
                .HasName("UQ_AvatarAccounts")
                .IsUnique();
        }
    }
}
