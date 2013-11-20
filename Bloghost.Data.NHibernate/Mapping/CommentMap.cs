using FluentNHibernate.Mapping;
using Bloghost.Core.Entities;

namespace Bloghost.Data.NHibernate.Mapping
{
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Table("Comment");

            Id(x => x.ID).GeneratedBy.GuidComb();

            Map(x => x.Title).Length(256);

            Map(x => x.CommentBody).Length(4000).Not.Nullable();

            Map(x => x.CreatedDate).Default("getDate()").Not.Insert().Not.Update().Generated.Always().Not.Nullable();

            Map(x => x.Modified);

            References(x => x.User).Column("UserID").Not.Nullable().Not.LazyLoad();

            References(x => x.Entry).Column("EntryID").Not.Nullable().Not.LazyLoad();
        }
    }
}
