using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace PaycoreProject.Model
{
    public class GiveOffer
    {
        public virtual int Id { get; set; }
        public virtual Product ProductId { get; set; }
        public virtual User BidderUser { get; set; }
        public virtual double Offer { get; set; }
        public virtual int ApprovalStatus { get; set; }

       
    }
    
}
