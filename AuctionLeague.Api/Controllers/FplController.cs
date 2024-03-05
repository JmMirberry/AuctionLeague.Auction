using AuctionLeague.Data;
using AuctionLeague.Fpl;
using AuctionLeague.MongoDb.Entities;
using AuctionLeague.MongoDb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace SlackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FplController : ControllerBase
    {
        private readonly IFplService _fplService;

        public FplController(IFplService fplService) =>
            _fplService = fplService;

        [HttpGet]
        public async Task<List<Player>> Get() => await _fplService.PopulateFplData();
    }
}
