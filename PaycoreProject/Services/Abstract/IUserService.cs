
using Microsoft.AspNetCore.Mvc;
using PaycoreProject.Helpers;
using PaycoreProject.Model;
using System.Collections.Generic;

namespace PaycoreProject.Services.Abstract
{
    public interface IUserService
    {
        BaseResponse<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Register(RegisterRequest model);
        BaseResponse<IEnumerable<GiveOfferResult>> GetUserOffer(int userId);
        BaseResponse<IEnumerable<GiveOfferResult>> OfferForProducts(int userId);
        //BaseResponse<IActionResult> OfferApproval(int offerId, int approval);
        //BaseResponse<IActionResult> OfferDenied(int offerId, int approval);
     
    }
}
