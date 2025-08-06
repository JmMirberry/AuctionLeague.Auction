using AuctionLeague.Service.Auction.Interfaces;
using AuctionLeague.Service.Interfaces;
using SlackNet.Blocks;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace AuctionLeague.SlackHandlers.SlackCommandHandlers
{
    public class AutoNominationHandler : ISlashCommandHandler
    {
        public const string SlashCommand = "/autonominate";
        private readonly IAutoNominationService _service;
        private readonly ISlackAuctionService _slackAuctionService;

        public AutoNominationHandler(IAutoNominationService service, ISlackAuctionService slackAuctionService)
        {
            _service = service;
            _slackAuctionService = slackAuctionService;
        }
        public async Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            try
            {
                var result = await _service.GetAutoNomination();

                if (result.IsFailed)
                {
                    return new SlashCommandResponse
                    {
                        Message = new Message
                        {
                            Text = result.Errors[0].Message,
                        },
                        ResponseType = ResponseType.Ephemeral
                    };
                }

                var nominatedPlayer = await _slackAuctionService.NominateById(result.Value.PlayerId, null, 0, command.ChannelName);

                var slackMessage = new Message
                {
                    Channel = command.ChannelName,
                    Blocks =
                    {
                        new SectionBlock
                        {
                            Text = new Markdown($"*{nominatedPlayer.PlayerId} - {nominatedPlayer.FirstName} {nominatedPlayer.LastName}*"),
                        },
                        new SectionBlock
                        {
                            Fields = new List<TextObject>
                            {
                                new Markdown($"*Position:*\n{nominatedPlayer.Position}"),
                                new Markdown($"*Club:*\n{nominatedPlayer.Team}"),
                                new Markdown($"*FPL Value*\n{nominatedPlayer.Value}"),
                                new Markdown($"*FPL Points*\n{nominatedPlayer.TotalPointsPreviousYear}"),
                            }
                        }
                    }
                };

                return new SlashCommandResponse
                {
                    Message = slackMessage,
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
