using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_OrderBLL : DM_OrderIBLL
    {
        private DM_OrderService dM_OrderService = new DM_OrderService();

        public IEnumerable<dm_orderEntity> GetList(string queryJson)
        {
            try
            {
                return dM_OrderService.GetList(queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public IEnumerable<dm_orderEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_OrderService.GetPageList(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_orderEntity GetEntity(string keyValue)
        {
            try
            {
                return dM_OrderService.GetEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public void DeleteEntity(string keyValue)
        {
            try
            {
                dM_OrderService.DeleteEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public void SaveEntity(string keyValue, dm_orderEntity entity)
        {
            try
            {
                dM_OrderService.SaveEntity(keyValue, entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
        /// 订单绑定
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="appid">平台id</param>
        /// <param name="OrderSn">订单编号</param>
        public void BindOrder(int user_id, string appid, string OrderSn)
        {
            try
            {
                dM_OrderService.BindOrder(user_id, appid, OrderSn);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        /// <summary>
        /// 获取我的订单
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="plaformType">大平台类型:1=淘宝和天猫,3=京东,4=拼多多</param>
        /// <param name="status">本站订单归类状态: 0=未处理,1=付款,2=收货未结,3=失效,4=结算至余额</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public IEnumerable<dm_orderEntity> GetMyOrder(int user_id, int plaformType, int status, Pagination pagination)
        {
            try
            {
                return dM_OrderService.GetMyOrder(user_id, plaformType, status, pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
        /// 执行返利
        /// </summary>
        /// <param name="appid">应用id</param>
        public void ExcuteSubCommission(string appid) {
            try
            {
                dM_OrderService.ExcuteSubCommission(appid);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
        /// 手动同步订单
        /// </summary>
        /// <param name="plaform">平台</param>
        /// <param name="timetype">时间类型</param>
        /// <param name="status">订单状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        public void SyncOrder(int plaform, int timetype, int status, string startTime, string endTime) {
            try
            {
                dM_OrderService.SyncOrder(plaform, timetype, status, startTime, endTime);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
