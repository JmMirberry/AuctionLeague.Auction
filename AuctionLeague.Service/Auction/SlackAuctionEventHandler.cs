using AuctionLeague.Data;
using AuctionLeague.Data.Slack;
using AuctionLeague.Service.Auction.Interfaces;
using AuctionLeague.Service.PlayerSale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionLeague.Service.Auction
{
    public class SlackAuctionEventHandler : ITimerEventHandler
    {
        protected readonly AuctionDataStore<SlackAuctionData> _dataStore;
        private readonly IPlayerSaleService _playerSaleService;

        public SlackAuctionEventHandler(AuctionDataStore<SlackAuctionData> dataStore, IPlayerSaleService playerSaleService)
        {
            _dataStore = dataStore;
            _playerSaleService = playerSaleService;
        }
        public void HandleFirstEvent()
        {
            throw new NotImplementedException();
        }

        public void HandleSecondEvent()
        {
            throw new NotImplementedException();
        }

        public void HandleTimerEnd()
        {
            _playerSaleService.ProcessSaleByBidder(new SoldPlayer(_dataStore.Data.Player, _dataStore.Data.Bid), _dataStore.Data.Bidder, _dataStore.Data.Bid > 0);
            throw new NotImplementedException();
        }
    }
}
