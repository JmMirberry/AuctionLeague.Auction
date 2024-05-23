using AuctionLeague.Data;
using FluentResults;
namespace AuctionLeague.Service.Auction.Interfaces;

public interface IAuctionNominationService 
{ 
    Task<Result<Player>> NominateByName(string lastNameSearch); 
    Task<Result<Player>> NominateById(int playerId); 
}