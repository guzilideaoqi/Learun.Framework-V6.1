/*===================================Copyright© 2021 xxx Ltd. All rights reserved.==============================

文件类名 : CommonSettingInfo.cs
创建人员 : Mr.Hu
创建时间 : 2021-03-06 17:29:02 
备注说明 : 

 =====================================End=======================================================*/
using Learun.Application.TwoDevelopment.DM_APPManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.Common.Model
{
    /// <summary>
    /// CommonSettingInfo
    /// </summary>
    public class CommonSettingInfo
    {
        /// <summary>
        /// 是否为审核模式 1=审核模式  0=非审核模式
        /// </summary>
        public int ischecked { get; set; }
        /// <summary>
        /// 欢迎新人
        /// </summary>
        public string welcomenewperson { get; set; }
        /// <summary>
        /// 显示佣金类型  0显示低佣金   1显示高佣金
        /// </summary>
        public int? showcommission { get; set; }
        /// <summary>
        /// 米圈积分说明
        /// </summary>
        public string miquan_remark { get; set; }
        /// <summary>
        /// 任务说明
        /// </summary>
        public string task_remark { get; set; }
        /// <summary>
        /// 任务提交说明标题
        /// </summary>
        public string task_submit_remark_title { get; set; }
        /// <summary>
        /// 任务提交说明
        /// </summary>
        public string task_submit_remark { get; set; }
        /// <summary>
        /// 无数据提示
        /// </summary>
        public string nodatatip { get; set; }
        /// <summary>
        /// 签到规则
        /// </summary>
        public string sign_rule { get; set; }
        /// <summary>
        /// 提现手续费
        /// </summary>
        public decimal cashrecord_fee { get; set; }
        /// <summary>
        /// 提现说明
        /// </summary>
        public string cashrecord_remark { get; set; }
        /// <summary>
        /// 活动信息
        /// </summary>
        public dm_activity_manageEntity activitysetting { get; set; }

        /// <summary>
        /// 加入活动状态
        /// </summary>
        public int JoinActivity { get; set; }
    }
}
