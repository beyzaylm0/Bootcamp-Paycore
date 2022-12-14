using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaycoreProject.Model;
using PaycoreProject.Services.Abstract;
using QueueManagement.Mail;
using QueueManagement.ValueObjects;

namespace PaycoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly MailService mailService;

        public UserController(IUserService userService, MailService mailService)
        {
            this.userService = userService;
            this.mailService = mailService;
        }

        //api​/User​/login
        [HttpPost("login")]
        public IActionResult Login(AuthenticateRequest model)
        {
            var response = userService.Authenticate(model);
            return Ok(response);
        }
        [Authorize]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest user)
        {

            userService.Register(user);
            MailModel emailresult = new MailModel()
            {
                Message = "Welcome",
                Email = user.Email,
            };
            mailService.AddToMailQueue(emailresult);
            return Ok(new { message = "Registration successfull" });
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetUserOffer(int userId)
        {
            return Ok(userService.GetUserOffer(userId));
        }
        [Authorize]
        [HttpGet("OfferForProducts")]
        public IActionResult OfferForProducts(int userId)
        {
            return Ok(userService.OfferForProducts(userId));
        }
        //[HttpPost("approval")]
        //public IActionResult OfferApproval(int offerId, int approval)
        //{
        //    return Ok(userService.OfferApproval(offerId,approval));
        //}
        //[HttpPost("denied")]
        //public IActionResult OfferDenied(int offerId, int approval)
        //{
        //    return Ok(userService.OfferDenied(offerId, approval));
        //}
    }
}
