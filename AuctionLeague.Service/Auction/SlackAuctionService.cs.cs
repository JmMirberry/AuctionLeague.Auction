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
        private readonly IAutoNominationService _autoNominationSevice;
        private readonly ISlackApiClient _slack;

        public SlackAuctionService(ISlackAuctionManager auctionManager, IAuctionNominationService nominationService, ISlackApiClient slack)
        {
            _auctionManager = auctionManager;
            _nominationService = nominationService;
            _slack = slack;
        }

        public Result<string> StartAuction()
        {
            _slack.Chat.PostMessage(new SlackNet.WebApi.Message() { Text = "Message", Channel = "dev" }, null);
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

        public async Task<Result<string>> NominateByName(string lastNameSearch, string bidder)
        {
            var nominationSearchResult = await _nominationService.NominateByName(lastNameSearch);
            return NominatePlayer(nominationSearchResult.Value, bidder);
        }

        public async Task<Result<string>> NominateById(int playerId, string bidder)
        {
            var nominationSearchResult = await _nominationService.NominateById(playerId);
            return NominatePlayer(nominationSearchResult.Value, bidder);
        }

        private Result<string> NominatePlayer(Result<AuctionPlayer> playerSearchResult, string bidder)
        {
            if (playerSearchResult.IsFailed)
            {
                return Result.Fail(playerSearchResult.Errors.ToString());
            }

            _auctionManager.NominatePlayer(playerSearchResult.Value, bidder);

            return Result.Ok($"{playerSearchResult.Value.FirstName} {playerSearchResult.Value.LastName} Nominated");
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