using AuctionLeague.Service.Auction.Interfaces;
using SlackNet;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace SlackAPI.Handlers
{
    public class BeginAuctionHandler : ISlashCommandHandler
    {
        public const string SlashCommand = "/beginauction";
        private readonly ISlackAuctionService _slackAuctionService;
        private readonly ISlackApiClient _slack;

        public BeginAuctionHandler(ISlackApiClient slack, ISlackAuctionService slackAuctionService)
        {
            _slack = slack;
            _slackAuctionService = slackAuctionService;
        }
        public async Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            await _slack.Chat.PostMessage(new Message
            {
                Text = "Begin auction sent",
                Channel = command.ChannelName
            }).ConfigureAwait(false);

            //var result = _slackAuctionService.StartAuction();

            return new SlashCommandResponse
            {
                Message = new Message
                {
                    Text =  "yay"//result.IsSuccess ? result.Value : result.Errors[0].Message
                },
                ResponseType = ResponseType.InChannel
            };
        }
    }
}