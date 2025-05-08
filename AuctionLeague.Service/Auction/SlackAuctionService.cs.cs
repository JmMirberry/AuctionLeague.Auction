using AuctionLeague.Data.Auction;
using AuctionLeague.Service.Auction.Interfaces;
using FluentResults;
using SlackNet;
using System.Security.Cryptography;

namespace AuctionLeague.Service.Auction
{
    public class SlackAuctionService : ISlackAuctionService
    {
        private readonly ISlackAuctionManager _auctionManager;
        private readonly IAuctionNominationService _nominationService;
        private readonly ISlackApiClient _slackClient;


        public SlackAuctionService(ISlackAuctionManager auctionManager, IAuctionNominationService nominationService, ISlackApiClient slackClient)
        {
            _auctionManager = auctionManager;
            _nominationService = nominationService;
            _slackClient = slackClient;
        }

        public Result<string> StartAuction()
        {
            
            var result = _auctionManager.StartAuction();

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors[0].Message.ToString());
            }

            return Result.Ok($"Auction started for {result.Value.FirstName} {result.Value.LastName}");
        }

        public void EndAuction()
        {
            _auctionManager.EndAuction();
        }

        public void SubmitBid(int bid, string bidderId)
        {
            _auctionManager.BidMade(bid, bidderId);
        }

        public async Task<AuctionPlayer> NominateByName(string lastNameSearch, string bidder, string channel)
        {
            var player = await _nominationService.NominateByName(lastNameSearch);
            _auctionManager.NominatePlayer(player, bidder, 1, channel);
            return player;
        }

        public async Task<AuctionPlayer> NominateById(int playerId, string bidder, int? bid, string channel)
        {
            var player = await _nominationService.NominateById(playerId);
            _auctionManager.NominatePlayer(player, bidder, bid, channel);
            return player;
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

        public async Task<Result<string>> CheckCurrentBid()
        {
            var bid = _auctionManager.CurrentBid();
            var bidder = (await _slackClient.Users.Info(bid.BidderUserId)).Name;
            return Result.Ok($"Current bid is {bid.Bid} by {bidder}");
        }
    }
}