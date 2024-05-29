using AuctionLeague.Data.FplPlayer;
using AuctionLeague.MongoDb;

public static class PlayerMapper
{
    public static PlayerEntity ToEntity(this Player player)
    {
        return new PlayerEntity
        {
            _id = player.PlayerId.ToString(),
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Team = player.Team,
            Position = player.Position,
            Value = player.Value,
            TotalPointsPreviousYear = player.TotalPointsPreviousYear,
        };
    }

    public static Player ToPlayer(this PlayerEntity player)
    {
        return new Player
        {
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Team = player.Team,
            Position = player.Position,
            Value = player.Value,
            TotalPointsPreviousYear = player.TotalPointsPreviousYear,
        };
    }
}
