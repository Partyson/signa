using EntityFrameworkCore.UnitOfWork.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using signa.DataAccess;
using signa.Interfaces;
using signa.Models;
using signa.Repositories;
using signa.Services;

namespace signa.FunctionalTests;

public class TestBase
{
    public IServiceProvider Container { get; }
    
    public TestBase()
    {
        var container = new ServiceCollection();
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
            .WriteTo.File($"logs/log_{DateTime.Now:yy-MM-dd-HH-mm}.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        container.AddScoped<IUserRepository, UserRepository>();
        container.AddScoped<IUsersService, UsersService>();
        container.AddScoped<ITournamentRepository, TournamentRepository>();
        container.AddScoped<ITournamentsService, TournamentsService>();
        container.AddScoped<ITeamRepository, TeamRepository>();
        container.AddScoped<ITeamsService, TeamsService>();
        container.AddScoped<IMatchRepository, MatchRepository>();
        container.AddScoped<IMatchesService, MatchesService>();
        container.AddScoped<IMatchTeamsService, MatchTeamsService>();
        container.AddScoped<IMatchTeamRepository, MatchTeamRepository>();

        container.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        MappingConfig.RegisterMappings();

        container.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql("server=localhost;Port=3306;user=dbuser;password=111;database=application_db;",
                    ServerVersion.AutoDetect("server=localhost;Port=3306;user=dbuser;password=111;database=application_db;"), 
                    optionsBuilder => optionsBuilder.EnableStringComparisonTranslations())
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(LoggerFactory.Create(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                }));
        });
        
        container.AddScoped<DbContext>(provider => provider.GetService<ApplicationDbContext>()!);
        container.AddUnitOfWork();
        container.AddUnitOfWork<ApplicationDbContext>();
        
        Container = container.BuildServiceProvider();
    }
}