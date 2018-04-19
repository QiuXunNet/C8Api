using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Service.Dtos.Personal;
using Qiuxun.C8.Api.Public;
using System.Data.SqlClient;
using Qiuxun.C8.Api.Service.Enum;
using Qiuxun.C8.Api.Service.Common;
using System.Data;

namespace Qiuxun.C8.Api.Service.Data
{
    public class PersonalService
    {
        public IndexResDto GetPersonalIndexData(long userId)
        {

            string usersql = @"select (select count(1)from Follow where UserId=u.Id and Status=1)as Follow,(select count(1)from Follow where Followed_UserId=u.Id and Status=1)as Fans, r.RPath as Headpath,u.* from UserInfo  u 
                              left  JOIN (select RPath,FkId from ResourceMapping where Type = @Type)  r 
                              on u.Id=r.FkId  where u.Id=@userId ";
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@userId", userId), new SqlParameter("@Type", (int)ResourceTypeEnum.用户头像) };

            IndexResDto resDto = Util.ReaderToModel<IndexResDto>(usersql, sp);

            string noticeCountSql = "select count(1) from dbo.UserInternalMessage where IsDeleted=0 and IsRead=0 and UserId=" + userId;
            object noticeCount = SqlHelper.ExecuteScalar(noticeCountSql);

            resDto.NoticeCount = Convert.ToInt32(noticeCount);
            return resDto;
        }

        public ApiResult ModifyPWD(string oldpwd, string newpwd,long userId)
        {
            string sql = "SELECT Password FROM dbo.UserInfo WHERE Id=" + userId;
            object currentPwd = SqlHelper.ExecuteScalar(sql);
            if (currentPwd == null)
            {
                return new ApiResult(60000, "登录超时，需要重新登录");
            }
            if (Tool.GetMD5(oldpwd) != currentPwd.ToString())
            {
                return new ApiResult(60004, "旧密码错误，请重新输入");
            }
            sql = string.Format("UPDATE dbo.UserInfo SET Password='{0}' WHERE Id={1}", Tool.GetMD5(newpwd), userId);
            int result = SqlHelper.ExecuteNonQuery(sql);
            if (result > 0)
            {
                return new ApiResult(100, "success");
            }
            else
            {
                return new ApiResult(-999, "保存数据出现错误。");
            }
        }
    }
}
