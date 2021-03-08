/*===================================Copyright© 2021 xxx Ltd. All rights reserved.==============================

文件类名 : ModuleItemInfoEntity.cs
创建人员 : Mr.Hu
创建时间 : 2021-03-04 16:27:22 
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
    /// ModuleItemInfoEntity
    /// </summary>
    public class ModuleItemInfoEntity
    {
        public int? id { get; set; }
        /// <summary>
        /// 模块项名称
        /// </summary>
        public string module_item_name { get; set; }

        /// <summary>
        /// 模块项图片
        /// </summary>
        public string module_item_image { get; set; }

        /// <summary>
        /// 模块项对应功能
        /// </summary>
        public int? module_fun_id { get; set; }

        /// <summary>
        /// 模块对应功能项的名称
        /// </summary>
        public string module_fun_name { get; set; }

        /// <summary>
        /// 模块对应功能类型
        /// </summary>
        public int? module_fun_type { get; set; }

        /// <summary>
        /// 功能模块对应参数
        /// </summary>
        public string module_fun_param { get; set; }

        /// <summary>
        /// 模块所属类别
        /// </summary>
        public int? module_fun_category { get; set; }

        /// <summary>
        /// 模块类别名称
        /// </summary>
        public string module_fun_category_name { get; set; }

        /// <summary>
        /// 模块项对应排序
        /// </summary>
        public int? module_sort { get; set; }

        /// <summary>
        /// 模块项类型
        /// </summary>
        public string module_item_type { get; set; }
    }
}
