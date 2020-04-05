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
    /// 日 期：2019-11-19 21:15
    /// 描 述：代理商管理
    /// </summary>
    public class AgentManageService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public AgentManageService()
        {
            fieldSql = @"
                t.F_AgentId,
                t.F_Account,
                t.F_Password,
                t.F_NickName,
                t.F_AllowStartTime,
                t.F_AllowEndTime,
                t.F_PDD_ComissionRate,
                t.F_JD_ComissionRate,
                t.F_TB_ComissionRate,
                t.F_TB_RelationId,
                t.F_TB_PID,
                t.F_JD_PID,
                t.F_PDD_PID,
                t.F_ApplicationId,
                t.F_EnabledMark,
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
        public IEnumerable<s_data_agentEntity> GetList(string queryJson)
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
                strSql.Append(" FROM s_data_agent t ");
                return this.BaseRepository("robot_DB").FindList<s_data_agentEntity>(strSql.ToString());
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
        public IEnumerable<s_data_agentEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                UserInfo userInfo = LoginUserInfo.Get();
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM s_data_agent t ");
                if (userInfo.roleIds == "660c49b5-b3b9-4675-a651-e7c7490a6c29")
                {//发单机器人客户角色
                    string userId = userInfo.userId;
                    strSql.Append(" where t.F_APPLICATIONID='");
                    strSql.Append(userId);
                    strSql.Append("' ");
                }
                return this.BaseRepository("robot_DB").FindList<s_data_agentEntity>(strSql.ToString(), pagination);
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
        public s_data_agentEntity GetEntity(string keyValue)
        {
            try
            {
                return this.BaseRepository("robot_DB").FindEntity<s_data_agentEntity>(keyValue);
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
        /// 客户端登录
        /// </summary>
        /// <param name="Account">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <returns></returns>
        public s_data_agentEntity Login(string Account,string PassWord) {
            try
            {
                PassWord = Md5Helper.Hash(PassWord);
                return this.BaseRepository("robot_DB").FindEntity<s_data_agentEntity>(t=>t.F_Account==Account&&t.F_Password== PassWord);
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
                this.BaseRepository("robot_DB").Delete<s_data_agentEntity>(t => t.F_AgentId == keyValue);
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
        public void SaveEntity(string keyValue, s_data_agentEntity entity)
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
