using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Models;

namespace Todo.Shared.Data.Mapping
{
  public class TodoMap : IEntityTypeConfiguration<TodoModel>
  {
    public void Configure(EntityTypeBuilder<TodoModel> builder)
    {
      builder.ToTable("Todo");

      builder.HasKey(todo => todo.Id);

      builder.Property(todo => todo.Id)
        .ValueGeneratedOnAdd()
        .UseIdentityColumn();

      builder.Property(todo => todo.Title)
        .IsRequired()
        .HasColumnName("Title")
        .HasColumnType("NVARCHAR")
        .HasMaxLength(80);

      builder.Property(todo => todo.Text)
        .IsRequired()
        .HasColumnName("Text")
        .HasColumnType("NVARCHAR")
        .HasMaxLength(160);

      builder.Property(todo => todo.Done)
        .IsRequired()
        .HasColumnName("Done")
        .HasConversion<int>()
        .HasColumnType("INTEGER");

      builder.Property(todo => todo.CreatedAt)
        .HasDefaultValue(DateTime.Now)
        .HasColumnName("CreatedAt");
    }
  }
}