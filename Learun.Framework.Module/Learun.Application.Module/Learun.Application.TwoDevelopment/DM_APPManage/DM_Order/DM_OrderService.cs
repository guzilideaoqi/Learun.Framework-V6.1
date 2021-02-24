using Common;
using HYG.CommonHelper.ShoppingAPI;
using JDModel;
using Learun.DataBase.Repository;
using Learun.Util;
using PDDModel;
using ShoppingAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TBModel;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_OrderService : RepositoryFactory
    {
        private DM_BaseSettingService dm_BaseSettingService = new DM_BaseSettingService();

        private string fieldSql;

        private List<dm_accountdetailEntity> dm_AccountdetailEntities = new List<dm_accountdetailEntity>();

        private List<dm_userEntity> calculateComissionEntities = new List<dm_userEntity>();

        dm_basesetting_tipService dm_Basesetting_TipService = new dm_basesetting_tipService();

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
                var queryParam = queryJson.ToJObject();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_order t ");

                if (!queryParam["AppID"].IsEmpty())
                {
                    strSql.Append(" where t.appid='" + queryParam["AppID"].ToString() + "'");
                }
                else
                {
                    UserInfo userInfo = LoginUserInfo.Get();
                    strSql.Append(" where t.appid='" + userInfo.companyId + "'");
                }

                if (!queryParam["txt_OrderSN"].IsEmpty())
                {
                    strSql.Append(" and (t.order_sn like '%" + queryParam["txt_OrderSN"].ToString() + "%' or t.sub_order_sn like '%" + queryParam["txt_OrderSN"].ToString() + "%')");
                }

                if (!queryParam["txt_UserID"].IsEmpty())
                {
                    strSql.Append(" and t.userid='" + queryParam["txt_UserID"].ToString() + "'");
                }

                if (!queryParam["txt_status"].IsEmpty())
                {
                    strSql.Append(" and t.order_type_new='" + queryParam["txt_status"].ToString() + "'");
                }

                if (!queryParam["txt_type_big"].IsEmpty())
                {
                    strSql.Append(" and t.type_big='" + queryParam["txt_type_big"].ToString() + "'");
                }

                if (!queryParam["StartTime"].IsEmpty())
                {
                    strSql.Append(" and t.order_pay_time>'" + queryParam["StartTime"].ToString() + "'");
                }

                if (!queryParam["EndTime"].IsEmpty())
                {
                    strSql.Append(" and t.order_pay_time<'" + queryParam["EndTime"].ToString() + "'");
                }

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
                dm_basesetting_tipEntity dm_Basesetting_TipEntity = dm_Basesetting_TipService.GetEntityByAppID(appid);
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
                        /*2020-05-13  订单实际返利的佣金计算放到触发器中进行*/
                        if (pay_user.userlevel == 0)
                        {
                            //pay_comission = ConvertComission(Convert.ToDecimal(item.estimated_effect) * (decimal)dm_BasesettingEntity.shopping_pay_junior);
                            pay_comission = ConvertComission(Convert.ToDecimal(item.commission_amount));
                        }
                        else if (pay_user.userlevel == 1)
                        {
                            //pay_comission = ConvertComission(Convert.ToDecimal(item.estimated_effect) * (decimal)dm_BasesettingEntity.shopping_pay_middle);
                            pay_comission = ConvertComission(Convert.ToDecimal(item.commission_amount));
                        }
                        else if (pay_user.userlevel == 2)
                        {
                            //pay_comission = ConvertComission(Convert.ToDecimal(item.estimated_effect) * (decimal)dm_BasesettingEntity.shopping_pay_senior);
                            pay_comission = ConvertComission(Convert.ToDecimal(item.commission_amount));
                        }
                        if (pay_comission > 0m)
                        {
                            calculateComissionEntity2 = CalculateComission(pay_user.user_id, pay_comission, pay_user.accountprice);
                            dm_AccountdetailEntities.Add(GeneralAccountDetail(pay_user.user_id, 1, dm_Basesetting_TipEntity.shop_pay_tip, "您的订单" + item.order_status.ToString() + "佣金已到账,请查收!", pay_comission, calculateComissionEntity2.accountprice));
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
                                    dm_AccountdetailEntities.Add(GeneralAccountDetail(one_user.user_id, 2, dm_Basesetting_TipEntity.shop_one_tip, "您的" + dm_Basesetting_TipEntity.shop_one_tip + "有订单已结算,提成已到账,请查收!", pay_one_comission2, calculateComissionEntity_one2.accountprice));
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
                                            dm_AccountdetailEntities.Add(GeneralAccountDetail(two_user2.user_id, 3, dm_Basesetting_TipEntity.shop_two_tip, "您的" + dm_Basesetting_TipEntity.shop_two_tip + "有订单已结算,提成已到账,请查收!", pay_two_comission2, calculateComissionEntity_two2.accountprice));
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
                                    dm_AccountdetailEntities.Add(GeneralAccountDetail(one_partners_user.user_id, 4, dm_Basesetting_TipEntity.shop_parners_one_tip, "您的" + dm_Basesetting_TipEntity.shop_parners_one_tip + "有订单已结算,提成已到账,请查收!", one_partners_comission2, calculateComissionEntity_one_partners2.accountprice));
                                }
                                if (one_partners_user.parent_id != -1)
                                {
                                    two_partners_user2 = userRelationEntities.Where((UserRelationEntity t) => t.user_id == one_partners_user.parent_id).FirstOrDefault();
                                    if (!two_partners_user2.IsEmpty())
                                    {
                                        if (two_partners_user2.partnersstatus == 2)
                                        {//必须上级为合伙人
                                            two_partners_comission2 = ConvertComission(pay_comission * (decimal)dm_BasesettingEntity.shopping_two_partners);
                                            if (two_partners_comission2 > 0m)
                                            {
                                                calculateComissionEntity_two_partners2 = CalculateComission(two_partners_user2.user_id, two_partners_comission2, two_partners_user2.accountprice);
                                                dm_AccountdetailEntities.Add(GeneralAccountDetail(two_partners_user2.user_id, 5, dm_Basesetting_TipEntity.shop_parners_two_tip, "您的" + dm_Basesetting_TipEntity.shop_parners_two_tip + "有订单已结算,提成已到账,请查收!", two_partners_comission2, calculateComissionEntity_two_partners2.accountprice));
                                            }
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
                user_id = user_id,
                profitLoss = CommonHelper.GetProfitLoss(type)
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
                if (status != 0)
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
        public int GetMyOrderCount(int user_id)
        {
            string querySql = string.Format("select count(id) from dm_order where (userid='{0}' or userid in (select ur.user_id from dm_user_relation ur where ur.parent_id='{0}')) and order_type_new<>3", user_id);
            return int.Parse(BaseRepository("dm_data").FindObject(querySql).ToString());
        }
        #endregion

        #region 手动同步订单
        TBApi tbApi = null;
        JDApi jDApi = null;
        PDDApi pDDApi = null;
        string tb_tool_appkey = "25552805", tb_tool_appsecret = "7341a330d97862f21447f34c0fc326c9", appid = "e2b3ec3a-310b-4ab8-aa81-b563ac8f3006";
        List<dm_orderEntity> commonOrderList = new List<dm_orderEntity>();
        public int SyncOrder(int plaform, int timetype, int status, string startTime, string endTime)
        {
            int effectCount = 0;
            try
            {
                UserInfo userInfo = LoginUserInfo.Get();
                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingService.GetEntityByCache(userInfo.companyId);

                appid = userInfo.companyId;

                if (plaform == 1)
                {
                    endTime = Extensions.ToDateTimeString(DateTime.Parse(startTime).AddMinutes(20));
                    effectCount = synd_tb_order(timetype == 1 ? 2 : 3, 1, 20, "", startTime, endTime, dm_BasesettingEntity);
                }
                else if (plaform == 3)
                {
                    endTime = Extensions.ToDateTimeString(DateTime.Parse(startTime).AddHours(1));
                    effectCount = synd_jd_order(1, 20, timetype, startTime, endTime, dm_BasesettingEntity);
                }
                else if (plaform == 4)
                {
                    effectCount = synd_pdd_order(1, 20, TimeSpanConvert.ConvertDateTimeToInt(DateTime.Parse(startTime)), TimeSpanConvert.ConvertDateTimeToInt(DateTime.Parse(startTime).AddDays(1)), dm_BasesettingEntity);
                }
                else
                {
                    throw new Exception("请选择平台!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return effectCount;
        }

        #region 同步淘宝订单
        int synd_tb_order(int queryType, int pageNo, int pageSize, string postion_index, string startTime, string endTime, dm_basesettingEntity baseSettingEntity)
        {
            int effectCount = 0;
            try
            {
                tbApi = new TBApi(tb_tool_appkey, tb_tool_appsecret, baseSettingEntity.tb_sessionkey);
                if (tbApi != null)
                {
                    #region 淘宝订单处理
                    OrderData orderData = tbApi.GetOrder(queryType.ToString(), postion_index, pageSize.ToString(), "", startTime, endTime, "1", pageNo.ToString(), "2");

                    if (!orderData.IsEmpty() && !orderData.results.IsEmpty() && orderData.results.publisher_order_dto != null)
                    {
                        foreach (tb_order item in orderData.results.publisher_order_dto)
                        {
                            dm_orderEntity commonOrderEntity = new dm_orderEntity();
                            commonOrderEntity.order_sn = item.trade_parent_id;//订单id
                            commonOrderEntity.sub_order_sn = item.trade_id;//子订单id
                            commonOrderEntity.origin_id = item.item_id;//商品id
                            commonOrderEntity.id = Md5Helper.Hash(commonOrderEntity.order_sn + commonOrderEntity.sub_order_sn + commonOrderEntity.origin_id);//id通过
                            commonOrderEntity.type_big = 1;
                            commonOrderEntity.order_type = getordertype(item.order_type);
                            commonOrderEntity.type = commonOrderEntity.order_type;
                            commonOrderEntity.title = item.item_title;
                            commonOrderEntity.order_status = item.tk_status;
                            commonOrderEntity.order_type_new = gettbstatus(commonOrderEntity.order_status);
                            commonOrderEntity.rebate_status = 0;
                            commonOrderEntity.image = item.item_img.StartsWith("//") ? ("http:" + item.item_img) : item.item_img;
                            commonOrderEntity.product_num = item.item_num;
                            commonOrderEntity.product_price = item.item_price;
                            commonOrderEntity.payment_price = item.alipay_total_price;
                            commonOrderEntity.estimated_effect = item.pub_share_fee;//结算预估收入(包含补贴金额)
                            commonOrderEntity.estimated_income = item.pub_share_pre_fee;//付款的预估佣金金额
                            commonOrderEntity.commission_rate = item.total_commission_rate;//佣金比例
                            commonOrderEntity.income_ratio = item.pub_share_rate;
                            commonOrderEntity.commission_amount = 0;//保存返利成功的金额
                            commonOrderEntity.subsidy_ratio = item.subsidy_rate;
                            commonOrderEntity.subsidy_amount = item.subsidy_fee;
                            commonOrderEntity.subsidy_type = item.subsidy_type;
                            commonOrderEntity.order_createtime = Extensions.ToDateOrNull(item.tk_create_time);
                            commonOrderEntity.order_settlement_at = Extensions.ToDateOrNull(item.tk_earning_time);
                            commonOrderEntity.order_pay_time = Extensions.ToDateOrNull(item.tb_paid_time);
                            commonOrderEntity.createtime = DateTime.Now;
                            commonOrderEntity.updatetime = DateTime.Now;
                            commonOrderEntity.shopname = item.seller_shop_title;
                            commonOrderEntity.category_name = item.item_category_name;
                            commonOrderEntity.media_id = item.site_id;
                            commonOrderEntity.media_name = item.site_name;
                            commonOrderEntity.pid = item.adzone_id;
                            commonOrderEntity.pid_name = item.adzone_name;
                            commonOrderEntity.relation_id = item.relation_id.ToString();
                            commonOrderEntity.special_id = Extensions.ToString(item.special_id);
                            commonOrderEntity.protection_status = item.refund_tag;
                            commonOrderEntity.insert_type = 1;
                            commonOrderEntity.order_create_date = ConvertDate(commonOrderEntity.order_createtime);
                            commonOrderEntity.order_create_month = ConvertMonth(commonOrderEntity.order_createtime);
                            commonOrderEntity.order_receive_date = ConvertDate(commonOrderEntity.order_settlement_at);
                            commonOrderEntity.order_receive_month = ConvertMonth(commonOrderEntity.order_settlement_at);

                            commonOrderList.Add(commonOrderEntity);
                        }
                    }

                    if (orderData.has_next)
                    {
                        tbApi.GetOrder(queryType.ToString(), orderData.position_index, pageSize.ToString(), "", startTime, endTime, "1", pageNo.ToString(), "2");
                    }
                    else
                    {
                        effectCount = InsertCommonOrder(commonOrderList);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception("订单同步异常" + ex.Message + ex.StackTrace, ex);
            }

            return effectCount;
        }
        #endregion

        #region 同步京东订单
        int synd_jd_order(int pageNo, int pageSize, int type, string startTime, string endTime, dm_basesettingEntity baseSettingEntity)
        {
            int effectCount = 0;
            try
            {
                jDApi = new JDApi(baseSettingEntity.jd_appkey, baseSettingEntity.jd_appsecret, baseSettingEntity.jd_sessionkey);
                if (jDApi != null)
                {
                    #region 京东订单处理
                    jd_union_open_order_row_query_response jdRowOrder = jDApi.GetRowOrder(1, 20, type, startTime, endTime);
                    if (jdRowOrder != null)
                    {
                        if (jdRowOrder.data != null)
                        {
                            foreach (JDRowOrder item in jdRowOrder.data)
                            {
                                dm_orderEntity commonOrderEntity = new dm_orderEntity();
                                commonOrderEntity.order_sn = item.parentId.ToString();//订单id
                                commonOrderEntity.sub_order_sn = item.orderId.ToString();//子订单id
                                commonOrderEntity.origin_id = item.skuId.ToString();//商品id
                                commonOrderEntity.id = item.id;//id通过
                                commonOrderEntity.type_big = 3;
                                commonOrderEntity.order_type = 0;
                                commonOrderEntity.type = commonOrderEntity.order_type;
                                commonOrderEntity.title = item.skuName;
                                commonOrderEntity.order_status = item.validCode;
                                commonOrderEntity.order_type_new = getjdstatus(commonOrderEntity.order_status);
                                commonOrderEntity.rebate_status = 0;
                                JDGoodDetailEntity jDGoodDetailEntity = jDApi.GetGoodDetail(commonOrderEntity.origin_id);
                                commonOrderEntity.image = jDGoodDetailEntity == null ? "" : jDGoodDetailEntity.imgUrl;
                                commonOrderEntity.product_num = item.skuNum;
                                commonOrderEntity.product_price = item.price;
                                commonOrderEntity.payment_price = item.estimateCosPrice;
                                commonOrderEntity.estimated_effect = item.actualFee;//结算预估收入(包含补贴金额)
                                commonOrderEntity.estimated_income = item.estimateFee;//付款的预估佣金金额
                                commonOrderEntity.commission_rate = item.commissionRate;//佣金比例
                                commonOrderEntity.income_ratio = item.finalRate;
                                commonOrderEntity.commission_amount = 0;//保存返利成功的金额
                                commonOrderEntity.subsidy_ratio = item.subSideRate.ToString();
                                commonOrderEntity.subsidy_amount = item.subsidyRate;
                                commonOrderEntity.subsidy_type = "0";
                                commonOrderEntity.order_createtime = Extensions.ToDateOrNull(item.orderTime);
                                commonOrderEntity.order_settlement_at = Extensions.ToDateOrNull(item.finishTime);
                                commonOrderEntity.order_pay_time = Extensions.ToDateOrNull(item.orderTime);
                                commonOrderEntity.createtime = DateTime.Now;
                                commonOrderEntity.updatetime = DateTime.Now;
                                commonOrderEntity.shopname = item.popId.ToString();
                                commonOrderEntity.category_name = item.cid1.ToString();
                                commonOrderEntity.media_id = item.pid;
                                commonOrderEntity.media_name = "jd";
                                commonOrderEntity.pid = item.positionId.ToString();
                                commonOrderEntity.pid_name = "";
                                commonOrderEntity.relation_id = "";
                                commonOrderEntity.special_id = "";
                                commonOrderEntity.protection_status = 0;
                                commonOrderEntity.insert_type = 1;
                                commonOrderEntity.order_create_date = ConvertDate(commonOrderEntity.order_createtime);
                                commonOrderEntity.order_create_month = ConvertMonth(commonOrderEntity.order_createtime);
                                commonOrderEntity.order_receive_date = ConvertDate(commonOrderEntity.order_settlement_at);
                                commonOrderEntity.order_receive_month = ConvertMonth(commonOrderEntity.order_settlement_at);

                                commonOrderList.Add(commonOrderEntity);
                            }

                            if (jdRowOrder.hasMore)
                            {
                                synd_jd_order(pageNo + 1, pageSize, type, startTime, endTime, baseSettingEntity);
                            }
                            else
                            {
                                effectCount = InsertCommonOrder(commonOrderList);
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception("订单同步异常", ex);
            }
            return effectCount;
        }
        #endregion

        #region 同步拼多多订单
        int synd_pdd_order(int pageNo, int pageSize, string startTime, string endTime, dm_basesettingEntity baseSettingEntity)
        {
            int effectCount = 0;
            try
            {
                pDDApi = new PDDApi(baseSettingEntity.pdd_clientid, baseSettingEntity.pdd_clientsecret, "");

                if (pDDApi != null)
                {
                    #region 拼多多订单处理
                    List<pdd_order> orderList = pDDApi.GetOrderList(startTime, endTime, pageNo, pageSize, false);
                    if (orderList != null)
                    {
                        foreach (pdd_order item in orderList)
                        {
                            dm_orderEntity commonOrderEntity = new dm_orderEntity();
                            commonOrderEntity.order_sn = item.order_sn;//订单id
                            commonOrderEntity.sub_order_sn = item.order_sn;//子订单id
                            commonOrderEntity.origin_id = item.goods_id;//商品id
                            commonOrderEntity.id = Md5Helper.Hash(commonOrderEntity.order_sn + commonOrderEntity.sub_order_sn + commonOrderEntity.origin_id);//id通过
                            commonOrderEntity.type_big = 4;
                            commonOrderEntity.order_type = 0;
                            commonOrderEntity.type = commonOrderEntity.order_type;
                            commonOrderEntity.title = item.goods_name;
                            commonOrderEntity.order_status = item.order_status;
                            commonOrderEntity.order_type_new = getpddstatus(commonOrderEntity.order_status);
                            commonOrderEntity.rebate_status = 0;
                            commonOrderEntity.image = item.goods_thumbnail_url;
                            commonOrderEntity.product_num = item.goods_quantity;
                            commonOrderEntity.product_price = Math.Round(decimal.Parse(item.goods_price) / 100, 2);
                            commonOrderEntity.payment_price = Math.Round(decimal.Parse(item.order_amount) / 100, 2);
                            commonOrderEntity.estimated_effect = Math.Round(decimal.Parse(item.promotion_amount) / 100, 2);//结算预估收入(包含补贴金额)
                            commonOrderEntity.estimated_income = commonOrderEntity.estimated_effect;//付款的预估佣金金额
                            commonOrderEntity.commission_rate = Math.Round((decimal.Parse(item.promotion_rate) / 10), 2);//佣金比例
                            commonOrderEntity.income_ratio = commonOrderEntity.commission_rate;
                            commonOrderEntity.commission_amount = 0;//保存返利成功的金额
                            commonOrderEntity.subsidy_ratio = "0";
                            commonOrderEntity.subsidy_amount = 0;
                            commonOrderEntity.subsidy_type = "0";
                            commonOrderEntity.order_createtime = TimeSpanConvert.TimeSpanToDateTime(long.Parse(item.order_create_time));
                            commonOrderEntity.order_settlement_at = TimeSpanConvert.TimeSpanToDateTime(long.Parse(item.order_settle_time));
                            commonOrderEntity.order_pay_time = TimeSpanConvert.TimeSpanToDateTime(long.Parse(item.order_pay_time));
                            commonOrderEntity.order_group_success_time = TimeSpanConvert.TimeSpanToDateTime(long.Parse(item.order_group_success_time));
                            commonOrderEntity.createtime = DateTime.Now;
                            commonOrderEntity.updatetime = DateTime.Now;
                            commonOrderEntity.shopname = "";
                            commonOrderEntity.category_name = "";
                            commonOrderEntity.media_id = "";
                            commonOrderEntity.media_name = "";
                            commonOrderEntity.pid = item.p_id;
                            commonOrderEntity.pid_name = "pdd";
                            commonOrderEntity.relation_id = "";
                            commonOrderEntity.special_id = "";
                            commonOrderEntity.protection_status = 0;
                            commonOrderEntity.insert_type = 1;
                            commonOrderEntity.order_create_date = ConvertDate(commonOrderEntity.order_createtime);
                            commonOrderEntity.order_create_month = ConvertMonth(commonOrderEntity.order_createtime);
                            commonOrderEntity.order_receive_date = ConvertDate(commonOrderEntity.order_settlement_at);
                            commonOrderEntity.order_receive_month = ConvertMonth(commonOrderEntity.order_settlement_at);

                            commonOrderList.Add(commonOrderEntity);
                        }
                        if (orderList.Count >= 20)
                        {
                            synd_pdd_order(pageNo + 1, pageSize, startTime, endTime, baseSettingEntity);
                        }
                        else
                        {
                            effectCount = InsertCommonOrder(commonOrderList);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception("订单同步异常", ex);
            }

            return effectCount;
        }
        #endregion

        #region 订单统一化管理
        public int InsertCommonOrder(List<dm_orderEntity> commonOrderList)
        {
            int effetcCount = commonOrderList.Count;

            if (commonOrderList.Count <= 0)
                effetcCount = 0;
            try
            {
                IEnumerable<string> hashids = commonOrderList.Select(t => t.id);

                IEnumerable<dm_orderEntity> orderList = BaseRepository("dm_data").IQueryable<dm_orderEntity>(t => hashids.Contains(t.id));

                foreach (dm_orderEntity commonOrderEntity in commonOrderList)
                {
                    dm_orderEntity dm_OrderEntity = orderList.Where(t => t.id == commonOrderEntity.id).FirstOrDefault();

                    commonOrderEntity.appid = appid;
                    if (dm_OrderEntity.IsEmpty())
                    {
                        BaseRepository("dm_data").Insert(commonOrderEntity);
                    }
                    else
                    {
                        if (dm_OrderEntity.order_type_new != commonOrderEntity.order_type_new)
                        {
                            BaseRepository("dm_data").Update(commonOrderEntity);
                        }
                    }
                }
                commonOrderList.Clear();

            }
            catch (Exception ex)
            {
                throw new Exception("订单插入异常" + ex.Message + ex.StackTrace, ex);
            }

            return effetcCount;
        }
        #endregion

        int getordertype(string ordertype)
        {
            int order_type = 0;
            switch (ordertype)
            {
                case "天猫":
                    order_type = 1;
                    break;
                case "淘宝":
                    order_type = 2;
                    break;
                case "聚划算":
                    order_type = 3;
                    break;
            }

            return order_type;
        }

        int gettbstatus(int? status)
        {
            if (new int?[] { 3, 14 }.Contains(status))
            {
                return 2;
            }
            else if (new int?[] { 12 }.Contains(status))
            {
                return 1;
            }
            else if (new int?[] { 13 }.Contains(status))
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }

        int getjdstatus(int? status)
        {
            if (new int?[] { 17 }.Contains(status))
            {
                return 2;
            }
            else if (new int?[] { 16 }.Contains(status))
            {
                return 1;
            }
            else if (new int?[] { 3 }.Contains(status))
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取拼多多订单状态
        /// </summary>
        /// <param name="status"></param>
        int getpddstatus(int? status)
        {
            if (new int?[] { 5 }.Contains(status))
            {
                return 2;
            }
            else if (new int?[] { 0, 1 }.Contains(status))
            {
                return 1;
            }
            else if (new int?[] { 4 }.Contains(status))
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 时间转换
        int ConvertDate(DateTime? time)
        {
            return time == null ? 0 : int.Parse(time.Value.ToString("yyyyMMdd"));
        }

        int ConvertMonth(DateTime? time)
        {
            return time == null ? 0 : int.Parse(time.Value.ToString("yyyyMM"));
        }
        #endregion
    }
    public class CommonOrderEntity
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// appid
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 订单编号(父订单编号)
        /// </summary>
        public string order_sn { get; set; }
        /// <summary>
        /// 子订单编号
        /// </summary>
        public string sub_order_sn { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string origin_id { get; set; }
        /// <summary>
        /// 大平台类型:1=淘宝和天猫,3=京东,4=拼多多
        /// </summary>
        public int type_big { get; set; }
        /// <summary>
        /// 详细平台类型:1=淘宝,2=天猫,3=京东,4=拼多多
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 订单类型1=天猫2=淘宝3=聚划算
        /// </summary>
        public int order_type { get; set; }
        /// <summary>
        /// 商品标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 订单原始状态
        /// </summary>
        public int? order_status { get; set; }
        /// <summary>
        /// 返佣状态:0=未返,1=已返
        /// </summary>
        public int rebate_status { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int? product_num { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal? product_price { get; set; }
        /// <summary>
        /// 商品实付金额
        /// </summary>
        public decimal? payment_price { get; set; }
        /// <summary>
        /// 结算预估佣金
        /// </summary>
        public decimal? estimated_effect { get; set; }
        /// <summary>
        /// 收入比率
        /// </summary>
        public decimal? income_ratio { get; set; }
        /// <summary>
        /// 付款预估佣金
        /// </summary>
        public decimal? estimated_income { get; set; }
        /// <summary>
        /// 佣金比例
        /// </summary>
        public decimal? commission_rate { get; set; }
        /// <summary>
        /// 实际结算佣金金额
        /// </summary>
        public decimal? commission_amount { get; set; }
        /// <summary>
        /// 补贴比例
        /// </summary>
        public string subsidy_ratio { get; set; }
        /// <summary>
        /// 补贴金额
        /// </summary>
        public decimal? subsidy_amount { get; set; }
        /// <summary>
        /// 补贴类型
        /// </summary>
        public string subsidy_type { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string order_createtime { get; set; }
        /// <summary>
        /// 订单结算时间
        /// </summary>
        public string order_settlement_at { get; set; }
        /// <summary>
        /// 订单付款时间
        /// </summary>
        public string order_pay_time { get; set; }
        /// <summary>
        /// 订单成团时间
        /// </summary>
        public string order_group_success_time { get; set; }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime createtime { get; set; }
        /// <summary>
        /// 订单修改时间
        /// </summary>
        public DateTime updatetime { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string shopname { get; set; }
        /// <summary>
        /// 类目名称
        /// </summary>
        public string category_name { get; set; }
        /// <summary>
        /// 来源媒体名称
        /// </summary>
        public string media_name { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 广告位名称
        /// </summary>
        public string pid_name { get; set; }
        /// <summary>
        /// 广告位ID
        /// </summary>
        public string pid { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public string relation_id { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public string special_id { get; set; }
        /// <summary>
        /// 维权状态
        /// </summary>
        public int? protection_status { get; set; }
        /// <summary>
        /// 订单来源  1同步服务  2后台
        /// </summary>
        public int insert_type { get; set; }
        /// <summary>
        /// 本站订单归类状态: 0=未处理,1=付款,2=订单结算,3=失效,4=订单已返利
        /// </summary>
        public int order_type_new { get; set; }
        /// <summary>
        /// 订单创建日期
        /// </summary>
        public int order_create_date { get; set; }
        /// <summary>
        /// 订单创建月份
        /// </summary>
        public int order_create_month { get; set; }
        /// <summary>
        /// 订单确认收货日期
        /// </summary>
        public int order_receive_date { get; set; }
        /// <summary>
        /// 订单确认收货月份
        /// </summary>
        public int order_receive_month { get; set; }
    }
}
