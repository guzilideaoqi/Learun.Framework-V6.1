using Learun.Util;
using System.Collections.Generic;
using System.Data;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_UserIBLL
	{
		IEnumerable<dm_userEntity> GetList(string queryJson);

		IEnumerable<dm_userEntity> GetPageList(Pagination pagination, string queryJson);
		DataTable GetPageListByDataTable(Pagination pagination, string queryJson);

		dm_userEntity GetEntity(int? keyValue);

		dm_userEntity GetEntityByPhone(string phone, string appid);

		dm_userEntity GetEntityByCache(int id);

		void DeleteEntity(int keyValue);

		void SaveEntity(int keyValue, dm_userEntity entity);

		dm_userEntity Login(dm_userEntity entity);

		dm_userEntity Register(dm_userEntity dm_UserEntity, string VerifiCode, string appid);

		string EncodeInviteCode(int? id);

		int? DecodeInviteCode(string InviteCode);

		dynamic SignIn(int userid);
		/// <summary>
		/// 更改账户余额
		/// </summary>
		/// <param name="user_id">用户id</param>
		/// <param name="updateprice">变更金额</param>
		/// <param name="updatetype">变更类型 0减少  1增加</param>
		/// <param name="remark">描述信息</param>
		void UpdateAccountPrice(int user_id, decimal updateprice, int updatetype, string remark);
		#region 获取推广图片
		List<string> GetShareImage(int user_id, string appid);
        #endregion

        #region 获取用户数据
        IEnumerable<dm_userEntity> GetParentUser(int user_id);
        IEnumerable<dm_userEntity> GetChildUser(int user_id);

        dm_userEntity GetUserByPartnersID(int partnersid);
		#endregion

		#region 获取平台统计数据
		DataTable GetStaticData1();
		DataTable GetStaticData2();
		DataTable GetStaticData3();
		DataTable GetStaticData4();
		#endregion
	}
}
