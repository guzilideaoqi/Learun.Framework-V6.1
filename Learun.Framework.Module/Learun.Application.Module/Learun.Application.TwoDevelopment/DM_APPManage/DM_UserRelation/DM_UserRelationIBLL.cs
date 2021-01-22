using Learun.Util;
using System.Collections.Generic;
using System.Data;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_UserRelationIBLL
	{
		IEnumerable<dm_user_relationEntity> GetList(string queryJson);

		IEnumerable<dm_user_relationEntity> GetPageList(Pagination pagination, string queryJson);

		dm_user_relationEntity GetEntity(int? keyValue);

        dm_user_relationEntity GetEntityByUserID(int? id);

        void DeleteEntity(int? keyValue);

		void SaveEntity(int? keyValue, dm_user_relationEntity entity);

        #region 获取用户关系
        /// <summary>
        /// 向上获取用户关系
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        IEnumerable<dm_user_relationEntity> GetParentRelation(int user_id);
        /// <summary>
        /// 向下获取用户关系
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        IEnumerable<dm_user_relationEntity> GetChildRelation(int user_id);
        #endregion

        #region 获取直属粉丝详情
        DataTable GetMyChildDetail(int user_id, int PageNo, int PageSize);
        #endregion

        #region 获取二级粉丝详情
        DataTable GetMySonChildDetail(int user_id, int PageNo, int PageSize);
        #endregion

        #region 获取团队粉丝详情
        DataTable GetPartnersChildDetail(int? partners_id, int PageNo, int PageSize);
        #endregion

        #region 获取效果收益报表
        dm_user_relationEntity GetIncomeReport(int User_ID);
        #endregion

        #region 更改会员上级并重置统计信息
        void UpdateUserParent(int UserID, int ParentID);
        #endregion

        #region 重置用户统计信息
        void ResetUserStatistic(int UserID);
        #endregion
    }
}
