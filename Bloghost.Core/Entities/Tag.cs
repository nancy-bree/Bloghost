using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bloghost.Core.Entities
{
    //[Bind(Exclude = "ID")]
    public class Tag : BaseEntity
    {
        [DisplayName("Tag Name")]
        [Required]
        [StringLength(256)]
        public virtual string Name { get; set; }

        public virtual System.Collections.Generic.IList<Entry> Entries { get; set; }

        public Tag()
        {
            this.Entries = new System.Collections.Generic.List<Entry>();
        }
    }
}
