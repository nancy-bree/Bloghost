using FluentNHibernate.Mapping;
using Bloghost.Core.Entities;

namespace Bloghost.Data.NHibernate.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("[User]");

            Id(x => x.ID).GeneratedBy.GuidComb();

            Map(x => x.Name).Length(50)/*.Not.Nullable()*/;

            Map(x => x.Surname).Length(50)/*.Not.Nullable()*/;

            Map(x => x.Login).Length(50)/*.Not.Nullable()*/;

            Map(x => x.Password).Length(64)/*.Not.Nullable()*/;

            Map(x => x.PasswordSalt).Length(64)/*.Not.Nullable()*/;

            Map(x => x.Email).Length(100)/*.Not.Nullable()*/;

            Map(x => x.Userpic).Length(50);

            Map(x => x.UserpicMin).Length(50);

            Map(x => x.CreatedDate).Default("getDate()").Not.Insert().Not.Update().Generated.Always().Not.Nullable();

            HasOne(x => x.Blog).PropertyRef(x => x.User).Not.LazyLoad();

            HasMany<Comment>(x => x.Comments).KeyColumn("UserID").Inverse().Not.LazyLoad();
        }
    }
}
