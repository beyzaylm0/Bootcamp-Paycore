using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using PaycoreProject.Helpers;
using PaycoreProject.Model;
using PaycoreProject.Repository;
using PaycoreProject.Services.Abstract;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PaycoreProject.Services.Concrete
{
    public class ProductService : IProductService
    {

        private readonly ISession session;
        private readonly IMapper mapper;
        private readonly IHibernateRepository<Product> hibernateRepository;
        private readonly IHibernateRepository<Category> hibernateCategoryRepository;
        private readonly IHibernateRepository<User> hibernateUserRepository;
        private readonly IHibernateRepository<GiveOffer> hibernateOfferRepository;
        private readonly IHibernateRepository<Sold> hibernateSoldRepository;
        private readonly ILogger<ProductService> logger;
        public ProductService(IMapper mapper, ISession session, ILogger<ProductService> logger)
        {
            this.session = session;
            this.mapper = mapper;

            hibernateRepository = new HibernateRepository<Product>(session);
            hibernateCategoryRepository = new HibernateRepository<Category>(session);
            hibernateUserRepository = new HibernateRepository<User>(session);
            hibernateOfferRepository = new HibernateRepository<GiveOffer>(session);
            hibernateSoldRepository = new HibernateRepository<Sold>(session);
            this.logger = logger;
        }
        //This method fetch all products
        public BaseResponse<IEnumerable<ProductDto>> GetAll()
        {
            try
            {
                var tempEntity = hibernateRepository.Entities.ToList();
                var result = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(tempEntity);
                return new BaseResponse<IEnumerable<ProductDto>>(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return new BaseResponse<IEnumerable<ProductDto>>(e.Message);
            }
        }
        //This method returns product by product id
        public BaseResponse<ProductDto> GetById(int id)
        {
            try
            {
                var tempEntity = hibernateRepository.GetById(id);
                var result = mapper.Map<Product, ProductDto>(tempEntity);
                return new BaseResponse<ProductDto>(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new BaseResponse<ProductDto>(ex.Message);
            }
        }
        // This method adds product
        public BaseResponse<ProductDto> Insert(ProductDto insertResource)
        {
            try
            {
                var tempEntity = mapper.Map<ProductDto, Product>(insertResource);
                var category = hibernateCategoryRepository.GetAll().Find(x => x.Id == insertResource.Category.Id);
                var user = hibernateUserRepository.GetById(tempEntity.UserId);
                if (category == null)
                {
                    return new BaseResponse<ProductDto>("Category not found.");
                }
                if (user == null)
                {
                    return new BaseResponse<ProductDto>("User not found.");
                }
                hibernateRepository.BeginTransaction();
                hibernateRepository.Save(tempEntity);
                hibernateRepository.Commit();

                hibernateRepository.CloseTransaction();
                return new BaseResponse<ProductDto>(mapper.Map<Product, ProductDto>(tempEntity));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                hibernateRepository.Rollback();
                hibernateRepository.CloseTransaction();
                return new BaseResponse<ProductDto>(ex.Message);
            }
        }
        //This method deletes product
        public BaseResponse<ProductDto> Remove(int id)
        {
            try
            {
                var tempEntity = hibernateRepository.GetById(id);
                if (tempEntity is null)
                {
                    return new BaseResponse<ProductDto>("Record Not Found");
                }

                hibernateRepository.BeginTransaction();
                hibernateRepository.Delete(id);
                hibernateRepository.Commit();
                hibernateRepository.CloseTransaction();

                return new BaseResponse<ProductDto>(mapper.Map<Product, ProductDto>(tempEntity));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                hibernateRepository.Rollback();
                hibernateRepository.CloseTransaction();
                return new BaseResponse<ProductDto>(ex.Message);
            }
        }
        // This method updates product
        public BaseResponse<ProductDto> Update(int id, ProductDto updateResource)
        {
            try
            {
                var tempEntity = hibernateRepository.GetById(id);
                if (tempEntity is null)
                {
                    return new BaseResponse<ProductDto>("Record Not Found");
                }


                var entity = mapper.Map(updateResource, tempEntity);

                hibernateRepository.BeginTransaction();
                hibernateRepository.Update(entity);
                hibernateRepository.Commit();
                hibernateRepository.CloseTransaction();

                var resource = mapper.Map<Product, ProductDto>(entity);
                return new BaseResponse<ProductDto>(resource);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                hibernateRepository.Rollback();
                hibernateRepository.CloseTransaction();
                return new BaseResponse<ProductDto>(ex.Message);
            }
        }
       
        public BaseResponse<SoldDto> Buy(SoldDto soldDto)
        {
            try
            {

                var tempEntity = mapper.Map<SoldDto, Sold>(soldDto);
                var user = hibernateUserRepository.GetAll().Find(x => x.Id == tempEntity.User.Id);
                var product = hibernateRepository.GetAll().Find(x => x.Id == tempEntity.Product.Id);

                if (product.isSold == true)
                {
                    return new BaseResponse<SoldDto>("This item has been sold.");
                }
                product.isSold = tempEntity.IsSold;
                if (user == null)
                {
                    return new BaseResponse<SoldDto>("User not found");
                }
                if (product == null)
                {
                    return new BaseResponse<SoldDto>("Product not found");
                }

                hibernateRepository.BeginTransaction();
                hibernateRepository.Update(product);
                hibernateRepository.Commit();

                hibernateRepository.CloseTransaction();
                return new BaseResponse<SoldDto>(mapper.Map<Sold, SoldDto>(tempEntity));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                hibernateSoldRepository.Rollback();
                hibernateSoldRepository.CloseTransaction();
                return new BaseResponse<SoldDto>(ex.Message);
            }
        }
    }
}
