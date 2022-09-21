using Microsoft.AspNetCore.Mvc;
using PaycoreProject.Helpers;
using PaycoreProject.Model;

namespace PaycoreProject.Services.Abstract
{
    public interface IOfferService
    {
        BaseResponse<GiveOfferDto> GiveOffer(GiveOfferDto giveOfferDto);
        BaseResponse<IActionResult> OfferApproval(int offerId, int approval);
        BaseResponse<IActionResult> OfferDenied(int offerId, int approval);

    }
}
