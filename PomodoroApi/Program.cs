using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PomodoroApi.Data;
using PomodoroApi.Services.Auth;
using PomodoroApi.Services.Tasks;
using PomodoroApi.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

{
    // cors
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy.WithOrigins(["http://localhost:5173", "https://localhost:3000", "https://pomodoroapp-three.vercel.app"])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
    });

    // identity
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>();

    // db
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

    var conStrBuilder = new MySqlConnectionStringBuilder(connectionString)
    {
        Server = builder.Configuration["DB_SERVER"] ?? "127.0.0.1",
        Password = builder.Configuration["DB_PASSWORD"],
        UserID = builder.Configuration["DB_USER"],
        Database = builder.Configuration["DB_NAME"]
    };

    connectionString = conStrBuilder.ConnectionString;

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });

    // services
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITaskService, TaskService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (bool.Parse(builder.Configuration["DB_MIGRATE_ON_STARTUP"] ?? "false"))
{
    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
