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

        #region ��ȡ�û���ϵ
        /// <summary>
        /// ���ϻ�ȡ�û���ϵ
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        IEnumerable<dm_user_relationEntity> GetParentRelation(int user_id);
        /// <summary>
        /// ���»�ȡ�û���ϵ
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        IEnumerable<dm_user_relationEntity> GetChildRelation(int user_id);
        #endregion

        #region ��ȡֱ����˿����
        DataTable GetMyChildDetail(int user_id, int PageNo, int PageSize);
        #endregion

        #region ��ȡ������˿����
        DataTable GetMySonChildDetail(int user_id, int PageNo, int PageSize);
        #endregion

        #region ��ȡ�Ŷӷ�˿����
        DataTable GetPartnersChildDetail(int? partners_id, int PageNo, int PageSize);
        #endregion

        #region ��ȡЧ�����汨��
        dm_user_relationEntity GetIncomeReport(int User_ID);
        #endregion

        #region ���Ļ�Ա�ϼ�������ͳ����Ϣ
        void UpdateUserParent(int UserID, int ParentID);
        #endregion

        #region �����û�ͳ����Ϣ
        void ResetUserStatistic(int UserID);
        #endregion
    }
}
