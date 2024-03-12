using AuctionLeague.Data;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AuctionLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRepository _playersRepository;

        public PlayersController(IPlayerRepository playersRepository) =>
            _playersRepository = playersRepository;

        [HttpGet]
        public async Task<List<Player>> Get() =>
            await _playersRepository.GetPlayersAsync();

        [HttpGet("{playerId:length(24)}")]
        public async Task<ActionResult<Player>> Get(int playerId)
        {
            var player = await _playersRepository.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }

            return player;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Player newPlayer)
        {
            await _playersRepository.AddPlayerAsync(newPlayer);

            return CreatedAtAction(nameof(Get), newPlayer);
        }

        [HttpPut("{playerId:length(24)}")]
        public async Task<IActionResult> Update(int playerId, Player updatedPlayer)
        {
            var player = await _playersRepository.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }


            await _playersRepository.UpdatePlayerAsync(playerId, updatedPlayer);

            return NoContent();
        }

        [HttpDelete("{playerId:length(24)}")]
        public async Task<IActionResult> Delete(int playerId)
        {
            var player = await _playersRepository.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }

            await _playersRepository.RemovePlayerAsync(playerId);

            return NoContent();
        }
        
        [HttpDelete()]
        [Route("Delete-Fpl")]
        public async Task<IActionResult> DeleteFplPLayers()
        {
            await _playersRepository.RemoveAllFplPlayersAsync();

            return NoContent();
        }
    }
}
