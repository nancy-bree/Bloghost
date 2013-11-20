using System;
using System.Web;
using System.Web.Security;
using Castle.Windsor;
using System.Collections;

namespace Bloghost.Web.Infrastructure
{
    public abstract class WindsorMembershipProvider : MembershipProvider
    {
        private string providerId;

        public abstract IWindsorContainer GetContainer();

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
            providerId = config["providerId"];
            if (string.IsNullOrWhiteSpace(providerId))
                throw new Exception("Please configure the providerId from the membership provider " + name);
        }

        private MembershipProvider GetProvider()
        {
            try
            {
                var provider = GetContainer().Resolve(providerId, new Hashtable()) as MembershipProvider;
                if (provider == null)
                    throw new Exception(string.Format("Component '{0}' does not inherit MembershipProvider", providerId));
                return provider;
            }
            catch (Exception e)
            {
                throw new Exception("Error resolving MembershipProvider " + providerId, e);
            }
        }

        private T WithProvider<T>(Func<MembershipProvider, T> f)
        {
            var provider = GetProvider();
            try
            {
                return f(provider);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        private void WithProvider(Action<MembershipProvider> f)
        {
            var provider = GetProvider();
            try
            {
                f(provider);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }


        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            var provider = GetProvider();
            try
            {
                return provider.CreateUser(username, password, email, null, null, true, null, out status);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return WithProvider(p => p.ChangePasswordQuestionAndAnswer(username, password, newPasswordAnswer, newPasswordAnswer));
        }

        public override string GetPassword(string username, string answer)
        {
            return WithProvider(p => p.GetPassword(username, answer));
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return WithProvider(p => p.ChangePassword(username, oldPassword, newPassword));
        }

        public override string ResetPassword(string username, string answer)
        {
            return WithProvider(p => p.ResetPassword(username, answer));
        }

        public override void UpdateUser(MembershipUser user)
        {
            WithProvider(p => p.UpdateUser(user));
        }

        public override bool ValidateUser(string username, string password)
        {
            return WithProvider(p => p.ValidateUser(username, password));
        }

        public override bool UnlockUser(string userName)
        {
            return WithProvider(p => p.UnlockUser(userName));
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return WithProvider(p => p.GetUser(providerUserKey, userIsOnline));
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return WithProvider(p => p.GetUser(username, userIsOnline));
        }

        public override string GetUserNameByEmail(string email)
        {
            return WithProvider(p => p.GetUserNameByEmail(email));
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return WithProvider(p => p.DeleteUser(username, deleteAllRelatedData));
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.GetAllUsers(pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            return WithProvider(p => p.GetNumberOfUsersOnline());
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                GetContainer().Release(provider);
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return WithProvider(p => p.EnablePasswordRetrieval); }
        }

        public override bool EnablePasswordReset
        {
            get { return WithProvider(p => p.EnablePasswordReset); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return WithProvider(p => p.RequiresQuestionAndAnswer); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { return WithProvider(p => p.MaxInvalidPasswordAttempts); }
        }

        public override int PasswordAttemptWindow
        {
            get { return WithProvider(p => p.PasswordAttemptWindow); }
        }

        public override bool RequiresUniqueEmail
        {
            get { return WithProvider(p => p.RequiresUniqueEmail); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return WithProvider(p => p.PasswordFormat); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return WithProvider(p => p.MinRequiredPasswordLength); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return WithProvider(p => p.MinRequiredNonAlphanumericCharacters); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return WithProvider(p => p.PasswordStrengthRegularExpression); }
        }
    }

    public class WebWindsorMembershipProvider : WindsorMembershipProvider
    {
        public override IWindsorContainer GetContainer()
        {
            var context = HttpContext.Current;
            if (context == null)
                throw new Exception("No HttpContext");
            var accessor = context.ApplicationInstance as IContainerAccessor;
            if (accessor == null)
                throw new Exception("The global HttpApplication instance needs to implement " + typeof(IContainerAccessor).FullName);
            if (accessor.Container == null)
                throw new Exception("HttpApplication has no container initialized");
            return accessor.Container;
        }
    }
}