using AuctionLeague.Data;
using AuctionLeague.Data.Slack;
using AuctionLeague.Service.Auction.Interfaces;
using AuctionLeague.Service.DataStore;
using FluentResults;

namespace AuctionLeague.Service.Auction {
    
public class SlackAuctionManager : ISlackAuctionManager 
    {      
        private readonly IAuctionTimer _timer; 
        private readonly DataStore<SlackAuctionData> _dataStore; 

        public SlackAuctionManager(IAuctionTimer timer, DataStore<SlackAuctionData> dataStore)
        { 
            _timer = timer;
            _dataStore = dataStore;
        }
        
        public Result<Player> StartAuction() 
        {
            var player = _dataStore.Data?.Player; 
            if (player == null)
            {
                return Result.Fail("No player nominated");
            }  
            _timer.StartTimer();  
            return Result.Ok(player);
        }
        
        public void NominatePlayer(Player player, string bidder)
        {   
            _dataStore.Data = new SlackAuctionData
            {           
                Bidder = bidder, 
                Bid = 1, 
                Player = player   
            };
        }
        
        public Player NominatedPlayer()
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
            _dataStore.Data.Bidder = bidder;  
        } 
        
        private void ResetData() 
        {  
            _dataStore.Data = new SlackAuctionData 
            {  
                Bidder = null,   
                Bid = 0,  
                Player = null 
            };
        }
    }
}
