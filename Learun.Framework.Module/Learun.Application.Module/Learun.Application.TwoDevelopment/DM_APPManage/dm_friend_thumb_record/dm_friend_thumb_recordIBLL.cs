using Learun.Util;
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
    public interface dm_friend_thumb_recordIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_friend_thumb_recordEntity> GetList(string queryJson);
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_friend_thumb_recordEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        dm_friend_thumb_recordEntity GetEntity(int keyValue);
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void DeleteEntity(int keyValue);
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        void SaveEntity(int keyValue, dm_friend_thumb_recordEntity entity);
        #endregion

        #region 点赞扩展
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="dm_Friend_Thumb_RecordEntity"></param>
        void ClickPraise(dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity);

        /// <summary>
        /// 取消点赞
        /// </summary>
        /// <param name="dm_Friend_Thumb_RecordEntity"></param>
        void CanclePraise(dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity);

        /// <summary>
        /// 获取点赞记录
        /// </summary>
        /// <param name="friend_ids"></param>
        /// <returns></returns>
        DataTable GetPraiseRecord(List<int> friend_ids);

        /// <summary>
        /// 获取我的点赞记录
        /// </summary>
        /// <param name="friend_ids"></param>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        IEnumerable<dm_friend_thumb_recordEntity> GetPraiseRecord(List<int> friend_ids, int User_ID);
        #endregion

        #region 分享扩展
        void ClickShare(dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity);
        #endregion
    }
}
