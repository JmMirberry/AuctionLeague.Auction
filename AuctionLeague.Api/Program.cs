using AuctionLeague.Data.Slack;
using AuctionLeague.Fpl;
using AuctionLeague.Handlers.SlackCommandHandlers;
using AuctionLeague.MongoDb;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.MongoDb.Repositories;
using AuctionLeague.SaleService;
using AuctionLeague.Service;
using AuctionLeague.Service.Auction;
using AuctionLeague.Service.Auction.Interfaces;
using AuctionLeague.Service.AuctionSetup;
using AuctionLeague.Service.AutoNomination;
using AuctionLeague.Service.DataStore;
using AuctionLeague.Service.Interfaces;
using AuctionLeague.SlackHandlers.SlackCommandHandlers;
using SlackAPI.Handlers;
using SlackAPI.Models;
using SlackNet.AspNetCore;

namespace AuctionLeague;

public class Program
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

        AddRepositories(builder.Services);
        AddServices(builder);

        builder.Services.AddSlackNet(c => c
            .UseApiToken(accessToken)
            //.RegisterEventHandler<MessageEvent, SlackMessageHandler>()
            .RegisterSlashCommandHandler<EchoDemo>(EchoDemo.SlashCommand)
            .RegisterSlashCommandHandler<BeginAuctionHandler>(BeginAuctionHandler.SlashCommand)
            .RegisterSlashCommandHandler<KillAuctionHandler>(KillAuctionHandler.SlashCommand)
            .RegisterSlashCommandHandler<NominateByIdHandler>(NominateByIdHandler.SlashCommand)
            .RegisterSlashCommandHandler<NominateByNameHandler>(NominateByNameHandler.SlashCommand)
            .RegisterSlashCommandHandler<BidHandler>(BidHandler.SlashCommand));

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }

    private static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient<FplClient>();
        builder.Services.AddSingleton<IFplClient, FplClient>();
        builder.Services.AddSingleton<IAuctionSetupService, AuctionSetupService>();
        builder.Services.AddSingleton<IFplService, FplService>();
        builder.Services.AddSingleton<IPlayerSaleService, PlayerSaleService>();
        builder.Services.AddSingleton<IAutoNominationService, AutoNominationService>();
        AddAuctionServices(builder);
    }

    private static void AddAuctionServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IAuctionNominationService, AuctionNominationService>();
        builder.Services.AddSingleton<IDataStore<SlackAuctionData>, DataStore<SlackAuctionData>>();
        builder.Services.AddSingleton<IAuctionTimer, AuctionTimer>();
        builder.Services.AddSingleton<ISlackAuctionManager, SlackAuctionManager>();
        builder.Services.AddSingleton<ISlackAuctionService, SlackAuctionService>();
        builder.Services.AddSingleton<ITimerEventHandler, SlackAuctionEventHandler>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddSingleton<IAuctionPlayerRepository, AuctionPlayerRepository>();
        services.AddSingleton<IManualPlayerRepository, ManualPlayerRepository>();
        services.AddSingleton<IFplPlayerRepository, FplPlayerRepository>();
        services.AddSingleton<IAuctionTeamRepository, AuctionTeamRepository>();
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