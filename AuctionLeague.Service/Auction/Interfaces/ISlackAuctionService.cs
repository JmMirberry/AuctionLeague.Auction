using AuctionLeague.Data.Auction;
using FluentResults;

namespace AuctionLeague.Service.Auction.Interfaces
{
    public interface ISlackAuctionService
    {
        Result<string> CheckCurrentBid();
        Result<AuctionPlayer> CheckNominatedPlayer();
        Result<string> StartAuction();
        void EndAuction();
        Task<Result<AuctionPlayer>> NominateById(int playerId, string bidderId, int? bid, string channel);
        Task<Result<AuctionPlayer>> NominateByName(string lastNameSearch, string bidderId, string channel);
        void SubmitBid(int bid, string bidder);
    }
}