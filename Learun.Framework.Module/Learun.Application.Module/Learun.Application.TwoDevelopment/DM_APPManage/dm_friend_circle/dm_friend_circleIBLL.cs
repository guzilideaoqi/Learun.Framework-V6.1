using Learun.Util;
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
    public interface dm_friend_circleIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_friend_circleEntity> GetList( string queryJson );
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_friend_circleEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        dm_friend_circleEntity GetEntity(int keyValue);
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
        void SaveEntity(int keyValue, dm_friend_circleEntity entity);
        #endregion

        #region API EXTEND METHOD
        /// <summary>
        /// 获取官推任务
        /// </summary>
        /// <returns></returns>
        IEnumerable<dm_friend_circleEntity> GetCircleByGovernment(Pagination pagination, string appid);

        /// <summary>
        /// 获取普通任务
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        IEnumerable<dm_friend_circleEntity> GetCircleByGeneral(Pagination pagination, string appid);

        /// <summary>
        /// 获取单条哆米圈任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        dm_friend_circleEntity GetSingleCircle(int id);

        /// <summary>
        /// 获取我的哆米圈
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        IEnumerable<dm_friend_circleEntity> GetMyCircle(Pagination pagination, string User_ID);

        /// <summary>
        /// 发布哆米圈文章
        /// </summary>
        /// <param name="AppID">站长id</param>
        /// <param name="Content">文章内容</param>
        /// <param name="Images">图片信息</param>
        /// <param name="User_ID">用户信息</param>
        void PubCircle(string AppID, string Content, string Images, string User_ID);
        #endregion

    }
}
