using AuctionLeague.Data.Auction;
namespace AuctionLeague.Service.Auction.Interfaces;

public interface IAuctionNominationService 
{ 
    Task<AuctionPlayer> NominateByName(string lastNameSearch); 
    Task<AuctionPlayer> NominateById(int playerId); 
}