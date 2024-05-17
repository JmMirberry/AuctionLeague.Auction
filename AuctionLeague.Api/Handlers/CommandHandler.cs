using SlackNet;
using SlackNet.Events;
using SlackNet.WebApi;

namespace SlackAPI.Handlers
{
    public class EchoDemo : ISlashCommandHandler
{
    public const string SlashCommand = "/echo";
    
    public async Task<SlashCommandResponse> Handle(SlashCommand command)
    {
        Console.WriteLine($"{command.UserName} used the {SlashCommand} slash command in the {command.ChannelName} channel");
        
        return new SlashCommandResponse
            {
                Message = new Message
                    {
                        Text = command.Text
                    }
            };
    }
}
}