namespace Qiuxun.C8.Api.Service.Model
{
    /// <summary>
    /// 用户任务表
    /// </summary>
    public partial class UserTask
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set;
            get;
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId
        {
            set;
            get;
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int TaskId
        {
            set;
            get;
        }
        /// <summary>
        /// 已完成次数
        /// </summary>
        public int CompletedCount
        {
            set;
            get;
        }

    }
}
