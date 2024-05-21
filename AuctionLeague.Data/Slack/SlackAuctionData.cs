using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionLeague.Data.Slack
{
    public class SlackAuctionData
    {
        public string channel;
        public int Bid = 0;
        public Player Player = null;
        public string Bidder = null;
    }
}
