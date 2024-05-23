using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using FluentResults;
namespace AuctionLeague.Service.Auction.Interfaces;

public interface IAuctionNominationService 
{ 
    Task<Result<AuctionPlayer>> NominateByName(string lastNameSearch); 
    Task<Result<AuctionPlayer>> NominateById(int playerId); 
}