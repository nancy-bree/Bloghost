using System;

namespace Bloghost.Core.Repositories
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute { }
}
