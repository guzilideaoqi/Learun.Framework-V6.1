using Learun.Util;
using System.Collections.Generic;
using System.Data;

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

        /// <summary>
        /// 积分兑换记录
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        DataTable GetMyIntegralGoodRecord(int UserID, Pagination pagination);

        /// <summary>
        /// 积分兑换记录
        /// </summary>
        /// <param name="pagination">分页数据</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        DataTable GetIntegralGoodRecord(Pagination pagination, string queryJson);
    }
}
