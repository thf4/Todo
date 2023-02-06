namespace Todo.Shared;
public static class Configuration
{
  public static string CompanyEmail { get; set; } = String.Empty;
  public static string JwtKey { get; set; } = String.Empty;
  public static string ApiKey { get; set; } = String.Empty;
  public static string ApiKeyName { get; set; } = String.Empty;
  public static SmtpConfiguration Smtp = new();
  public class SmtpConfiguration
  {
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
  }
}
