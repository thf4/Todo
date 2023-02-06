using Microsoft.EntityFrameworkCore;
using Todo.Shared.Data.Mapping;
using Todo.Models;

namespace Todo.Shared.Data
{
  public class TodoDbContext : DbContext
  {
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
    public DbSet<TodoModel> Todo { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new TodoMap());
      modelBuilder.ApplyConfiguration(new UserMap());
    }
  }
}