using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.Data.Settings;
using FluentResults;

namespace AuctionLeague.Service.Interfaces;

public interface IAutoNominationService
{
    Task SetAutoNominations(List<AutonominationSettings> settings);
    Task<Result<AuctionPlayer>> GetAutoNomination();
    Task<Result<AuctionPlayer>> GetAutoNominationForRound(int round);
}