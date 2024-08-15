using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.Data.Slack;
using AuctionLeague.Service.Auction.Interfaces;
using AuctionLeague.Service.DataStore;
using FluentResults;
using SlackNet;
using System.ComponentModel;

namespace AuctionLeague.Service.Auction
{

    public class SlackAuctionManager : ISlackAuctionManager 
    {      
        private readonly IAuctionTimer _timer; 
        private readonly IDataStore<SlackAuctionData> _dataStore;

        public SlackAuctionManager(IAuctionTimer timer, IDataStore<SlackAuctionData> dataStore, ISlackApiClient slack)
        { 
            _timer = timer;
            _dataStore = dataStore;
        }

        public bool AuctionLive()
        {
            return _timer.TimerRunning();
        }
        
        public Result<AuctionPlayer> StartAuction() 
        {
            _dataStore.Data.Channel ??= "v2-bot-test";
            
            var player = _dataStore.Data?.Player; 
            if (player == null)
            {
                return Result.Fail("No player nominated");
            }  
            _timer.StartTimer();  
            return Result.Ok(player);
        }
        
        public void NominatePlayer(AuctionPlayer player, string bidder, int? bid, string channel)
        {   
            _dataStore.Data = new SlackAuctionData
            {           
                BidderUserId = bidder, 
                Bid = bid ?? 0, 
                Player = player,
                Channel = channel
            };
        }
        
        public AuctionPlayer NominatedPlayer()
        {     
            return _dataStore.Data?.Player;
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
            _dataStore.Data.BidderUserId = bidder;  
        } 
        
        private void ResetData() 
        {  
            _dataStore.Data = new SlackAuctionData 
            {
                BidderUserId = null,   
                Bid = 0,  
                Player = null 
            };
        }
    }
}
