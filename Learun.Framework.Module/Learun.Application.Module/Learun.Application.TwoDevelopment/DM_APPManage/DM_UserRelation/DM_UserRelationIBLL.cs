using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_UserRelationIBLL
	{
		IEnumerable<dm_user_relationEntity> GetList(string queryJson);

		IEnumerable<dm_user_relationEntity> GetPageList(Pagination pagination, string queryJson);

		dm_user_relationEntity GetEntity(int? keyValue);

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
    }
}
