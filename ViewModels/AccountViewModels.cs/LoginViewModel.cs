using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels
{
  public class LoginViewModel
  {
    [Required(ErrorMessage = "O E-mail é obrigatório")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Password { get; set; }
  }
}