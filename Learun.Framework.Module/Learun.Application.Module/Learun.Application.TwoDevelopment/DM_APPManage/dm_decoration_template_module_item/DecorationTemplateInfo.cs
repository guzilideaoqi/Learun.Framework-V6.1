/*===================================Copyright© 2021 xxx Ltd. All rights reserved.==============================

文件类名 : DecorationTemplateInfo.cs
创建人员 : Mr.Hu
创建时间 : 2021-03-04 17:10:53 
备注说明 : 

 =====================================End=======================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.DM_APPManage.dm_decoration_template_module_item
{
    /// <summary>
    /// 装修模板信息
    /// </summary>
    public class DecorationTemplateInfo
    {
        /// <summary>
        /// 主色
        /// </summary>
        public string MainColor { get; set; }

        /// <summary>
        /// 辅色
        /// </summary>
        public string SecondaryColor { get; set; }

        /// <summary>
        /// 模板对应模块
        /// </summary>
        public List<ModuleInfoEntity> ModuleInfoList { get; set; }
    }
}
