using AuctionLeague.Data.Auction;
using FluentResults;

namespace AuctionLeague.SaleService.Validation
{
    public static class PlayerSaleResultValidator
    {
        public static Result ValidateSale(AuctionTeam team, SoldPlayer soldPlayer)
        {
            var playersRemaining = 11 - team.Players.Count();

            if (playersRemaining <= 0) return Result.Fail("Team is already complete");

            var playerPosition = soldPlayer.Position;
            var positionConstraints = PositionSettings.GetPositionConstraints();
            var playersRequiredForMinimums = 0;

            foreach (var position in positionConstraints)
            {
                var playersPurchasedInPosition = team.Players.Count(x => x.Position == position.Position);

                if (playerPosition == position.Position)
                {
                    if (playersPurchasedInPosition == position.MaxPlayers)
                    {
                        return Result.Fail($"{team.TeamName} already has {position.MaxPlayers} {playerPosition}s");
                    }
                    break;
                }

                playersRequiredForMinimums += Math.Max(position.MinPlayers - playersPurchasedInPosition, 0);
            }

            if (playersRequiredForMinimums > playersRemaining - 1)
            {
                return Result.Fail($"{team.TeamName} cannot purchase another {playerPosition}");
            }

            return Result.Ok();
        }
    }
}