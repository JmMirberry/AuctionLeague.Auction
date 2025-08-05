using AuctionLeague.Data.Auction;
using FluentResults;

namespace AuctionLeague.SaleService.Validation
{
    public static class PlayerSaleResultValidator
    {
        private const int MaxPlayersFromTeam = 3;
        private const int MaxPlayersPerTeam = 11;
        private const int TotalBudget = 100;

        public static Result ValidateSale(AuctionTeam team, SoldPlayer soldPlayer)
        {
            var playersRemaining = MaxPlayersPerTeam - team.Players.Count;
            
            if (playersRemaining <= 0) return Result.Fail("Team is already complete");

            var budgetValidationResult = ValidateBudget(team, soldPlayer, playersRemaining);
            if (budgetValidationResult.IsFailed) return budgetValidationResult;

            if (team.Players.Count(x => x.Team == soldPlayer.Team) >= MaxPlayersFromTeam)
            {
                return Result.Fail($"Invalid purchase. {team.TeamName} already has {MaxPlayersFromTeam} players from {soldPlayer.Team}");
            }

            return ValidatePosition(team, soldPlayer, playersRemaining);
        }

        private static Result ValidateBudget(AuctionTeam team, SoldPlayer soldPlayer, int playersRemaining)
        {
            var budgetRemaining = TotalBudget - team.Players.Sum(x => x.SalePrice);

            var maxPerPlayer = budgetRemaining - (playersRemaining - 1);

            if (maxPerPlayer < soldPlayer.SalePrice)
            {
                return Result.Fail($"Invalid purchase. {team.TeamName} has a max bid of {maxPerPlayer} per player");
            }
            return Result.Ok();
        }

        private static Result ValidatePosition(AuctionTeam team, SoldPlayer soldPlayer, int playersRemaining)
        {
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
