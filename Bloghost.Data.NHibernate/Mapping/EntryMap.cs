using FluentNHibernate.Mapping;
using Bloghost.Core.Entities;

namespace Bloghost.Data.NHibernate.Mapping
{
    public class EntryMap : ClassMap<Entry>
    {
        public EntryMap()
        {
            Table("Entry");

            Id(x => x.ID).GeneratedBy.GuidComb();

            Map(x => x.Title).Length(265);

            Map(x => x.EntryBody).Length(4000).Not.Nullable();

            Map(x => x.CreatedDate).Default("getDate()").Not.Insert().Not.Update().Generated.Always().Not.Nullable();

            Map(x => x.Modified);

            HasManyToMany(x => x.Tags).ParentKeyColumn("EntryID").ChildKeyColumn("TagID").Table("EntryTag").Not.LazyLoad();

            HasMany<Comment>(x => x.Comments).KeyColumn("EntryID").Not.LazyLoad().Inverse();

            References(x => x.Blog).Column("BlogID").Not.Nullable().Not.LazyLoad();
        }
    }
}
