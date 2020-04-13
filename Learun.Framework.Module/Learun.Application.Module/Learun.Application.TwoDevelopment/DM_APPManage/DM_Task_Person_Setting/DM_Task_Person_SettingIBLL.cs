using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-10 13:55
    /// 描 述：进度任务设置
    /// </summary>
    public interface DM_Task_Person_SettingIBLL
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_task_person_settingEntity> GetList(string queryJson);
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        IEnumerable<dm_task_person_settingEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        dm_task_person_settingEntity GetEntity(int keyValue);
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
        void SaveEntity(int keyValue, dm_task_person_settingEntity entity);
        #endregion

        #region 获取个人任务进度
        /// <summary>
        /// 获取个人任务进度
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        IEnumerable<dm_task_person_settingEntity> GetPersonProcess(int user_id, string appid);
        #endregion

        #region 领取任务
        /// <summary>
        /// 领取任务
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="task_id"></param>
        void ReceiveAwards(int user_id, int? task_id);
        #endregion

        #region 获取升级合伙人任务
        /// <summary>
        /// 获取合伙人任务
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        IEnumerable<dm_task_person_settingEntity> GetPartnersProcess(int user_id, string appid);
        #endregion

        #region 申请成为合伙人
        /// <summary>
        /// 申请成为合伙人
        /// </summary>
        /// <param name="user_id"></param>
        void ApplyPartners(int user_id, string appid);
        #endregion
    }
}
