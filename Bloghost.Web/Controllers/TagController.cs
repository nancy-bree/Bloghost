using Bloghost.Core.Services.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Bloghost.Web.Properties;

namespace Bloghost.Web.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        private readonly IEntryService _entryService;

        public TagController(ITagService tagService, IEntryService entryService)
        {
            _tagService = tagService;
            _entryService = entryService;
        }

        public ActionResult Index(Guid id, int page = 1)
        {
            ViewBag.Tag = _tagService.GetTag(id);
            return View(_entryService.GetEntryList().Where(x => x.Tags.Any(y => y.ID == id)).ToPagedList(page, Settings.Default.ItemsPerPage));
        }

        public JsonResult GetTags(string term)
        {
            var tags = _tagService.GetTagList().Where(x => x.Name.ToUpper().Contains(term.ToUpper())).Select(x => x.Name).ToArray();
            return Json(tags, JsonRequestBehavior.AllowGet);
        }

    }
}
