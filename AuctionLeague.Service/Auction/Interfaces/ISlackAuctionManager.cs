using AuctionLeague.Data;
using AuctionLeague.Data.Slack;
using FluentResults;

namespace AuctionLeague.Service.Auction.Interfaces;

public interface ISlackAuctionManager
{  
    Result<Player> StartAuction();
    void NominatePlayer(Player player, string bidder);
    Player NominatedPlayer();
    SlackAuctionData CurrentBid();
    void EndAuction();
    void BidMade(int bid, string bidder);
}