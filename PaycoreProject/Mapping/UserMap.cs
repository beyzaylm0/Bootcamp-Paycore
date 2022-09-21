using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PaycoreProject.Model;

namespace PaycoreProject.Mapping
{
    public class UserMap: ClassMapping<User>
    {
        public UserMap()
        {
            Id(x => x.Id, x =>
            {
                x.Type(NHibernateUtil.Int32);
                x.Column("id");
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            });

            Property(b => b.Name, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
                x.Column("lastname");
            });

            Property(b => b.Surname, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
                x.Column("surname");
            });

            Property(b => b.Address, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.String);
                x.NotNullable(false);
                x.Column("address");
            });

            Property(x => x.Email, x =>
            {
                x.Type(NHibernateUtil.String);
                x.Length(50);
                x.Column("email");
                x.NotNullable(false);
            });
            Property(x => x.Password, x =>
            {
                x.Type(NHibernateUtil.String);
                x.Length(200);
                x.Column("passwords");
                x.NotNullable(false);
            });

            Table("users");
        }
    }
}
