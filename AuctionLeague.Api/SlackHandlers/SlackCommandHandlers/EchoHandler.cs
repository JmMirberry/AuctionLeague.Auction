using AuctionLeague.Data.Slack;
using AuctionLeague.Service.DataStore;
using SlackNet;
using SlackNet.Events;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace AuctionLeague.Handlers.SlackCommandHandlers
{
    public class EchoDemo : ISlashCommandHandler
    {
        public const string SlashCommand = "/echo";
        private readonly ISlackApiClient _slackClient;

        public EchoDemo(ISlackApiClient slackClient)
        {
            _slackClient = slackClient;
        }

        public async Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            await SendMessage(command.ChannelName, "This is a test message");
            Console.WriteLine($"{command.UserName} used the {SlashCommand} slash command in the {command.ChannelName} channel");

            return new SlashCommandResponse
            {
                Message = new Message
                {
                    Text = command.Text
                }
            };
        }

        private async Task SendMessage(string channel, string message)
        {
            var slackMessage = new Message()
            {
                Text = message,
                Channel = channel
            };

            await _slackClient.Chat.PostMessage(slackMessage, null);
        }
    }
}