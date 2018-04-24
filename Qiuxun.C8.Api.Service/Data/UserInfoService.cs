using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CryptSharp;
using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Auth;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;
using Qiuxun.C8.Api.Service.Extension;

namespace Qiuxun.C8.Api.Service.Data
{
    public class UserInfoService
    {
        public ApiResult<LoginResDto> Login(LoginReqDto reqDto, HttpRequestMessage request)
        {
            UserInfoService service = new UserInfoService();
            UserInfo accountInfo = service.GetFullUserInfoByMobile(reqDto.Account);
            if (accountInfo == null)
            {
                throw new ApiException(15023, "用户名不存在或密码错误");
            }

            if (accountInfo.Password.StartsWith("$2y"))
            {
                if (!Crypter.CheckPassword(reqDto.Password, accountInfo.Password))
                {
                    throw new ApiException(15023, "用户名不存在或密码错误");
                }
            }
            else
            {
                if (Tool.GetMD5(reqDto.Password) != accountInfo.Password)
                {
                    throw new ApiException(15023, "用户名不存在或密码错误");
                }
            }

            string webHost = ConfigurationManager.AppSettings["webHost"];
            string avater = string.IsNullOrWhiteSpace(accountInfo.Avater)
                ? string.Format("{0}/images/default_avater.png", webHost)
                : accountInfo.Avater;

            LoginResDto resDto = new LoginResDto()
            {
                Account = accountInfo.Mobile,
                Avater = avater,
                UserId = accountInfo.Id,
                Mobile = accountInfo.Mobile,
                NickName = accountInfo.NickName
            };

            #region 下发登录token

            IdentityInfo authInfo = new IdentityInfo()
            {
                UserId = accountInfo.Id,
                UserAccount = accountInfo.Mobile,
                UserStatus = (int)accountInfo.State,
                UserName = accountInfo.NickName,
                IsTemp = false,
                Avater = avater
            };

            var tokenAuth = new QiuxunTokenAuthorizer(new ApiAuthContainer(request));
            tokenAuth.Authorize(authInfo);

            #endregion

            return new ApiResult<LoginResDto>()
            {
                Data = resDto
            };
        }


        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="reqDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApiResult SetPassword(SetPasswordReqDto reqDto, long userId)
        {
            var userInfo = Util.GetEntityById<UserInfo>((int)userId);
            if (userInfo.Password.StartsWith("$2y"))
            {
                if (!Crypter.CheckPassword(reqDto.OldPassword, userInfo.Password))
                {
                    return new ApiResult(15023, "旧密码不正确");
                }
            }
            else
            {
                if (Tool.GetMD5(reqDto.OldPassword) != userInfo.Password)
                {
                    return new ApiResult(15023, "旧密码不正确");
                }
            }

            string password = Tool.GetMD5(reqDto.Password);

            string sql = "update dbo.userInfo set [Password]=@Password where Id=@UserId";
            var sqlParameter = new[]
            {
                new SqlParameter("@Password", password),
                new SqlParameter("@UserId", userId),
            };
            int count = SqlHelper.ExecuteNonQuery(sql, sqlParameter);

            if (count < 1)
            {
                return new ApiResult(11001, "设置失败");
            }

            return new ApiResult();
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="reqDto"></param>
        /// <returns></returns>
        public ApiResult ForgotPassword(ForgotPasswordReqDto reqDto)
        {
            var user = GetUserInfoByMobile(reqDto.Phone);

            if (user == null)
                throw new ApiException(40000, "账户不存在或未注册");

            string password = Tool.GetMD5(reqDto.Password);

            string sql = "update dbo.userInfo set [Password]=@Password where Mobile=@mobile";
            var sqlParameter = new[]
            {
                new SqlParameter("@Password", password),
                new SqlParameter("@mobile", reqDto.Phone),
            };
            int count = SqlHelper.ExecuteNonQuery(sql, sqlParameter);

            if (count < 1)
            {
                return new ApiResult(11001, "找回密码失败");
            }

            return new ApiResult();
        }


        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ApiResult DeleteAccount(string phone)
        {
            string sql = "Delete from dbo.UserInfo where Mobile=@Mobile";

            int result = SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@Mobile", phone));

            if (result < 1) return new ApiResult(40000, "删除失败");

            return new ApiResult();
        }

        /// <summary>
        /// 根据手机号查询用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByMobile(string account)
        {
            string sql = "select top(1) *,Name as NickName from dbo.UserInfo where Mobile = @mobile ";

            var list = Util.ReaderToList<UserInfo>(sql, new SqlParameter("@Mobile", account));

            if (list != null)
                return list.FirstOrDefault();
            return null;
        }
        /// <summary>
        /// 根据用户Id查询用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public UserInfo GetUserInfo(long uid)
        {
            return Util.GetEntityById<UserInfo>((int)uid);
        }

        /// <summary>
        /// 验证手机号是否存在
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool ExistsMobile(string mobile)
        {
            string sql = "select count(1) from dbo.UserInfo where Mobile = @mobile ";

            object obj = SqlHelper.ExecuteScalar(sql, new SqlParameter("@Mobile", mobile));

            return obj != null && Convert.ToInt32(obj) > 0;
        }

        /// <summary>
        /// 验证昵称是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsNickName(string userName)
        {
            string sql = "select count(1) from dbo.UserInfo where Name = @Name ";

            object obj = SqlHelper.ExecuteScalar(sql, new SqlParameter("@Name", userName));

            return obj != null && Convert.ToInt32(obj) > 0;
        }

        /// <summary>
        /// 获取用户完整信息，
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public UserInfo GetFullUserInfoByMobile(string account)
        {
            string sql = @"select top(1) a.*,isnull(b.RPath,'') as Avater,a.Name as NickName from dbo.UserInfo a 
        left join dbo.ResourceMapping b on b.FkId=a.Id and b.[Type]=@ResourceType
        where a.Mobile = @mobile ";

            var sqlParameter = new[]
            {
                new SqlParameter("@Mobile", account),
                new SqlParameter("@ResourceType", (int) ResourceTypeEnum.用户头像),
            };
            var list = Util.ReaderToList<UserInfo>(sql, sqlParameter);

            if (list != null)
                return list.FirstOrDefault();
            return null;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="request"></param>
        public ApiResult<LoginResDto> UserRegister(RegisterReqDto dto, HttpRequestMessage request)
        {
            string password = Tool.GetMD5(dto.Password);
            string ip = Tool.GetIP();
            string regsql = @"
  insert into UserInfo(UserName, Name, Password, Mobile, Coin, Money, Integral, SubTime, LastLoginTime, State,Pid,RegisterIP)
  values(@UserName, @Name, @Password, @Mobile, 0,0, 0, getdate(), getdate(), 0,@Pid,@RegisterIP);select @@identity ";
            SqlParameter[] regsp = new SqlParameter[]
            {
                new SqlParameter("@UserName", dto.Phone),
                new SqlParameter("@Name", dto.NickName),
                new SqlParameter("@Password", password),
                new SqlParameter("@Mobile", dto.Phone),
                new SqlParameter("@Pid", dto.InviteCode.HasValue ? dto.InviteCode.Value : 0),
                new SqlParameter("@RegisterIP", ip)
            };

            object obj = SqlHelper.ExecuteScalar(regsql, regsp);

            if (obj == null)
                throw new ApiException(50000, "注册失败，请重试");

            int userId = Convert.ToInt32(obj);

            //TODO:事务优化处理
            #region 发放邀请注册奖励
            if (dto.InviteCode.HasValue)
            {
                try
                {
                    var inviteUser = GetUserInfo(dto.InviteCode.Value);
                    if (inviteUser != null)
                    {
                        //受邀奖励
                        int myReward = GetRadomReward(3);
                        AddCoinReward(userId, myReward, 6, (int)inviteUser.Id, 1);
                        //邀请奖励
                        int upReward = GetRadomReward(1);
                        AddCoinReward((int)inviteUser.Id, upReward, 7, userId, 1);
                        //添加邀请任务记录
                        AddUserTask((int)dto.InviteCode.Value, 105);

                        //上级的上级奖励
                        if (inviteUser.Pid.HasValue && inviteUser.Pid > 0)
                        {
                            var superUser = GetUserInfo(inviteUser.Pid.Value);

                            if (superUser != null)
                            {
                                int superReward = GetRadomReward(2);
                                AddCoinReward((int)superUser.Id, superReward, 7, (int)inviteUser.Id, userId);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("发放邀请注册奖励异常", ex);
                }

            }
            #endregion

            #region 下发登录token
            string webHost = ConfigurationManager.AppSettings["webHost"];
            string avater = string.Format("{0}/images/default_avater.png", webHost);
            IdentityInfo authInfo = new IdentityInfo()
            {
                UserId = userId,
                UserAccount = dto.Phone,
                UserStatus = 0,
                UserName = dto.NickName,
                IsTemp = false,
                Avater = avater
            };

            var tokenAuth = new QiuxunTokenAuthorizer(new ApiAuthContainer(request));
            tokenAuth.Authorize(authInfo);

            #endregion

            LoginResDto resDto = new LoginResDto()
            {
                Account = dto.Phone,
                Avater = avater,
                UserId = userId,
                Mobile = dto.Phone,
                NickName = dto.NickName
            };

            return new ApiResult<LoginResDto>()
            {
                Data = resDto
            };
        }


        /// <summary>
        /// 获取随机金币奖励
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public int GetRadomReward(int gradeId)
        {
            int num = 0;

            List<int> listNum = new List<int>();
            try
            {
                string strsql = "select * from CoinRate where GradeId = @GradeId";
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@GradeId", gradeId)
                };
                List<CoinRate> list = Util.ReaderToList<CoinRate>(strsql, sp);
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        for (int i = 0; i < item.Rate; i++)
                        {
                            listNum.Add(item.Num);
                        }

                    }
                }
                Random rm = new Random();
                int j = rm.Next(listNum.Count);
                num = listNum[j];

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("生成随机奖励金币异常。异常消息：{0}，异常堆栈：{1}。", ex.Message, ex.StackTrace));
                return 0;
            }
            return num;
        }

        /// <summary>
        /// 添加金币奖励
        /// </summary>
        /// <param name="id"></param>
        /// <param name="coin"></param>
        /// <param name="type"></param>
        /// <param name="otherId"></param>
        /// <param name="state"></param>
        public void AddCoinReward(int id, int coin, int type, int otherId, int state)
        {
            try
            {

                StringBuilder strSql = new StringBuilder();

                strSql.Append("update UserInfo set Coin+=@Coin where Id =@UserId;");
                strSql.Append(@"insert into CoinRecord(lType, UserId, OtherId, Type, [Money],[State], SubTime)
                             values(0, @UserId, @OtherId, @Type, @Coin,@State, getdate());");

                var parameters = new[]
                {
                    new SqlParameter("@UserId",id),
                    new SqlParameter("@OrderId",otherId),
                    new SqlParameter("@Type",type),
                    new SqlParameter("@Money",coin),
                    new SqlParameter("@State",state)
                };

                SqlHelper.ExecuteTransaction(strSql.ToString(), parameters);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(string.Format("添加金币奖励异常。异常消息：{0}，异常堆栈：{1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// 增加邀请成功注册任务记录
        /// </summary>
        public void AddUserTask(int userId, int taskCode)
        {
            string countsql = "select * from UserTask where UserId=@UserId and TaskId=@TaskId ";
            string strsqlup = "update UserTask set CompletedCount=CompletedCount+1 where UserId=@UserId and TaskId=@TaskId";
            string strsqlins = "insert into UserTask(UserId, TaskId, CompletedCount)values(@UserId, @TaskId, 1)";
            string strtasksql = "select * from MakeMoneyTask   where Code = 105";//邀请成功注册任务

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@UserId",userId),
                new SqlParameter("@TaskId",taskCode)

            };
            try
            {
                UserTask task = Util.ReaderToModel<UserTask>(countsql, sp);
                if (task != null)
                {
                    int update = SqlHelper.ExecuteNonQuery(strsqlup, sp);
                    if (update > 0)
                    {
                        UserTask task1 = Util.ReaderToModel<UserTask>(string.Format("select * from UserTask where UserId={0} and TaskId={1}", userId, taskCode));
                        MakeMoneyTask mtask = Util.ReaderToModel<MakeMoneyTask>(strtasksql);
                        if (task1.CompletedCount == mtask.Count)
                        {
                            string strsqlinstask = string.Format(@"insert into ComeOutRecord(UserId, OrderId, Type, Money, SubTime)
                                   values({0}, {1}, 8, {2}, getdate())", userId, taskCode, mtask.Coin);
                            SqlHelper.ExecuteNonQuery(strsqlinstask);

                        }
                    }
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(strsqlins, sp);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }

}
