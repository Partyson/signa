using Microsoft.EntityFrameworkCore;
using signa.DataAccess;
using signa.Interfaces;
using signa.Models;
using signa.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllers();

MappingConfig.RegisterMappings();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// todo перенести в appsetings.Deveploment.json
const string connectionString = "server=127.0.0.1;Port=3306;user=root;password=gr5+*4nW-8Zp_bS;database=application_db;";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

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
