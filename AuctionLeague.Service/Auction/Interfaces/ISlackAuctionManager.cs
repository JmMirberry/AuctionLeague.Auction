using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.Data.Slack;
using FluentResults;

namespace AuctionLeague.Service.Auction.Interfaces;

public interface ISlackAuctionManager
{
    bool AuctionLive();
    Result<AuctionPlayer> StartAuction();
    void NominatePlayer(AuctionPlayer player, string bidder);
    AuctionPlayer NominatedPlayer();
    SlackAuctionData CurrentBid();
    void EndAuction();
    void BidMade(int bid, string bidder);
}