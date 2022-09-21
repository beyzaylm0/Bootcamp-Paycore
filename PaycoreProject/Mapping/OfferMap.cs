using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PaycoreProject.Model;

namespace PaycoreProject.Mapping
{
    public class OfferMap:ClassMapping<GiveOffer>
    {
        public OfferMap()
        {
            Id(x => x.Id, x =>
            {
                x.Type(NHibernateUtil.Int32);
                x.Column("id");
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            });

            //Property(b => b.ProductId, x =>
            //{
            //    x.Length(50);
            //    x.Type(NHibernateUtil.Int32);
            //    x.NotNullable(true);
            //    x.Column("productid");
            //});
            ManyToOne(c => c.ProductId, p =>
            {
                p.Column("productid");
                p.Fetch(FetchKind.Join);
                p.NotNullable(true);
                Lazy(false);
            });

            //Property(b => b.BidderUserId, x =>
            //{
            //    x.Length(150);
            //    x.Type(NHibernateUtil.Int32);
            //    x.NotNullable(true);
            //    x.Column("bidderuserid");
            //});
            ManyToOne(c => c.BidderUser, p =>
            {
                p.Column("bidderuserid");
                p.Fetch(FetchKind.Join);
                p.NotNullable(true);
                Lazy(false);
            });

            Property(b => b.Offer, x =>
            {
                
                x.Type(NHibernateUtil.Double);
                x.NotNullable(true);
                x.Column("offer");
            }); 
            Property(b => b.ApprovalStatus, x =>
            {
                
                x.Type(NHibernateUtil.Int32);
                x.NotNullable(false);
                x.Column("approvalstatus");
            });



            Table("offer");
        }
    }
}
