using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_UserIBLL
	{
		IEnumerable<dm_userEntity> GetList(string queryJson);

		IEnumerable<dm_userEntity> GetPageList(Pagination pagination, string queryJson);

		dm_userEntity GetEntity(int keyValue);

		dm_userEntity GetEntityByPhone(string phone, string appid);

		dm_userEntity GetEntityByCache(int id);

		void DeleteEntity(int keyValue);

		void SaveEntity(int keyValue, dm_userEntity entity);

		dm_userEntity Login(dm_userEntity entity);

		dm_userEntity Register(dm_userEntity dm_UserEntity, string VerifiCode, string appid);

		string EncodeInviteCode(int? id);

		int? DecodeInviteCode(string InviteCode);
	}
}
