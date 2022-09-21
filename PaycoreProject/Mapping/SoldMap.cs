using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PaycoreProject.Model;

namespace PaycoreProject.Mapping
{
    public class SoldMap:  ClassMapping<Sold>
    {
        public SoldMap()
        {
            Id(x => x.Id, x =>
            {
                x.Type(NHibernateUtil.Int32);
                x.Column("id");
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            });

            //Property(b => b.User, x =>
            //{
                
            //    x.Type(NHibernateUtil.Boolean);
            //    x.NotNullable(false);
            //    x.Column("user_id");
            //});
            ManyToOne(c => c.User, p =>
            {
                p.Column("user_id");
                p.Fetch(FetchKind.Join);
                p.NotNullable(true);
                Lazy(false);
            });

            //Property(b => b.Product, x =>
            //{
            //    x.Type(NHibernateUtil.Int32);
            //    x.NotNullable(false);
            //    x.Column("product_id");
            //});
            ManyToOne(c => c.Product, p =>
            {
                p.Column("product_id");
                p.Fetch(FetchKind.Join);
                p.NotNullable(true);
                Lazy(false);
            });
            Property(b => b.IsSold, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.Boolean);
                x.NotNullable(false);
                x.Column("sold_status");
            });

            

            Table("sold");
        }
    }
}
