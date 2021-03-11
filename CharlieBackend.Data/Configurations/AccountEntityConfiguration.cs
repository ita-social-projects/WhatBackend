using CharlieBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CharlieBackend.Data.Configurations
{
    class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> entity)
        {
            entity.ToTable("account");

            entity.HasIndex(e => e.Email)
                .HasName("email_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasColumnType("varchar(50)")
                .HasComment("email has been set to not null and unique")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("varchar(30)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasColumnName("is_active")
                .HasDefaultValueSql("'1'")
                .HasComment("is_active has been set to not null with true as a default value");

            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasColumnType("varchar(30)")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnName("password")
                .HasColumnType("varchar(65)")
                .HasComment("password has been set to not null")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.Role)
                .HasColumnName("role")
                .HasComment("from enum of roles:\\\\\\\\\\\\\\\\n            1 - student\\\\\\\\\\\\\\\\n            2 - mentor\\\\\\\\\\\\\\\\n            4 - admin");

            entity.Property(e => e.Salt)
                .IsRequired()
                .HasColumnName("salt")
                .HasColumnType("varchar(65)")
                .HasComment("salt has been set to not null")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.ForgotPasswordToken)
                .HasColumnName("forgot_password_token")
                .HasColumnType("varchar(100)")
                .HasComment("token for resetting password")
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.ForgotTokenGenDate)
                .HasColumnName("forgot_token_gen_date")
                .HasColumnType("datetime")
                .HasComment("date of generation for users forgot password token");

            entity.Property(e => e.AvatarId).HasColumnName("avatar_id");

            entity.HasOne(x => x.Avatar)
                .WithOne(x => x.Account)
                .HasForeignKey<Account>(x => x.AvatarId);
        }
    }
}
