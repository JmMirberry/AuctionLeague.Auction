using AuctionLeague.Data.FplPlayer;
using AuctionLeague.MongoDb;

public static class ManualPlayerMapper
{
    public static ManualPlayerEntity ToEntity(this ManualPlayer player)
    {
        return new ManualPlayerEntity
        {
            _id = player.PlayerId.ToString(),
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName
        };
    }

    public static ManualPlayer ToPlayer(this ManualPlayerEntity player)
    {
        return new ManualPlayer
        {
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
        };
    }
}
