using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2021-03-03 10:56
    /// 描 述：装修模板
    /// </summary>
    public class dm_decoration_templateService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public dm_decoration_templateService()
        {
            fieldSql = @"
                t.id,
                t.template_name,
                t.template_remark,
                t.main_color,
                t.secondary_color,
                t.template_status,
                t.ischecktemplate,
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
        public IEnumerable<dm_decoration_templateEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_decoration_template t ");
                return this.BaseRepository("dm_data").FindList<dm_decoration_templateEntity>(strSql.ToString());
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
        public IEnumerable<dm_decoration_templateEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_decoration_template t ");
                return this.BaseRepository("dm_data").FindList<dm_decoration_templateEntity>(strSql.ToString(), pagination);
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
        public dm_decoration_templateEntity GetEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_decoration_templateEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_decoration_templateEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int? keyValue, dm_decoration_templateEntity entity)
        {
            IRepository db = null;
            try
            {
                if (keyValue > 0)
                {
                    entity.Modify(keyValue);
                    if (entity.template_status == 1)
                    {
                        db = this.BaseRepository("dm_data").BeginTrans();
                        db.ExecuteBySql("update dm_decoration_template set template_status=0");
                        db.Update(entity);
                        db.Commit();
                    }
                    else
                    {
                        this.BaseRepository("dm_data").Update(entity);
                    }
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

        #endregion

        #region 获取模板ID
        /// <summary>
        /// 是否为审核模式
        /// </summary>
        /// <param name="IsCheckMode"></param>
        /// <returns></returns>
        public int GetTemplateID(bool IsCheckMode = false)
        {
            try
            {
                dm_decoration_templateEntity dm_Decoration_TemplateEntity = null;
                if (IsCheckMode)
                {
                    dm_Decoration_TemplateEntity = this.BaseRepository("dm_data").FindEntity<dm_decoration_templateEntity>(t => t.ischecktemplate == 1);
                }
                else
                {
                    dm_Decoration_TemplateEntity = this.BaseRepository("dm_data").FindEntity<dm_decoration_templateEntity>(t => t.template_status == 1);
                }

                if (dm_Decoration_TemplateEntity.IsEmpty())
                    throw new Exception("未找到已启用或审核中的模板!");
                else
                {
                    return (int)dm_Decoration_TemplateEntity.id;
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
        #endregion

    }
}
