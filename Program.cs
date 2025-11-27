using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore ;
using Microsoft.Extensions.FileProviders;
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

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();

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

    });
// Db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Repos & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped< UserRepository>();

builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
builder.Services.AddScoped<IComplaintService, ComplaintService>();
builder.Services.AddScoped<IGovernmentAgencyRepository, GovernmentAgencyRepository>();
builder.Services.AddScoped<IGovernmentAgencyService, GovernmentAgencyService>();
builder.Services.AddScoped<IComplaintStatusRepository, ComplaintStatusRepository>();
builder.Services.AddScoped<IComplaintStatusService, ComplaintStatusService>();
builder.Services.AddScoped<IGovermentEmployeeRepositry, GovermentEmployeeRepository>();
builder.Services.AddScoped<IGovermentEmployeeService, GovermentEmployeeService>();
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

builder.Services.Configure<SmtpSettings>(
 builder.Configuration.GetSection("SmtpSettings"));



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

});
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true);  
        });
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAll");

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
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ComplaintHub>("/complainthub");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});
using (var scope = app.Services.CreateScope())
{
    var useRepo = scope.ServiceProvider.GetRequiredService<UserRepository>();
    await useRepo.SeedAdminAsync();
}
    app.Run();
