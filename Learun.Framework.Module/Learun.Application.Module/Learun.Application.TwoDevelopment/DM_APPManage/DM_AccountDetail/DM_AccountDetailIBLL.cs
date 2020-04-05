using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_AccountDetailIBLL
	{
		IEnumerable<dm_accountdetailEntity> GetList(string queryJson);

		IEnumerable<dm_accountdetailEntity> GetPageList(Pagination pagination, string queryJson);

		dm_accountdetailEntity GetEntity(int? keyValue);

		void DeleteEntity(int? keyValue);

		void SaveEntity(int? keyValue, dm_accountdetailEntity entity);
	}
}
