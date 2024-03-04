using AuctionLeague.MongoDb.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Entities;

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
            await _playersService.GetAsync();

        [HttpGet("{playerId:length(24)}")]
        public async Task<ActionResult<PlayerEntity>> Get(int playerId)
        {
            var player = await _playersService.GetAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }

            return player;
        }

        [HttpPost]
        public async Task<IActionResult> Post(PlayerEntity newPlayer)
        {
            await _playersService.CreateAsync(newPlayer);

            return CreatedAtAction(nameof(Get), newPlayer);
        }

        [HttpPut("{playerId:length(24)}")]
        public async Task<IActionResult> Update(int playerId, PlayerEntity updatedPlayer)
        {
            var player = await _playersService.GetAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }


            await _playersService.UpdateAsync(playerId, updatedPlayer);

            return NoContent();
        }

        [HttpDelete("{playerId:length(24)}")]
        public async Task<IActionResult> Delete(int playerId)
        {
            var player = await _playersService.GetAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }

            await _playersService.RemoveAsync(playerId);

            return NoContent();
        }
    }
}
