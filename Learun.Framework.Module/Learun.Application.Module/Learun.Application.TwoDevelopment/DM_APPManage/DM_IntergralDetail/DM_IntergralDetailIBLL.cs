using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_IntergralDetailIBLL
	{
		IEnumerable<dm_intergraldetailEntity> GetList(string queryJson);

		IEnumerable<dm_intergraldetailEntity> GetPageList(Pagination pagination, string queryJson);

		dm_intergraldetailEntity GetEntity(int? keyValue);

		void DeleteEntity(int? keyValue);

		void SaveEntity(int? keyValue, dm_intergraldetailEntity entity);
	}
}
