using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_OrderIBLL
	{
		IEnumerable<dm_orderEntity> GetList(string queryJson);

		IEnumerable<dm_orderEntity> GetPageList(Pagination pagination, string queryJson);

		dm_orderEntity GetEntity(string keyValue);

		void DeleteEntity(string keyValue);

		void SaveEntity(string keyValue, dm_orderEntity entity);
	}
}
