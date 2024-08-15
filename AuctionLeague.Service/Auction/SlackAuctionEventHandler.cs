using AuctionLeague.Data.Auction;
using AuctionLeague.Data.Slack;
using AuctionLeague.SaleService;
using AuctionLeague.Service.Auction.Interfaces;
using AuctionLeague.Service.DataStore;
using SlackNet;

namespace AuctionLeague.Service.Auction
{
    public class SlackAuctionEventHandler : ITimerEventHandler
    {        
        private readonly IDataStore<SlackAuctionData> _dataStore;
        private readonly IPlayerSaleService _playerSaleService;
        private readonly ISlackApiClient _slackClient;

        public SlackAuctionEventHandler(IDataStore<SlackAuctionData> dataStore, ISlackApiClient slackClient, IPlayerSaleService playerSaleService)
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
            var sold = _dataStore.Data.Bid > 0;
            var initialMessage = sold ? "Sold!" : "Not sold";
            await SendMessage(initialMessage);

            try
            {
                var displayName = (await _slackClient.Users.Info(_dataStore.Data.BidderUserId)).RealName;

                if (!sold) return;

                var result = await _playerSaleService.ProcessSaleByBidder(new SoldPlayer(_dataStore.Data.Player, _dataStore.Data.Bid), displayName);

                if (result.IsSuccess)
                {
                    await SendMessage($"{_dataStore.Data.Player.FirstName} {_dataStore.Data.Player.FirstName} sold to {result.Value.SoldTo} for {_dataStore.Data.Bid}");
                }
                else
                {
                    await SendMessage($"{_dataStore.Data.Player.FirstName} {_dataStore.Data.Player.FirstName} cannot be sold to {displayName}. {result.Errors}");
                }
                _dataStore.Data = new SlackAuctionData();
            }
            catch (Exception e) 
            {
                await SendMessage(e.ToString());
            }
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