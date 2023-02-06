using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using Todo.Shared.Data;
using Todo.Models;
using Todo.Services;
using Todo.ViewModels;
using Todo.Shared.Extensions;
using Todo.Shared.Attributes;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;

namespace Todo.Controllers
{

  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly TokenService _tokenService;
    public AccountController(TokenService tokenService)
    {
      _tokenService = tokenService;
    }

    [ApiKeyAttribute]
    [HttpPost("v1/login")]
    public async Task<IActionResult> Login(
      [FromBody] LoginViewModel model,
      [FromServices] TodoDbContext context,
      [FromServices] TokenService tokenService
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

      var user = await context
        .Users
        .AsNoTracking()
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Email == model.Email);

      if (user == null)
        return StatusCode(401, new ResultViewModel<string>("Usuário não encontrado"));

      if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
        return StatusCode(401, new ResultViewModel<string>("Usuário ou senha invalidos"));

      try
      {
        var token = tokenService.GenerateToken(user);
        return Ok(new ResultViewModel<string>(token, null));
      }
      catch
      {
        return StatusCode(401, new ResultViewModel<string>("Falha no Servidor"));
      }
    }

    [ApiKeyAttribute]
    [HttpPost("v1/user")]
    public async Task<IActionResult> CreateLogin(
      [FromBody] UserViewModel model,
      [FromServices] TodoDbContext context,
      [FromServices] EmailService mail,
      [FromHeader] ApiKeyAttribute apikey
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

      var user = new User
      {
        Name = model.Name,
        Email = model.Email,
        Slug = model.Email.Replace("@", "-").Replace(".", "-")
      };

      var password = PasswordGenerator.Generate(25);
      user.PasswordHash = PasswordHasher.Hash(password);

      try
      {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        mail.Send(user.Name, user.Email, "Bem vindo ao blog!", $"Sua senha é {password}, Você poderá altera a sua senha a qualquer momento!");
        return Ok(new ResultViewModel<dynamic>(new
        {
          user = user.Email,
          password
        }));
      }
      catch (DbUpdateException)
      {
        return StatusCode(400, new ResultViewModel<string>(" Este e-mail já está cadastrado"));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("Internal Server Error"));
      }
    }

    [ApiKeyAttribute]
    [Authorize]
    [HttpPut("/v1/change-password/{id:int}")]
    public IActionResult Password(
      [FromBody] PasswordUpdateViewModel model,
      [FromRoute] int id,
      [FromServices] TodoDbContext context
    )
    {
      
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

      var userModel = context.Users.FirstOrDefault(user => user.Id == id);
      if (userModel == null)
        return StatusCode(400, new ResultViewModel<string>("User not found!"));

      var password = PasswordHasher.Hash(model.PasswordHash);

      userModel.PasswordHash = password;
      try
      {
        context.Users.Update(userModel);
        context.SaveChanges();

        return StatusCode(200, "User password has been updated!");
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("Internal Server Error"));
      }
    }
  }
}