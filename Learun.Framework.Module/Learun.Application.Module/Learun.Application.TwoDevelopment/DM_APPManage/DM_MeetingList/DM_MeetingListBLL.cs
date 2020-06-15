using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-06-13 15:08
    /// 描 述：直播房间列表
    /// </summary>
    public class DM_MeetingListBLL : DM_MeetingListIBLL
    {
        private DM_MeetingListService dM_MeetingListService = new DM_MeetingListService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_meetinglistEntity> GetList(string queryJson)
        {
            try
            {
                return dM_MeetingListService.GetList(queryJson);
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
        public IEnumerable<dm_meetinglistEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_MeetingListService.GetPageList(pagination, queryJson);
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
        public dm_meetinglistEntity GetEntity(int? keyValue)
        {
            try
            {
                return dM_MeetingListService.GetEntity(keyValue);
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
                dM_MeetingListService.DeleteEntity(keyValue);
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
        public void SaveEntity(int? keyValue, dm_meetinglistEntity entity)
        {
            try
            {
                dM_MeetingListService.SaveEntity(keyValue, entity);
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

        #region 直播间操作
        /// <summary>
        /// 获取直播间列表
        /// </summary>
        /// <param name="keyWord">关键词</param>
        /// <param name="User_ID">用户ID</param>
        /// <returns></returns>
        public IEnumerable<dm_meetinglistEntity> GetMeetingList(string keyWord, int User_ID)
        {
            try
            {
                return dM_MeetingListService.GetMeetingList(keyWord, User_ID);
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
        /// 创建房间
        /// </summary>
        /// <param name="dm_Meetinglist"></param>
        public void CreateMetting(List<dm_meetinglistEntity> dm_Meetinglist)
        {
            try
            {
                dM_MeetingListService.CreateMetting(dm_Meetinglist);
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
        /// 生成会议推广图片
        /// </summary>
        /// <param name="dm_BasesettingEntity"></param>
        /// <param name="Join_Url"></param>
        /// <returns></returns>
        public string GeneralMeetingImage(dm_basesettingEntity dm_BasesettingEntity, string Join_Url)
        {
            try
            {
                return dM_MeetingListService.GeneralMeetingImage(dm_BasesettingEntity, Join_Url);
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
