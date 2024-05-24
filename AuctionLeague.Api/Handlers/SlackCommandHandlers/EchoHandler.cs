using SlackNet;
using SlackNet.Events;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace AuctionLeague.Handlers.SlackCommandHandlers
{
    public class EchoDemo : ISlashCommandHandler
    {
        public const string SlashCommand = "/echo";

        public Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            Console.WriteLine($"{command.UserName} used the {SlashCommand} slash command in the {command.ChannelName} channel");

            return Task.FromResult(new SlashCommandResponse
            {
                Message = new Message
                {
                    Text = command.Text
                }
            });
        }
    }
}