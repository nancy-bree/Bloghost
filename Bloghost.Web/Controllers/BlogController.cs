using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Bloghost.Core.Services.Interfaces;
using Bloghost.Web.Models;

namespace Bloghost.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IEntryService _entryService;


        public BlogController(IBlogService blogService, IEntryService entryService)
        {
            _blogService = blogService;
            _entryService = entryService;
        }

        public ViewResult Blogs(int? page)
        {
            var viewModel = new BlogListViewModel(_blogService, page);
            ViewBag.Title = "Blogs";
            return View("Blogs", viewModel);
        }

        public ViewResult Entries(Guid id, int? page)
        {
            var viewModel = new EntryListViewModel(_blogService, id, page);
            ViewBag.Title = _blogService.GetBlog(id).BlogName;
            return View(viewModel);
        }

        public ActionResult SearchResults(string searchString)
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.Title = searchString;
                result = _entryService.GetEntryList().Where(x => x.EntryBody.Contains(searchString)).Select(x => x.Title).ToList();
            }
            return View(result);
        }

    }
}
