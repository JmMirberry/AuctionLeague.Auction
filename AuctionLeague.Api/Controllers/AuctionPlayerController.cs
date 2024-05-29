using AuctionLeague.Data;
using AuctionLeague.Data.Auction;
using AuctionLeague.MongoDb.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AuctionLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionPlayerController : ControllerBase
    {
        private readonly IAuctionPlayerRepository _playersRepository;

        public AuctionPlayerController(IAuctionPlayerRepository playersRepository) =>
            _playersRepository = playersRepository;

        [HttpGet]
        public async Task<IEnumerable<AuctionPlayer>> Get() =>
            await _playersRepository.GetPlayersAsync();

        [HttpGet("{playerId}")]
        public async Task<ActionResult<AuctionPlayer>> Get(int playerId)
        {
            var player = await _playersRepository.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }

            return player;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AuctionPlayer newPlayer)
        {
            await _playersRepository.AddPlayerAsync(newPlayer);

            return CreatedAtAction(nameof(Get), newPlayer);
        }

        [HttpPut("{playerId}")]
        public async Task<IActionResult> Update(int playerId, AuctionPlayer updatedPlayer)
        {
            var player = await _playersRepository.GetPlayerAsync(playerId);

            if (player is null)
            {
                return NotFound();
            }


            await _playersRepository.UpdatePlayerAsync(playerId, updatedPlayer);

            return NoContent();
        }

        [HttpDelete("{playerId}")]
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
        [Route("Delete-all")]
        public async Task<IActionResult> DeleteAllPlayers()
        {
            await _playersRepository.RemoveAllPlayersAsync();

            return NoContent();
        }
    }
}
