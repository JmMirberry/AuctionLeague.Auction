using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.Service.Auction.Interfaces;
using FluentResults;
using SlackNet;

namespace AuctionLeague.Service.Auction
{
    public class SlackAuctionService
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
        
        public async Task StartAuction(string channel)
        {
            var result = _auctionManager.StartAuction();

            if (result.IsFailed)
            {
                await SendMessage(result.Errors.ToString(), channel);
            }

            await SendMessage($"Auction started for {result.Value.FirstName} {result.Value.FirstName}", channel);
        }
        
        public async Task EndAuction(string channel)
        {
            await SendMessage("Auction ended", channel);
        }

        public void SubmitBid(int bid, string bidder)
        {
            _auctionManager.BidMade(bid, bidder);
        }

        public async Task NominateByName(string lastNameSearch, string bidder, string channel)
        {
            var nominationSearchResult = await _nominationService.NominateByName(lastNameSearch);
            await NominatePlayer(nominationSearchResult.Value, bidder, channel);
        }
        
        public async Task NominateById(int playerId, string bidder, string channel)
        {
            var nominationSearchResult = await _nominationService.NominateById(playerId);
            await NominatePlayer(nominationSearchResult.Value, bidder, channel);
        } 
        
        private async Task NominatePlayer(Result<AuctionPlayer> playerSearchResult, string bidder, string channel)
        { 
            if (playerSearchResult.IsFailed)
            {
                await SendMessage(playerSearchResult.Errors.ToString(), channel); 
            }

            _auctionManager.NominatePlayer(playerSearchResult.Value, bidder);

            await SendMessage($"Auction started for {playerSearchResult.Value.FirstName} {playerSearchResult.Value.FirstName}", channel);
        }
        
        public async Task CheckNominatedPlayer(string channel)
        {
            var player = _auctionManager.NominatedPlayer();

            if (player == null)
            {
                await SendMessage("No player nominated", channel);
            } 
        }
        
        public async Task CheckCurrentBid(string channel)
        { 
            var bid = _auctionManager.CurrentBid();
            await SendMessage($"Current bid is {bid.Bid} by {bid.Bidder}", channel);
        }
        
        private async Task SendMessage(string message, string channel)
        {
            var slackMessage = new SlackNet.WebApi.Message()
            {
                Text = message,
                Channel = channel
            };

            await _slackClient.Chat.PostMessage(slackMessage, null);
        }
    } 
}