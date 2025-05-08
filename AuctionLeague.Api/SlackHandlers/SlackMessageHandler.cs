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
            
                await _slack.Chat.PostMessage(new Message
                {
                    Text = JsonConvert.SerializeObject(slackEvent.ExtraProperties),
                    Channel = slackEvent.Channel
                });
            return;


            if (slackEvent.Subtype == "bot_message") return;
            if (!_auctionManager.AuctionLive()) return;

            if (!int.TryParse(slackEvent.Text, out var bid))
            {
                await _slack.Chat.PostMessage(new Message
                {
                    Text = "Bids must be integers",
                    Channel = slackEvent.Channel
                });
            }

            if (bid <= 1)
            {
                await _slack.Chat.PostMessage(new Message
                {
                    Text = "Bids must be > 1",
                    Channel = slackEvent.Channel
                });
            }

            if (bid >= 91)
            {
                await _slack.Chat.PostMessage(new Message
                {
                    Text = "Bids must be < 91",
                    Channel = slackEvent.Channel
                });
            }

            _auctionManager.BidMade(bid, slackEvent.User);
        }
    }
}