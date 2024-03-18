using AuctionLeague.Data;

namespace AuctionLeague.Service.PlayerSale.Validation
{
    public static class PositionSettings
    {
        public static List<PositionConstraints> GetPositionConstraints()
        {
            return new List<PositionConstraints>
            {
                new() { Position = Position.GKP, MinPlayers = 1, MaxPlayers = 1 },
                new() { Position = Position.DEF, MinPlayers = 3, MaxPlayers = 5 },
                new() { Position = Position.MID, MinPlayers = 3, MaxPlayers = 5 },
                new() { Position = Position.FWD, MinPlayers = 1, MaxPlayers = 3 }
            };
        }
    }
}