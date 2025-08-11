using AuctionLeague.Service.Auction.Interfaces;
using Newtonsoft.Json;
using SlackNet;
using SlackNet.Events;
using SlackNet.WebApi;

namespace SlackAPI.Handlers
{
    public class SlackMessageHandler : IEventHandler<MessageEvent>
    {

        private readonly ISlackApiClient _slack;
        private readonly ISlackAuctionManager _auctionManager;
        public SlackMessageHandler(ISlackApiClient slack, ISlackAuctionManager auctionManager)
        {
            _slack = slack;
            _auctionManager = auctionManager;
        }

        public async Task Handle(MessageEvent slackEvent)
        {
            if (slackEvent.ExtraProperties.ContainsKey("bot_id") || slackEvent.ExtraProperties.ContainsKey("app_id")) return;
                
            if (!_auctionManager.AuctionLive()) return;

            if (!int.TryParse(slackEvent.Text, out var bid))
            {
                await _slack.Chat.PostMessage(new Message
                {
                    Text = "Bids must be integers",
                    Channel = slackEvent.Channel
                });
                return;
            }

            if (bid <= 1)
            {
                await _slack.Chat.PostMessage(new Message
                {
                    Text = "Bids must be > 1",
                    Channel = slackEvent.Channel
                });
                return;
            }

            if (bid >= 91)
            {
                await _slack.Chat.PostMessage(new Message
                {
                    Text = "Bids must be < 91",
                    Channel = slackEvent.Channel
                });
                return;
            }

            var currentBid = _auctionManager.CurrentBid();
            if (bid <= currentBid.Bid)
            {
                
                await _slack.Chat.PostMessage(new Message
                {
                    Text = $"Bid must be greater than current high bid of {currentBid.Bid}.",
                    Channel = slackEvent.Channel
                });
                
                var bidder = (await _slack.Users.Info(currentBid.BidderUserId)).Name;
                
                await _slack.Chat.PostMessage(new Message
                {
                    Text = $"{bidder} it the current highest bidder",
                    Channel = slackEvent.Channel
                });
                return;
            }

            _auctionManager.BidMade(bid, slackEvent.User);
        }
    }
}
