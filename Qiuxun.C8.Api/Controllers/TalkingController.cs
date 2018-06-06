using Qiuxun.C8.Api.Service.Api;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Data;
using Qiuxun.C8.Api.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Qiuxun.C8.Api.Service.Cache;
using Qiuxun.C8.Api.Public;

namespace Qiuxun.C8.Api.Controllers
{
    /// <summary>
    /// 聊天接口控制器
    /// </summary>
    public class TalkingController : QiuxunApiController
    {
        TalkingService _service = new TalkingService();

        /// <summary>
        /// 获取房间列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<ChatRoomResDto>> GetChatRoomList()
        {
            var result = new ApiResult<List<ChatRoomResDto>>();
            result.Data = _service.GetChatRoomList();
            return result;
        }

        /// <summary>
        /// 根据房间Id获取已被拉黑的用户Id串
        /// </summary>
        /// <param name="roomId">房间Id</param>
        /// <returns>返回 ,3,4,5, 这种字符串,其中逗号直接的数字为用户Id</returns>
        [HttpGet]
        public ApiResult<string> GetBlackListStr(int roomId)
        {
            var result = new ApiResult<string>();
            result.Data = _service.GetBlackListStr(roomId);
            return result;
        }

        /// <summary>
        /// 根据用户Id获取用户状态[ChatBlack:聊天是是否拉黑 1：拉黑 0：正常][IsChatAD:是否是聊天室管理员][MasterLottery:积分最高的彩种名称(空则说明没有)]
        /// </summary>
        /// <returns>[UserId:用户Id]</returns>
        [HttpGet]
        public ApiResult<UserStateResDto> GetUserState()
        {
            var result = new ApiResult<UserStateResDto>();
            result.Data = _service.GetUserState((int)this.UserInfo.UserId);
            return result;
        }

        /// <summary>
        /// 发送图片时保存图片到服务器
        /// </summary>
        /// <param name="model">Img Base64格式字符串</param>
        /// <returns>返回小图URL地址,大图地址去掉_Min即可</returns>
        [HttpPost]
        public ApiResult<string> SaveImg(SaveImgReqDto model)
        {
            //model.Img = HttpUtility.UrlDecode(model.Img);

            model.Img = model.Img.Replace("data:image/jpeg;", "");

            var result = new ApiResult<string>();

            var xPath = "/Upload/TalkingImg/";
            var datePath = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "/";
            string path = System.Web.Hosting.HostingEnvironment.MapPath(xPath) + datePath;//设定上传的文件路径
            string fileName = Guid.NewGuid().ToString();

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                byte[] arr = Convert.FromBase64String(model.Img);

                MemoryStream ms = new MemoryStream(arr);
                Image image = Image.FromStream(ms);
                image.Save(path + fileName + ".jpg");
                image.Dispose();
                ms.Close();

                MemoryStream ms2 = new MemoryStream(ConvertToThumbnail(arr, 70, 100, 100));
                Image image2 = Image.FromStream(ms2);
                image2.Save(path + fileName + "_Min.jpg");
                image2.Dispose();
                ms2.Close();

                string path1 = Tool.UploadFileToOss(xPath + datePath + fileName + "_Min.jpg");
                string path2 = Tool.UploadFileToOss(xPath + datePath + fileName + ".jpg");
                if (string.IsNullOrWhiteSpace(path1) || string.IsNullOrWhiteSpace(path2))
                {
                    result.Data = "";
                    throw new ApiException(50000,"图片服务出错OSS");
                }
                result.Data = path1;

            }
            catch (Exception ex)
            {
                result.Data = "";

                throw new ApiException(ex);
            }

            return result;
        }

