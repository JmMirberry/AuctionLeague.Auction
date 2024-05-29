using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionLeague.Data.FplPlayer;

namespace AuctionLeague.Data.Auction
{
    public class AuctionData
    {
        public int Bid { get; set; }
        public AuctionPlayer Player { get; set; }
        public string Bidder { get; set; }
    }
}
