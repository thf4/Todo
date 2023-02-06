using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Todo.Shared.Attributes
{
  [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
  public class TokenAttribute : Attribute, IAsyncActionFilter
  {
    public async Task OnActionExecutionAsync(
      ActionExecutingContext context,
      ActionExecutionDelegate next)
    {
      if (!context.HttpContext.Request.Headers.TryGetValue("x-app-token", out var extractedAppToken))
      {
        context.Result = new ContentResult()
        {
          StatusCode = 401,
          Content = "x-app-token não encontrada!"
        };
        return;
      }

      var token = new JwtSecurityTokenHandler();
      var validToken = token.CanReadToken(extractedAppToken);
      var user = token.ReadJwtToken(extractedAppToken).Payload["unique_name"];
      var email = token.ReadJwtToken(extractedAppToken).Payload["email"];
      
      if (!validToken)
      {
        context.Result = new ContentResult()
        {
          StatusCode = 403,
          Content = "Token is not valid!"
        };
        return;
      }
      await next();
    }

  }
}