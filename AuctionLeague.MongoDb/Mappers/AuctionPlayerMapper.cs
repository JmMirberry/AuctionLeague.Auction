using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.MongoDb;

public static class AuctionPlayerMapper
{
    public static AuctionPlayerEntity ToEntity(this AuctionPlayer player)
    {
        return new AuctionPlayerEntity
        {
            _id = player.PlayerId.ToString(),
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Team = player.Team,
            Position = player.Position,
            Value = player.Value,
            TotalPointsPreviousYear = player.TotalPointsPreviousYear,
            IsFplPlayer = player.IsFplPlayer,
            IsAutoNomination = player.IsAutonomination,
            IsSold = player.IsSold
        };
    }

    public static AuctionPlayer ToPlayer(this AuctionPlayerEntity player)
    {
        return new AuctionPlayer
        {
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Team = player.Team,
            Position = player.Position,
            Value = player.Value,
            TotalPointsPreviousYear = player.TotalPointsPreviousYear,
            IsFplPlayer = player.IsFplPlayer,
            IsAutonomination = player.IsAutoNomination,
            IsSold = player.IsSold
        };
    }
}
