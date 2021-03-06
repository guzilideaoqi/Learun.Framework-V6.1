using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using Learun.Application.TwoDevelopment.DM_APPManage.dm_decoration_template_module_item;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:58
    /// 描 述：模块对应功能
    /// </summary>
    public class dm_decoration_template_module_itemService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public dm_decoration_template_module_itemService()
        {
            fieldSql = @"
                t.id,
                t.module_item_name,
                t.module_item_image,
                t.module_fun_id,
                t.module_sort,
                t.createtime,
                t.updatetime
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_decoration_template_module_itemEntity> GetList(string queryJson)
        {
            try
            {
                //参考写法
                //var queryParam = queryJson.ToJObject();
                // 虚拟参数
                //var dp = new DynamicParameters(new { });
                //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_decoration_template_module_item t ");
                return this.BaseRepository("dm_data").FindList<dm_decoration_template_module_itemEntity>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
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
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_decoration_template_module_item t ");
                return this.BaseRepository("dm_data").FindList<dm_decoration_template_module_itemEntity>(strSql.ToString(), pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
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
                return this.BaseRepository("dm_data").FindEntity<dm_decoration_template_module_itemEntity>(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
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
                this.BaseRepository("dm_data").Delete<dm_decoration_template_module_itemEntity>(t => t.id == keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
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
                if (keyValue > 0)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("dm_data").Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository("dm_data").Insert(entity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 保存装修模板数据
        /// </summary>
        /// <param name="jsondata"></param>
        public void SaveDecorationTemplateData(int templateid, string jsondata)
        {
            IRepository db = null;
            try
            {
                List<ModuleItem> moduleItemList = jsondata.ToObject<List<ModuleItem>>();
                List<dm_decoration_template_moduleEntity> dm_decoration_template_moduleList = new List<dm_decoration_template_moduleEntity>();
                List<dm_decoration_template_module_itemEntity> dm_decoration_template_module_itemList = new List<dm_decoration_template_module_itemEntity>();
                foreach (ModuleItem item in moduleItemList)
                {
                    dm_decoration_template_moduleEntity dm_Decoration_Template_ModuleEntity = new dm_decoration_template_moduleEntity();
                    dm_Decoration_Template_ModuleEntity.template_id = item.template_id;
                    dm_Decoration_Template_ModuleEntity.module_id = item.module_id;
                    dm_Decoration_Template_ModuleEntity.Create();
                    dm_decoration_template_moduleList.Add(dm_Decoration_Template_ModuleEntity);

                    foreach (dm_decoration_template_module_itemEntity entity in item.module_item_list)
                    {
                        entity.template_module_id = dm_Decoration_Template_ModuleEntity.id;
                        entity.template_id = templateid;
                        entity.Create();
                        dm_decoration_template_module_itemList.Add(entity);
                    }
                }
                if (templateid > 0)
                {
                    db = this.BaseRepository("dm_data").BeginTrans();
                    db.Delete<dm_decoration_template_moduleEntity>(t => t.template_id == templateid);
                    db.Delete<dm_decoration_template_module_itemEntity>(t => t.template_id == templateid);
                    if (dm_decoration_template_moduleList.Count > 0)
                    {
                        db.Insert(dm_decoration_template_moduleList);
                        db.Insert(dm_decoration_template_module_itemList);
                    }
                    db.Commit();
                }

            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Rollback();

                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        public DecorationTemplateInfo GetDecorationTemplateData(int templateid)
        {
            try
            {
                dm_decoration_templateEntity dm_Decoration_TemplateEntity = this.BaseRepository("dm_data").FindEntity<dm_decoration_templateEntity>(templateid);
                if (dm_Decoration_TemplateEntity.IsEmpty())
                    throw new Exception("模板不存在,请添加后再获取!");

                List<ModuleInfoEntity> moduleInfoEntities = new List<ModuleInfoEntity>();

                ///获取模板中包含的模块
                IEnumerable<dm_decoration_template_moduleEntity> dm_Decoration_Template_ModuleEntities = this.BaseRepository("dm_data").FindList<dm_decoration_template_moduleEntity>(t => t.template_id == templateid);
                ///获取所有的模块ID去查询模块的信息
                IEnumerable<int?> module_id_list = dm_Decoration_Template_ModuleEntities.Select(t => t.module_id);
                ///模块列表
                IEnumerable<dm_decoration_moduleEntity> dm_Decoration_ModuleEntities = this.BaseRepository("dm_data").FindList<dm_decoration_moduleEntity>(t => module_id_list.Contains(t.id));
                ///模块子项列表
                IEnumerable<dm_decoration_template_module_itemEntity> dm_Decoration_Template_Module_ItemEntities = this.BaseRepository("dm_data").FindList<dm_decoration_template_module_itemEntity>(t => t.template_id == templateid);

                ///获取所有功能项名称
                IEnumerable<dm_decoration_fun_manageEntity> dm_decoration_fun_manageEntitys = this.BaseRepository("dm_data").FindList<dm_decoration_fun_manageEntity>();

                foreach (dm_decoration_template_moduleEntity item in dm_Decoration_Template_ModuleEntities)
                {
                    dm_decoration_moduleEntity dm_Decoration_ModuleEntity = dm_Decoration_ModuleEntities.Where(t => t.id == item.module_id).FirstOrDefault();
                    if (!dm_Decoration_ModuleEntity.IsEmpty())
                    {
                        ModuleInfoEntity moduleInfoEntity = new ModuleInfoEntity
                        {
                            ModuleName = dm_Decoration_ModuleEntity.module_name,
                            ModuleType = dm_Decoration_ModuleEntity.module_type,
                            ModuleID = dm_Decoration_ModuleEntity.id
                        };

                        List<ModuleItemInfoEntity> moduleItemInfoEntities = new List<ModuleItemInfoEntity>();
                        IEnumerable<dm_decoration_template_module_itemEntity> dm_Decoration_Template_Module_Items = dm_Decoration_Template_Module_ItemEntities.Where(t => t.template_module_id == item.id);
                        foreach (dm_decoration_template_module_itemEntity dm_Decoration_Template_Module_ItemEntity in dm_Decoration_Template_Module_Items)
                        {
                            dm_decoration_fun_manageEntity dm_Decoration_Fun_ManageEntity = dm_decoration_fun_manageEntitys.Where(t => t.id == dm_Decoration_Template_Module_ItemEntity.module_fun_id).FirstOrDefault();
                            if (!dm_Decoration_Fun_ManageEntity.IsEmpty())
                            {
                                ModuleItemInfoEntity moduleItemInfoEntity = new ModuleItemInfoEntity
                                {
                                    id = dm_Decoration_Template_Module_ItemEntity.id,
                                    module_fun_id = dm_Decoration_Template_Module_ItemEntity.module_fun_id,
                                    module_fun_name = dm_Decoration_Fun_ManageEntity.fun_name,
                                    module_fun_type = dm_Decoration_Fun_ManageEntity.fun_type,
                                    module_fun_param = dm_Decoration_Fun_ManageEntity.fun_param,
                                    module_item_image = dm_Decoration_Template_Module_ItemEntity.module_item_image,
                                    module_item_name = dm_Decoration_Template_Module_ItemEntity.module_item_name,
                                    module_item_type = dm_Decoration_ModuleEntity.module_type,
                                    module_sort = dm_Decoration_Template_Module_ItemEntity.module_sort
                                };
                                moduleItemInfoEntities.Add(moduleItemInfoEntity);
                            }
                        }
                        moduleInfoEntity.ModuleItemInfoList = moduleItemInfoEntities;
                        moduleInfoEntities.Add(moduleInfoEntity);
                    }
                }

                DecorationTemplateInfo decorationTemplateInfo = new DecorationTemplateInfo
                {
                    MainColor = dm_Decoration_TemplateEntity.main_color,
                    SecondaryColor = dm_Decoration_TemplateEntity.secondary_color,
                    ModuleInfoList = moduleInfoEntities
                };

                return decorationTemplateInfo;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion
    }

    public class ModuleItem
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public int template_id { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        public int module_id { get; set; }

        public List<dm_decoration_template_module_itemEntity> module_item_list { get; set; }
    }
}
