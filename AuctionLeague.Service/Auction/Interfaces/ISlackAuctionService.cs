using AuctionLeague.Data.Auction;
using FluentResults;

namespace AuctionLeague.Service.Auction.Interfaces
{
    public interface ISlackAuctionService
    {
        Task<Result<string>> CheckCurrentBid();
        Result<AuctionPlayer> CheckNominatedPlayer();
        Result<string> StartAuction();
        void EndAuction();
        Task<AuctionPlayer> NominateById(int playerId, string bidderId, int? bid, string channel);
        Task<AuctionPlayer> NominateByName(string lastNameSearch, string bidderId, string channel);
        void SubmitBid(int bid, string bidder);
    }
}