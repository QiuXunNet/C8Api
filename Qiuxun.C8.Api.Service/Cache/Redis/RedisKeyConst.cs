using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiuxun.C8.Api.Service
{
    /// <summary>
    /// Redis键 常量类
    /// </summary>
    public sealed class RedisKeyConst
    {
        /// <summary>
        /// 首页子彩种列表缓存：{0}为父类ID
        /// </summary>
        public const string Home_ChildLotteryType = "home:child_lottery_type:{0}:website";

        /// <summary>
        /// 首页彩种列表缓存：{0}为父类ID
        /// </summary>
        public const string Home_IndexLotteryList = "home:index_Lottery_List:{0}";

        /// <summary>
        /// 六合彩开奖记录
        /// </summary>
        public const string Home_ChildLotteryTypeLHC = "home:child_lottery_type:5:lhc";

        /// <summary>
        /// 新闻栏目SEO信息,{0}频道ID,{1}栏目ID
        /// </summary>
        public const string News_SeoInfo = "news:seoinfo:{0}:{1}";

        /// <summary>
        /// 新闻频道列表
        /// </summary>
        public const string Base_NewslTypeList = "base:news_ltype_list:all";

        /// <summary>
        /// 新闻频道栏目列表,{0}频道ID
        /// </summary>
        public const string News_ChannelList = "news:channel_list:{0}";

        /// <summary>
        /// 新闻频道栏目总列表
        /// </summary>
        public const string News_ChannelListALL = "news:channel_list:all";

        /// <summary>
        /// 新闻列表数据，{0}栏目ID,{1}分页页码
        /// </summary>
        public const string News_NewsList = "news:news_list:{0}:{1}";

        /// <summary>
        /// 新闻列表广告列表总数据
        /// </summary>
        public const string News_AdvertisementAll = "news:advertisement:all";

        /// <summary>
        /// 新闻图库类型列表，{0}频道ID,{1}栏目ID
        /// </summary>
        public const string News_GalleryTypeList = "news:gallery_type_list:{0}:{1}";

        /// <summary>
        /// 新闻图库列表，{0}频道ID,{1}栏目ID
        /// </summary>
        public const string News_GalleryList = "news:gallery_list:{0}:{1}";

        /// <summary>
        /// 新闻详情推荐阅读
        /// </summary>
        public const string News_Recommend = "news:recommend:{0}";

        /// <summary>
        /// 新闻详情竞猜红人
        /// </summary>
        public const string News_GuessGood = "news:guess_good:{0}";

        /// <summary>
        /// 新闻图库推荐图
        /// </summary>
        public const string News_RecommendGallery = "news:gallery_list:recommend:{0}";

        /// <summary>
        /// 新闻PV
        /// </summary>
        public const string News_NewsPV = "news:news_pv:{0}";

        /// <summary>
        /// 新闻PV
        /// </summary>
        public const string News_NewsListApi = "news:news_list:api";

        /// <summary>
        /// 网站信息
        /// </summary>
        public const string Base_SiteSetting = "base:site_setting:site";

        /// <summary>
        /// 彩种分类信息
        /// </summary>
        public const string Base_LotteryType = "base:lottery_type:type";

        /// <summary>
        /// 新闻栏目分类列表,{0}彩种ID
        /// </summary>
        public const string Base_NewsType = "base:news_type:{0}";

        /// <summary>
        /// 当前分类的玄机图库,{0}新闻Id
        /// </summary>
        public const string Base_GalleryId = "base:gallery_id:{0}";

        /// <summary>
        /// 彩种玩法,{0}彩种ID
        /// </summary>
        public const string Base_PlayName = "base:play_name:{0}";

        /// <summary>
        /// 贴子点阅扣费配置表
        /// </summary>
        public const string Base_LotteryChargeSettings = "base:setting:lottery_charge_settings";

        /// <summary>
        /// 分佣配置
        /// </summary>
        public const string Base_CommissionSettings = "base:setting:commission_settings";

        /// <summary>
        /// 敏感词
        /// </summary>
        public const string Base_SensitiveWords = "base:sensitive_words:all";

        /// <summary>
        /// 安装包,{0}客户端来源
        /// </summary>
        public const string Installationpackage_Sourceversion = "installationpackage:sourceversion:{0}";

        /// <summary>
        /// 安装包来源
        /// </summary>
        public const string Installationpackage_ClientSourceKey = "installationpackage:client_source:{0}_{1}_{2}_{3}";

        /// <summary>
        /// 登录信息,{0}guid
        /// </summary>
        public const string Login_LoginGuid = "login:logon_guid:{0}";

        /// <summary>
        /// api登录信息,{0}token
        /// </summary>
        public const string Login_LoginApiToken = "login:logon_apiToken:{0}";

        /// <summary>
        /// 父彩种
        /// </summary>
        public const string Home_FatherLotteryType = "home:father_lottery_type:website";

        /// <summary>
        /// 首页新闻
        /// </summary>
        public const string Home_NewsList = "home:news_list:website";

        /// <summary>
        /// 友链总列表
        /// </summary>
        public const string FriendshipLinks_List = "friendshipLinks:list:all";

        /// <summary>
        /// 友链链接
        /// </summary>
        public const string FriendshipLinks_Link = "friendshipLinks:link:{0}";

        /// <summary>
        /// 友链链接IP
        /// </summary>
        public const string FriendshipLinks_LinkIP = "friendshipLinks:IP:{0}:{1}";

        /// <summary>
        /// 友链链接总IP
        /// </summary>
        public const string FriendshipLinks_LinkIPs = "friendshipLinks:link_ip:{0}";

        /// <summary>
        /// 友链链接总UV
        /// </summary>
        public const string FriendshipLinks_LinkUVs = "friendshipLinks:link_uv:{0}";

        /// <summary>
        /// 友链链接总PV
        /// </summary>
        public const string FriendshipLinks_LinkPVs = "friendshipLinks:link_pv:{0}";

        /// <summary>
        /// 佣金榜
        /// </summary>
        public const string Rank_MoneyList = "rank:moneylist:website:{0}:{1}_{2}";

        /// <summary>
        /// 积分榜
        /// </summary>
        public const string Rank_IntegralList = "rank:integrallist:website:{0}";

        /// <summary>
        /// 高手总榜
        /// </summary>
        public const string Rank_SuperiorTotal = "rank:superior:{0}:total:{1}";

        /// <summary>
        /// 高手月榜
        /// </summary>
        public const string Rank_SuperiorMonth = "rank:superior:{0}:month:{1}";

        /// <summary>
        /// 高手周榜
        /// </summary>
        public const string Rank_SuperiorWeek = "rank:superior:{0}:week:{1}";

        /// <summary>
        /// 高手日榜
        /// </summary>
        public const string Rank_SuperiorDay = "rank:superior:{0}:day:{1}";

        /// <summary>
        /// 粉丝榜
        /// </summary>
        public const string Rank_Fans = "rank:fans:{0}";

        /// <summary>
        /// 精彩评论
        /// </summary>
        public const string Comment_Wonderful = "comment:wonderful:{0}:{1}:{2}";

        /// <summary>
        /// 最新评论
        /// </summary>
        public const string Comment_Newest = "comment:newest:{0}:{1}:{2}:{3}";

        /// <summary>
        /// 评论详情
        /// </summary>
        public const string Comment_Detail = "comment:detail:{0}:{1}";

        /// <summary>
        /// 计划官方推荐列表
        /// </summary>
        public const string Plan_RecommendList = "plan:recommend:list:{0}:{1}";

        /// <summary>
        /// 计划官方推荐列表Api
        /// </summary>
        public const string Plan_RecommendListApi = "plan:recommend_api:list:{0}:{1}";

        /// <summary>
        /// 计划官方推荐十期中奖
        /// </summary>
        public const string Plan_RecommendLotteryRecord = "plan:recommend:lotteryRecord:{0}:{1}";

        /// <summary>
        /// 计划专家列表
        /// </summary>
        public const string Plan_ExpertList = "plan:expert:list:{0}:{1}:{2}";

        /// <summary>
        /// 计划专家热搜
        /// </summary>
        public const string Plan_ExpertHistory = "plan:expert:history:{0}:{1}";

        /// <summary>
        /// 开奖记录
        /// </summary>
        public const string Record_List = "record:list:{0}";

        /// <summary>
        /// 开奖时间
        /// </summary>
        public const string LotteryTime_List = "lotteryTime:list:all";

        /// <summary>
        /// 开奖时间
        /// </summary>
        public const string LotteryTime_Type = "lotteryTime:type:{0}";

        /// <summary>
        /// 广告列表
        /// </summary>
        public const string Advertisement_List = "advertisement:{0}:{1}:{2}";
    }
}
