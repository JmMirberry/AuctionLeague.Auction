﻿using AuctionLeague.Service.Auction.Interfaces;
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

                return new SlashCommandResponse
                {
                    Message = new Message
                    {
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
                                new Markdown($"*FPL Value*\n{result.Value.Value}"),
                                new Markdown($"*FPL Points*\n{result.Value.TotalPointsPreviousYear}"),
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
