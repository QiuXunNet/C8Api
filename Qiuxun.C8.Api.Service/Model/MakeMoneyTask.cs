using System;

namespace Qiuxun.C8.Api.Service.Model
{
    /// <summary>
    ///赚钱任务 实体
    /// </summary>
    [Serializable]
    public partial class MakeMoneyTask
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
        /// 任务项
        /// </summary>
        public string TaskItem
        {
            set;
            get;
        }
        /// <summary>
        /// 奖励金币
        /// </summary>
        public int Coin
        {
            set;
            get;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime SubTime
        {
            set;
            get;
        }
        /// <summary>
        /// 任务次数
        /// </summary>
        public int Count
        {
            set;
            get;
        }
        /// <summary>
        /// 任务状态 1-正常 2-禁用 3-删除
        /// </summary>
        public int State
        {
            set;
            get;
        }

        public int Code
        {
            set;
            get;
        }

    }
}
