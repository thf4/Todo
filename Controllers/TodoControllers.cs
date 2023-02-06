using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Shared.Data;
using Todo.Shared.Extensions;
using Todo.Models;
using Todo.ViewModels;

namespace Todo.Controllers
{
  [ApiController]
  public class TodoManageControlller : ControllerBase
  {
    [HttpGet("/home")]
    public IActionResult Get(
      [FromServices] TodoDbContext context)
    {
      try {
      var todos = context.Todo.ToList();
      return Ok(new ResultViewModel<List<TodoModel>>(todos));
      } catch (Exception err)
      {
        return StatusCode(500, new ResultViewModel<List<TodoModel>>("Falha ao fazer requisição!"));
      }
    }

    [HttpPost("/")]
    public IActionResult Post(
      [FromServices] TodoDbContext context,
      [FromBody] CreateTodoViewModel model
      )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<TodoModel>(ModelState.GetErros()));
      
      try
      {
        var todos = new TodoModel
        {
          Title = model.Title,
          Text = model.Text,
          Done = model.Done
        };

        context.Todo.Add(todos);
        context.SaveChanges();

        return Ok(todos);
      }
      catch (Exception error)
      {
        throw error;
      }
    }

    [HttpGet("/{id:int}")]
    public IActionResult GetById(
      [FromServices] TodoDbContext context,
      [FromBody] TodoModel todo,
      [FromRoute] int id
      )
    {
      try
      {
        var model = context.Todo.FirstOrDefault(x => x.Id == id);

        if (model == null)
          return NotFound(new ResultViewModel<TodoModel>("Conteúdo não encontrado!"));

        context.SaveChanges();
        return Ok(model);
      }
      catch (Exception error)
      {
        throw error;
      }
    }

    [HttpPut("/{id:int}")]
    public IActionResult Put(
      [FromServices] TodoDbContext context,
      [FromBody] TodoModel todo,
      [FromRoute] int id
      )
    {
      try
      {
        var model = context.Todo.FirstOrDefault(x => x.Id == id);

        if (model == null)
          return NotFound();

        model.Title = todo.Title;
        model.Done = todo.Done;
        model.UpdatedAt = DateTime.Now;

        context.Todo.Update(model);
        context.SaveChanges();

        return Ok(model);
      }
      catch (Exception error)
      {
        throw error;
      }
    }

    [HttpDelete("/{id:int}")]
    [Authorize]
    public IActionResult Delete(
      [FromServices] TodoDbContext context,
      [FromBody] TodoModel todo,
      [FromRoute] int id
      )
    {
      try
      {
        var model = context.Todo.FirstOrDefault(x => x.Id == id);

        if (model == null)
          return NotFound();

        context.Todo.Remove(model);
        context.SaveChanges();
        return NoContent();
      }
      catch (Exception error)
      {
        throw error;
      }
    }
  }
}