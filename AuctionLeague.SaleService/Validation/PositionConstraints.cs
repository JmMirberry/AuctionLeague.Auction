using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.SaleService.Validation
{
    public class PositionConstraints
    {
        public Position Position { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
    }
}
