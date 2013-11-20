using System;
using System.Linq;
using Bloghost.Core.Entities;
using Bloghost.Core.Services.Interfaces;
using PagedList;

namespace Bloghost.Web.Models
{
    public class EntryListViewModel
    {
        public IPagedList<Entry> Entries { get; set; }
        public string BlogTitle { get; set; }
        public string BlogSubtitle { get; set; }
        public Guid BlogID { get; set; }
        //public List<Tag> Tags { get; set; }

        public EntryListViewModel(IBlogService blogService, Guid blogID, int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            Entries = blogService.GetBlog(blogID).Entries.OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, pageSize);
            BlogTitle = blogService.GetBlog(blogID).BlogName;
            BlogSubtitle = blogService.GetBlog(blogID).BlogSubtitle;
            BlogID = blogID;
        }
    }
}