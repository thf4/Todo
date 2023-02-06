namespace Todo.Models
{
  public class TodoModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public bool Done { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IList<User> Users { get; set; }
  }
}