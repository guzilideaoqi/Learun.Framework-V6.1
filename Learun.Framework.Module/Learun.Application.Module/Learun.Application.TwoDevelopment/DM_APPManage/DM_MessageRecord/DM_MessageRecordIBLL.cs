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
	}
}
