using AuctionLeague.Data;
using AuctionLeague.Data.Auction;
using FluentResults;

namespace AuctionLeague.SaleService;

public interface IPlayerSaleService
{
    Task<Result<SoldData>> ProcessSaleByBidder(SoldPlayer soldPlayer, string bidder, bool isSold);
    Task<Result<SoldData>> ProcessSaleByTeamName(SoldPlayer soldPlayer, string teamName, bool isSold);
    Task<Result<SoldData>> ProcessSaleByTeamName(int playerId, string teamName, double salePrice, bool isSold);
}