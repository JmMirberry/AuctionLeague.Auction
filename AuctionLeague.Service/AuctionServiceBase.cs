using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.MongoDb.Repositories;
using AuctionLeague.Service.PlayerSale;
using FluentResults;
using Microsoft.Extensions.Options;

namespace AuctionLeague.Service;

public abstract class AuctionServiceBase
{
    private readonly Timer _timer;
    private readonly int _timeToOnce;
    private readonly int _timeToTwice;
    private readonly int _timeToSold;
    internal int Bid = 0;
    internal Player Player = null;
    internal string Bidder = null;
    private bool GoingOnce;
    private bool GoingTwice;
    internal readonly IPlayerSaleService _playerSaleService;
    private readonly IPlayerRepository _playerRepository;

    public AuctionServiceBase(IOptions<AuctionSettings> settings, IPlayerSaleService playerSaleService, IPlayerRepository playerRepository)
    {
        _playerSaleService = playerSaleService;
        _playerRepository = playerRepository;
        _timeToOnce = settings.Value.TimeToOnceMs;
        _timeToTwice = settings.Value.TimeToTwiceMs;
        _timeToSold = settings.Value.TimeToSoldMs;
        _timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
    }

    public async Task<Result<Player>> NominateByName(string lastNameSearch, string bidder)
    {
        var playerMatches = await _playerRepository.GetPlayerAsync(lastNameSearch);

        if (!playerMatches.Any())
        {
            return Result.Fail($"No player found with last name mathing {lastNameSearch}");
        }

        if (playerMatches.Count() > 1)
        {
            return Result.Fail($"Multiple players matched");
        }

        var player = playerMatches.First();
        NominatePlayer(player, bidder);
        return Result.Ok(player);
    }

    public async Task<Result<Player>> NominateById(int playerId, string bidder)
    {
        var player = await _playerRepository.GetPlayerAsync(playerId);

        if (player == null)
        {
            NominatePlayer(player, bidder);
            return Result.Ok(player);
        }
        return Result.Fail($"No player found with id {playerId}");
    }

    public void NominatePlayer(Player player, string bidder)
    {
        Bid = 1;
        Player = player;
        Bidder = bidder;
    }

    public Player NominatedPlayer()
    {
        return Player;
    }

    public (Player, string, int) CurrentBid()
    {
        return (Player, Bidder, Bid);
    }

    public void StartAuction()
    {
        _timer.Change(_timeToOnce, Timeout.Infinite);
    }

    public void EndAuction()
    {
        // Reset the timer interval without stopping it
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
        ResetData();
    }

    public void BidMade(int bid, string bidder)
    {
        // Reset the timer interval without stopping it
        _timer.Change(_timeToOnce, Timeout.Infinite);
        Bid = bid;
        Bidder = bidder;
    }

    private void TimerCallback(object state)
    {
        if (!GoingOnce)
        {
            _timer.Change(_timeToTwice, Timeout.Infinite);
            ExecuteGoingOnce();
            GoingOnce = true;
        }

        // Save data when there are 3 seconds left
        else if (!GoingTwice)
        {
            _timer.Change(_timeToSold, Timeout.Infinite);
            ExecuteGoingTwice();
            GoingTwice = true;
        }

        // Save data when the timer ends
        else
        {
            ExecuteSold(new SoldPlayer(Player, Bid));
            ResetData();
        }
    }

    public abstract void ExecuteGoingOnce();

    public abstract void ExecuteGoingTwice();

    public abstract void ExecuteSold(SoldPlayer soldPlayer);


    private void ResetData()
    {
        GoingOnce = false;
        GoingTwice = false;
        Bid = 0;
        Bidder = null;
        Player = null;
    }
}
