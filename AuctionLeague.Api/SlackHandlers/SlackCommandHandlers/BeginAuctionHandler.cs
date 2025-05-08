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
        private readonly ISlackApiClient _slackClient;

        public BeginAuctionHandler(ISlackAuctionService slackAuctionService, ISlackApiClient slackClient)
        {
            _slackAuctionService = slackAuctionService;
            _slackClient = slackClient;
        }

        public async Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            try
            {
                var result = _slackAuctionService.StartAuction();
                var slackMessage = new Message()
                {
                    Text = result.IsSuccess ? result.Value : result.Errors[0].Message,
                    Channel = command.ChannelId
                };

                await _slackClient.Chat.PostMessage(slackMessage, null);
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = ""
                    },
                    ResponseType = ResponseType.Ephemeral
                };
            }
            catch (Exception e)
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = e.ToString(),
                    },
                    ResponseType = ResponseType.Ephemeral
                };
            }
        }
    }
}