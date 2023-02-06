using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Models;

namespace Todo.Shared.Data.Mapping
{
  public class UserMap : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.ToTable("User");

      builder.HasKey(user => user.Id);

      builder.Property(user => user.Id)
        .ValueGeneratedOnAdd()
        .UseIdentityColumn();

      builder.Property(user => user.Name)
        .IsRequired()
        .HasColumnName("Name")
        .HasColumnType("NVARCHAR")
        .HasMaxLength(80);

      builder.Property(user => user.Email)
        .IsRequired()
        .HasColumnName("Email")
        .HasColumnType("NVARCHAR")
        .HasMaxLength(160);

      builder.Property(user => user.Slug)
        .IsRequired()
        .HasColumnName("Slug")
        .HasColumnType("NVARCHAR")
        .HasMaxLength(160);

      builder.Property(user => user.PasswordHash)
        .IsRequired()
        .HasColumnName("PasswordHash")
        .HasColumnType("VARCHAR")
        .HasMaxLength(255);

      builder.Property(user => user.CreatedAt)
        .HasColumnName("CreatedAt")
        .HasDefaultValue(DateTime.Now);

      builder.Property(user => user.UpdatedAt)
        .HasColumnName("UpdatedAt")
        .HasDefaultValue(DateTime.Now);

      builder.HasIndex(user => user.Slug, "IX_User_Slug")
        .IsUnique();

      builder.HasMany(user => user.Roles)
        .WithMany(user => user.Users)
        .UsingEntity<Dictionary<string, object>>(
          "UserRole",
          role => role
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey("RoleId")
            .HasConstraintName("FK_UserRole_RoleId")
            .OnDelete(DeleteBehavior.Cascade),
            user => user
            .HasOne<User>()
            .WithMany()
            .HasForeignKey("UserId")
            .HasConstraintName("FK_UserRole_UserId")
            .OnDelete(DeleteBehavior.Cascade));

      builder.HasMany(user => user.Todos)
        .WithMany(user => user.Users)
        .UsingEntity<Dictionary<string, object>>(
          "UserTodo",
          todo => todo
            .HasOne<TodoModel>()
            .WithMany()
            .HasForeignKey("TodoId")
            .HasConstraintName("FK_UserTodo_TodoId")
            .OnDelete(DeleteBehavior.Cascade),
            user => user
            .HasOne<User>()
            .WithMany()
            .HasForeignKey("UserId")
            .HasConstraintName("FK_UserTodo_UserId")
            .OnDelete(DeleteBehavior.Cascade));
    }
  }
}