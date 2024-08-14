using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.MongoDb;

public static class AutoNomiatedPlayerMapper
{
    public static AutoNominatedPlayerEntity ToAutoNominatedEntity(this AuctionPlayer player)
    {
        return new AutoNominatedPlayerEntity
        {
            _id = player.PlayerId.ToString(),
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Team = player.Team,
            Position = player.Position,
            Value = player.Value,
            TotalPointsPreviousYear = player.TotalPointsPreviousYear,
            Nominated = false
        };
    }

    public static AuctionPlayer ToPlayer(this AutoNominatedPlayerEntity player)
    {
        return new AuctionPlayer
        {
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Team = player.Team,
            Position = player.Position,
            Value = player.Value,
            TotalPointsPreviousYear = player.TotalPointsPreviousYear
        };
    }
}
