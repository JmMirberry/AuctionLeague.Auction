using AuctionLeague.Service.Auction.Interfaces;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace SlackAPI.Handlers
{
    public class NominateByNameHandler : ISlashCommandHandler
    {
        public const string SlashCommand = "/nominatebyname";
        private readonly ISlackAuctionService _slackAuctionService;

        public NominateByNameHandler(ISlackAuctionService slackAuctionService)
        {
            _slackAuctionService = slackAuctionService;
        }
        public async Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            Console.WriteLine($"{command.UserName} used the {SlashCommand} slash command in the {command.ChannelName} channel");
            var result = await _slackAuctionService.NominateByName(command.Text, command.UserName);

            return new SlashCommandResponse
            {
                Message = new Message
                {
                    Text = result.IsSuccess ? result.Value : result.Errors[0].Message
                },
                ResponseType = ResponseType.InChannel
            };
        }
    }
}