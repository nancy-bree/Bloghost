using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Bloghost.Web.Models
{
    public class LogInModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        public Guid ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Image")]
        public HttpPostedFileBase Image { get; set; }

        [Required]
        [Display(Name = "Blog Title")]
        [StringLength(256)]
        public string BlogName { get; set; }

        [Display(Name = "Blog Subtitle")]
        [StringLength(256)]
        public string BlogSubtitle { get; set; }
    }

    public class EditUserModel
    {
        public Guid ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "User Image")]
        public HttpPostedFileBase Image { get; set; }

        [Required]
        [Display(Name = "Blog Title")]
        [StringLength(256)]
        public string BlogName { get; set; }

        [Display(Name = "Blog Subtitle")]
        [StringLength(256)]
        public string BlogSubtitle { get; set; }
    }
}