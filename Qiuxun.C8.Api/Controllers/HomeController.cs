using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Qiuxun.C8.Api.Service.Common;

namespace Qiuxun.C8.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var name = GetVerisonFileName();
            ViewData["Version"] = name;

            var files = this.GetFileList();
            ViewData["Files"] = string.Join(",", files);

            return View();
        }


        public ActionResult NotFound(string aspxerrorpath)
        {
            if (this.Request.IsAjaxRequest())
            {
                return Json(new ApiResult(404, "你请求的地址不存在"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View();
            }
        }


        private List<string> GetFileList()
        {
            var list = new List<string>();
            string path = Server.MapPath("/");//得到文件夹的物理路径
            DirectoryInfo dirInfo = new DirectoryInfo(path);//获取文件夹的目录信息
            try
            {
                FileInfo[] files = dirInfo.GetFiles();//得到所有文件信息
                if (files.Length > 0)
                {
                    foreach (FileInfo file in files)
                    {
                        list.Add(file.Name);
                    }
                }
            }
            catch { }
            return list;
        }

        private string GetVerisonFileName()
        {
            var fileList = this.GetFileList();
            string versionFile = string.Empty;
            Regex r = new Regex(@"^\d+(\.\d+)+$");
            var matchedFiles = new List<string>();
            foreach (var file in fileList)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (string.IsNullOrEmpty(fileName))
                {
                    break;
                }
                bool isMatch = r.Match(fileName).Success;
                if (isMatch)
                {
                    matchedFiles.Add(fileName);
                }
            }
            if (matchedFiles.Any())
            {
                versionFile = matchedFiles.LastOrDefault();
            }
            if (string.IsNullOrEmpty(versionFile))
            {
                versionFile = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

            return versionFile;
        }


        /// <summary>
        /// JS调用Android和IOS方法测试
        /// </summary>
        /// <returns></returns>
        public ActionResult JsAndroidIosTransfer()
        {
            return View();
        }

    }
}