using Learun.Application.TwoDevelopment.DM_APPManage.dm_decoration_template_module_item;
using Learun.Util;
using System;
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
    public class dm_decoration_template_module_itemBLL : dm_decoration_template_module_itemIBLL
    {
        private dm_decoration_template_module_itemService dm_decoration_template_module_itemService = new dm_decoration_template_module_itemService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_decoration_template_module_itemEntity> GetList(string queryJson)
        {
            try
            {
                return dm_decoration_template_module_itemService.GetList(queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_decoration_template_module_itemEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dm_decoration_template_module_itemService.GetPageList(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public dm_decoration_template_module_itemEntity GetEntity(int? keyValue)
        {
            try
            {
                return dm_decoration_template_module_itemService.GetEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void DeleteEntity(int? keyValue)
        {
            try
            {
                dm_decoration_template_module_itemService.DeleteEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveEntity(int? keyValue, dm_decoration_template_module_itemEntity entity)
        {
            try
            {
                dm_decoration_template_module_itemService.SaveEntity(keyValue, entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        #endregion

        #region 保存/获取模板数据
        /// <summary>
        /// 保存模板数据
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveDecorationTemplateData(int templateid, string jsondata)
        {
            try
            {
                dm_decoration_template_module_itemService.SaveDecorationTemplateData(templateid,jsondata);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        /// <summary>
        /// 获取模板数据
        /// </summary>
        /// <param name="templateid"></param>
        /// <returns></returns>
        public DecorationTemplateInfo GetDecorationTemplateData(int templateid)
        {
            try
            {
                return dm_decoration_template_module_itemService.GetDecorationTemplateData(templateid);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        #endregion
    }
}
