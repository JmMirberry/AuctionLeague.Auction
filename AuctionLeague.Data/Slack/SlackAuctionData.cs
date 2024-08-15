using AuctionLeague.Data.Auction;

namespace AuctionLeague.Data.Slack
{
    public class SlackAuctionData
    {
        public string Channel { get; set; }
        public string BidderUserId { get; set; }
        public int Bid { get; set; }
        public AuctionPlayer Player { get; set; }
    }
}
