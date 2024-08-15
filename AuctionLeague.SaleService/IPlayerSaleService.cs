using AuctionLeague.Data;
using AuctionLeague.Data.Auction;
using FluentResults;

namespace AuctionLeague.SaleService;

public interface IPlayerSaleService
{
    Task<Result<SoldData>> ProcessSaleByBidder(SoldPlayer soldPlayer, string bidder);
    Task<Result<SoldData>> ProcessSaleByTeamName(SoldPlayer soldPlayer, string teamName);
    Task<Result<SoldData>> ProcessSaleByTeamName(int playerId, string teamName, double salePrice);
}