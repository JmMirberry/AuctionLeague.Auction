using AuctionLeague.MongoDb.Entities;
using AuctionLeague.MongoDb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace SlackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerRepository _playersService;

        public PlayersController(PlayerRepository playersService) =>
            _playersService = playersService;

        [HttpGet]
        public async Task<List<PlayerEntity>> Get() =>
            await _playersService.GetPlayersAsync();

        [HttpGet("{playerId:length(24)}")]
        public async Task<ActionResult<PlayerEntity>> Get(int playerId)
        {
            var player = await _playersService.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }

            return player;
        }

        [HttpPost]
        public async Task<IActionResult> Post(PlayerEntity newPlayer)
        {
            await _playersService.AddPlayerAsync(newPlayer);

            return CreatedAtAction(nameof(Get), newPlayer);
        }

        [HttpPut("{playerId:length(24)}")]
        public async Task<IActionResult> Update(int playerId, PlayerEntity updatedPlayer)
        {
            var player = await _playersService.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }


            await _playersService.UpdatePlayerAsync(playerId, updatedPlayer);

            return NoContent();
        }

        [HttpDelete("{playerId:length(24)}")]
        public async Task<IActionResult> Delete(int playerId)
        {
            var player = await _playersService.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }

            await _playersService.RemovePlayerAsync(playerId);

            return NoContent();
        }
        
        [HttpDelete()]
        [Route("Delete-Fpl")]
        public async Task<IActionResult> DeleteFplPLayers()
        {
            await _playersService.RemoveAllFplPlayersAsync();

            return NoContent();
        }
    }
}
