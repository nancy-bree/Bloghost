using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Bloghost.Core.Entities;
using Bloghost.Core.Services.Interfaces;
using Bloghost.Web.Models;
using System.Data;
using Bloghost.Web.Services;

namespace Bloghost.Web.Controllers
{
    [Authorize]
    public class EntryController : Controller
    {
        private readonly IEntryService _entryService;
        private readonly ICommentService _commentService;
        private readonly IBlogService _blogService;
        private readonly ITagService _tagService;

        public EntryController(IEntryService entryService, ICommentService commentService, IBlogService blogService, ITagService tagService)
        {
            _entryService = entryService;
            _commentService = commentService;
            _blogService = blogService;
            _tagService = tagService;
        }

        public ActionResult Entry(Guid id)
        {
            return View(_entryService.GetEntry(id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EntryViewModel entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newEntry = new Entry {Title = entry.Title, EntryBody = entry.EntryBody};
                    Blog blog = _blogService.GetBlog(_blogService.GetBlogID((Guid)System.Web.Security.Membership.GetUser().ProviderUserKey));
                    newEntry.Blog = blog;
                    if (!String.IsNullOrEmpty(entry.Tags))
                    {
                        IEnumerable<string> tags = TagEditor.SplitTags(entry.Tags).Distinct();
                        newEntry.Tags = new List<Tag>();
                        foreach (var tag in tags)
                        {
                            var tmp = _tagService.GetTag(tag.Trim());
                            if (tmp == null)
                            {
                                tmp = new Tag
                                {
                                    Name = tag.Trim()
                                };
                                _tagService.CreateTag(tmp);
                            }
                            newEntry.Tags.Add(tmp);
                        }
                    }
                    _entryService.CreateEntry(newEntry);
                    return RedirectToAction("Entries", "Blog", new { id = blog.ID });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Error! Unable to create entry.");
            }
            return View("Create");
        }

        public ActionResult Edit(Guid id)
        {
            var entry = _entryService.GetEntry(id);
            var model = new EntryEditModel
            {
                ID = entry.ID,
                EntryBody = entry.EntryBody,
                Title = entry.Title
            };
            var tags = entry.Tags.Select(item => item.Name).ToList();
            model.Tags = String.Join(", ", tags);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EntryEditModel entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Entry entryToUpdate = _entryService.GetEntry(entry.ID);
                    entryToUpdate.Title = entry.Title;
                    entryToUpdate.EntryBody = entry.EntryBody;
                    if (entry.Tags != null)
                    {
                        IEnumerable<string> tags = TagEditor.SplitTags(entry.Tags).Distinct();
                        entryToUpdate.Tags = new List<Tag>();
                        foreach (var tag in tags)
                        {
                            var tmp = _tagService.GetTag(tag.Trim());
                            if (tmp == null)
                            {
                                tmp = new Tag
                                {
                                    Name = tag.Trim()
                                };
                                _tagService.CreateTag(tmp);
                            }
                            entryToUpdate.Tags.Add(tmp);
                        }
                    }
                    else
                    {
                        entryToUpdate.Tags = null;
                    }
                    _entryService.UpdateEntry(entryToUpdate);
                    return RedirectToAction("Entries", "Blog", new { id = entryToUpdate.Blog.ID });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Error! Unable to edit entry.");
            }
            return View("Edit");
        }

        //[HttpPost]
        public ActionResult Delete(Guid id)
        {
            var commentsToDelete = _commentService.GetCommentList().Where(x => x.Entry.ID == id);
            foreach (var item in commentsToDelete)
            {
                _commentService.DeleteComment(item.ID);
            }
            _entryService.DeleteEntry(id);
            return RedirectToAction("Entries", "Blog", new { id = _blogService.GetBlogID((Guid)System.Web.Security.Membership.GetUser().ProviderUserKey)});
            //return Json("Entry deleted successfully!");
        }
    }
}
