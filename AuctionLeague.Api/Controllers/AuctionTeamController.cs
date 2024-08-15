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
        private readonly IPlayerSaleService _playerSaleService;

        public AuctionTeamController(IAuctionTeamRepository auctionTeamsRepository, IPlayerSaleService playerSaleService)
        {
            _auctionTeamsRepository = auctionTeamsRepository;
            _playerSaleService = playerSaleService;
        }

        [HttpGet]
        [Route("all-teams")]
        public async Task<IEnumerable<AuctionTeam>> Get() =>
            await _auctionTeamsRepository.GetAuctionTeamsAsync();

        [HttpGet("{AuctionTeamName:length(24)}")]
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
        [Route("add-player")]
        public async Task<IActionResult> AddPlayer(string teamName, SoldPlayer player)
        {
            var result = await _playerSaleService.ProcessSaleByTeamName(player, teamName);

            return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(string auctionTeamName, AuctionTeam updatedAuctionTeam)
        {
            var auctionTeam = await _auctionTeamsRepository.GetAuctionTeamAsync(auctionTeamName);

            if (auctionTeam is null)
            {
                return NotFound();
            }

            await _auctionTeamsRepository.UpdateAuctionTeamAsync(updatedAuctionTeam);

            return NoContent();
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
        [Route("/remove-all-players")]
        public async Task<IActionResult> DeletePlayersFromAllTeams(string auctionTeamName)
        {
            var auctionTeam = await _auctionTeamsRepository.GetAuctionTeamAsync(auctionTeamName);

            if (auctionTeam is null)
            {
                return NotFound();
            }

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
