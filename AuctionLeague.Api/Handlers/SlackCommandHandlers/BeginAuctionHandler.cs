using AuctionLeague.Service.Auction.Interfaces;
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
        public Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            Console.WriteLine($"{command.UserName} used the {SlashCommand} slash command in the {command.ChannelName} channel");
            var result = _slackAuctionService.StartAuction();

            return Task.FromResult(new SlashCommandResponse
            {
                Message = new Message
                {
                    Text = result.IsSuccess ? result.Value : result.Errors[0].Message
                },
                ResponseType = ResponseType.InChannel
            });
        }
    }
}