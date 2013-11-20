using FluentNHibernate.Mapping;
using Bloghost.Core.Entities;

namespace Bloghost.Data.NHibernate.Mapping
{
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Table("Tag");

            Id(x => x.ID).GeneratedBy.GuidComb();

            Map(x => x.Name).Length(50).Not.Nullable();

            HasManyToMany(x => x.Entries).ParentKeyColumn("EntryID").ChildKeyColumn("TagID").Inverse().Table("EntryTag").Not.LazyLoad();
        }
    }
}
