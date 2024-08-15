using AuctionLeague.Service.Auction.Interfaces;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace AuctionLeague.SlackHandlers.SlackCommandHandlers
{
    public class BidHandler : ISlashCommandHandler
    {
        public const string SlashCommand = "/bid";

        private readonly ISlackAuctionManager _auctionManager;
        
        public BidHandler(ISlackAuctionManager auctionManager)
        {
            _auctionManager = auctionManager;
        }
        
        public async Task<SlashCommandResponse> Handle(SlashCommand command)
        {
            if (!int.TryParse(command.Text, out var bid))
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = "Bids must be integers",
                        
                    },
                    ResponseType = ResponseType.Ephemeral
                };
            }
            
            if (bid < 0)
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = "Bids must be > 0",
                        
                    },
                    ResponseType = ResponseType.Ephemeral
                };
            }
            
            if (bid > 90)
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = "Bids must be < 91",
                        
                    },
                    ResponseType = ResponseType.Ephemeral
                };
            }

            var currentBid = _auctionManager.CurrentBid();
            if (bid <= currentBid.Bid)
            {
                return new SlashCommandResponse
                {
                    Message = new Message
                    {
                        Text = $"Bid must be greater than current high bid of {currentBid.Bid} ",
                    },
                    ResponseType = ResponseType.Ephemeral
                };
            }
            
            _auctionManager.BidMade(bid, command.UserId);

            return new SlashCommandResponse
            {
                ResponseType = ResponseType.InChannel // simple response means that only the command shows
            };
        }
    }
}