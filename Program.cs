using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Todo;
using Todo.Shared.Data;
using Todo.Shared;
using Todo.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);

var app = builder.Build();
LoadConfiguration(app);

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();

void LoadConfiguration(WebApplication app)
{
  Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
  Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");
  Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");

  var smtp = new Configuration.SmtpConfiguration();
  app.Configuration.GetSection("SmtpConfiguration").Bind(smtp);
  Configuration.Smtp = smtp;
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
  Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey");
  var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
  builder.Services.AddAuthentication(configureOptions: X =>
  {
    X.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    X.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  }).AddJwtBearer(x =>
  {
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(key),
      ValidateIssuer = false,
      ValidateAudience = false,
      ValidateLifetime = true,
      RequireExpirationTime = true,
    };
  });
}

void ConfigureMvc(WebApplicationBuilder builder)
{
  builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
  {
    options.SuppressModelStateInvalidFilter = true;
  });
}

void ConfigureServices(WebApplicationBuilder builder)
{
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  builder.Services.AddDbContext<TodoDbContext>(
    options =>
      options.UseSqlServer(connectionString));

  builder.Services.AddTransient<TokenService>();
  builder.Services.AddTransient<EmailService>();
}