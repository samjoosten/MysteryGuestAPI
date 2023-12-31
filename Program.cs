using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MysteryGuestAPI.Contexts;
using MysteryGuestAPI.DbContext;
using MysteryGuestAPI.Handlers.Authentication;
using MysteryGuestAPI.Handlers.Company;
using MysteryGuestAPI.Handlers.User;
using MysteryGuestAPI.Repositories;
using MysteryGuestAPI.Services.Implementations;
using MysteryGuestAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseNpgsql("host=ep-gentle-recipe-75487514.eu-central-1.aws.neon.tech; database=neondb; search path=neondb; port=5432; user id=sam.joosten90; password=uvKc8XowqGx0;")
        .UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole())));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!))
    };
});

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


// Authentication
app.MapPost("/register", RegisterHandler.Register);
app.MapPost("/login", LoginHandler.Login);
app.MapPost("/token/refresh", RefreshAccessTokenHandler.RefreshAccessToken);

// Users
app.MapPost("/users/invite", InviteUserHandler.InviteUser).RequireAuthorization();
app.MapGet("/users/invites/{token}", GetInviteByTokenHandler.GetInvite);
app.MapGet("/users/me", GetUserHandler.GetUser).RequireAuthorization();

// Companies
app.MapGet("/companies", GetCompaniesHandler.GetCompanies);
app.Run();