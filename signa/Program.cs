using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using signa.DataAccess;
using signa.Interfaces;
using signa.Models;
using signa.Repositories;
using signa.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentsService, TournamentsService>();

builder.Services.AddControllers();

MappingConfig.RegisterMappings();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
        .EnableSensitiveDataLogging()
        .UseLoggerFactory(LoggerFactory.Create(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        }));
});

var app = builder.Build();


using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
context.Database.Migrate();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
