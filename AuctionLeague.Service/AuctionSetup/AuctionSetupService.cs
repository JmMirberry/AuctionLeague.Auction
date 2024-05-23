using AuctionLeague.Data.Auction;
using AuctionLeague.Data.FplPlayer;
using AuctionLeague.Data.Settings;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.MongoDb.Repositories;
using AuctionLeague.Service.Interfaces;

namespace AuctionLeague.Service
{
    public class AuctionSetupService : IAuctionSetupService
    {
        private readonly IAuctionPlayerRepository _playerRepository;
        private readonly IFplPlayerRepository _fplPlayerRepository;
        private readonly IManualPlayerRepository _manualPlayerRepository;
        private readonly IAutoNominationService _autoNominationService;
        private readonly IAuctionTeamRepository _auctionTeamRepository;

        public AuctionSetupService(IAuctionPlayerRepository playerRepository, IFplPlayerRepository fplPlayerRepository, IManualPlayerRepository manualPlayerRepository, IAutoNominationService autoNominationService, IAuctionTeamRepository auctionTeamRepository)
        {
            _playerRepository = playerRepository;
            _fplPlayerRepository = fplPlayerRepository;
            _manualPlayerRepository = manualPlayerRepository;
            _autoNominationService = autoNominationService;
            _auctionTeamRepository = auctionTeamRepository;
        }

        public async Task InitialiseAuctionData()
        {
            await _playerRepository.RemoveAllPlayersAsync();
            var fplPlayersTask = _fplPlayerRepository.GetPlayersAsync();
            var manualPlayersTask = _manualPlayerRepository.GetPlayersAsync();


            await _playerRepository.AddPlayersAsync(ToDefaultAuctionPlayers(await fplPlayersTask).Concat(ToDefaultAuctionPlayers(await manualPlayersTask)));
        }

        public async Task SetAutoNomination(List<AutonominationSettings> settings)
        {
            await _autoNominationService.SetAutoNominations( settings);
        }

        public async Task ResetSold()
        {
            var resetSold = _playerRepository.ResetSold();
            var removeFromTeams = _auctionTeamRepository.RemovePlayersFromAllAuctionTeams();

            await Task.WhenAll(resetSold, removeFromTeams);
        }

        private IEnumerable<AuctionPlayer> ToDefaultAuctionPlayers(IEnumerable<Player> fplPlayers)
        {
            return fplPlayers.Select(player => new AuctionPlayer
            {
                PlayerId = player.PlayerId,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Team = player.Team,
                Position = player.Position,
                Value = player.Value,
                TotalPointsPreviousYear = player.TotalPointsPreviousYear,
                IsFplPlayer = true,
                IsAutonomination = false,
                IsSold = false
            });
        }

        private IEnumerable<AuctionPlayer> ToDefaultAuctionPlayers(IEnumerable<ManualPlayer> manualPlayers)
        {
            return manualPlayers.Select(player => new AuctionPlayer
            {
                PlayerId = player.PlayerId,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Team = player.Team,
                Position = player.Position,
                IsFplPlayer = true,
                IsAutonomination = false,
                IsSold = false
            });
        }
    }
}
