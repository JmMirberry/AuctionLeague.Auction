using AuctionLeague.Data.Auction;
using AuctionLeague.Data.Slack;
using AuctionLeague.Service.Auction.Interfaces;
using AuctionLeague.Service.DataStore;
using SlackNet;

namespace AuctionLeague.Service.Auction
{
    public class SlackAuctionEventHandler : ITimerEventHandler
    {        
        private readonly IDataStore<SlackAuctionData> _dataStore;
        //private readonly IPlayerSaleService _playerSaleService;
        private readonly ISlackApiClient _slackClient;

        public SlackAuctionEventHandler(IDataStore<SlackAuctionData> dataStore, ISlackApiClient slackClient)
        {
            _dataStore = dataStore;
            //_playerSaleService = playerSaleService;
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
            // TODO - remove this once using sale service
            
            await SendMessage($"{_dataStore.Data.Player.FirstName} {_dataStore.Data.Player.LastName} sold to {_dataStore.Data.Bidder} for {_dataStore.Data.Bid}");
            // var initialMessage = _dataStore.Data.Bid > 0 ? "Sold!" : "Not sold";
            // await SendMessage(initialMessage);
            //
            // var result = await _playerSaleService.ProcessSaleByBidder(new SoldPlayer(_dataStore.Data.Player, _dataStore.Data.Bid), _dataStore.Data.Bidder, _dataStore.Data.Bid > 0);
            //
            // if (result.IsSuccess)
            // {
            //     await SendMessage($"{_dataStore.Data.Player.FirstName} {_dataStore.Data.Player.FirstName} sold to {_dataStore.Data.Bidder} for {_dataStore.Data.Bid}");
            // }
            //
            // await SendMessage($"{_dataStore.Data.Player.FirstName} {_dataStore.Data.Player.FirstName} cannot be sold to {_dataStore.Data.Bidder}. {result.Errors}");
            _dataStore.Data = new SlackAuctionData();
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