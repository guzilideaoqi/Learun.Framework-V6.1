/*===================================Copyright© 2021 xxx Ltd. All rights reserved.==============================

文件类名 : ActivityInfoSetting.cs
创建人员 : Mr.Hu
创建时间 : 2021-02-02 16:11:06 
备注说明 : 

 =====================================End=======================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.Common.Model
{
    /// <summary>
    /// 活动配置
    /// </summary>
    public class ActivityInfoSetting
    {
        /// <summary>
        /// APP端红包图片
        /// </summary>
        public string APP_RedPaper_Image { get; set; }

        /// <summary>
        /// 红包上文本说明
        /// </summary>
        public string APP_RedPaper_Text { get; set; }

        /// <summary>
        /// APP端摇晃红包图片
        /// </summary>
        public string APP_Rock_RedPaper_Image { get; set; }

        /// <summary>
        /// APP端活动地址
        /// </summary>
        public string APP_To_ActivityUrl { get; set; }

        /// <summary>
        /// 活动标题
        /// </summary>
        public string ActivityTitle { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime ActivityStartTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime ActivityEndTime { get; set; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// 活动说明
        /// </summary>
        public string ActivityRemark { get; set; }

        /// <summary>
        /// 活动状态
        /// </summary>
        public int ActivityStatus { get; set; }
    }
}
