using AuctionLeague.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionLeague.Service.Auction.Interfaces
{
    public interface ITimerEventHandler
    {
        void HandleTimerEnd();
        void HandleFirstEvent();
        void HandleSecondEvent();
    }
}
