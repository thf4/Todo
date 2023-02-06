using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels
{
  public class PasswordUpdateViewModel
  {
    [Required]
    public string PasswordHash { get; set; }
  }
}