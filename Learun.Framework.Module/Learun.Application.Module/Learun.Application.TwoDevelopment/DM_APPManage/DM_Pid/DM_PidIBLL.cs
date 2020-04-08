using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-07 07:57
    /// 描 述：pid管理
    /// </summary>
    public interface DM_PidIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_pidEntity> GetList( string queryJson );
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_pidEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        dm_pidEntity GetEntity(int keyValue);
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
        void SaveEntity(int keyValue, dm_pidEntity entity);
        #endregion

        #region PID自动分配
        /// <summary>
        /// 自动分配京东pid
        /// </summary>
        /// <param name="dm_UserEntity"></param>
        /// <returns></returns>
        dm_userEntity AutoAssignJDPID(dm_userEntity dm_UserEntity);

        /// <summary>
        /// 自动分配拼多多pid
        /// </summary>
        /// <param name="dm_UserEntity"></param>
        /// <returns></returns>
        dm_userEntity AutoAssignPDDPID(dm_userEntity dm_UserEntity);
        #endregion
    }
}
