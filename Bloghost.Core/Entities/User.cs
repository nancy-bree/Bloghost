using System;

namespace Bloghost.Core.Entities
{
    public class User : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public virtual string PasswordSalt { get; set; }

        public virtual string Email { get; set; }

        public virtual string Userpic { get; set; }

        public virtual string UserpicMin { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual System.Collections.Generic.IList<Comment> Comments { get; set; }

        public User()
        {
            this.Comments = new System.Collections.Generic.List<Comment>();
        }

    }
}
