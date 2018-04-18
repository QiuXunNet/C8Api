using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Model
{
    /// <summary>
    /// 用户任务表
    /// </summary>
    [Serializable]
    public partial class UserTask
    {
        public UserTask()
        { }
        #region Model
        private int _id;
        private long _userid;
        private int _taskid;
        private int _completedcount = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int TaskId
        {
            set { _taskid = value; }
            get { return _taskid; }
        }
        /// <summary>
        /// 已完成次数
        /// </summary>
        public int CompletedCount
        {
            set { _completedcount = value; }
            get { return _completedcount; }
        }
        #endregion Model

    }
}
