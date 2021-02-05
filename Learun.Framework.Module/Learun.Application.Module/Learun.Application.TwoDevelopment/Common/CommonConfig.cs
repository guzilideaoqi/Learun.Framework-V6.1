using Learun.Application.TwoDevelopment.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.Common
{
    public class CommonConfig
    {
        /// <summary>
        /// 图片的前缀
        /// </summary>
        //public static string ImageQianZhui = "";

        public const string tb_auth_address = "http://dlaimi.cn/TBUserInfoController/AuthorCallBack";

        /// <summary>
        /// 淘宝官方接口地址
        /// </summary>
        public const string tb_api_url = "http://gw.api.taobao.com/router/rest";

        /// <summary>
        /// 无数据提示
        /// </summary>
        public const string NoDataTip = "人家也是有底线的";

        public static string[] NoRecordRequestKeyWord = new string[] { "ALL_HTTP", "ALL_RAW", "APPL_MD_PATH", "APPL_PHYSICAL_PATH", "AUTH_TYPE", "AUTH_USER", "AUTH_PASSWORD", "LOGON_USER", "REMOTE_USER", "CERT_COOKIE", "CERT_FLAGS", "CERT_ISSUER", "CERT_KEYSIZE", "CERT_SECRETKEYSIZE", "CERT_SERIALNUMBER", "CERT_SERVER_ISSUER", "CERT_SERVER_SUBJECT", "CERT_SUBJECT", "CONTENT_LENGTH", "CONTENT_TYPE", "GATEWAY_INTERFACE", "HTTPS", "HTTPS_KEYSIZE", "HTTPS_SECRETKEYSIZE", "HTTPS_SERVER_ISSUER", "HTTPS_SERVER_SUBJECT", "INSTANCE_ID", "INSTANCE_META_PATH", "LOCAL_ADDR", "PATH_INFO", "PATH_TRANSLATED", "QUERY_STRING", "REMOTE_ADDR", "REMOTE_HOST", "REMOTE_PORT", "REQUEST_METHOD", "SCRIPT_NAME", "SERVER_NAME", "SERVER_PORT", "SERVER_PORT_SECURE", "SERVER_PROTOCOL", "SERVER_SOFTWARE", "URL", "HTTP_CONNECTION", "HTTP_CONTENT_LENGTH", "HTTP_CONTENT_TYPE", "HTTP_ACCEPT", "HTTP_ACCEPT_ENCODING", "HTTP_ACCEPT_LANGUAGE", "HTTP_HOST", "HTTP_USER_AGENT", "HTTP_ORIGIN", "HTTP_APPID", "HTTP_TIMESTAMP", "HTTP_IMEI_CODE", "HTTP_PLATFORM", "HTTP_TOKEN", "HTTP_VERSION" };

        public static List<IP_Limit> iP_Limits = new List<IP_Limit>();//IP地址限制

        public static string NoActivityTip = "未找到正在进行中的任务";

        /// <summary>
        /// 活动信息配置
        /// </summary>
        public static ActivityInfoSetting activityInfoSetting1 = new ActivityInfoSetting
        {
            APP_RedPaper_Image = "",
            APP_RedPaper_Text = "瓜分2000万红包",
            APP_Rock_RedPaper_Image = "",
            APP_To_ActivityUrl = "http://dev.dlaimi.cn/DLM_Page/ActivityPage",
            ActivityTitle = "瓜分2021万",
            ActivityType = 1,
            ActivityStartTime = DateTime.Parse("2021-02-01 00:00:00"),
            ActivityEndTime = DateTime.Parse("2021-02-15 00:00:00"),
            ActivityCode = "DLM10001",
            ActivityStatus = 1,//1进行中的活动  0活动已暂停
            ActivityRemark = string.Format(@"<p>1、活动任务每人限制接受一次</p>
<p>2、任务一旦接受不能取消</p>
<p>3、任务结束未完成将不发放金额</p>")
        };
    }

    public class IP_Limit
    {
        /// <summary>
        /// 记录开始请求时间点
        /// </summary>
        public DateTime RequestTime { get; set; }
        /// <summary>
        /// 记录请求数量
        /// </summary>
        public int RequestCount { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }
    }
}
