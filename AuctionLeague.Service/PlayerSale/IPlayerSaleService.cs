using AuctionLeague.Data;
using FluentResults;

namespace AuctionLeague.Service.PlayerSale;

public interface IPlayerSaleService
{
    Task<Result<SoldData>> ProcessSaleByBidder(SoldPlayer soldPlayer, string bidder, bool isSold);
    Task<Result<SoldData>> ProcessSaleByTeamName(SoldPlayer soldPlayer, string teamName, bool isSold);
    Task<Result<SoldData>> ProcessSaleByTeamName(int playerId, string teamName, double salePrice, bool isSold);
    Task ResetSold();
}