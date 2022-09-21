using PaycoreProject.Helpers;
using PaycoreProject.Model;
using System.Collections.Generic;

namespace PaycoreProject.Services.Abstract
{
    public interface IProductService
    {
        BaseResponse<ProductDto> GetById(int id);
        BaseResponse<IEnumerable<ProductDto>> GetAll();
        BaseResponse<ProductDto> Insert(ProductDto insertResource);
        BaseResponse<ProductDto> Update(int id, ProductDto updateResource);
        BaseResponse<ProductDto> Remove(int id);
        //BaseResponse<GiveOfferDto> GiveOffer(GiveOfferDto giveOfferDto);
        BaseResponse<SoldDto> Buy(SoldDto soldDto);
    }
}
