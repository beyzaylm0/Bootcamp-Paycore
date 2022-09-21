using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaycoreProject.Model;
using PaycoreProject.Services.Abstract;
using PaycoreProject.Services.Concrete;

namespace PaycoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {

        private readonly IUserService userService;
        private readonly IOfferService offerService;

        public OfferController(IUserService userService, IOfferService offerService)
        {
            this.userService = userService;
            this.offerService = offerService;
        }

        [HttpPost("giveoffer")]
        public virtual IActionResult GiveOffer([FromBody] GiveOfferDto dto)
        {
            var result = offerService.GiveOffer(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            if (result.Response is null)
            {
                return NoContent();
            }

            if (result.Success)
            {
                return StatusCode(201, result);
            }

            return BadRequest(result);
        }
        [HttpPost("approval")]
        public IActionResult OfferApproval(int offerId)
        {
            return Ok(offerService.OfferApproval(offerId));
        }
        [HttpPost("denied")]
        public IActionResult OfferDenied(int offerId)
        {
            return Ok(offerService.OfferDenied(offerId));
        }
    }
}
