using AutoMapper;
using NHibernate;
using NHibernate.Mapping.ByCode.Impl;
using PaycoreProject.Authenticate;
using PaycoreProject.Helpers;
using PaycoreProject.Model;
using PaycoreProject.Repository;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System;
using PaycoreProject.Services.Abstract;

namespace PaycoreProject.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ISession session;
        private readonly IMapper mapper;
        private readonly IHibernateRepository<Category> hibernateRepository;

        public CategoryService(IMapper mapper, ISession session)
        {
            this.session = session;
            this.mapper = mapper;

            hibernateRepository = new HibernateRepository<Category>(session);
        }

        //This method get all list category 
        public BaseResponse<IEnumerable<CategoryDto>> GetAll()
        {
            try
            {
                var tempEntity = hibernateRepository.Entities.ToList();
                var result = mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(tempEntity);
                return new BaseResponse<IEnumerable<CategoryDto>>(result);
            }
            catch (Exception ex)
            {
                Log.Error("GetAll", ex);
                return new BaseResponse<IEnumerable<CategoryDto>>(ex.Message);
            }
        }
        public BaseResponse<CategoryDto> GetById(int id)
        {
            try
            {
                var tempEntity = hibernateRepository.GetById(id);
                var result = mapper.Map<Category, CategoryDto>(tempEntity);
                return new BaseResponse<CategoryDto>(result);
            }
            catch (Exception ex)
            {
                Log.Error("GetById", ex);
                return new BaseResponse<CategoryDto>(ex.Message);
            }
        }
        //This method adds category
        public BaseResponse<CategoryDto> Insert(CategoryDto insertResource)
        {
            try
            {
                var tempEntity = mapper.Map<CategoryDto, Category>(insertResource);

                hibernateRepository.BeginTransaction();
                hibernateRepository.Save(tempEntity);
                hibernateRepository.Commit();

                hibernateRepository.CloseTransaction();
                return new BaseResponse<CategoryDto>(mapper.Map<Category, CategoryDto>(tempEntity));
            }
            catch (Exception ex)
            {
                Log.Error("Insert", ex);
                hibernateRepository.Rollback();
                hibernateRepository.CloseTransaction();
                return new BaseResponse<CategoryDto>(ex.Message);
            }

        }
        //This method delets category
        public BaseResponse<CategoryDto> Remove(int id)
        {
            try
            {
                var tempEntity = hibernateRepository.GetById(id);
                if (tempEntity is null)
                {
                    return new BaseResponse<CategoryDto>("Record Not Found");
                }

                hibernateRepository.BeginTransaction();
                hibernateRepository.Delete(id);
                hibernateRepository.Commit();
                hibernateRepository.CloseTransaction();

                return new BaseResponse<CategoryDto>(mapper.Map<Category, CategoryDto>(tempEntity));
            }
            catch (Exception ex)
            {
                Log.Error("Remove", ex);
                hibernateRepository.Rollback();
                hibernateRepository.CloseTransaction();
                return new BaseResponse<CategoryDto>(ex.Message);
            }
        }
        //This method delets category
        public BaseResponse<CategoryDto> Update(int id, CategoryDto updateResource)
        {
            try
            {
                var tempEntity = hibernateRepository.GetById(id);
                if (tempEntity is null)
                {
                    return new BaseResponse<CategoryDto>("Record Not Found");
                }


                var entity = mapper.Map(updateResource, tempEntity);

                hibernateRepository.BeginTransaction();
                hibernateRepository.Update(entity);
                hibernateRepository.Commit();
                hibernateRepository.CloseTransaction();

                var resource = mapper.Map<Category, CategoryDto>(entity);
                return new BaseResponse<CategoryDto>(resource);
            }
            catch (Exception ex)
            {
                Log.Error("Update", ex);
                hibernateRepository.Rollback();
                hibernateRepository.CloseTransaction();
                return new BaseResponse<CategoryDto>(ex.Message);
            }
        }
    }
}

