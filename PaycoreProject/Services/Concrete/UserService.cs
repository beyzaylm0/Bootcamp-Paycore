using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode.Impl;
using NHibernate.Util;
using PaycoreProject.Authenticate;
using PaycoreProject.Helpers;
using PaycoreProject.Model;
using PaycoreProject.Repository;
using PaycoreProject.Services.Abstract;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using BCryptNet = BCrypt.Net.BCrypt;

namespace PaycoreProject.Services.Concrete
{
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;
        private readonly IHibernateRepository<User> _hibernateRepository;
        private readonly IHibernateRepository<GiveOffer> _hibernateOfferRepository;
        private readonly IHibernateRepository<Product> _hibernateProductRepository;
        private IJwtUtils _jwtUtils;
        private ILogger<UserService> _logger;

        public UserService(IMapper mapper, IHibernateRepository<User> hibernateRepository, IJwtUtils jwtUtils, IHibernateRepository<GiveOffer> hibernateOfferRepository,
            IHibernateRepository<Product> hibernateProductRepository, ILogger<UserService> logger)
        {
            _hibernateRepository = hibernateRepository;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
            _hibernateOfferRepository = hibernateOfferRepository;
            _hibernateProductRepository = hibernateProductRepository;
            _logger = logger;
        }

        public IEnumerable<User> GetAll()
        {
            return _hibernateRepository.Entities;
        }
        public User GetById(int id)
        {
            return getUser(id);
        }
        private User getUser(int id)
        {
            var user = _hibernateRepository.Entities.FirstOrDefault(x => x.Id == id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
        public BaseResponse<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            try
            {
                var user = _hibernateRepository.Entities.SingleOrDefault(x => x.Email == model.Email);

                // validate
                if (user == null || !BCryptNet.Verify(model.Password, user.Password))
                    throw new AppException("Username or password is incorrect");

                // authentication successful

                var response = _jwtUtils.GenerateToken(model);
                return response;
            }
            catch (Exception e)
            {

                _logger.LogError(e.ToString());
                return new BaseResponse<AuthenticateResponse>(e.Message);

            }

        }


        // method of user registration
        public void Register(RegisterRequest model)
        {
            try
            {
                _hibernateRepository.BeginTransaction();
                var user = _mapper.Map<User>(model);

                // hash password
                user.Password = BCryptNet.HashPassword(model.Password);

                // save user
                _hibernateRepository.Save(user);
                _hibernateRepository.Commit();
            }
            catch (Exception e)
            {
                _hibernateRepository.Rollback();
                _logger.LogError(e.ToString());
            }
            finally
            {
                _hibernateRepository.CloseTransaction();
            }
        }

        //method to list user offers
        public BaseResponse<IEnumerable<GiveOfferResult>> GetUserOffer(int userId)
        {
            try
            {
                var tempEntity = _hibernateOfferRepository.Entities.Where(x => x.BidderUser.Id == userId).ToList();
                var resultList = new List<GiveOfferResult>();
                for (int i = 0; i < tempEntity.Count; i++)
                {
                    resultList.Add(new GiveOfferResult()
                    {
                        Id = tempEntity[i].Id,
                        ProductId = tempEntity[i].ProductId.Id,
                        BidderUser = tempEntity[i].BidderUser.Id,
                        Offer = tempEntity[i].Offer,
                    });

                }
                return new BaseResponse<IEnumerable<GiveOfferResult>>(resultList);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new BaseResponse<IEnumerable<GiveOfferResult>>(e.Message);
            }
        }
        //method that lists offers of the user's products
        public BaseResponse<IEnumerable<GiveOfferResult>> OfferForProducts(int userId)
        {
            try
            {
                var tempEntity = _hibernateProductRepository.Entities.Where(x => x.UserId == userId).ToList();
                var resultList = new List<GiveOfferResult>();

                var tempoffer = _hibernateOfferRepository.GetAll().ToList();
                for (int i = 0; i < tempEntity.Count; i++)
                {
                    for (int a = 0; a < tempoffer.Count; a++)
                    {
                        if (tempEntity[i].Id == tempoffer[a].ProductId.Id)
                            resultList.Add(new GiveOfferResult()
                            {
                                Id = tempoffer[a].Id,
                                ProductId = tempoffer[a].ProductId.Id,
                                BidderUser = tempoffer[a].BidderUser.Id,
                                Offer = tempoffer[a].Offer,
                            });

                    }
                }

                return new BaseResponse<IEnumerable<GiveOfferResult>>(resultList);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new BaseResponse<IEnumerable<GiveOfferResult>>(e.Message);
            }
        }

       
    }

}





