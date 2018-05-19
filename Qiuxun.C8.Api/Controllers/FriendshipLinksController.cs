using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Qiuxun.C8.Api.Service.Cache;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 友情链接接口
    /// </summary>
    public class FriendshipLinksController : ApiController
    {
        FriendshipLinksService _service = new FriendshipLinksService();
        private static Timer sendTimer; //计时器

        private static object objPv = new object(); //pv锁对象
        private static object objUv = new object(); //uv锁对象

        public FriendshipLinksController()
        {
            if (sendTimer == null)
                sendTimer = new Timer(new TimerCallback(AddPvUv), this, 10000, 10 * 60 * 1000);
        }

        /// <summary>
        /// 添加PV UV IP
        /// </summary>
        /// <param name="type"></param>
        /// <param name="linkCode"></param>
        /// <param name="ip"></param>
        [AllowAnonymous]
        [HttpGet]
        public void Add(string type, string linkCode, string ip = "")
        {
            FriendLink model = GetFriendLink(linkCode);
            if (model != null && model.Id > 0)
            {
                //#region 向缓存中加入linkCode
                //var linkCodesObj = CacheHelper.GetCache<string>("FriendshipLinksControllerlinkCodes");
                //var linkCodes = linkCodesObj == null ? "": linkCodesObj.ToString();
                //if (string.IsNullOrEmpty(linkCodes))
                //{
                //    CacheHelper.AddCache("FriendshipLinksControllerlinkCodes", linkCode + ",");
                //}
                //else if (linkCodes.IndexOf(linkCode) == -1)
                //{
                //    CacheHelper.SetCache("FriendshipLinksControllerlinkCodes", linkCodes + linkCode + ",");
                //}
                //#endregion

                if (type == "ip")
                {
                    var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                    var ipsObj = CacheHelper.GetCache<string>("FriendshipLinksControllerIp" + linkCode);
                    var ips = ipsObj == null ? "" : ipsObj.ToString();
                    if (string.IsNullOrEmpty(ips) || ips.IndexOf("," + ip + ",") == -1)
                    {
                        if (string.IsNullOrEmpty(ips))
                        {
                            CacheHelper.SetCache("FriendshipLinksControllerIp" + linkCode, "," + ip + ",", endDate);
                        }
                        else
                        {
                            CacheHelper.SetCache("FriendshipLinksControllerIp" + linkCode, ips + ip + ",", endDate);
                        }

                        _service.AddIp(model.Id);
                    }
                }
                if (type == "uv")
                {
                    #region 向缓存中增加PV数
                    //lock (objUv)
                    //{
                        var obj = CacheHelper.GetCache<int>("FriendshipLinksControllerUv" + linkCode);
                        if (obj == default(int))
                        {
                            CacheHelper.AddCache("FriendshipLinksControllerUv" + linkCode, 1);
                        }
                        else
                        {
                            var uvNum = Convert.ToInt32(CacheHelper.GetCache<int>("FriendshipLinksControllerUv" + linkCode));
                            CacheHelper.SetCache("FriendshipLinksControllerUv" + linkCode, uvNum + 1);
                        }
                   // }
                    #endregion
                }
                if (type == "pv")
                {
                    #region 向缓存中增加PV数
                    //lock (objPv)
                    //{
                        var obj = CacheHelper.GetCache<int>("FriendshipLinksControllerPv" + linkCode);
                        if (obj == default(int))
                        {
                            CacheHelper.AddCache("FriendshipLinksControllerPv" + linkCode, 1);
                        }
                        else
                        {
                            var pvNum = Convert.ToInt32(CacheHelper.GetCache<int>("FriendshipLinksControllerPv" + linkCode));
                            CacheHelper.SetCache("FriendshipLinksControllerPv" + linkCode, pvNum + 1);
                        }
                   // }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 添加PV UV
        /// </summary>
        /// <param name="state"></param>
        private void AddPvUv(object state)
        {
            //var linkCodesObj = CacheHelper.GetCache<string>("FriendshipLinksControllerlinkCodes");
            //var linkCodes = linkCodesObj == null ? "" : linkCodesObj.ToString();
            var list = CacheHelper.GetCache<List<FriendLink>>("FriendshipLinksControllerFriendLinkList");

            if (list != null && list.Any())
            {
               // var linkCodeArray = linkCodes.Split(',');

                foreach (var model in list)
                {
                    //if (string.IsNullOrEmpty(linkCode))
                    //    continue;

                    //FriendLink model = GetFriendLink(linkCode);
                    //if (model.Id > 0)
                    //{
                        #region 保存PV
                        var pvCount = 0;
                        //lock (objPv)
                        //{
                            pvCount = Convert.ToInt32(CacheHelper.GetCache<string>("FriendshipLinksControllerPv" + model.Code));
                            CacheHelper.SetCache("FriendshipLinksControllerPv" + model.Code, 0);
                       // }

                        if (pvCount != 0)
                        {
                            _service.AddPv(model.Id, pvCount);
                        }
                        #endregion

                        #region 保存UV
                        var uvCount = 0;
                        //lock (objUv)
                        //{
                            uvCount = Convert.ToInt32(CacheHelper.GetCache<string>("FriendshipLinksControllerUv" + model.Code));
                            CacheHelper.SetCache("FriendshipLinksControllerUv" + model.Code, 0);
                      //  }

                        if (uvCount != 0)
                        {
                            _service.AddUv(model.Id, uvCount);
                        }
                        #endregion
                  //  }
                }
            }
        }

        /// <summary>
        /// 获取友情链接对象
        /// </summary>
        /// <param name="linkCode"></param>
        /// <returns></returns>
        private FriendLink GetFriendLink(string linkCode)
        {
            List<FriendLink> list = null;
            var friendLinkList = CacheHelper.GetCache<List<FriendLink>>("FriendshipLinksControllerFriendLinkList");
            if (friendLinkList == null)
            {
                list = _service.GetFriendLink();

                CacheHelper.SetCache("FriendshipLinksControllerFriendLinkList", list, DateTime.Now.AddHours(2));
            }
            else
            {
                list = friendLinkList as List<FriendLink>;
            }

            return list.FirstOrDefault(e => e.Code == linkCode);
        }
    }
}
