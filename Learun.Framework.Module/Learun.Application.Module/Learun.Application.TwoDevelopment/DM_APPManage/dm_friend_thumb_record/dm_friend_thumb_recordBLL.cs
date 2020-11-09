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
    /// 日 期：2020-11-09 10:21
    /// 描 述：点赞记录
    /// </summary>
    public class dm_friend_thumb_recordBLL : dm_friend_thumb_recordIBLL
    {
        private dm_friend_thumb_recordService dm_friend_thumb_recordService = new dm_friend_thumb_recordService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_friend_thumb_recordEntity> GetList( string queryJson )
        {
            try
            {
                return dm_friend_thumb_recordService.GetList(queryJson);
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
        public IEnumerable<dm_friend_thumb_recordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dm_friend_thumb_recordService.GetPageList(pagination, queryJson);
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
        public dm_friend_thumb_recordEntity GetEntity(int keyValue)
        {
            try
            {
                return dm_friend_thumb_recordService.GetEntity(keyValue);
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
                dm_friend_thumb_recordService.DeleteEntity(keyValue);
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
        public void SaveEntity(int keyValue, dm_friend_thumb_recordEntity entity)
        {
            try
            {
                dm_friend_thumb_recordService.SaveEntity(keyValue, entity);
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

        #region 点赞扩展
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="dm_Friend_Thumb_RecordEntity"></param>
        public void ClickPraise(dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity) {
            try
            {
                dm_friend_thumb_recordService.ClickPraise(dm_Friend_Thumb_RecordEntity);
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
        /// 取消点赞
        /// </summary>
        /// <param name="dm_Friend_Thumb_RecordEntity"></param>
        public void CanclePraise(dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity) {
            try
            {
                dm_friend_thumb_recordService.CanclePraise(dm_Friend_Thumb_RecordEntity);
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
        /// 获取点赞记录
        /// </summary>
        /// <param name="friend_ids"></param>
        /// <returns></returns>
        public DataTable GetPraiseRecord(List<int> friend_ids) {
            try
            {
               return dm_friend_thumb_recordService.GetPraiseRecord(friend_ids);
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
        /// 获取我的点赞记录
        /// </summary>
        /// <param name="friend_ids"></param>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public IEnumerable<dm_friend_thumb_recordEntity> GetPraiseRecord(List<int> friend_ids, int User_ID) {
            try
            {
                return dm_friend_thumb_recordService.GetPraiseRecord(friend_ids, User_ID);
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
