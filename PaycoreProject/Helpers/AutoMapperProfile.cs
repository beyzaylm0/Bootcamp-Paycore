using AutoMapper;
using PaycoreProject.Model;

namespace PaycoreProject.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AuthenticateResponse>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<Sold, SoldDto>();
            CreateMap<SoldDto, Sold>();
            CreateMap<GiveOffer, GiveOfferDto>();
            CreateMap<GiveOfferDto, GiveOffer>();
            // RegisterRequest -> User
            CreateMap<RegisterRequest, User>();

        }
    }
}
