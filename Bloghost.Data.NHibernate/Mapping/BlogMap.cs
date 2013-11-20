using FluentNHibernate.Mapping;
using Bloghost.Core.Entities;

namespace Bloghost.Data.NHibernate.Mapping
{
    public class BlogMap : ClassMap<Blog>
    {
        public BlogMap()
        {
            Table("Blog");

            Id(x => x.ID).GeneratedBy.GuidComb();

            Map(x => x.BlogName).Length(256).Not.Nullable();

            Map(x => x.BlogSubtitle).Length(256);

            HasMany<Entry>(x => x.Entries).KeyColumn("BlogID").Inverse().Not.LazyLoad();

            References(x => x.User).Column("UserID").Unique().Not.LazyLoad();
        }
    }
}
