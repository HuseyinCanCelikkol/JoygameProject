using FluentValidation.AspNetCore;
using FluentValidation;
using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.Features.Commands.Product.Add;
using JoygameProject.Application.Mappings;
using JoygameProject.Application.Validators.Login;
using JoygameProject.Infrastructure.Concretes.Services;
using JoygameProject.Persistence.Concretes.UnitOfWorks;
using JoygameProject.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<JoygameProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ProductAddCommandHandler).Assembly));
builder.Services.AddAutoMapper(typeof(ProductsProfile).Assembly);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = "JoyJWT";     // doğrulama
    options.DefaultChallengeScheme = "JoyJWT"; 
    })
    .AddJwtBearer("JoyJWT", options =>
    {
       
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateLifetime = true,
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrWhiteSpace(token))
                    context.Token = token;

                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.Response.Redirect("/Auth/Login");
                context.HandleResponse(); // önemli, yoksa 401 fırlatır
                return Task.CompletedTask;
            }
        };
    });

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // her şeyi al
    .WriteTo.File("Logs/info-WEB.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, rollingInterval: RollingInterval.Day)
    .WriteTo.File("Logs/error-WEB.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error, rollingInterval: RollingInterval.Day)
    .WriteTo.File("Logs/warning-WEB.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
    .WriteTo.File("Logs/fatal-WEB.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>(); // tüm validatörleri bul
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "Joygame_"; // Key prefix
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == 403)
    {
        response.Redirect("/Forbidden");
    }

    return Task.CompletedTask;
});
app.Run();
