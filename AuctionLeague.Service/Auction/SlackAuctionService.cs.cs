using Amazon.Runtime.Internal;
using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.Service.Auction.Interfaces;
using AuctionLeague.Service.Interfaces;
using FluentResults;
using SlackNet;

namespace AuctionLeague.Service.Auction
{
    public class SlackAuctionService : ISlackAuctionService
    {
        private readonly ISlackAuctionManager _auctionManager;
        private readonly IAuctionNominationService _nominationService;

        public SlackAuctionService(ISlackAuctionManager auctionManager, IAuctionNominationService nominationService)
        {
            _auctionManager = auctionManager;
            _nominationService = nominationService;
            
        }

        public Result<string> StartAuction()
        {
            
            var result = _auctionManager.StartAuction();

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors[0].Message.ToString());
            }

            return Result.Ok($"Auction started for {result.Value.FirstName} {result.Value.FirstName}");
        }

        public void EndAuction()
        {
            _auctionManager.EndAuction();
        }

        public void SubmitBid(int bid, string bidder)
        {
            _auctionManager.BidMade(bid, bidder);
        }

        public async Task<Result<AuctionPlayer>> NominateByName(string lastNameSearch, string bidder, string channel)
        {
            var nominationSearchResult = await _nominationService.NominateByName(lastNameSearch);
            return NominatePlayer(nominationSearchResult.Value, bidder, 1, channel);
        }

        public async Task<Result<AuctionPlayer>> NominateById(int playerId, string bidder, int? bid, string channel)
        {
            var nominationSearchResult = await _nominationService.NominateById(playerId);
            return NominatePlayer(nominationSearchResult.Value, bidder, bid, channel);
        }

        private Result<AuctionPlayer> NominatePlayer(Result<AuctionPlayer> playerSearchResult, string bidder, int? bid, string channel)
        {
            if (playerSearchResult.IsFailed)
            {
                return Result.Fail(playerSearchResult.Errors.ToString());
            }

            _auctionManager.NominatePlayer(playerSearchResult.Value, bidder, bid, channel);

            return Result.Ok(playerSearchResult.Value);
        }

        public Result<AuctionPlayer> CheckNominatedPlayer()
        {
            var player = _auctionManager.NominatedPlayer();

            if (player == null)
            {
                return Result.Fail("No player nominated");
            }

            return Result.Ok<AuctionPlayer>(player);
        }

        public Result<string> CheckCurrentBid()
        {
            var bid = _auctionManager.CurrentBid();
            return Result.Ok($"Current bid is {bid.Bid} by {bid.Bidder}");
        }
    }
}