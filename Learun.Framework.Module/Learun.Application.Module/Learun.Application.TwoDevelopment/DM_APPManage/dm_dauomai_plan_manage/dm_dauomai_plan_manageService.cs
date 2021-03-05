using Dapper;
using Hyg.Common.DuoMaiTools;
using Hyg.Common.DuoMaiTools.DuoMaiModel;
using Hyg.Common.DuoMaiTools.DuoMaiRequest;
using Hyg.Common.DuoMaiTools.DuoMaiResponse;
using Learun.Application.TwoDevelopment.Common;
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
    /// 日 期：2021-03-03 10:50
    /// 描 述：多麦计划
    /// </summary>
    public class dm_dauomai_plan_manageService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public dm_dauomai_plan_manageService()
        {
            fieldSql = @"
                t.id,
                t.ads_id,
                t.ads_name,
                t.store_name,
                t.channel,
                t.rddays,
                t.commission,
                t.category_area,
                t.category,
                t.apply_mode,
                t.stime,
                t.etime,
                t.url,
                t.ads_logo,
                t.status,
                t.ads_apply_status,
                t.use_status,
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
        public IEnumerable<dm_dauomai_plan_manageEntity> GetList(string queryJson)
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
                strSql.Append(" FROM dm_dauomai_plan_manage t ");
                return this.BaseRepository("dm_data").FindList<dm_dauomai_plan_manageEntity>(strSql.ToString());
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
        public IEnumerable<dm_dauomai_plan_manageEntity> GetPageList(Learun.Util.Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_dauomai_plan_manage t ");
                return this.BaseRepository("dm_data").FindList<dm_dauomai_plan_manageEntity>(strSql.ToString(), pagination);
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
        public dm_dauomai_plan_manageEntity GetEntity(int? keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_dauomai_plan_manageEntity>(keyValue);
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
                this.BaseRepository("dm_data").Delete<dm_dauomai_plan_manageEntity>(t => t.id == keyValue);
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
        public void SaveEntity(int? keyValue, dm_dauomai_plan_manageEntity entity)
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

        #endregion

        #region 同步推广计划
        public void SyncPlanList(Query_CPS_Stores_PlansRequest query_CPS_Stores_PlansRequest)
        {
            try
            {
                DuoMai_ApiManage duoMai_ApiManage = new DuoMai_ApiManage(CommonConfig.duomai_appkey, CommonConfig.duomai_appsecret);

                query_CPS_Stores_PlansRequest.is_apply = 0;
                query_CPS_Stores_PlansRequest.page_size = 200;
                List<CPS_Stores_PlansEntity> cPS_Stores_PlansEntities = duoMai_ApiManage.Query_CPS_Stores_Plans(query_CPS_Stores_PlansRequest);

                List<dm_dauomai_plan_manageEntity> dm_dauomai_plan_manageList = new List<dm_dauomai_plan_manageEntity>();
                foreach (var item in cPS_Stores_PlansEntities)
                {
                    dm_dauomai_plan_manageEntity dm_Dauomai_Plan_ManageEntity = new dm_dauomai_plan_manageEntity
                    {
                        ads_apply_status = item.ads_apply_status,
                        ads_id = item.ads_id,
                        ads_logo = item.ads_logo,
                        ads_name = item.ads_name,
                        apply_mode = item.apply_mode,
                        category = item.category,
                        category_area = item.category_area,
                        channel = item.channel,
                        commission = item.commission,
                        etime = item.etime,
                        stime = item.stime,
                        rddays = item.rddays,
                        status = item.status,
                        store_name = item.store_name,
                        url = item.url
                    };
                    dm_Dauomai_Plan_ManageEntity.Create();
                    dm_dauomai_plan_manageList.Add(dm_Dauomai_Plan_ManageEntity);
                }
                if (dm_dauomai_plan_manageList.Count > 0)
                    this.BaseRepository("dm_data").Insert(dm_dauomai_plan_manageList);

                if (cPS_Stores_PlansEntities.Count >= 200)
                {
                    query_CPS_Stores_PlansRequest.page++;
                    SyncPlanList(query_CPS_Stores_PlansRequest);
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

        #region 激活功能
        public void StartPlan(int plan_id)
        {
            IRepository db = null;
            try
            {
                dm_dauomai_plan_manageEntity dm_Dauomai_Plan_ManageEntity = GetEntity(plan_id);
                if (dm_Dauomai_Plan_ManageEntity.IsEmpty())
                    throw new Exception("推广计划不存在!");

                dm_Dauomai_Plan_ManageEntity.use_status = 1;
                dm_Dauomai_Plan_ManageEntity.Modify(dm_Dauomai_Plan_ManageEntity.id);

                dm_decoration_fun_manageEntity dm_Decoration_Fun_ManageEntity = this.BaseRepository("dm_data").FindEntity<dm_decoration_fun_manageEntity>(t => t.fun_param == plan_id.ToString());

                db = this.BaseRepository("dm_data").BeginTrans();

                ///功能中不存在该计划
                if (dm_Decoration_Fun_ManageEntity.IsEmpty())
                {
                    dm_Decoration_Fun_ManageEntity = new dm_decoration_fun_manageEntity();
                    dm_Decoration_Fun_ManageEntity.fun_name = dm_Dauomai_Plan_ManageEntity.ads_name;
                    dm_Decoration_Fun_ManageEntity.fun_param = dm_Dauomai_Plan_ManageEntity.id.ToString();
                    dm_Decoration_Fun_ManageEntity.fun_remark = dm_Dauomai_Plan_ManageEntity.commission;
                    dm_Decoration_Fun_ManageEntity.fun_type = 2;
                    dm_Decoration_Fun_ManageEntity.Create();
                    db.Insert(dm_Decoration_Fun_ManageEntity);
                }
                else
                {
                    dm_Decoration_Fun_ManageEntity.fun_name = dm_Dauomai_Plan_ManageEntity.ads_name;
                    dm_Decoration_Fun_ManageEntity.fun_param = dm_Dauomai_Plan_ManageEntity.id.ToString();
                    dm_Decoration_Fun_ManageEntity.fun_remark = dm_Dauomai_Plan_ManageEntity.commission;
                    dm_Decoration_Fun_ManageEntity.Modify(dm_Decoration_Fun_ManageEntity.id);
                    db.Update(dm_Decoration_Fun_ManageEntity);
                }
                db.Update(dm_Dauomai_Plan_ManageEntity);
                db.Commit();
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
        #endregion

        #region 停止使用
        public void StopPlan(int plan_id)
        {
            IRepository db = null;
            try
            {
                dm_dauomai_plan_manageEntity dm_Dauomai_Plan_ManageEntity = GetEntity(plan_id);
                if (dm_Dauomai_Plan_ManageEntity.IsEmpty())
                    throw new Exception("推广计划不存在!");

                dm_Dauomai_Plan_ManageEntity.use_status = 0;
                dm_Dauomai_Plan_ManageEntity.Modify(dm_Dauomai_Plan_ManageEntity.id);

                db = this.BaseRepository("dm_data").BeginTrans();
                db.Delete<dm_decoration_fun_manageEntity>(t => t.fun_param == plan_id.ToString());
                db.Delete<dm_decoration_template_module_itemEntity>(t => t.module_fun_id == plan_id);
                db.Update(dm_Dauomai_Plan_ManageEntity);
                db.Commit();
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
        #endregion

        #region 推广转链
        public CPS_Convert_LinkResponse ConvertLink(int plan_id, int user_id)
        {
            try
            {
                dm_dauomai_plan_manageEntity dm_Dauomai_Plan_ManageEntity = GetEntity(plan_id);
                if (dm_Dauomai_Plan_ManageEntity.IsEmpty())
                    throw new Exception("推广计划不存在!");
                DuoMai_ApiManage duoMai_ApiManage = new DuoMai_ApiManage(CommonConfig.duomai_appkey, CommonConfig.duomai_appsecret);
                CPS_Convert_LinkRequest cPS_Convert_LinkRequest = new CPS_Convert_LinkRequest();
                cPS_Convert_LinkRequest.ads_id = dm_Dauomai_Plan_ManageEntity.ads_id;
                cPS_Convert_LinkRequest.ext = new CPS_Convert_Link_ext
                {
                    euid = user_id.ToString()
                };
                cPS_Convert_LinkRequest.site_id = CommonConfig.duomai_pid;
                cPS_Convert_LinkRequest.url = dm_Dauomai_Plan_ManageEntity.url;
                return duoMai_ApiManage.Get_CPS_Convert_Link(cPS_Convert_LinkRequest);
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
