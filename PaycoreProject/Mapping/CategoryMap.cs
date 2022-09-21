using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PaycoreProject.Model;

namespace PaycoreProject.Mapping
{
    public class CategoryMap:ClassMapping<Category>
    {
        public CategoryMap()
        {
            Id(x => x.Id, x =>
            {
                x.Type(NHibernateUtil.Int32);
                x.Column("id");
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            });

            Property(b => b.CategoryName, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
                x.Column("categoryname");
            });

            Property(b => b.Description, x =>
            {
                x.Length(150);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
                x.Column("description");
            });



            Table("category");
        }
    }
}
