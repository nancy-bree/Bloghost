using System.Collections.Generic;

namespace Bloghost.Web.Services
{
    public static class TagEditor
    {
        public static List<string> SplitTags(string tags)
        {
            var list = new List<string>(tags.Trim().Split(','));
            list.Remove(string.Empty);
            return list;
        }
    }
}