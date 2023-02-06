namespace Todo.Models
{
  public class User
  {

    public int Id { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public string Slug { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IList<Role> Roles { get; set; }
    public IList<TodoModel> Todos { get; set; }
  }
}