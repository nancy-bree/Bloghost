using System;
using System.Web;
using System.IO;


namespace Bloghost.Web.Services
{
    public static class ImageService
    {
        public static string SaveImage(HttpPostedFileBase file)
        {
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string path = GetImagePath(filename);
            file.SaveAs(path);
            return filename;
        }

        private static string GetImagePath(string filename)
        {
            return Path.Combine(HttpContext.Current.Server.MapPath(Properties.Settings.Default.ImagePath), filename);
        }
    }
}