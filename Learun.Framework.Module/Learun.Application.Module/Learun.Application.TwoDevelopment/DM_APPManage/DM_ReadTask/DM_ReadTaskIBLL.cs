using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_ReadTaskIBLL
	{
		IEnumerable<dm_readtaskEntity> GetList(string queryJson);

		IEnumerable<dm_readtaskEntity> GetPageList(Pagination pagination, string queryJson);

		dm_readtaskEntity GetEntity(int keyValue);

		IEnumerable<dm_readtaskEntity> GetPageListByCache(Pagination pagination, string appid);

		void DeleteEntity(int keyValue);

		void SaveEntity(int keyValue, dm_readtaskEntity entity);

		void AddClickReadEarnTask(int id, int count, string appid);
	}
}
