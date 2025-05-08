using AuctionLeague.Service.Auction;
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
                                new Markdown($"*FPL Value*\n${result.Value}"),
                                new Markdown($"*FPL Points*\n${result.TotalPointsPreviousYear}"),
                            }
                        }
                    }
                    },
                    ResponseType = ResponseType.InChannel
                };
            }
            catch (PlayerNotFoundException e)
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = e.Message.ToString(),
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
                        Text = e.Message.ToString(),
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