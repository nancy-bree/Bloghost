using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bloghost.Core.Entities
{
    public class Entry : BaseEntity
    {
        [DisplayName("Title")]
        [Required]
        [StringLength(256)]
        public virtual string Title { get; set; }

        [DisplayName("Text")]
        [Required]
        [StringLength(4000)]
        [AllowHtml]
        public virtual string EntryBody { get; set; }

        [ScaffoldColumn(false)]
        public virtual Blog Blog { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime? Modified { get; set; }

        public virtual System.Collections.Generic.IList<Comment> Comments { get; set; }

        public virtual System.Collections.Generic.IList<Tag> Tags { get; set; }

        public Entry()
        {
            this.Comments = new System.Collections.Generic.List<Comment>();
            this.Tags = new System.Collections.Generic.List<Tag>();
        }
    }
}
