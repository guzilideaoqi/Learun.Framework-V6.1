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

        /// <summary>
        /// 订单绑定
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="appid">平台id</param>
        /// <param name="OrderSn">订单编号</param>
        void BindOrder(int user_id, string appid, string OrderSn);
        /// <summary>
        /// 获取我的订单
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="plaformType">大平台类型:1=淘宝和天猫,3=京东,4=拼多多</param>
        /// <param name="status">本站订单归类状态: 0=未处理,1=付款,2=收货未结,3=失效,4=结算至余额</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        IEnumerable<dm_orderEntity> GetMyOrder(int user_id, int plaformType, int status, Pagination pagination);

        /// <summary>
        /// 执行返利(上个月结算的订单)
        /// </summary>
        /// <param name="appid"></param>
        void ExcuteSubCommission(string appid);

        /// <summary>
        /// 手动同步订单
        /// </summary>
        /// <param name="plaform">平台</param>
        /// <param name="timetype">时间类型</param>
        /// <param name="status">状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        int SyncOrder(int plaform, int timetype, int status, string startTime, string endTime);
    }
}
