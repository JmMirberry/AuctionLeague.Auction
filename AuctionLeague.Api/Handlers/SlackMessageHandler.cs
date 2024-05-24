using AuctionLeague.Service.Auction.Interfaces;
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
            if (_auctionManager.AuctionLive()) return;


            if (slackEvent.Text.Contains("ping"))
            {
                await _slack.Chat.PostMessage(new Message
                {
                    Text = "pong",
                    Channel = slackEvent.Channel
                }).ConfigureAwait(false);
            }
        }
    }
}