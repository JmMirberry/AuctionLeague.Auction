using AuctionLeague.Service.Auction.Interfaces;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace SlackAPI.Handlers
{
    public class KillAuctionHandler : ISlashCommandHandler
    {
        public const string SlashCommand = "/killauction";
        private readonly ISlackAuctionService _slackAuctionService;

        public KillAuctionHandler(ISlackAuctionService slackAuctionService)
        {
            _slackAuctionService = slackAuctionService;
        }
        public Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            Console.WriteLine($"{command.UserName} used the {SlashCommand} slash command in the {command.ChannelName} channel");
            _slackAuctionService.EndAuction();

            return Task.FromResult(new SlashCommandResponse
            {
                Message = new Message
                {
                    Text = "Auction cancelled"
                },
                ResponseType = ResponseType.InChannel
            });
        }
    }
}