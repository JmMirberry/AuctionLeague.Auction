using AuctionLeague.Service.Auction.Interfaces;
using SlackNet.Blocks;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace AuctionLeague.SlackHandlers.SlackCommandHandlers
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
            try
            {
                var result = await _slackAuctionService.NominateByName(command.Text, command.UserId, command.ChannelName);

                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        //Text = result.IsSuccess ? result.Value : result.Errors[0].Message,
                        Channel = command.ChannelName,
                        Blocks =
                    {
                        new SectionBlock
                        {
                            Text = new Markdown($"*{result.Value.PlayerId} - {result.Value.FirstName} {result.Value.LastName}*"),
                        },
                        new SectionBlock
                        {
                            Fields = new List<TextObject>
                            {
                                new Markdown($"*Position:*\n{result.Value.Position}"),
                                new Markdown($"*Club:*\n{result.Value.Team}"),
                                new Markdown($"*FPL Value*\n${result.Value.Value}"),
                                new Markdown($"*FPL Points*\n${result.Value.TotalPointsPreviousYear}"),
                            }
                        }
                    }
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