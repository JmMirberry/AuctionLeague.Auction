using AuctionLeague.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuctionLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IPlayerSaleService _playerSaleService;

        public AuctionController(IPlayerSaleService playerSaleService)
        {
            _playerSaleService = playerSaleService;
        }
        
        [HttpDelete]
        [Route("reset-sold")]
        public async Task<IActionResult> ResetSold()
        {
            await _playerSaleService.ResetSold();

            return NoContent();
        }
    }
}
