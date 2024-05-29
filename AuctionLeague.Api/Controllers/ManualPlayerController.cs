using AuctionLeague.Data.FplPlayer;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AuctionLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManualPlayerController : ControllerBase
    {
        private readonly IManualPlayerRepository _playerRepository;

        public ManualPlayerController(IManualPlayerRepository playerRepository) =>
            _playerRepository = playerRepository;

        [HttpGet]
        public async Task<IEnumerable<ManualPlayer>> Get() =>
            await _playerRepository.GetPlayersAsync();

        [HttpGet("{playerId}")]
        public async Task<ActionResult<ManualPlayer>> Get(int playerId)
        {
            var player = await _playerRepository.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound("Player not found");
            }

            return Ok(player);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(ManualPlayer newPlayer)
        {
            await _playerRepository.AddPlayerAsync(newPlayer);

            return CreatedAtAction(nameof(Get), newPlayer);
        }

        [HttpPut("{playerId}")]
        public async Task<IActionResult> Update(int playerId, ManualPlayer updatedPlayer)
        {
            var player = await _playerRepository.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }


            await _playerRepository.UpdatePlayerAsync(playerId, updatedPlayer);

            return NoContent();
        }

        [HttpDelete("{playerId}")]
        public async Task<IActionResult> Delete(int playerId)
        {
            var player = await _playerRepository.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound("Player not found");
            }

            await _playerRepository.RemovePlayerAsync(playerId);

            return NoContent();
        }
        
        [HttpDelete]
        [Route("Delete-all")]
        public async Task<IActionResult> DeletePlayers()
        {
            await _playerRepository.RemoveAllPlayersAsync();

            return NoContent();
        }
    }
}
