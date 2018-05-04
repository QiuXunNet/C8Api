using Qiuxun.C8.Api.Model;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Caching;
using Qiuxun.C8.Api.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 聊天数据库服务类
    /// </summary>
    public class TalkingService
    {
        /// <summary>
        /// 获取房间列表
        /// </summary>
        public List<ChatRoomResDto> GetChatRoomList()
        {
            string sql = "select Id,Name,LogoPath from ChatRoom order by Code ";
            var list = Util.ReaderToList<ChatRoomResDto>(sql);

            return list;
        }

        /// <summary>
        /// 根据房间Id查询该房间拉黑的用户
        /// </summary>
        /// <param name="roomId">房间Id</param>
        /// <returns></returns>
        public string GetBlackListStr(int roomId)
        {
            string sql = "select UserId from TalkBlackList where RoomId = @RoomId and (IsEverlasting =1 or EndTime > GETDATE())";

            SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@RoomId",roomId)
                 };

            var blackListStr = "," + string.Join(",", Util.ReaderToList<TalkBlackList>(sql, regsp).Select(e => e.UserId)) + ",";

            return blackListStr;
        }

        /// <summary>
        /// 根据用户ID返回用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserStateResDto GetUserState(int userId)
        {
            string usersql = @"select top(1) * from UserState where UserId = @UserId ";

            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@UserId", userId) };
            List<UserStateResDto> list = Util.ReaderToList<UserStateResDto>(usersql, sp);
            UserStateResDto model;
            if (list.Any())
            {
                model = list.FirstOrDefault();
            }
            else
            {
                model = new UserStateResDto() {
                    ChatBlack = 0,
                    ChatShut = 0,
                    ChatShutBegin = null,
                    ChatShutEnd = null,
                    CommentBlack = 0,
                    CommentShut = 0,
                    CommentShutBegin = null,
                    CommentShutEnd = null,
                    IsChatAD = 0,
                    IsInfoAD = 0,
                    IsPlanAD = 0,                   
                    UserId = userId
                };
            }

            //获取积分大于100分且排在第一未的彩种
            usersql = @"select top(1) tab.Name from (
	                        select sum(br.Score) as Score , lt.Name from BettingRecord br
	                        left join LotteryType2 lt on br.lType= lt.lType
	                        where br.UserId = @UserId
	                        group by lt.Name
                        ) as tab 
                        where tab.Score>=10
                        order by score desc";

            model.MasterLottery = Convert.ToString(SqlHelper.ExecuteScalar(usersql, sp)).Replace("(PC蛋蛋)","");
            return model;
        }

        /// <summary>
        /// 添加聊天记录
        /// </summary>
        /// <param name="model"></param>
        public bool AddMessage(TalkNotesReqDto model)
        {
            string regsql = @"insert into TalkNotes (Content,UserId,UserName,PhotoImg,SendTime,RoomId,MsgTypeChild,Status,Guid,IsAdmin,MasterLottery)
                                values (@Content,@UserId,@UserName,@PhotoImg,@SendTime,@RoomId,@MsgTypeChild,@Status,@Guid,@IsAdmin,@MasterLottery);";
            SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@Content",model.Content),
                    new SqlParameter("@UserId",model.UserId),
                    new SqlParameter("@UserName",model.UserName),
                    new SqlParameter("@PhotoImg",model.PhotoImg??""),
                    new SqlParameter("@SendTime",DateTime.Now),
                    new SqlParameter("@RoomId",model.RoomId),
                    new SqlParameter("@MsgTypeChild",model.MsgTypeChild),
                    new SqlParameter("@Status",1),
                    new SqlParameter("@Guid",model.Guid),
                    new SqlParameter("@IsAdmin",model.IsAdmin),
                    new SqlParameter("@MasterLottery",model.MasterLottery??"")
                 };

            var i = SqlHelper.ExecuteNonQuery(regsql, regsp);

            return i > 0;
        }

        /// <summary>
        /// 根据前端Guid和房间号获取聊天记录
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public List<TalkNotesResDto> GetMessageList(int roomId, string guid = "")
        {
            string sql = @"select top 20 * from TalkNotes 
                            where RoomId = @RoomId and Status = 1 {0}                            
                            order by id desc ";

            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@RoomId", roomId), new SqlParameter("@Guid", guid) };

            if (!string.IsNullOrEmpty(guid))
            {
                sql = string.Format(sql, " and id<(select top(1) id from TalkNotes where RoomId = @RoomId and Status = 1 and Guid = @Guid) ");
            }
            else
            {
                sql = string.Format(sql, " ");
            }

            var list = Util.ReaderToList<TalkNotesResDto>(sql, sp);
            list.ForEach(e => e.SendTimeStr = e.SendTime.ToString("HH:mm"));
            return list;
        }

        /// <summary>
        /// 管理员删除消息
        /// </summary>
        /// <returns></returns>
        public void DelMessage(DelMessageReqDto model)
        {
            string sql = @" update TalkNotes set Status = 0 where Guid=@Guid ";

            int i = SqlHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@Guid", model.Guid) });

            var processingRecords = new ProcessingRecords()
            {
                ProcessToId = model.UserId,
                ProcessToName = model.UserName,
                Type = 1,
                RoomId = model.RoomId,
                ProcessId = model.ProcessId,
                ProcessName = model.ProcessName
            };

            AddProcessingRecords(processingRecords);
        }

        /// <summary>
        /// 管理员删除某人全部消息
        /// </summary>
        /// <returns></returns>
        public void DelMessageAll(DelMessageReqDto model)
        {
            string sql = @" update TalkNotes set Status = 0 where RoomId = @RoomId and UserId = @UserId   ";

            int i = SqlHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@RoomId", model.RoomId), new SqlParameter("@UserId", model.UserId) });

            var processingRecords = new ProcessingRecords()
            {
                ProcessToId = model.UserId,
                ProcessToName = model.UserName,
                Type = 1,
                RoomId = model.RoomId,
                ProcessId = model.ProcessId,
                ProcessName = model.ProcessName
            };

            AddProcessingRecords(processingRecords);
        }

        /// <summary>
        /// 加入黑名单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void AddBlackList(AddBlackReqDto model)
        {
            string regsql = @" delete TalkBlackList where UserId = @UserId and RoomId = @RoomId ;
                                    insert into TalkBlackList (UserId,RoomId,BanTime,IsEverlasting) 
                                    values (@UserId,@RoomId,@BanTime,@IsEverlasting); ";
            SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@UserId",model.UserId),
                    new SqlParameter("@RoomId",model.RoomId),
                    new SqlParameter("@BanTime",DateTime.Now),
                    new SqlParameter("@IsEverlasting",true)
                 };

            SqlHelper.ExecuteNonQuery(regsql, regsp);

            var processingRecords = new ProcessingRecords()
            {
                ProcessToId = model.UserId,
                ProcessToName = model.UserName,
                Type = 2,
                RoomId = model.RoomId,
                ProcessId = model.ProcessId,
                ProcessName = model.ProcessName
            };

            AddProcessingRecords(processingRecords);
        }

        /// <summary>
        /// 插入处理记录
        /// </summary>
        /// <param name="model"></param>
        private int AddProcessingRecords(ProcessingRecords model)
        {
            model.ProcessDate = DateTime.Now;
            model.ProcessTime = DateTime.Now;

            string regsql = @"insert into ProcessingRecords (ProcessId,ProcessName,Type,ProcessToId,ProcessToName,ProcessDate,ProcessTime,RoomId)
                                values (@ProcessId,@ProcessName,@Type,@ProcessToId,@ProcessToName,@ProcessDate,@ProcessTime,@RoomId)";
            SqlParameter[] regsp = new SqlParameter[] {
                    new SqlParameter("@ProcessId",model.ProcessId),
                    new SqlParameter("@ProcessName",model.ProcessName),
                    new SqlParameter("@Type",model.Type),
                    new SqlParameter("@ProcessToId",model.ProcessToId),
                    new SqlParameter("@ProcessToName",model.ProcessToName),
                    new SqlParameter("@ProcessDate",model.ProcessDate),
                    new SqlParameter("@ProcessTime",model.ProcessTime),
                    new SqlParameter("@RoomId",model.RoomId)
                 };

            var i = SqlHelper.ExecuteNonQuery(regsql, regsp);
            return i;
        }

        /// <summary>
        /// 获取处理记录列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProcessingRecordsResDto> GetProcessingRecords(string roomId, int id = 0)
        {
            string sql = "select top(20) * from ProcessingRecords where RoomId =@RoomId {0} order by ProcessTime desc";

            if (id == 0)
            {
                sql = string.Format(sql, "");
            }
            else
            {
                sql = string.Format(sql, " and Id <@Id ");
            }

            var list = Util.ReaderToList<ProcessingRecords>(sql, new SqlParameter[] { new SqlParameter("@Id", id), new SqlParameter("@RoomId", roomId) });

            List<ProcessingRecordsResDto> dyList = new List<ProcessingRecordsResDto>();

            list.ForEach(e =>
            {
                var msg = "";
                switch (e.Type)
                {
                    case 1:
                        msg = "删除消息";
                        break;
                    case 2:
                        msg = "拉黑";
                        break;
                    case 3:
                        msg = "解除拉黑";
                        break;
                }

                dyList.Add(new ProcessingRecordsResDto()
                {
                    Id = e.Id,
                    Date = e.ProcessDate.ToString("yyyy-MM-dd"),
                    Message = "管理员对用户\"" + e.ProcessToName + "\"进行" + msg + "处理",
                    Time = e.ProcessTime.ToString("HH:mm")
                });
            });

            return dyList;
        }

        /// <summary>
        /// 获取拉黑列表
        /// </summary>
        /// <param name="id">拉黑记录Id</param>
        /// <param name="roomId">房间Id</param>
        /// <returns></returns>
        public List<BlackListResDto> GetBlackList(string roomId, int id = 0)
        {
            string sql = @"select top(20) tbl.Id,tbl.UserId,u.UserName,rm.RPath PhotoImg,tbl.RoomId from TalkBlackList tbl
                                left join UserInfo u on tbl.UserId = u.Id
                                left join ResourceMapping rm on u.Id = rm.FkId and rm.Type =2  
                                where tbl.RoomId =@RoomId {0} and (tbl.IsEverlasting = 1 or EndTime >GETDATE())
                                order by tbl.Id desc";

            if (id == 0)
            {
                sql = string.Format(sql, "");
            }
            else
            {
                sql = string.Format(sql, " and tbl.Id <@Id ");
            }

            var list = Util.ReaderToList<BlackListResDto>(sql, new SqlParameter[] { new SqlParameter("@Id", id), new SqlParameter("@RoomId", roomId) });

            return list;
        }

        /// <summary>
        /// 解禁
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public void RemoveBlackList(DelMessageReqDto model)
        {
            string sql = "delete TalkBlackList where UserId = @UserId and RoomId = @RoomId";

            SqlHelper.ExecuteScalar(sql, new SqlParameter[] { new SqlParameter("@UserId", model.UserId), new SqlParameter("@RoomId", model.RoomId) });

            var processingRecords = new ProcessingRecords()
            {
                ProcessToId = model.UserId,
                ProcessToName = model.UserName,
                Type = 3,
                RoomId = model.RoomId,
                ProcessId = model.ProcessId,
                ProcessName = model.ProcessName
            };

            AddProcessingRecords(processingRecords);
        }

        /// <summary>
        /// 获取屏蔽字
        /// </summary>
        /// <returns></returns>
        public string GetSensitiveWordsList()
        {
            //屏蔽字一般不会变动，为减少数据库操作，加入2小时缓存
            var str = "";
            if (CacheHelper.GetCache("GetSensitiveWordsList") == null)
            {
                string sql = " select content from SensitiveWords ";
                str = Convert.ToString(SqlHelper.ExecuteScalar(sql));

                CacheHelper.AddCache("GetSensitiveWordsList", str, DateTime.Now.AddHours(2));
            }
            else
            {
                str = CacheHelper.GetCache("GetSensitiveWordsList").ToString();
            }

            return str;
        }
    }
}
