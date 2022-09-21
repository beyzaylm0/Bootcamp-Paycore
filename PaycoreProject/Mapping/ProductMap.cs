using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using PaycoreProject.Model;

namespace PaycoreProject.Mapping
{
    public class ProductMap:ClassMapping<Product>
    {
        public ProductMap()
        {
            Id(x => x.Id, x =>
            {
                x.Type(NHibernateUtil.Int32);
                x.Column("id");
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            });

            Property(b => b.ProductName, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
                x.Column("productyname");
            });

            Property(b => b.Description, x =>
            {
                x.Length(150);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
                x.Column("description");
            });
            Property(b => b.Color, x =>
            {
                x.Length(30);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
                x.Column("color");
            });
            Property(b => b.Brand, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
                x.Column("brand");
            });
            Property(b => b.Price, x =>
            {
                
                x.Type(NHibernateUtil.Double);
                x.NotNullable(false);
                x.Column("price");
            });
            Property(b => b.isOfferable, x =>
            {

                x.Type(NHibernateUtil.Boolean);
                x.NotNullable(true);
                x.Column("isofferable");
            });
            Property(b => b.isSold, x =>
            {

                x.Type(NHibernateUtil.Boolean);
                x.NotNullable(true);
                x.Column("issold");
            });
            Property(b => b.UserId, x =>
            {

                x.Type(NHibernateUtil.Int32);
                x.NotNullable(true);
                x.Column("userid");
            });
            //Property(b => b.CategoryId, x =>
            //{

            //    x.Type(NHibernateUtil.Int32);
            //    x.NotNullable(true);
            //    x.Column("categoryId");
            //    //ManyToOne(category => category.Categories, map => map.Column("categoryıd"));
            //});
            ManyToOne(c => c.Category, p =>
            {
                p.Column("category_id");
                p.Fetch(FetchKind.Join);
                p.NotNullable(true);
                Lazy(false);
            });

            Table("product");
        }
    }
}
