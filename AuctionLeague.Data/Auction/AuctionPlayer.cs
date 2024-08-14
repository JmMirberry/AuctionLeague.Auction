using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.Data.Auction
{
    public class AuctionPlayer : Player
    {
        public bool IsFplPlayer { get; set; }
        public bool IsSold { get; set; }
    }
}
