using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_BaseSettingIBLL
	{
		IEnumerable<dm_basesettingEntity> GetList(string queryJson);

		IEnumerable<dm_basesettingEntity> GetPageList(Pagination pagination, string queryJson);

		dm_basesettingEntity GetEntity(string keyValue);

		dm_basesettingEntity GetEntityByCache(string appid);

		void DeleteEntity(string keyValue);

		void SaveEntity(string keyValue, dm_basesettingEntity entity);
	}
}
