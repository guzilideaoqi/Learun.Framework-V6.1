using Learun.Util;
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
    public interface DM_MeetingListIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_meetinglistEntity> GetList( string queryJson );
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_meetinglistEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        dm_meetinglistEntity GetEntity(int? keyValue);
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
        void SaveEntity(int? keyValue, dm_meetinglistEntity entity);
        #endregion

        #region 直播间操作
        /// <summary>
        /// 获取直播间列表
        /// </summary>
        /// <param name="keyWord">关键词</param>
        /// <param name="User_ID">用户ID</param>
        /// <returns></returns>
        IEnumerable<dm_meetinglistEntity> GetMeetingList(Pagination pagination, string keyWord, int User_ID);

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="dm_Meetinglist"></param>
        void CreateMetting(List<dm_meetinglistEntity> dm_Meetinglist);

        /// <summary>
        /// 生成推广图片
        /// </summary>
        /// <param name="dm_BasesettingEntity"></param>
        /// <param name="Join_Url"></param>
        /// <returns></returns>
        string GeneralMeetingImage(dm_basesettingEntity dm_BasesettingEntity, string Join_Url);
        #endregion
    }
}
