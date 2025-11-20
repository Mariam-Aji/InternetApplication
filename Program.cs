using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
=======
>>>>>>> 0ee3cc3ba9dbb367e20882d6611ecb5855c87999
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore ;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Application.Interfaces;
using WebAPI.Application.Services;
using WebAPI.Application.Validation;
using WebAPI.Domain.Entities;
using WebAPI.Hubs;
using WebAPI.Infrastructure.Db;
using WebAPI.Infrastructure.Email;
using WebAPI.Infrastructure.Repositories;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// SignalR + UserId provider
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, WebAPI.NameIdentifierUserIdProvider>();
<<<<<<< HEAD
=======

>>>>>>> 0ee3cc3ba9dbb367e20882d6611ecb5855c87999
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
<<<<<<< HEAD
        fv.RegisterValidatorsFromAssemblyContaining<CreateComplaintRequestValidator>();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                                .Where(x => x.Value.Errors.Count > 0)
                                .SelectMany(x => x.Value.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToArray();

            var response = new
            {
                Status = 400,
                Message = string.Join(" | ", errors) 
            };

            return new BadRequestObjectResult(response);
        };
=======
>>>>>>> 0ee3cc3ba9dbb367e20882d6611ecb5855c87999
    });
// Db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Repos & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped< UserRepository>();
<<<<<<< HEAD
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
builder.Services.AddScoped<IComplaintService, ComplaintService>();
builder.Services.AddScoped<IGovernmentAgencyRepository, GovernmentAgencyRepository>();
builder.Services.AddScoped<IGovernmentAgencyService, GovernmentAgencyService>();

builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();


builder.Services.Configure<SmtpSettings>(
 builder.Configuration.GetSection("SmtpSettings"));

=======
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

builder.Services.Configure<SmtpSettings>(
 builder.Configuration.GetSection("SmtpSettings"));


>>>>>>> 0ee3cc3ba9dbb367e20882d6611ecb5855c87999
// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
<<<<<<< HEAD
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json; charset=utf-8";
            return context.Response.WriteAsync("{\"message\": \"Unauthorized.\"}");
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json; charset=utf-8";
            return context.Response.WriteAsync("{\"message\": \"You do not have permission to access this resource.\"}");
        }
    };


=======
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken) &&
                context.HttpContext.Request.Path.StartsWithSegments("/lockoutHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
>>>>>>> 0ee3cc3ba9dbb367e20882d6611ecb5855c87999
});
builder.Services.AddSignalR();

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<LockoutHub>("/lockoutHub");

using (var scope = app.Services.CreateScope())
{
    var useRepo = scope.ServiceProvider.GetRequiredService<UserRepository>();
    await useRepo.SeedAdminAsync();
}
<<<<<<< HEAD
=======

>>>>>>> 0ee3cc3ba9dbb367e20882d6611ecb5855c87999
    app.Run();
