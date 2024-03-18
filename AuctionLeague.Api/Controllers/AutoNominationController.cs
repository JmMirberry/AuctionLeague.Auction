using AuctionLeague.Data;
using AuctionLeague.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuctionLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoNominationController : ControllerBase
    {
        private readonly IAutoNominationService _service;

        public AutoNominationController(IAutoNominationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<Player> Get() => await _service.GetAutoNomination();

        [HttpPost]
        public async Task<IActionResult> Post(List<AutonominationSettings> settings)
        {
            await _service.SetAutoNomination(settings);

            return NoContent();
        }
    }
}
