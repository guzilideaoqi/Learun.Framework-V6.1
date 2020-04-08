using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_MessageRecordIBLL
	{
		IEnumerable<dm_messagerecordEntity> GetList(string queryJson);

		IEnumerable<dm_messagerecordEntity> GetPageList(Pagination pagination, string queryJson);

		dm_messagerecordEntity GetEntity(int? keyValue);

		void DeleteEntity(int? keyValue);

		void SaveEntity(int? keyValue, dm_messagerecordEntity entity);

        #region 未读消息转已读
        /// <summary>
        /// 未读消息转已读(通过用户ID)
        /// </summary>
        /// <param name="user_id"></param>
        void MessageToReadByUserID(int user_id);

        /// <summary>
        /// 未读消息转已读(通过消息ID)
        /// </summary>
        /// <param name="id"></param>
        void MessageToReadByID(int id);
        #endregion
    }
}
