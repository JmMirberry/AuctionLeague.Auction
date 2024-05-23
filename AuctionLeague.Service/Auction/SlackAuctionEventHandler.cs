using AuctionLeague.Data;
using AuctionLeague.Data.Slack;
using AuctionLeague.Service.Auction.Interfaces;
using AuctionLeague.Service.DataStore;
using AuctionLeague.Service.PlayerSale;
using SlackNet;
using System.Security.Cryptography;
using System.Threading.Channels;

namespace AuctionLeague.Service.Auction
{
    public class SlackAuctionEventHandler : ITimerEventHandler
    {        
        private readonly DataStore<SlackAuctionData> _dataStore;
        private readonly IPlayerSaleService _playerSaleService;
        private readonly ISlackApiClient _slackClient;

        public SlackAuctionEventHandler(DataStore<SlackAuctionData> dataStore, IPlayerSaleService playerSaleService, ISlackApiClient slackClient)
        {
            _dataStore = dataStore;
            _playerSaleService = playerSaleService;
            _slackClient = slackClient;
        }
        public async Task HandleFirstEvent()
        {
            await SendMessage("Going once");
        }
        public async Task HandleSecondEvent()
        {
            await SendMessage("Going twice");
        }

        public async Task HandleTimerEnd()
        {
            var initialMessage = _dataStore.Data.Bid > 0 ? "Sold!" : "Not sold";
            await SendMessage(initialMessage);

            var result = await _playerSaleService.ProcessSaleByBidder(new SoldPlayer(_dataStore.Data.Player, _dataStore.Data.Bid), _dataStore.Data.Bidder, _dataStore.Data.Bid > 0);

            if (result.IsSuccess)
            {
                await SendMessage($"{_dataStore.Data.Player.FirstName} {_dataStore.Data.Player.FirstName} sold to {_dataStore.Data.Bidder} for {_dataStore.Data.Bid}");
            }

            await SendMessage($"{_dataStore.Data.Player.FirstName} {_dataStore.Data.Player.FirstName} cannot be sold to {_dataStore.Data.Bidder}. {result.Errors}");

        }

        private async Task SendMessage(string message)
        {
            var slackMessage = new SlackNet.WebApi.Message()
            {
                Text = message,
                Channel = _dataStore.Data.Channel
            };

            await _slackClient.Chat.PostMessage(slackMessage, null);
        }
    }
}