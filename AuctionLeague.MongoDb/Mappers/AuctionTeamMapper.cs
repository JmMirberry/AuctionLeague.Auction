using AuctionLeague.Data.Auction;
using AuctionLeague.MongoDb;
using System.Numerics;

public static class AuctionTeamMapper
{
    public static AuctionTeamEntity ToEntity(this AuctionTeam team)
    {
        return new AuctionTeamEntity
        {
            _id = team.TeamName,
            TeamName = team.TeamName,
            SlackBidders = team.SlackBidders,
            Players = team.Players.Select(p => p.ToSoldPlayerEntity()),
        };
    }

    public static AuctionTeam ToTeam(this AuctionTeamEntity team)
    {
        return new AuctionTeam
        {
            TeamName = team.TeamName,
            SlackBidders = team.SlackBidders.ToList(),
            Players = team.Players.Select(p => new SoldPlayer(p.ToPlayer(), p.SalePrice) ).ToList(),
        };
}

    public static SoldPlayerEntity ToSoldPlayerEntity(this SoldPlayer player)
    {

        return new SoldPlayerEntity(player.ToEntity(), player.SalePrice);
    }
}
