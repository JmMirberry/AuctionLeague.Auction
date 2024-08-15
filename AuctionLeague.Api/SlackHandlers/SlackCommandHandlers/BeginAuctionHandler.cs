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

        public BeginAuctionHandler(ISlackAuctionService slackAuctionService)
        {
            _slackAuctionService = slackAuctionService;
        }

        public async Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            try
            {
                var result = _slackAuctionService.StartAuction();

                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = result.IsSuccess ? result.Value : result.Errors[0].Message
                    },
                    ResponseType = ResponseType.InChannel
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