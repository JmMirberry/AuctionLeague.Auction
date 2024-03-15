using AuctionLeague.Data;
using AuctionLeague.MongoDb;

public static class AuctionTeamMapper
{
    public static AuctionTeamEntity ToEntity(this AuctionTeam team)
    {
        return new AuctionTeamEntity
        {
            _id = team.TeamName,
            TeamName = team.TeamName,
            SlackBidders = team.SlackBidders,
            Players = team.Players.Select(p => new SoldPlayerEntity(p.ToEntity(), p.SalePrice)),
        };
    }

    public static AuctionTeam ToTeam(this AuctionTeamEntity team)
    {
        return new AuctionTeam
        {
            TeamName = team.TeamName,
            SlackBidders = team.SlackBidders.ToList(),
            Players = team.Players.Select(p => new SoldPlayer(p.ToPlayer(), p.SalePrice)).ToList(),
        };
    }
}
