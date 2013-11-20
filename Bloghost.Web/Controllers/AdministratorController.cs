using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Bloghost.Core.Entities;
using Bloghost.Core.Services.Interfaces;
using Bloghost.Web.Models;
using PagedList;
using System.Data;
using Bloghost.Web.Interfaces;
using Bloghost.Web.Services;
using Bloghost.Web.Properties;

namespace Bloghost.Web.Controllers
{
    [Authorize(Users = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IEntryService _entryService;
        private readonly IUserService _userService;
        private readonly ITagService _tagService;
        private readonly ICommentService _commentService;
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        public AdministratorController(IBlogService blogService, IEntryService entryService, IUserService userService, ITagService tagService, ICommentService commentService)
        {
            _blogService = blogService;
            _entryService = entryService;
            _userService = userService;
            _tagService = tagService;
            _commentService = commentService;
        }
        //
        // GET: /Administrator/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users(int? page)
        {
            int pageNumber = (page ?? 1);
            return View(_userService.GetUserList().OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, Settings.Default.ItemsPerPage));
        }

        public ActionResult Entries(int? page)
        {
            int pageNumber = (page ?? 1);
            return View(_entryService.GetEntryList().OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, Settings.Default.ItemsPerPage));
        }

        public ActionResult Tags(int? page)
        {
            int pageNumber = (page ?? 1);
            return View(_tagService.GetTagList().OrderBy(x => x.Name).ToPagedList(pageNumber, Settings.Default.ItemsPerPage));
        }

        public ActionResult Comments(Guid id, int? page)
        {
            int pageNumber = (page ?? 1);
            return View(_commentService.GetCommentList().Where(x => x.Entry.ID == id).OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, Settings.Default.ItemsPerPage));
        }

        public ActionResult EditEntry(Guid id)
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
            return View("~/Views/Entry/Edit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEntry(EntryEditModel entry)
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
                    return RedirectToAction("Entries", "Administrator", new { id = entryToUpdate.Blog.ID });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Произошла ошибка. Невозможно отредактировать запись.");
            }
            return View("~/Views/Entry/Edit.cshtml");
        }

        [HttpPost]
        public JsonResult DeleteUser(Guid id)
        {
            var entriesToDelete = _entryService.GetEntryList().Where(x => x.Blog.User.ID == id);
            foreach (var item in entriesToDelete)
            {
                var entryCommentsToDelete = _commentService.GetCommentList().Where(x => x.Entry.ID == item.ID);
                foreach (var comment in entryCommentsToDelete)
                {
                    _commentService.DeleteComment(comment.ID);
                }
                _entryService.DeleteEntry(item.ID);
            }
            var commentsToDelete = _commentService.GetCommentList().Where(x => x.User.ID == id);
            foreach (var item in commentsToDelete)
            {
                _commentService.DeleteComment(item.ID);
            }
            _blogService.DeleteBlog(_blogService.GetBlogID(id));
            _userService.DeleteUser(id);

            return Json("User and blog deleted successfully!");
        }

        [HttpPost]
        public JsonResult DeleteEntry(Guid id)
        {
            var commentsToDelete = _commentService.GetCommentList().Where(x => x.Entry.ID == id);
            foreach (var item in commentsToDelete)
            {
                _commentService.DeleteComment(item.ID);
            }
            _entryService.DeleteEntry(id);

            return Json("Entry deleted successfully!");
        }

        [HttpPost]
        public JsonResult DeleteComment(Guid id)
        {
            _commentService.DeleteComment(id);

            return Json("Comment deleted successfully!");
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    var user = _userService.GetUser(model.UserName);
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    if (model.Image != null)
                    {
                        user.Userpic = ImageService.SaveImage(model.Image);
                    }
                    _userService.EditUser(user);

                    var blog = new Blog
                    {
                        BlogName = model.BlogName,
                        BlogSubtitle = model.BlogSubtitle,
                        User = user,
                    };
                    _blogService.CreateBlog(blog);
                    return RedirectToAction("Users", "Administrator");
                }
                ModelState.AddModelError("", ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult EditUser(Guid id)
        {
            var user = _userService.GetUser(id);
            var userModel = new EditUserModel
            {
                ID = user.ID,
                Name = user.Name,
                Surname = user.Surname,
                BlogName = user.Blog.BlogName,
                BlogSubtitle = user.Blog.BlogSubtitle
            };
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User userToUpdate = _userService.GetUser(model.ID);
                    Blog blogToUpdate = _blogService.GetBlog(userToUpdate.Blog.ID);
                    userToUpdate.Name = model.Name;
                    userToUpdate.Surname = model.Surname;
                    //userToUpdate.Userpic = ImageService.SaveImage(model.Image);
                    blogToUpdate.BlogName = model.BlogName;
                    blogToUpdate.BlogSubtitle = model.BlogSubtitle;
                    _userService.EditUser(userToUpdate);
                    _blogService.UpdateBlog(blogToUpdate);
                    return RedirectToAction("Users", "Administrator");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to edit user. Please correct the errors and try again.");
            }
            return View("EditUser");
        }

        public ActionResult AddTag()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTag(Tag tag)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_tagService.GetTag(tag.Name) != null) { throw new DataException(); }
                    _tagService.CreateTag(tag);
                    return RedirectToAction("Tags", "Administrator");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Tag name already exists. Please enter a different tag name..");
            }
            return View("AddTag");
        }

        public ActionResult EditTag(Guid id)
        {
            Tag tag = _tagService.GetTag(id);
            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTag(Tag tag)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Tag tagToUpdate = _tagService.GetTag(tag.ID);
                    tagToUpdate.Name = tag.Name;
                    _tagService.UpdateTag(tagToUpdate);
                    return RedirectToAction("Tags", "Administrator");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to edit tag. Please correct the errors and try again.");
            }
            return View("EditTag");
        }

        public ActionResult EditComment(Guid id)
        {
            var comment = _commentService.GetComment(id);
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(Comment model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var commentToUpdate = _commentService.GetComment(model.ID);
                    commentToUpdate.Title = model.Title;
                    commentToUpdate.CommentBody = model.CommentBody;
                    _commentService.UpdateComment(commentToUpdate);
                    return RedirectToAction("Comments", "Administrator", new { id = commentToUpdate.Entry.ID });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to edit comment. Please correct the errors and try again.");
            }
            return View("EditComment");
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
