using AuctionLeague.Data;
using AuctionLeague.Fpl;
using Microsoft.AspNetCore.Mvc;

namespace AuctionLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FplController : ControllerBase
    {
        private readonly IFplService _fplService;

        public FplController(IFplService fplService) =>
            _fplService = fplService;

        [HttpGet]
        public async Task<IEnumerable<Player>> Get() => await _fplService.PopulateFplData();
    }
}
