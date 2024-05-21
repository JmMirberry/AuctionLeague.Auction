using AuctionLeague.Data;
using FluentResults;

namespace AuctionLeague.Service.Interfaces
{
    public interface IApiAuctionService
    {
        Task<Result<Player>> NominateByName(string lastNameSearch, string bidder);
        Task<Result<Player>> NominateById(int playerId, string bidder);
        void BidMade(int bid, string bidder);
        (Player, string, int) CurrentBid();
        void EndAuction();
        Player NominatedPlayer();
        void NominatePlayer(Player player, string bidder);
        void StartAuction();
    }
}