        /// <summary>
        /// 图像缩略图处理
        /// </summary>
        /// <param name="bytes">图像源数据</param>
        /// <param name="compression">压缩质量 1-100</param>
        /// <param name="thumbWidth">缩略图的宽</param>
        /// <param name="thumbHeight">缩略图的高</param>
        /// <returns></returns>
        private byte[] ConvertToThumbnail(byte[] bytes, int compression = 100, int thumbWidth = 0, int thumbHeight = 0)
        {
            byte[] bs = null;

            try
            {
                if (bytes != null)
                {
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        using (Bitmap srcimg = new Bitmap(ms))
                        {
                            if (thumbWidth == 0 && thumbHeight == 0)
                            {
                                thumbWidth = srcimg.Width;
                                thumbHeight = srcimg.Height;
                            }
                            if (srcimg.Width > srcimg.Height)
                            {
                                thumbHeight = thumbHeight * srcimg.Height / srcimg.Width;
                            }
                            else
                            {
                                thumbWidth = thumbWidth * srcimg.Width / srcimg.Height;
                            }

                            using (Bitmap dstimg = new Bitmap(thumbWidth, thumbHeight))//图片压缩质量
                            {
                                //从Bitmap创建一个System.Drawing.Graphics对象，用来绘制高质量的缩小图。
                                using (Graphics gr = Graphics.FromImage(dstimg))
                                {
                                    //把原始图像绘制成上面所设置宽高的缩小图
                                    Rectangle rectDestination = new Rectangle(0, 0, thumbWidth, thumbHeight);
                                    gr.Clear(Color.WhiteSmoke);
                                    gr.CompositingQuality = CompositingQuality.HighQuality;
                                    gr.SmoothingMode = SmoothingMode.HighQuality;
                                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    gr.DrawImage(srcimg, rectDestination, 0, 0, srcimg.Width, srcimg.Height, GraphicsUnit.Pixel);

                                    EncoderParameters ep = new EncoderParameters(1);
                                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compression);//设置压缩的比例1-100
                                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                                    ImageCodecInfo jpegICIinfo = arrayICI.FirstOrDefault(t => t.FormatID == System.Drawing.Imaging.ImageFormat.Png.Guid);
                                    using (MemoryStream dstms = new MemoryStream())
                                    {
                                        if (jpegICIinfo != null)
                                        {
                                            dstimg.Save(dstms, jpegICIinfo, ep);
                                        }
                                        else
                                        {
                                            dstimg.Save(dstms, System.Drawing.Imaging.ImageFormat.Png);//保存到内存里
                                        }
                                        bs = new Byte[dstms.Length];
                                        dstms.Position = 0;
                                        dstms.Read(bs, 0, bs.Length);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return bs;
        }

        /// <summary>
        /// 添加聊天记录
        /// </summary>
        /// <param name="model">请求实体</param>
        /// <returns>true:添加成功  false:添加失败</returns>
        [HttpPost]
        public ApiResult<bool> AddMessage(TalkNotesReqDto model)
        {
            var result = new ApiResult<bool>();
            bool bo = _service.AddMessage(model);
            result.Data = bo;
            return result;
        }

        /// <summary>
        /// 根据前端Guid和房间号获取聊天记录
        /// </summary>
        /// <param name="roomId">房间Id</param>
        /// <param name="guid">最上面哪天聊天记录的GUID</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<TalkNotesResDto>> GetMessageList(int roomId, string guid = "")
        {
            var result = new ApiResult<List<TalkNotesResDto>>();
            result.Data = _service.GetMessageList(roomId, guid);
            return result;
        }

        /// <summary>
        /// 管理员删除消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult DelMessage(DelMessageReqDto model)
        {
            var result = new ApiResult();
            _service.DelMessage(model);
            return result;
        }

        /// <summary>
        /// 管理员删除某人全部消息
        /// </summary>
        /// <param name="model">被删除人用户Id</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult DelMessageAll(DelMessageReqDto model)
        {
            var result = new ApiResult();
            _service.DelMessageAll(model);
            return result;
        }

        /// <summary>
        /// 加入黑名单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult AddBlackList(AddBlackReqDto model)
        {
            var result = new ApiResult();
            _service.AddBlackList(model);
            return result;
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="roomId">房间Id</param>
        /// <param name="id">最下面哪天处理记录Id</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<ProcessingRecordsResDto>> GetProcessingRecords(string roomId, int id = 0)
        {
            var result = new ApiResult<List<ProcessingRecordsResDto>>();
            result.Data = _service.GetProcessingRecords(roomId, id);
            return result;
        }

        /// <summary>
        /// 获取拉黑列表
        /// </summary>
        /// <param name="roomId">房间Id</param>
        /// <param name="id">最下面哪天处理记录Id</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<BlackListResDto>> GetBlackList(string roomId, int id = 0)
        {
            var result = new ApiResult<List<BlackListResDto>>();
            result.Data = _service.GetBlackList(roomId, id);
            return result;
        }

        /// <summary>
        /// 解禁
        /// </summary>
        /// <param name="model">被删除人用户Id</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult RemoveBlackList(DelMessageReqDto model)
        {
            var result = new ApiResult();
            _service.RemoveBlackList(model);
            return result;
        }

        /// <summary>
        /// 获取屏蔽字
        /// </summary>
        /// <returns>被屏蔽的关键字字符串</returns>
        [HttpGet][AllowAnonymous]
        public ApiResult<string> GetSensitiveWordsList()
        {
            var str = "";
            string cachekey = Service.RedisKeyConst.Base_SensitiveWords;
            if (CacheHelper.GetCache<string>(cachekey) == null)
            {                
                str = _service.GetSensitiveWordsList();

                CacheHelper.SetCache(cachekey, str, DateTime.Now.AddHours(2));
            }
            else
            {
                str = CacheHelper.GetCache<string>(cachekey).ToString();
            }

            var result = new ApiResult<string>();
            result.Data = str;
            return result;
        }
    }
}
