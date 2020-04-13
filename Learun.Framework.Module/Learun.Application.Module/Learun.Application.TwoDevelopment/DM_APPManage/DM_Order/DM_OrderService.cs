using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_OrderService : RepositoryFactory
    {
        private DM_BaseSettingService dm_BaseSettingService = new DM_BaseSettingService();

        private string fieldSql;

        private List<dm_accountdetailEntity> dm_AccountdetailEntities = new List<dm_accountdetailEntity>();

        private List<dm_userEntity> calculateComissionEntities = new List<dm_userEntity>();

        public DM_OrderService()
        {
            fieldSql = "    t.id,    t.appid,    t.order_sn,    t.sub_order_sn,    t.origin_id,    t.type_big,    t.type,    t.order_type,    t.title,    t.order_status,    t.rebate_status,    t.image,    t.product_num,    t.product_price,    t.payment_price,    t.estimated_effect,    t.income_ratio,    t.estimated_income,    t.commission_rate,    t.commission_amount,    t.subsidy_ratio,    t.subsidy_amount,    t.subsidy_type,    t.order_createtime,    t.order_settlement_at,    t.order_pay_time,    t.order_group_success_time,    t.createtime,    t.updatetime,    t.shopname,    t.category_name,    t.media_name,    t.media_id,    t.pid_name,    t.pid,    t.relation_id,    t.special_id,    t.protection_status,    t.insert_type,    t.order_type_new,    t.order_create_date,    t.order_create_month,    t.order_receive_date,    t.order_receive_month,    t.userid";
        }

        public IEnumerable<dm_orderEntity> GetList(string queryJson)
        {
            try
            {
                //ExcuteSubCommission("e2b3ec3a-310b-4ab8-aa81-b563ac8f3006");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_order t ");
                return BaseRepository("dm_data").FindList<dm_orderEntity>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        public IEnumerable<dm_orderEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                //ExcuteSubCommission("e2b3ec3a-310b-4ab8-aa81-b563ac8f3006");
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_order t ");
                return BaseRepository("dm_data").FindList<dm_orderEntity>(strSql.ToString(), pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        public dm_orderEntity GetEntity(string keyValue)
        {
            try
            {
                return BaseRepository("dm_data").FindEntity<dm_orderEntity>(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        public void DeleteEntity(string keyValue)
        {
            try
            {
                BaseRepository("dm_data").Delete((dm_orderEntity t) => t.id == keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        public void SaveEntity(string keyValue, dm_orderEntity entity)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    BaseRepository("dm_data").Update(entity);
                }
                else
                {
                    entity.Create();
                    BaseRepository("dm_data").Insert(entity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        #region 订单结算
        public void ExcuteSubCommission(string appid)
        {
            IRepository db = null;
            try
            {
                int upMonth = int.Parse(DateTime.Now.AddMonths(-1).ToString("yyyyMM"));
                IEnumerable<dm_orderEntity> orderList = BaseRepository("dm_data").FindList((dm_orderEntity t) => t.order_receive_month == (int?)upMonth && t.rebate_status == (int?)0 && t.order_type_new == (int?)2 && t.appid == appid);
                IEnumerable<UserRelationEntity> userRelationEntities = BaseRepository("dm_data").FindList<UserRelationEntity>("select ur.user_id,ur.parent_id,ur.partners_id,u.partnersstatus,u.userlevel,u.accountprice from dm_user_relation ur LEFT JOIN dm_user u on ur.user_id=u.id");
                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingService.GetEntityByCache(appid);
                decimal pay_comission = default(decimal);
                decimal pay_one_comission2 = default(decimal);
                decimal pay_two_comission2 = default(decimal);
                decimal one_partners_comission2 = default(decimal);
                decimal two_partners_comission2 = default(decimal);
                dm_userEntity calculateComissionEntity2 = null;
                dm_userEntity calculateComissionEntity_one2 = null;
                dm_userEntity calculateComissionEntity_two2 = null;
                dm_userEntity calculateComissionEntity_one_partners2 = null;
                dm_userEntity calculateComissionEntity_two_partners2 = null;
                UserRelationEntity pay_user = null;
                UserRelationEntity one_user = null;
                UserRelationEntity two_user2 = null;
                UserRelationEntity one_partners_user = null;
                UserRelationEntity two_partners_user2 = null;
                List<dm_orderEntity> update_orderList = new List<dm_orderEntity>();
                foreach (dm_orderEntity item in orderList)
                {
                    pay_user = userRelationEntities.Where((UserRelationEntity t) => t.user_id == item.userid).FirstOrDefault();
                    if (!pay_user.IsEmpty())
                    {
                        if (pay_user.userlevel == 0)
                        {
                            pay_comission = ConvertComission(item.estimated_effect * (decimal)dm_BasesettingEntity.shopping_pay_junior);
                        }
                        else if (pay_user.userlevel == 1)
                        {
                            pay_comission = ConvertComission(item.estimated_effect * (decimal)dm_BasesettingEntity.shopping_pay_middle);
                        }
                        else if (pay_user.userlevel == 2)
                        {
                            pay_comission = ConvertComission(item.estimated_effect * (decimal)dm_BasesettingEntity.shopping_pay_senior);
                        }
                        if (pay_comission > 0m)
                        {
                            calculateComissionEntity2 = CalculateComission(pay_user.user_id, pay_comission, pay_user.accountprice);
                            dm_AccountdetailEntities.Add(GeneralAccountDetail(pay_user.user_id, 1, "订单返佣", "您的订单" + item.order_status.ToString() + "佣金已到账,请查收!", pay_comission, calculateComissionEntity2.accountprice));
                        }
                        if (pay_user.parent_id != -1 && pay_comission > 0m)
                        {
                            one_user = userRelationEntities.Where((UserRelationEntity t) => t.user_id == pay_user.parent_id).FirstOrDefault();
                            if (!one_user.IsEmpty())
                            {
                                pay_one_comission2 = ConvertComission(pay_comission * (decimal)dm_BasesettingEntity.shopping_one);
                                if (pay_one_comission2 > 0m)
                                {
                                    calculateComissionEntity_one2 = CalculateComission(one_user.user_id, pay_one_comission2, one_user.accountprice);
                                    dm_AccountdetailEntities.Add(GeneralAccountDetail(one_user.user_id, 2, "粉丝订单返佣", "您的下级订单提成已到账,请查收!", pay_one_comission2, calculateComissionEntity_one2.accountprice));
                                }
                                if (one_user.parent_id != -1)
                                {
                                    two_user2 = userRelationEntities.Where((UserRelationEntity t) => t.user_id == one_user.parent_id).FirstOrDefault();
                                    if (!two_user2.IsEmpty())
                                    {
                                        pay_two_comission2 = ConvertComission(pay_comission * (decimal)dm_BasesettingEntity.shopping_two);
                                        if (pay_two_comission2 > 0m)
                                        {
                                            calculateComissionEntity_two2 = CalculateComission(two_user2.user_id, pay_two_comission2, two_user2.accountprice);
                                            dm_AccountdetailEntities.Add(GeneralAccountDetail(two_user2.user_id, 3, "粉丝订单返佣", "您的下下级订单提成已到账,请查收!", pay_two_comission2, calculateComissionEntity_two2.accountprice));
                                        }
                                    }
                                }
                            }
                        }
                        if (pay_user.partners_id > 0 && pay_comission > 0m)
                        {
                            one_partners_user = userRelationEntities.Where((UserRelationEntity t) => t.partners_id == pay_user.partners_id && t.partnersstatus == 1).FirstOrDefault();
                            if (!one_partners_user.IsEmpty())
                            {
                                one_partners_comission2 = ConvertComission(pay_comission * (decimal)dm_BasesettingEntity.shopping_one_partners);
                                if (one_partners_comission2 > 0m)
                                {
                                    calculateComissionEntity_one_partners2 = CalculateComission(one_partners_user.user_id, one_partners_comission2, one_partners_user.accountprice);
                                    dm_AccountdetailEntities.Add(GeneralAccountDetail(one_partners_user.user_id, 4, "团队订单返佣", "您的团队有订单已结算,提成已到账,请查收!", one_partners_comission2, calculateComissionEntity_one_partners2.accountprice));
                                }
                                if (one_partners_user.parent_id != -1)
                                {
                                    two_partners_user2 = userRelationEntities.Where((UserRelationEntity t) => t.user_id == one_partners_user.parent_id).FirstOrDefault();
                                    if (!two_partners_user2.IsEmpty())
                                    {
                                        two_partners_comission2 = ConvertComission(pay_comission * (decimal)dm_BasesettingEntity.shopping_two_partners);
                                        if (two_partners_comission2 > 0m)
                                        {
                                            calculateComissionEntity_two_partners2 = CalculateComission(two_partners_user2.user_id, two_partners_comission2, two_partners_user2.accountprice);
                                            dm_AccountdetailEntities.Add(GeneralAccountDetail(two_partners_user2.user_id, 5, "下级团队订单返佣", "您的下级团队有订单已结算,提成已到账,请查收!", two_partners_comission2, calculateComissionEntity_two_partners2.accountprice));
                                        }
                                    }
                                }
                            }
                        }
                        update_orderList.Add(new dm_orderEntity
                        {
                            id = item.id,
                            rebate_status = 1,
                            order_type_new = 4
                        });
                    }
                }
                if (dm_AccountdetailEntities.Count > 0)
                {
                    //清除当前结算用户的缓存信息
                    foreach (var item in calculateComissionEntities)
                    {
                        item.Modify(item.id);
                    }
                    db = BaseRepository("dm_data").BeginTrans();
                    db.Insert(dm_AccountdetailEntities);
                    db.Update(calculateComissionEntities);
                    db.Update(update_orderList);
                    db.Commit();
                }
            }
            catch (Exception ex)
            {
                db?.Rollback();
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }

        private dm_userEntity CalculateComission(int? user_id, decimal? commission, decimal? currentaccount)
        {
            dm_userEntity calculateComissionEntity = calculateComissionEntities.Where((dm_userEntity t) => t.id == user_id).FirstOrDefault();
            if (calculateComissionEntity.IsEmpty())
            {
                calculateComissionEntity = new dm_userEntity
                {
                    id = user_id,
                    accountprice = currentaccount + commission
                };
                calculateComissionEntities.Add(calculateComissionEntity);
            }
            else
            {
                dm_userEntity dm_userEntity = calculateComissionEntity;
                dm_userEntity.accountprice += commission;
            }
            return calculateComissionEntity;
        }

        private decimal ConvertComission(decimal comissionamount)
        {
            return Math.Round(comissionamount / 100m, 2);
        }

        private dm_accountdetailEntity GeneralAccountDetail(int user_id, int type, string title, string remark, decimal billdetailCommission, decimal? currentaccountprice)
        {
            return new dm_accountdetailEntity
            {
                createtime = DateTime.Now,
                remark = remark,
                stepvalue = billdetailCommission,
                currentvalue = currentaccountprice,
                title = title,
                type = type,
                user_id = user_id
            };
        }
        #endregion

        #region 绑定订单
        /// <summary>
        /// 绑定订单
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="OrderSn"></param>
        public void BindOrder(int user_id, string appid, string OrderSn)
        {
            try
            {
                dm_orderEntity dm_OrderEntity = BaseRepository("dm_data").FindEntity<dm_orderEntity>(t => t.sub_order_sn == OrderSn && t.appid == appid);
                if (dm_OrderEntity.IsEmpty())
                    throw new Exception("该订单未同步到，请稍后重试!");
                else if (!dm_OrderEntity.userid.IsEmpty())
                {
                    if (dm_OrderEntity.userid == user_id)
                        throw new Exception("该订单已在您的账号下，无需重新绑定!");
                    else
                        throw new Exception("该订单已被其他人绑定,请勿将订单号泄露给他人!");
                }
                else
                {
                    dm_OrderEntity.userid = user_id;
                    BaseRepository("dm_data").Update(dm_OrderEntity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }
        #endregion

        #region 获取我的订单
        /// <summary>
        /// 获取我的订单
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="plaformType">大平台类型:1=淘宝和天猫,3=京东,4=拼多多</param>
        /// <param name="status">本站订单归类状态: 0=未处理,1=付款,2=收货未结,3=失效,4=结算至余额</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public IEnumerable<dm_orderEntity> GetMyOrder(int user_id, int plaformType, int status, Pagination pagination)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_order t where userid=" + user_id);
                strSql.Append(" and type_big=" + plaformType);
                strSql.Append(" and order_type_new=" + status);
                IEnumerable<dm_orderEntity> dm_OrderEntities = BaseRepository("dm_data").FindList<dm_orderEntity>(strSql.ToString(), pagination);

                return dm_OrderEntities;
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex);
            }
        }
        #endregion

        #region 获取我的订单总数量
        public int GetMyOrderCount(int user_id) {
            return int.Parse(BaseRepository("dm_data").FindObject("select count(id) from dm_order where userid=" + user_id).ToString());
        }
        #endregion
    }
}
