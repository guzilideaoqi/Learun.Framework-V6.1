using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_IntergralChangeGoodIBLL
	{
		IEnumerable<dm_intergralchangegoodEntity> GetList(string queryJson);

		IEnumerable<dm_intergralchangegoodEntity> GetPageList(Pagination pagination, string queryJson);

		IEnumerable<dm_intergralchangegoodEntity> GetPageListByCache(Pagination pagination, string appid);

		dm_intergralchangegoodEntity GetEntity(int keyValue);

		void DeleteEntity(int keyValue);

		void SaveEntity(int keyValue, dm_intergralchangegoodEntity entity);
	}
}
