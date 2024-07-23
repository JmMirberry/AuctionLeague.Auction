using AuctionLeague.Service.Auction.Interfaces;
using FluentResults;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace SlackAPI.Handlers
{
    public class NominateByIdHandler : ISlashCommandHandler
    {
        public const string SlashCommand = "/nominatebyid";
        private readonly ISlackAuctionService _slackAuctionService;

        public NominateByIdHandler(ISlackAuctionService slackAuctionService)
        {
            _slackAuctionService = slackAuctionService;
        }
        public async Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            Console.WriteLine($"{command.UserId} used the {SlashCommand} slash command in the {command.ChannelId} channel");

            if (!int.TryParse(command.Text, out var id))
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = "Invalid player Id",
                        
                    },
                    ResponseType = ResponseType.Ephemeral
                };
            }

            var result = await _slackAuctionService.NominateById(id, command.UserId);

            return new SlashCommandResponse
            {
                Message = new Message
                {
                    Text = result.IsSuccess ? result.Value : result.Errors[0].Message,
                },
                ResponseType = ResponseType.InChannel
            };
        }
    }
}