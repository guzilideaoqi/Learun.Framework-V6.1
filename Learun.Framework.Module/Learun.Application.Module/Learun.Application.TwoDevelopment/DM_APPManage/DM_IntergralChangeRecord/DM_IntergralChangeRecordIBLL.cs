using Learun.Util;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public interface DM_IntergralChangeRecordIBLL
	{
		IEnumerable<dm_intergralchangerecordEntity> GetList(string queryJson);

		IEnumerable<dm_intergralchangerecordEntity> GetPageList(Pagination pagination, string queryJson);

		dm_intergralchangerecordEntity GetEntity(int keyValue);

		void DeleteEntity(int keyValue);

		void SaveEntity(int keyValue, dm_intergralchangerecordEntity entity);

		void ApplyChangeGood(dm_intergralchangerecordEntity dm_IntergralchangerecordEntity, dm_userEntity dm_UserEntity);
	}
}
