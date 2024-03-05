using AuctionLeague.AuctionService;
using AuctionLeague.Fpl;
using SlackAPI.Handlers;
using SlackAPI.Models;
using SlackNet.AspNetCore;
using SlackNet.Events;
using AuctionLeague.MongoDb;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.MongoDb.Repositories;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var settings = builder.Configuration.GetSection("Slack").Get<Settings>();

        var accessToken = Environment.GetEnvironmentVariable("SlackAccessToken") ?? settings.SlackAccessToken;
        var signingSecret = Environment.GetEnvironmentVariable("SlackSigningSecret") ?? settings.SlackSigningSecret;


#if DEBUG
        builder.Services.AddSingleton(new SlackEndpointConfiguration());
#else
builder.Services.AddSingleton(new SlackEndpointConfiguration().UseSigningSecret(signingSecret));
#endif

        ConfigureSettings(builder);

        builder.Services.AddSingleton<IPlayerRepository,PlayerRepository>();
        builder.Services.AddHttpClient<FplClient>();
        builder.Services.AddSingleton<IFplClient, FplClient>();
        builder.Services.AddSingleton<IFplService, FplService>();

        builder.Services.AddSlackNet(c => c
            .UseApiToken(accessToken)
            .RegisterEventHandler<MessageEvent, PingHandler>());

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }

    private static void ConfigureSettings(WebApplicationBuilder builder)
    {
        builder.Services.Configure<MongoDbSettings>(
            builder.Configuration.GetSection("MongoDb"));
        builder.Services.Configure<AuctionSettings>(
            builder.Configuration.GetSection("Auction"));
        builder.Services.Configure<FplSettings>(
            builder.Configuration.GetSection("Fpl"));
    }
}

