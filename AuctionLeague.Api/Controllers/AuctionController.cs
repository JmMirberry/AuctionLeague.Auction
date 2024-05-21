using AuctionLeague.Service.Interfaces;
using AuctionLeague.Service.PlayerSale;
using Microsoft.AspNetCore.Mvc;

namespace AuctionLeague.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IPlayerSaleService _playerSaleService;
        private readonly IApiAuctionService _apiAuctionService;

        public AuctionController(IPlayerSaleService playerSaleService, IApiAuctionService apiAuctionService)
        {
            _playerSaleService = playerSaleService;
            _apiAuctionService = apiAuctionService;
        }

        [HttpPost]
        [Route("nominate-id")]
        public async Task<IActionResult> NominateById(int playerId, string bidder)
        {
            var result = await _apiAuctionService.NominateById(playerId, bidder);
            if (result.IsSuccess) return Ok(result.Value);
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("nominate-name")]
        public async Task<IActionResult> NominateByName(string lastNameSearch, string bidder)
        {
            var result = await _apiAuctionService.NominateByName(lastNameSearch, bidder);
            if (result.IsSuccess) return Ok(result.Value);
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("start-auction")]
        public IActionResult StartAuction()
        {
            if (_apiAuctionService.NominatedPlayer() != null)
            {
                _apiAuctionService.StartAuction();
                return Ok($"Auction started for player id {_apiAuctionService.NominatedPlayer().PlayerId}");

            }
            return BadRequest("No plyer nominated");
        }

        [HttpPost]
        [Route("end-auction")]
        public IActionResult EndAuction()
        {
            if (_apiAuctionService.NominatedPlayer() != null)
            {
                _apiAuctionService.EndAuction();
                return Ok($"Auction ended");

            }
            return BadRequest("No live auction");
        }

        [HttpPost]
        [Route("bid")]
        public IActionResult Bid(int bid, string bidder)
        {
            _apiAuctionService.BidMade(bid, bidder);
            return Ok($"{bidder} bids {bid}");
        }

        [HttpGet]
        [Route("nominated-player")]
        public IActionResult CurrentPlayer()
        {
            var result = _apiAuctionService.NominatedPlayer();
            return Ok(result);
        }

        [HttpGet]
        [Route("current-bid")]
        public IActionResult CurrentBid()
        {
            var result = _apiAuctionService.CurrentBid();
            return Ok(result);
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
