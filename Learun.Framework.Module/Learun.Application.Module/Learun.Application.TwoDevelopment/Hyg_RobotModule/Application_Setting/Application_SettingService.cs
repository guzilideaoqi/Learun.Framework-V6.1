using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Learun.Application.TwoDevelopment.Hyg_RobotModule
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2019-11-19 20:34
    /// 描 述：应用商信息设置
    /// </summary>
    public class Application_SettingService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public Application_SettingService()
        {
            fieldSql=@"
                t.F_SettingId,
                t.F_ApplicationId,
                t.F_TB_AccountId,
                t.F_TB_AppKey,
                t.F_TB_Secret,
                t.F_TB_SessionKey,
                t.F_TB_AuthorEndTime,
                t.F_JD_AccountId,
                t.F_JD_AppKey,
                t.F_JD_Secret,
                t.F_JD_SessionKey,
                t.F_PDD_AccountId,
                t.F_PDD_ClientID,
                t.F_PDD_ClientSecret,
                t.F_ApplicationName,
                t.F_ApplicationLogo,
                t.F_CompanyName,
                t.F_Telephone,
                t.F_OICQ,
                t.F_WeChat,
                t.F_Description,
                t.F_CreateDate,
                t.F_CreateUserId,
                t.F_CreateUserName,
                t.F_ModifyDate,
                t.F_ModifyUserId,
                t.F_ModifyUserName
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<s_application_settingEntity> GetList( string queryJson )
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
                strSql.Append(" FROM s_application_setting t ");
                return this.BaseRepository("robot_DB").FindList<s_application_settingEntity>(strSql.ToString());
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
        public IEnumerable<s_application_settingEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM s_application_setting t ");
                return this.BaseRepository("robot_DB").FindList<s_application_settingEntity>(strSql.ToString(), pagination);
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
        public s_application_settingEntity GetEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("robot_DB").FindEntity<s_application_settingEntity>(keyValue);
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
        /// 通过应用商ID获取当前配置
        /// </summary>
        /// <returns></returns>
        public s_application_settingEntity GetEntityByApplicationId(string applicationId="") {
            try
            {
                UserInfo userInfo = LoginUserInfo.Get();
                applicationId = applicationId == "" ? userInfo.userId : applicationId;
                return this.BaseRepository("robot_DB").FindEntity<s_application_settingEntity>(t=>t.F_ApplicationId==applicationId);
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
        public void DeleteEntity(string keyValue)
        {
            try
            {
                this.BaseRepository("robot_DB").Delete<s_application_settingEntity>(t=>t.F_SettingId == keyValue);
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
        public void SaveEntity(string keyValue, s_application_settingEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("robot_DB").Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository("robot_DB").Insert(entity);
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
