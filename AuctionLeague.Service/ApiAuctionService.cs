using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.Service.PlayerSale;
using FluentResults;
using Microsoft.Extensions.Options;
using System.Numerics;

namespace AuctionLeague.Service;

public class ApiAuctionService : AuctionServiceBase, IApiAuctionService
{

    public ApiAuctionService(IOptions<AuctionSettings> settings, IPlayerSaleService playerSaleService, IPlayerRepository playerRepository) : base(settings, playerSaleService, playerRepository)
    {
    }

    public override void ExecuteGoingOnce()
    {
        Console.WriteLine("Going once");
    }

    public override void ExecuteGoingTwice()
    {
        Console.WriteLine("Going twice");
    }

    public override void ExecuteSold(SoldPlayer soldPlayer)
    {
        _playerSaleService.ProcessSaleByBidder(soldPlayer, Bidder, Bid > 0);
    }    
}
