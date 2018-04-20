using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiuxun.C8.Api.Public;
using Qiuxun.C8.Api.Service.Common;
using Qiuxun.C8.Api.Service.Dtos;
using Qiuxun.C8.Api.Service.Enum;

namespace Qiuxun.C8.Api.Service.Data
{
    /// <summary>
    /// 消息业务类
    /// </summary>
    public class SmsService
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool Send(SmsSendDto dto)
        {
            string code = SmsSender.GetVCode();
            int sendResult = SmsSender.SendMsgByTXY("", dto.Receiver, code);

            var sendLog = new SmsSendLog()
            {
                UserId = dto.UserId,
                Type = dto.Type,
                Code = code,
                Count = 1,
                Receiver = dto.Receiver,
                Sender = dto.Sender,
                SendPort = 0,
                SendTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Status = 0,
                SendResult = sendResult
            };

            if (sendResult != 0)
            {
                sendLog.Status = 2;
            }
            else
            {
                sendLog.Status = 1;
            }

            string sql = string.Format(@"INSERT INTO [dbo].[SmsSendLog]
      ([UserId],[Sender],[Receiver],[Type],[Code],[Status],[Count],[SendTime],[SendPort],[SendResult],[UpdateTime])
      VALUES({0},'{1}','{2}',{3},'{4}',{5},{6},GETDATE(),{7},{8},GETDATE())",
      sendLog.UserId, sendLog.Sender, sendLog.Receiver, sendLog.Type, sendLog.Code,
      sendLog.Status, sendLog.Status, sendLog.SendPort, sendLog.SendResult);

            SqlHelper.ExecuteNonQuery(sql);

            return sendLog.SendResult == 0;

        }

        /// <summary>
        /// 发送模板消息，包含参数
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool Send(SmsSendDto dto, Dictionary<string, string> param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="dto"></param>
        public void ValidateSmsCode(SmsCodeValidateDto dto)
        {
            #region 参数验证
            if (dto == null)
                throw new ApiException(50000, "参数不能为空");
            if (string.IsNullOrEmpty(dto.Code))
                throw new ApiException(50000, "验证码不能为空");
            if (string.IsNullOrEmpty(dto.Phone))
                throw new ApiException(50000, "手机号码不能为空");
            #endregion

            #region 开发模式
            if (ServerRoles.Instance.IsDevServer || ServerRoles.Instance.IsTestServer)
            {
                if (dto.Code.ToUpper() == "123456") return;
            }
            #endregion

            //查询十分钟内未使用的验证码
            string sql = @"SELECT Id,UserId,Receiver,Type,Code,Count,[Status] FROM dbo.SmsSendLog
    WHERE [Status]=1 AND SendTime > @SendTime AND Receiver=@Receiver AND Type=@Type ORDER BY Id DESC";

            var sinceTime = DateTime.Now.AddMinutes(-10);
            var paramters = new[]
            {
                new SqlParameter("@SendTime",sinceTime),
                new SqlParameter("@Receiver",dto.Phone),
                new SqlParameter("@Type",dto.Type),
            };

            var list = Util.ReaderToList<SmsSendLog>(sql, paramters);

            var valid = list.Any(x => x.Code == dto.Code);

            list.ForEach(x =>
            {
                x.Count++;
                x.Status = (!valid && x.Count < 5) ? (int)SmsStatusEnum.Used : (int)SmsStatusEnum.Failue;
                UpdateMsgLog(x);
            });

            if (!valid)
                throw new ApiException(50000, "验证码错误，请重新输入");

        }

        /// <summary>
        /// 更新消息日志
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateMsgLog(SmsSendLog dto)
        {
            string sql = @"UPDATE dbo.SmsSendLog SET Count=@Count,[Status]=@Status,UpdateTime=GETDATE()
    WHERE Id=@Id";
            var paramters = new[]
             {
                new SqlParameter("@Count",dto.Count),
                new SqlParameter("@Status",dto.Status),
                new SqlParameter("@Id",dto.Id),
            };

            SqlHelper.ExecuteNonQuery(sql, paramters);
        }
    }
}
