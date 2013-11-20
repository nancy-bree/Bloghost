using System;
using System.Web.Mvc;
using Bloghost.Core.Entities;
using Bloghost.Core.Services.Interfaces;
using System.Data;

namespace Bloghost.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IEntryService _entryService;
        private readonly IUserService _userService;


        public CommentController(ICommentService commentService, IEntryService entryService, IUserService userService)
        {
            _commentService = commentService;
            _entryService = entryService;
            _userService = userService;
        }

        public ActionResult Create(Guid entryID)
        {
            var newComment = new Comment();
            var entry = _entryService.GetEntry(entryID);
            newComment.Entry = entry;
            return PartialView(newComment);
        }

        [HttpPost]
        //[ActionName("Entry")]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    comment.Entry = _entryService.GetEntry(/*id*/comment.Entry.ID);
                    comment.User = _userService.GetUser(_userService.GetUser(User.Identity.Name).ID);
                    _commentService.CreateComment(comment);
                    return RedirectToAction("Entry", "Entry", new { id = comment.Entry.ID });
                    //return View("~/Views/Entry/Entry.cshtml");
                //}
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Произошла ошибка. Невозможно добавить запись.");
            }
            return View();
        }

    }
}
