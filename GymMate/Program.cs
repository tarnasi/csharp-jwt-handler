using GymMate.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("AppDb");
});

builder.Services.AddControllers();

var jwtSection = builder.Configuration.GetSection("Authentication:Schemes:Bearer");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["ValidIssuer"],
            ValidAudiences = jwtSection.GetSection("ValidAudiences").Get<List<string>>(),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])) // Replace with your key
        };
    });

var app = builder.Build();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.Run();
