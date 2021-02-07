using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-10-28 10:06
    /// 描 述：官推文案
    /// </summary>
    public class dm_friend_circleBLL : dm_friend_circleIBLL
    {
        private dm_friend_circleService dm_friend_circleService = new dm_friend_circleService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_friend_circleEntity> GetList(string queryJson)
        {
            try
            {
                return dm_friend_circleService.GetList(queryJson);
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
        public IEnumerable<dm_friend_circleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dm_friend_circleService.GetPageList(pagination, queryJson);
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
        public dm_friend_circleEntity GetEntity(int keyValue)
        {
            try
            {
                return dm_friend_circleService.GetEntity(keyValue);
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
        public void DeleteEntity(int keyValue)
        {
            try
            {
                dm_friend_circleService.DeleteEntity(keyValue);
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
        public void SaveEntity(int keyValue, dm_friend_circleEntity entity)
        {
            try
            {
                dm_friend_circleService.SaveEntity(keyValue, entity);
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

        #region API EXTEND METHOD
        /// <summary>
        /// 获取官推任务
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dm_friend_circleEntity> GetCircleByGovernment(Pagination pagination, string appid)
        {
            try
            {
                return dm_friend_circleService.GetCircleByGovernment(pagination, appid);
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
        /// 获取普通任务
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public IEnumerable<dm_friend_circleEntity> GetCircleByGeneral(Pagination pagination, string appid, int ischeck)
        {
            try
            {
                return dm_friend_circleService.GetCircleByGeneral(pagination, appid, ischeck);
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
        /// 获取单条哆米圈任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dm_friend_circleEntity GetSingleCircle(int id)
        {
            try
            {
                return dm_friend_circleService.GetSingleCircle(id);
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
        /// 获取我的哆米圈
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public IEnumerable<dm_friend_circleEntity> GetMyCircle(Pagination pagination, string User_ID)
        {
            try
            {
                return dm_friend_circleService.GetMyCircle(pagination, User_ID);
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
        /// 发布哆米圈文章
        /// </summary>
        /// <param name="AppID">站长id</param>
        /// <param name="Content">文章内容</param>
        /// <param name="Images">图片信息</param>
        /// <param name="User_ID">用户信息</param>
        public void PubCircle(string AppID, string Content, string Images, string User_ID)
        {
            try
            {
                dm_friend_circleService.PubCircle(AppID, Content, Images, User_ID);
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
