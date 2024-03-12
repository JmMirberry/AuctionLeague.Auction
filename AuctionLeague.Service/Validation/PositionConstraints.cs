using AuctionLeague.Data;

namespace AuctionLeague.Service.Validation
{
    public class PositionConstraints
    {
        public Position Position { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
    }
}
