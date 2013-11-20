using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Bloghost.Web.Models
{
    public class EntryViewModel
    {
        public Guid ID { get; set; }

        [DisplayName("Title")]
        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [DisplayName("Text")]
        [Required]
        [StringLength(4000)]
        [AllowHtml]
        public string EntryBody { get; set; }

        [DisplayName("Tags")]
        public string Tags { get; set; }
    }
}