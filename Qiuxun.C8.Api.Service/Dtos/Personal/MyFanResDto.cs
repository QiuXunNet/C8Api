using System;
namespace Qiuxun.C8.Api.Service.Dtos.Personal
{
    /// <summary>
    /// FanResDto:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class MyFanResDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public long UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public long FollowedUserId
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime FollowTime
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NickName
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Autograph
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Avater
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Isfollowed
        {
            set;
            get;
        }

    }
}

