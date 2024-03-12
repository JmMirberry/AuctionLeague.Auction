using AuctionLeague.Data;
using AuctionLeague.Data.Exceptions;
using AuctionLeague.MongoDb.Abstractions;
using AuctionLeague.Service;
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
        public async Task<List<AuctionTeam>> Get() =>
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

        [HttpPost]
        public async Task<IActionResult> Post(AuctionTeam newAuctionTeam)
        {
            await _auctionTeamsRepository.AddAuctionTeamAsync(newAuctionTeam);

            return CreatedAtAction(nameof(Get), newAuctionTeam);
        }

        [HttpPost]
        [Route("add-player")]
        public async Task<IActionResult> AddPlayer(string teamName, SoldPlayer player)
        {
            try
            {
                await _playerSaleService.ProcessSaleByTeamName(player, teamName, true);

                return NoContent();
            }
            catch (PlayerSaleException e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpPut("{auctionTeamName:length(24)}")]
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
        
        [HttpDelete("/{auctionTeamName:length(24)}")]
        [Route("/remove-players")]
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
        
        [HttpDelete()]
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

        [HttpDelete("{AuctionTeamId:length(24)}")]
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

        [HttpDelete()]
        [Route("delete-all")]
        public async Task<IActionResult> DeleteAuctionTeams()
        {
            await _auctionTeamsRepository.RemoveAllAuctionTeamsAsync();

            return NoContent();
        }
        
    }
}
