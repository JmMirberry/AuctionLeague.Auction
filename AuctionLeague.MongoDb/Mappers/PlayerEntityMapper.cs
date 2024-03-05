using AuctionLeague.Data;
using AuctionLeague.MongoDb.Entities;

namespace AuctionLeague.MongoDb.Mappers;

public static class PlayerEntityMapper
{
    public static PlayerEntity ToEntity(this Player player)
    {
        return new PlayerEntity
        {
            PlayerId = player.PlayerId,
            FirstName = player.FirstName,
            LastName = player.LastName,
            Team = player.Team,
            Position = player.Position,
            Value = player.Value,
            TotalPointsPreviousYear = player.TotalPointsPreviousYear,
            IsInFpl = player.IsInFpl
        };
    }
}