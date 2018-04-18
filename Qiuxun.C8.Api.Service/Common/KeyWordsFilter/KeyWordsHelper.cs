using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace Qiuxun.C8.Api.Service.Common.KeyWordsFilter
{


    public class KeyWordsHelper
    {
        private static readonly FilterKeyWords _filter;

        static KeyWordsHelper()
        {
            string str = ConfigurationManager.AppSettings["KeyWordsDictionary"];
            if (string.IsNullOrEmpty(str))
            {
                str = "KeyWordsDictionary.txt";
            }
            Uri uri = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase);
            string directoryName = Path.GetDirectoryName(uri.LocalPath);
            string path = Path.Combine(directoryName, str);
            if (!File.Exists(path))
            {
                path = Path.Combine(Directory.GetParent(directoryName).FullName, str);
            }
            if (!File.Exists(path))
            {
                HttpContext current = HttpContext.Current;
                if (current != null)
                {
                    path = current.Server.MapPath("~/" + str);
                }
                else
                {
                    path = HostingEnvironment.MapPath("~/" + str);
                }
            }
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            _filter = new FilterKeyWords(File.ReadAllLines(path));
        }

        public static string Replace(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            return _filter.Replace(text);
        }
    }
}

