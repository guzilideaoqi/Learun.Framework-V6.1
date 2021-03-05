/*===================================Copyright© 2021 xxx Ltd. All rights reserved.==============================

文件类名 : ModuleInfoEntity.cs
创建人员 : Mr.Hu
创建时间 : 2021-03-04 16:27:09 
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
    /// ModuleInfoEntity
    /// </summary>
    public class ModuleInfoEntity
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 模块类型(如果是多眼加强版需要返回类别)
        /// </summary>
        public string ModuleType { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        public int? ModuleID { get; set; }

        public List<ModuleItemInfoEntity> ModuleItemInfoList { get; set; }
    }
}
