using Antlr.Runtime.Misc;

namespace PaycoreProject.Model
{
    public class GiveOfferDto
    {
        public virtual Product Product { get; set; }
        public virtual User BidderUser { get; set; }
        public virtual double Offer { get; set; }
        public virtual int ApprovalStatus { get; set; }

       
    }
 
}
