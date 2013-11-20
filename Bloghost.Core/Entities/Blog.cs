using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bloghost.Core.Entities
{
    [Bind(Exclude = "ID")]
    public class Blog : BaseEntity
    {
        [DisplayName("Blog Title")]
        [Required]
        [StringLength(256)]
        public virtual string BlogName { get; set; }

        [DisplayName("Blog Subtitle")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(256)]
        public virtual string BlogSubtitle { get; set; }

        [ScaffoldColumn(false)]
        public virtual User User { get; set; }

        public virtual System.Collections.Generic.IList<Entry> Entries { get; set; }

        public Blog()
        {
            this.Entries = new System.Collections.Generic.List<Entry>();
        }
    }
}
