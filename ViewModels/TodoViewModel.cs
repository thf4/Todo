using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels
{
  public class CreateTodoViewModel
  {
    [Required]
    [StringLength(80)]
    public string Title { get; set; }

    [Required]
    [StringLength(180)]
    public string Text { get; set; }

    [Required]
    public bool Done { get; set; }
  }
}