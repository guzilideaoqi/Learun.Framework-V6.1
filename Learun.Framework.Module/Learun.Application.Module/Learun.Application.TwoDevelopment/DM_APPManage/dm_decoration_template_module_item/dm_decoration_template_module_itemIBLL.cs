﻿using Learun.Application.TwoDevelopment.DM_APPManage.dm_decoration_template_module_item;
using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:58
    /// 描 述：模块对应功能
    /// </summary>
    public interface dm_decoration_template_module_itemIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_decoration_template_module_itemEntity> GetList( string queryJson );
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_decoration_template_module_itemEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        dm_decoration_template_module_itemEntity GetEntity(int? keyValue);
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void DeleteEntity(int? keyValue);
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void SaveEntity(int? keyValue, dm_decoration_template_module_itemEntity entity);
        #endregion

        #region 保存/获取模板数据
        /// <summary>
        /// 保存模板数据
        /// </summary>
        /// <param name="jsondata"></param>
        void SaveDecorationTemplateData(int templateid,string jsondata);
        /// <summary>
        /// 获取模板数据
        /// </summary>
        /// <param name="templateid"></param>
        /// <returns></returns>
        DecorationTemplateInfo GetDecorationTemplateData(int templateid);
        #endregion
    }
}
