using AuctionLeague.Data.Settings;
using AuctionLeague.Service;
using AuctionLeague.Service.Interfaces;
using AuctionLeague.Service.PlayerSale;
using Microsoft.AspNetCore.Mvc;

namespace AuctionLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionSetupController : ControllerBase
    {
        private readonly IAuctionSetupService _auctionSetupService;

        public AuctionSetupController(IAuctionSetupService auctionSetupService)
        {
            _auctionSetupService = auctionSetupService;
        }

        [HttpPost]
        [Route("initialise-data")]
        public async Task<IActionResult> InitialiseData()
        {
            await _auctionSetupService.InitialiseAuctionData();

            return NoContent();
        }

        [HttpPost]
        [Route("reset-sold")]
        public async Task<IActionResult> ResetSold()
        {
            await _auctionSetupService.ResetSold();

            return NoContent();
        }       

        [HttpPost]
        [Route("set-auto-nomination")]
        public async Task<IActionResult> SetAutoNomination(List<AutonominationSettings> settings)
        {
            await _auctionSetupService.SetAutoNomination(settings);

            return NoContent();
        }
    }
}
