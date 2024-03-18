using AuctionLeague.Data;

namespace AuctionLeague.Service.PlayerSale;

public interface IPlayerSaleService
{
    Task ProcessSaleByBidder(SoldPlayer soldPlayer, string bidder, bool isSold);
    Task ProcessSaleByTeamName(SoldPlayer soldPlayer, string teamName, bool isSold);
    Task ProcessSaleByTeamName(int playerId, string teamName, double salePrice, bool isSold);
    Task ResetSold();
}