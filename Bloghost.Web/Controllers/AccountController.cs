using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Bloghost.Web.Models;
using Bloghost.Web.Interfaces;
using Bloghost.Web.Services;
using Bloghost.Core.Entities;
using Bloghost.Core.Services.Interfaces;
using System.Data;

namespace Bloghost.Web.Controllers
{
    public class AccountController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        public AccountController(IBlogService blogService, IUserService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }

        //
        // GET: /Account/

        public ActionResult Index()
        {
            var user = _userService.GetUser((Guid)System.Web.Security.Membership.GetUser().ProviderUserKey);
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
        public ActionResult Index(EditUserModel model)
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
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to edit user. Please correct the errors and try again.");
            }
            return View("Index");
        }

        //
        // GET: /Account/LogIn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogIn

        [HttpPost]
        public ActionResult LogOn(LogInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Blogs", "Blog");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
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
                    return RedirectToAction("Blogs", "Blog");
                }
                ModelState.AddModelError("", ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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
