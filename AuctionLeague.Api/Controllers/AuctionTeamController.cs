using AuctionLeague.Data.Auction;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.SaleService;
using Microsoft.AspNetCore.Mvc;

namespace AuctionLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionTeamController : ControllerBase
    {
        private readonly IAuctionTeamRepository _auctionTeamsRepository;
        private readonly IAuctionPlayerRepository _playerRepository;
        private readonly IPlayerSaleService _playerSaleService;

        public AuctionTeamController(IAuctionTeamRepository auctionTeamsRepository, IPlayerSaleService playerSaleService, IAuctionPlayerRepository playerRepository)
        {
            _auctionTeamsRepository = auctionTeamsRepository;
            _playerSaleService = playerSaleService;
            _playerRepository = playerRepository;
        }

        [HttpGet]
        [Route("all-teams")]
        public async Task<IEnumerable<AuctionTeam>> Get() =>
            await _auctionTeamsRepository.GetAuctionTeamsAsync();

        [HttpGet("{auctionTeamName}")]
        public async Task<ActionResult<AuctionTeam>> Get(string auctionTeamName)
        {
            var auctionTeam = await _auctionTeamsRepository.GetAuctionTeamAsync(auctionTeamName);

            if (auctionTeam is null)
            {
                return NotFound();
            }

            return auctionTeam;
        }

        [HttpPut]
        public async Task<IActionResult> Post(AuctionTeam newAuctionTeam)
        {
            await _auctionTeamsRepository.AddAuctionTeamAsync(newAuctionTeam);

            return CreatedAtAction(nameof(Get), newAuctionTeam);
        }

        [HttpPut]
        [Route("manually-add-player")]
        public async Task<IActionResult> ManuallyAddPlayer(string teamName, SoldPlayer player)
        {
            var result = await _playerSaleService.ProcessSaleByTeamName(player, teamName);

            return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
        }
        
        [HttpPut]
        [Route("add-player/{auctionTeamName}/{playerId:int}/{salePrice:int}")]
        public async Task<IActionResult> AddPlayer(string auctionTeamName, int playerId, int salePrice)
        {
            var player = await _playerRepository.GetPlayerAsync(playerId);

            if (player == null )
            {
                return BadRequest($"No player found with id {playerId}");
            }

            if (player.IsSold)
            {
                return BadRequest($"Player is has already been sold");
            }

            var soldPlayer = new SoldPlayer(player, salePrice, DateTime.Now);
            
            var result = await _playerSaleService.ProcessSaleByTeamName(soldPlayer, auctionTeamName);

            return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
        }
        
        [HttpDelete]
        [Route("/remove-players/{auctionTeamName}")]
        public async Task<IActionResult> DeletePlayers(string auctionTeamName)
        {
            var auctionTeam = await _auctionTeamsRepository.GetAuctionTeamAsync(auctionTeamName);

            if (auctionTeam is null)
            {
                return NotFound();
            }

            await _auctionTeamsRepository.RemovePlayersFromAuctionTeamAsync(auctionTeamName);

            return NoContent();
        }
        
        [HttpDelete]
        [Route("/remove-player/{auctionTeamName}/{playerId:int}")]
        public async Task<IActionResult> DeletePlayer(string auctionTeamName, int playerId)
        {
            var auctionTeam = await _auctionTeamsRepository.GetAuctionTeamAsync(auctionTeamName);

            if (auctionTeam is null)
            {
                return NotFound();
            }

            await _auctionTeamsRepository.RemovePlayerFromAuctionTeamAsync(auctionTeamName, playerId);
            await _playerRepository.SetPlayerAsNotSold(playerId);
            
            return NoContent();
        }
        
        [HttpDelete]
        [Route("/remove-all-players")]
        public async Task<IActionResult> DeletePlayersFromAllTeams()
        {
            await _auctionTeamsRepository.RemovePlayersFromAllAuctionTeams();

            return NoContent();
        }

        [HttpDelete]
        [Route("delete-team/{auctionTeamName}")]
        public async Task<IActionResult> Delete(string auctionTeamName)
        {
            var auctionTeam = await _auctionTeamsRepository.GetAuctionTeamAsync(auctionTeamName);

            if (auctionTeam is null)
            {
                return NotFound();
            }

            await _auctionTeamsRepository.RemoveAuctionTeamAsync(auctionTeamName);

            return NoContent();
        }

        [HttpDelete]
        [Route("delete-all")]
        public async Task<IActionResult> DeleteAuctionTeams()
        {
            await _auctionTeamsRepository.RemoveAllAuctionTeamsAsync();

            return NoContent();
        }
        
    }
}
