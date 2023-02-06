using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels
{
  public class UserViewModel
  {
    [Required]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "O E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O E-mail é inválido")]
    public string Email { get; set; }
  }
}