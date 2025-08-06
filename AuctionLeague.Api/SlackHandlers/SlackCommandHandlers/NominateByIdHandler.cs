using AuctionLeague.Service.Auction;
using AuctionLeague.Service.Auction.Interfaces;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace AuctionLeague.SlackHandlers.SlackCommandHandlers
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
            try
            {
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

                var result = await _slackAuctionService.NominateById(id, command.UserId, 1, command.ChannelName);

                var slackMessage = new Message
                {
                    Channel = command.ChannelName,
                    Blocks =
                    {
                        new SectionBlock
                        {
                            Text = new Markdown($"*{result.PlayerId} - {result.FirstName} {result.LastName}*"),
                        },
                        new SectionBlock
                        {
                            Fields = new List<TextObject>
                            {
                                new Markdown($"*Position:*\n{result.Position}"),
                                new Markdown($"*Club:*\n{result.Team}"),
                                new Markdown($"*FPL Value*\n{result.Value}"),
                                new Markdown($"*FPL Points*\n{result.TotalPointsPreviousYear}"),
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
            catch (PlayerNotFoundException e)
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = e.Message,
                    },
                    ResponseType = ResponseType.Ephemeral
                };
            }
            catch (PlayerUnavailableException e)
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = e.Message,
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
