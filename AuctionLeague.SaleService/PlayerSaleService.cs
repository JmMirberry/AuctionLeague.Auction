using AuctionLeague.Data;
using AuctionLeague.Data.Auction;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.SaleService.Validation;
using FluentResults;

namespace AuctionLeague.SaleService
{
    public class PlayerSaleService : IPlayerSaleService
    {
        private readonly IAuctionTeamRepository _auctionTeamsRepository;
        private readonly IAuctionPlayerRepository _playerRepository;

        public PlayerSaleService(IAuctionTeamRepository auctionTeamsRepository, IAuctionPlayerRepository playerRepository)
        {
            _auctionTeamsRepository = auctionTeamsRepository;
            _playerRepository = playerRepository;
        }

        public async Task<Result<SoldData>> ProcessSaleByBidder(SoldPlayer soldPlayer, string bidder)
        {
            var team = await GetAuctionTeamByBidder(bidder);
            if (team == null)
            {
                team = new AuctionTeam
                {
                    TeamName = bidder,
                    SlackBidders = new List<string> { bidder }

                };
                await _auctionTeamsRepository.AddAuctionTeamAsync(team);
            }

            return await ProcessSale(soldPlayer, team);
        }

        public async Task<Result<SoldData>> ProcessSaleByTeamName(SoldPlayer soldPlayer, string teamName)
        {
            var team = await _auctionTeamsRepository.GetAuctionTeamAsync(teamName);

            if (team == null)
            {
                team = new AuctionTeam
                {
                    TeamName = teamName
                };
                await _auctionTeamsRepository.AddAuctionTeamAsync(team);
            }

            return await ProcessSale(soldPlayer, team);
        }

        public async Task<Result<SoldData>> ProcessSaleByTeamName(int playerId, string teamName, double salePrice)
        {

            var playerTask = _playerRepository.GetPlayerAsync(playerId);
            var teamTask = _auctionTeamsRepository.GetAuctionTeamAsync(teamName);

            await Task.WhenAll(playerTask, teamTask);
            var player = playerTask.Result;
            var team = teamTask.Result;

            var soldPlayer = new SoldPlayer(player, salePrice);


            if (team == null)
            {
                team = new AuctionTeam
                {
                    TeamName = teamName
                };
                await _auctionTeamsRepository.AddAuctionTeamAsync(team);
            }

            return await ProcessSale(soldPlayer, team);
        }

        private async Task<Result<SoldData>> ProcessSale(SoldPlayer soldPlayer, AuctionTeam team)
        {
            var saleValidationResult = PlayerSaleResultValidator.ValidateSale(team, soldPlayer);

            if (saleValidationResult.IsFailed)
            {
                return Result.Fail(saleValidationResult.Errors);
            }

            await _auctionTeamsRepository.AddPlayerToAuctionTeamAsync(team.TeamName, soldPlayer);

            await _playerRepository.SetPlayerAsSold(soldPlayer.PlayerId);

            return new SoldData
            {
                PlayerId = soldPlayer.PlayerId,
                FirstName = soldPlayer.FirstName,
                LastName = soldPlayer.LastName,
                SalePrice = soldPlayer.SalePrice,
                SoldTo = team.TeamName
            }; ;
        }

        private async Task<AuctionTeam> GetAuctionTeamByBidder(string bidder)
        {
            return await _auctionTeamsRepository.GetAuctionTeamByBidderAsync(bidder);
        }
    }
}