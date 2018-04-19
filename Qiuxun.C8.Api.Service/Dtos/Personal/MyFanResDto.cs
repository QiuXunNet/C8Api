using System;
namespace Qiuxun.C8.Api.Service.Dtos.Personal
{
    /// <summary>
    /// FanResDto:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class MyFanResDto
    {
        public MyFanResDto()
        { }
        #region Model
        private long _id;
        private long _userid;
        private long _followed_userid;
        private int _status;
        private DateTime _followtime;
        private string _nickname;
        private string _autograph;
        private string _headpath;
        private int? _isfollowed;
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long Followed_UserId
        {
            set { _followed_userid = value; }
            get { return _followed_userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime FollowTime
        {
            set { _followtime = value; }
            get { return _followtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NickName
        {
            set { _nickname = value; }
            get { return _nickname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Autograph
        {
            set { _autograph = value; }
            get { return _autograph; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeadPath
        {
            set { _headpath = value; }
            get { return _headpath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Isfollowed
        {
            set { _isfollowed = value; }
            get { return _isfollowed; }
        }
        #endregion Model

    }
}

