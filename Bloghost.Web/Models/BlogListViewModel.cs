using System.Linq;
using Bloghost.Core.Entities;
using Bloghost.Core.Services.Interfaces;
using Bloghost.Web.Properties;
using PagedList;

namespace Bloghost.Web.Models
{
    public class BlogListViewModel
    {
        public IPagedList<Blog> Blogs { get; set; }

        public BlogListViewModel(IBlogService blogService, int? page)
        {
            int pageNumber = (page ?? 1);
            Blogs = blogService.GetBlogList().OrderByDescending(x => x.User.CreatedDate).ToPagedList(pageNumber, Settings.Default.ItemsPerPage);
        }
    }
}