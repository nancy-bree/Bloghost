using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bloghost.Core.Entities
{
    //[Bind(Exclude = "ID")]
    public class Comment : BaseEntity
    {
        [DisplayName("Comment Title")]
        [StringLength(256)]
        public virtual string Title { get; set; }

        [DisplayName("Comment Text")]
        [Required]
        [StringLength(4000)]
        public virtual string CommentBody { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTime? Modified { get; set; }

        [ScaffoldColumn(false)]
        public virtual Entry Entry { get; set; }

        [ScaffoldColumn(false)]
        public virtual User User { get; set; }
    }
}
