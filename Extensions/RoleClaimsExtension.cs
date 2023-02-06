using System.Security.Claims;
using Todo.Models;

namespace Todo.Shared.Extensions
{
  public static class RoleClaimsExtension
  {
    public static IEnumerable<Claim> GetClaims(this User user)
    {
      var result = new List<Claim>
      {
        new (ClaimTypes.Name, user.Name),
        new (ClaimTypes.Email, user.Email),
      };
      result.AddRange(
        user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug))
      );
      return result;
    }
  }
}