using Microsoft.AspNetCore.Mvc;
using PaycoreProject.Helpers;
using PaycoreProject.Model;

namespace PaycoreProject.Services.Abstract
{
    public interface IOfferService
    {
        BaseResponse<GiveOfferDto> GiveOffer(GiveOfferDto giveOfferDto);
        BaseResponse<IActionResult> OfferApproval(int offerId);
        BaseResponse<IActionResult> OfferDenied(int offerId);

    }
}
