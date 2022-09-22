using API.Enum;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using PaycoreProject.Helpers;
using PaycoreProject.Model;
using PaycoreProject.Repository;
using PaycoreProject.Services.Abstract;
using QueueManagement.Mail;
using QueueManagement.ValueObjects;
using Serilog;
using System;
using System.Linq;

namespace PaycoreProject.Services.Concrete
{
    public class OfferService : IOfferService
    {
        private readonly ISession session;
        private readonly IMapper mapper;
        private readonly IHibernateRepository<Product> hibernateProductRepository;
        private readonly IHibernateRepository<User> hibernateUserRepository;
        private readonly IHibernateRepository<GiveOffer> hibernateOfferRepository;
        private readonly MailService mailService;
        public OfferService(IMapper mapper, ISession session, MailService mailService)
        {
            this.session = session;
            this.mapper = mapper;

            hibernateProductRepository = new HibernateRepository<Product>(session);
            hibernateUserRepository = new HibernateRepository<User>(session);
            hibernateOfferRepository = new HibernateRepository<GiveOffer>(session);
            this.mailService = mailService;
        }
        //This method bids the product
        public BaseResponse<GiveOfferDto> GiveOffer(GiveOfferDto giveOfferDto)
        {
            try
            {

                var tempEntity = mapper.Map<GiveOfferDto, GiveOffer>(giveOfferDto);
                var user = hibernateUserRepository.GetAll().Find(x => x.Id == tempEntity.BidderUser.Id);
                var product = hibernateProductRepository.GetAll().Find(x => x.Id == tempEntity.ProductId.Id);

                if (user == null)
                {
                    return new BaseResponse<GiveOfferDto>("User not found");
                }
                if (product == null)
                {
                    return new BaseResponse<GiveOfferDto>("Product not found");
                }
                if (product.isOfferable == false)
                {
                    return new BaseResponse<GiveOfferDto>("This product can not be bid.");
                }

                hibernateOfferRepository.BeginTransaction();
                hibernateOfferRepository.Save(tempEntity);
                hibernateOfferRepository.Commit();

                hibernateOfferRepository.CloseTransaction();

                MailModel emailresult = new MailModel()
                {
                    Message = "Offer send",
                    Email = user.Email,
                };
                mailService.AddToMailQueue(emailresult);
                return new BaseResponse<GiveOfferDto>(mapper.Map<GiveOffer, GiveOfferDto>(tempEntity));
            }
            catch (Exception ex)
            {
                Log.Error("Insert", ex);
                hibernateOfferRepository.Rollback();
                hibernateOfferRepository.CloseTransaction();
                return new BaseResponse<GiveOfferDto>(ex.Message);
            }
        }
        //This method accepts incoming offers
        public BaseResponse<IActionResult> OfferApproval(int offerId)
        {
            try
            {

                var tempEntity = hibernateOfferRepository.Entities.FirstOrDefault(x => x.Id == offerId);

                var tempProduct = hibernateProductRepository.GetAll().FirstOrDefault(x => x.Id == tempEntity.Id);
                tempProduct.isSold = true;
                tempEntity.ApprovalStatus = (int)ApprovalStatusEnum.Approval;
                hibernateProductRepository.BeginTransaction();
                hibernateProductRepository.Update(tempProduct);
                hibernateProductRepository.Commit();
                hibernateProductRepository.CloseTransaction();

                hibernateOfferRepository.BeginTransaction();
                hibernateOfferRepository.Update(tempEntity);
                hibernateOfferRepository.Commit();
                hibernateOfferRepository.CloseTransaction();

                return new BaseResponse<IActionResult>("Offer approved");
            }
            catch (Exception ex)
            {
                hibernateProductRepository.Rollback();
                hibernateProductRepository.CloseTransaction();
                hibernateOfferRepository.Rollback();
                hibernateOfferRepository.CloseTransaction();
                Log.Error("update", ex);
                return new BaseResponse<IActionResult>(ex.Message);
            }
        }
        //This method rejects incoming offers
        public BaseResponse<IActionResult> OfferDenied(int offerId)
        {
            try
            {

                var tempEntity = hibernateOfferRepository.Entities.FirstOrDefault(x => x.Id == offerId);

                var tempProduct = hibernateProductRepository.GetAll().FirstOrDefault(x => x.Id == tempEntity.ProductId.Id);
                tempProduct.isSold = false;
                tempEntity.ApprovalStatus = (int)ApprovalStatusEnum.Denied;
                hibernateProductRepository.BeginTransaction();
                hibernateProductRepository.Update(tempProduct);
                hibernateProductRepository.Commit();
                hibernateProductRepository.CloseTransaction();

                hibernateOfferRepository.BeginTransaction();
                hibernateOfferRepository.Update(tempEntity);
                hibernateOfferRepository.Commit();
                hibernateOfferRepository.CloseTransaction();

                return new BaseResponse<IActionResult>("Offer denied");




            }
            catch (Exception ex)
            {
                hibernateProductRepository.Rollback();
                hibernateProductRepository.CloseTransaction();
                hibernateOfferRepository.Rollback();
                hibernateOfferRepository.CloseTransaction();
                Log.Error("update", ex);
                return new BaseResponse<IActionResult>(ex.Message);
            }
        }
    }
}
