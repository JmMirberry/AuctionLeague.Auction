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

            var nominatedPlayerResult = await _slackAuctionService.NominateById(result.Value.PlayerId, null, 0, command.ChannelName);

            if (nominatedPlayerResult.IsFailed)
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = nominatedPlayerResult.Errors[0].Message,
                    },
                    ResponseType = ResponseType.Ephemeral
                };
            }

            return new SlashCommandResponse
            {
                Message = new Message
                {
                    Channel = command.ChannelName,
                    Blocks =
                    {
                        new SectionBlock
                        {
                            Text = new Markdown($"*{nominatedPlayerResult.Value.PlayerId} - {nominatedPlayerResult.Value.FirstName} {nominatedPlayerResult.Value.LastName}*"),
                        },
                        new SectionBlock
                        {
                            Fields = new List<TextObject>
                            {
                                new Markdown($"*Position:*\n{nominatedPlayerResult.Value.Position}"),
                                new Markdown($"*Club:*\n{nominatedPlayerResult.Value.Team}"),
                                new Markdown($"*FPL Value*\n{nominatedPlayerResult.Value.Value}"),
                                new Markdown($"*FPL Points*\n{nominatedPlayerResult.Value.TotalPointsPreviousYear}"),
                                }
                        }
                    }
                },
                ResponseType = ResponseType.InChannel
            };
        }
    }
}
