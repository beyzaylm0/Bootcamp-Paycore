namespace PaycoreProject.Model
{
    public class GiveOfferResult
    {
        public virtual int Id { get; set; }
        public virtual int ProductId { get; set; }
        public virtual int BidderUser { get; set; }
        public virtual double Offer { get; set; }
        public virtual int ApprovalStatus { get; set; }

    }
}
