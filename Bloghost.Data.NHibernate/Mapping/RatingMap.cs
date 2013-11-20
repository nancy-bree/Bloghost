using FluentNHibernate.Mapping;
using Bloghost.Core.Entities;

namespace Bloghost.Data.NHibernate.Mapping
{
    public class RatingMap : ClassMap<Rating>
    {
        public RatingMap()
        {
            Table("Rating");

            Id(x => x.ID).GeneratedBy.GuidComb();

            Map(x => x.CommentID).Not.Nullable();

            Map(x => x.UserID).Not.Nullable();

            Map(x => x.Vote).Not.Nullable();
        }
    }
}
