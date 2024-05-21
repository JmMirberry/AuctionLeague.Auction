using AuctionLeague.Data;
using AuctionLeague.Data.Slack;
using AuctionLeague.Service.PlayerSale;
using FluentResults;
using Microsoft.Extensions.Options;

namespace AuctionLeague.Service.Auction
{

    public class SlackAuctionManager
    {

        private AuctionTimer _timer;
        private readonly AuctionSettings _settings;
        private readonly AuctionDataStore<SlackAuctionData> _dataStore;

        public SlackAuctionManager(IOptions<AuctionSettings> settings, AuctionDataStore<SlackAuctionData> dataStore)
        {
            _settings = settings.Value;
            _dataStore = dataStore;
        }

        public Result StartAuction()
        {
            if (_dataStore.Data.Player == null)
            {
                return Result.Fail("No player nominated");
            }
            _timer = new AuctionTimer(new SlackAuctionEventHandler(), _settings.TimeToSoldMs, _settings.TimeToOnceMs, _settings.TimeToTwiceMs);
            _timer.StartTimer();
            return Result.Ok();
        }

        public void NominatePlayer(Player player, string bidder)
        {
            _dataStore.Data.Bidder = bidder;
            _dataStore.Data.Bid = 1;
            _dataStore.Data.Player = player;
        }

        public Player NominatedPlayer()
        {
            return _dataStore.Data.Player;
        }

        public SlackAuctionData CurrentBid()
        {
            return _dataStore.Data;
        }

        public void EndAuction()
        {
            _timer.ResetTimer();
            ResetData();
        }

        public void BidMade(int bid, string bidder)
        {
            _timer.RestartTimer();
            _dataStore.Data.Bid = bid;
            _dataStore.Data.Bidder = bidder;
        }

        private void ResetData()
        {
            _dataStore.Data.Bid = 0;
            _dataStore.Data.Bidder = null;
            _dataStore.Data.Player = null;
        }
    }
}
