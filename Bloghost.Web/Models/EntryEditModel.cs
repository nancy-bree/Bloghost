using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bloghost.Web.Models
{
    public class EntryEditModel
    {
        public Guid ID { get; set; }

        [DisplayName("Title")]
        [Required]
        [StringLength(256)]
        public virtual string Title { get; set; }

        [DisplayName("Text")]
        [Required]
        [StringLength(4000)]
        [AllowHtml]
        public virtual string EntryBody { get; set; }

        public string Tags { get; set; }
    }
}