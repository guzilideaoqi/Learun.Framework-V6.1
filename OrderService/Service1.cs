using Common;
using HYG.CommonHelper.Common;
using HYG.CommonHelper.ShoppingAPI;
using JDModel;
using MySql.Data.MySqlClient;
using OrderService.Common;
using OrderService.Model;
using PDDModel;
using ShoppingAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using TBModel;

namespace OrderService
{
    public partial class Service1 : ServiceBase
    {
        TBApi tbApi = null;
        JDApi jDApi = null;
        PDDApi pDDApi = null;

        string tb_tool_appkey = "25552805", tb_tool_appsecret = "7341a330d97862f21447f34c0fc326c9", appid = "";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogHelper.WriteLog("服务启动成功!");

            // Set up a timer that triggers every minute. 设置定时器
            Timer timer = new Timer();
            timer.Interval = 180000; // 60 seconds 60秒执行一次
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();


            Timer order_timer = new Timer();
            order_timer.Interval = 1000;
            order_timer.Elapsed += new ElapsedEventHandler(this.OnOrderTimer);
            order_timer.Start();
        }

        public void OnOrderTimer(object sender, ElapsedEventArgs args)
        {
            //整点同步近3个小时的订单、每天凌晨同步近2天的失效订单

            DateTime currentTime = DateTime.Now;

            string rateTimerStr = currentTime.ToString("DD HH:mm:ss");
            //每个月25号2点开始执行返利
            if (rateTimerStr == "25 02:00:00")
            {
                #region 开始执行返利
                string apiurl = ConfigurationManager.AppSettings["ExcuteRate_ApiUrl"];
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("appid", appid);
                string resultContent = AjaxRequest.HttpPost(apiurl, "", keyValuePairs);

                LogHelper.WriteLog(resultContent);
                #endregion
            }
            else if (rateTimerStr == "23 02:00:00" || rateTimerStr == "24 02:00:00")
            {
                #region 开始同步上个月的结算订单
                new System.Threading.Thread(() =>
                {
                    int start_time = 0;
                    currentTime = DateTime.Parse(currentTime.AddMonths(-1).ToString("yyyy-MM-01 00:00:00"));
                    int total_day = DateTime.DaysInMonth(currentTime.Year, currentTime.Month);
                    int total_num = total_day * 8;
                    for (int i = 0; i < total_num; i++)
                    {
                        start_time = 3 * i;
                        string starttime = currentTime.AddHours(start_time).ToString("yyyy-MM-dd HH:mm:ss");
                        string endtime = currentTime.AddHours((start_time + 3)).ToString("yyyy-MM-dd HH:mm:ss");

                        #region 开始上个月的结算订单
                        //同步所有订单
                        synd_tb_order(3, 1, 50, "14", "", starttime, endtime);
                        System.Threading.Thread.Sleep(3000);
                        #endregion
                    }
                }).Start();
                #endregion
            }
            else if (currentTime.Hour == 1 && currentTime.Second == 0 && currentTime.Minute == 0)
            {
                new System.Threading.Thread(() =>
                {
                    int start_time = 0;
                    for (int i = 144; i > 0; i--)
                    {
                        start_time = 20 * i;
                        #region 开始同步淘宝订单
                        //同步所有订单
                        synd_tb_order(2, 1, 50, "13", "", currentTime.AddMinutes(start_time * -1).ToString("yyyy-MM-dd HH:mm:ss"), currentTime.AddMinutes((start_time - 20) * -1).ToString("yyyy-MM-dd HH:mm:ss"));
                        System.Threading.Thread.Sleep(3000);
                        #endregion
                    }
                }).Start();
            }
            else if (currentTime.Second == 0 && currentTime.Minute == 0)
            {//整点同步近3个小时的订单
                new System.Threading.Thread(() =>
                {
                    int start_time = 0;
                    for (int i = 9; i > 0; i--)
                    {
                        start_time = 20 * i;
                        #region 开始同步淘宝订单
                        //同步所有订单
                        synd_tb_order(2, 1, 50, "", "", currentTime.AddMinutes(start_time * -1).ToString("yyyy-MM-dd HH:mm:ss"), currentTime.AddMinutes((start_time - 20) * -1).ToString("yyyy-MM-dd HH:mm:ss"));
                        System.Threading.Thread.Sleep(5000);
                        #endregion
                    }
                }).Start();
            }
        }

        protected override void OnStop()
        {
            LogHelper.WriteLog("服务已停止!");
        }

        /// <summary>
        /// 继续服务
        /// </summary>
        protected override void OnContinue()
        {
            LogHelper.WriteLog("服务继续执行!");
        }

        /// <summary>
        /// 定时器中定时执行的任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            string apiurl = ConfigurationManager.AppSettings["apiUrl"];
            appid = ConfigurationManager.AppSettings["appid"];
            string starttime = ConfigurationManager.AppSettings["starttime"];
            // TODO: Insert monitoring activities here.
            #region 开始同步订单
            new System.Threading.Thread(() =>
            {
                try
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add("appid", appid);
                    string resultContent = AjaxRequest.HttpPost(apiurl, "", keyValuePairs);
                    BaseSettingReponse baseSettingReponse = JsonConvert.JsonDeserialize<BaseSettingReponse>(resultContent);
                    if (baseSettingReponse.code == 200)
                    {
                        BaseSettingEntity baseSettingEntity = baseSettingReponse.data;
                        if (baseSettingEntity != null)
                        {
                            tbApi = new TBApi(tb_tool_appkey, tb_tool_appsecret, baseSettingEntity.tb_sessionkey);
                            jDApi = new JDApi(baseSettingEntity.jd_appkey, baseSettingEntity.jd_appsecret, baseSettingEntity.jd_sessionkey);
                            pDDApi = new PDDApi(baseSettingEntity.pdd_clientid, baseSettingEntity.pdd_clientsecret, "");
                        }
                    }
                    else
                    {
                        LogHelper.WriteLog(resultContent);
                    }


                    DateTime currentTime = string.IsNullOrWhiteSpace(starttime) ? DateTime.Now : DateTime.Parse(starttime);

                    #region 开始同步淘宝订单
                    //同步付款订单
                    synd_tb_order(2, 1, 20, "12", "", currentTime.AddMinutes(-20).ToString("yyyy-MM-dd HH:mm:ss"), currentTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    System.Threading.Thread.Sleep(5000);
                    //同步结算订单
                    synd_tb_order(3, 1, 20, "3", "", currentTime.AddMinutes(-20).ToString("yyyy-MM-dd HH:mm:ss"), currentTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    System.Threading.Thread.Sleep(5000);
                    ///同步失效订单
                    synd_tb_order(2, 1, 20, "13", "", currentTime.AddMinutes(-20).ToString("yyyy-MM-dd HH:mm:ss"), currentTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    #endregion

                    #region 开始同步京东订单
                    System.Threading.Thread.Sleep(5000);
                    synd_jd_order(1, 20, currentTime.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), currentTime.ToString("yyyy-MM-dd HH:mm:ss"), jDApi.access_token);
                    #endregion

                    #region 开始同步拼多多订单
                    System.Threading.Thread.Sleep(5000);
                    synd_pdd_order(1, 20, TimeSpanConvert.ConvertDateTimeToInt(currentTime.AddMinutes(-100)), TimeSpanConvert.ConvertDateTimeToInt(currentTime));
                    #endregion

                    System.Threading.Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex.Message + @"\r\n" + ex.StackTrace);
                }
            }).Start();
            #endregion
        }

        #region 同步京东订单
        void synd_jd_order_old(int pageNo, int pageSize, string startTime, string endTime)
        {
            try
            {
                #region 京东订单处理
                jd_union_open_order_row_query_response jdRowOrder = jDApi.GetRowOrder(1, 20, 3, startTime, endTime, jDApi.access_token);
                if (jdRowOrder != null)
                {
                    if (jdRowOrder.data != null)
                    {
                        foreach (JDRowOrder item in jdRowOrder.data)
                        {
                            MySqlParameter[] ps = new MySqlParameter[]{
                        new MySqlParameter("@id",item.id),
                        new MySqlParameter("@orderId",item.orderId),
                        new MySqlParameter("@parentId",item.parentId),
                        new MySqlParameter("@orderTime",item.orderTime),
                        new MySqlParameter("@finishTime",item.finishTime),
                        new MySqlParameter("@modifyTime",item.modifyTime),
                        new MySqlParameter("@orderEmt",item.orderEmt),
                        new MySqlParameter("@plus",item.plus),
                        new MySqlParameter("@unionId",item.unionId),
                        new MySqlParameter("@skuId",item.skuId),
                        new MySqlParameter("@skuName",item.skuName),
                        new MySqlParameter("@skuNum",item.skuName),
                        new MySqlParameter("@skuReturnNum",item.skuReturnNum),
                        new MySqlParameter("@skuFrozenNum",item.skuFrozenNum),
                        new MySqlParameter("@price",item.price),
                        new MySqlParameter("@commissionRate",item.commissionRate),
                        new MySqlParameter("@subSideRate",item.subSideRate),
                        new MySqlParameter("@subsidyRate",item.subsidyRate),
                        new MySqlParameter("@finalRate",item.finalRate),
                        new MySqlParameter("@estimateCosPrice",item.estimateCosPrice),
                        new MySqlParameter("@estimateFee",item.estimateFee),
                        new MySqlParameter("@actualCosPrice",item.actualCosPrice),
                        new MySqlParameter("@actualFee",item.actualFee),
                        new MySqlParameter("@validCode",item.validCode),
                        new MySqlParameter("@traceType",item.traceType),
                        new MySqlParameter("@positionId",item.positionId),
                        new MySqlParameter("@siteId",item.siteId),
                        new MySqlParameter("@pid",item.pid),
                        new MySqlParameter("@cid1",item.cid1),
                        new MySqlParameter("@cid2",item.cid2),
                        new MySqlParameter("@cid3",item.cid3),
                        new MySqlParameter("@subUnionId",item.subUnionId),
                        new MySqlParameter("@unionTag",item.unionTag),
                        new MySqlParameter("@popId",item.popId),
                        new MySqlParameter("@payMonth",item.payMonth),
                        new MySqlParameter("@cpActId",item.cpActId),
                        new MySqlParameter("@unionRole",item.unionRole),
                        new MySqlParameter("@giftCouponOcsAmount",item.giftCouponOcsAmount),
                        new MySqlParameter("@giftCouponKey",item.giftCouponKey),
                        new MySqlParameter("@balanceExt",item.balanceExt),
                    };
                            string updateSql = string.Format(@"insert INTO dm_jd_order(id, orderId, parentId, orderTime, finishTime, modifyTime, orderEmt, plus, unionId, skuId, skuName, skuNum, skuReturnNum, skuFrozenNum, price, commissionRate, subSideRate, subsidyRate, finalRate, estimateCosPrice, estimateFee, actualCosPrice, actualFee, validCode, traceType, positionId, siteId, pid, cid1, cid2, cid3, subUnionId, unionTag, popId, payMonth, cpActId, unionRole, giftCouponOcsAmount, giftCouponKey, balanceExt) 
VALUES (@id, @orderId, @parentId, @orderTime, @finishTime, @modifyTime, @orderEmt, @plus, @unionId, @skuId, @skuName, @skuNum, @skuReturnNum, @skuFrozenNum, @price, @commissionRate, @subSideRate, @subsidyRate, @finalRate, @estimateCosPrice,
@estimateFee, @actualCosPrice, @actualFee, @validCode, @traceType, @positionId, @siteId, @pid, @cid1, @cid2, @cid3, @subUnionId, @unionTag, @popId, @payMonth, @cpActId, @unionRole, @giftCouponOcsAmount, @giftCouponKey, @balanceExt) ON DUPLICATE KEY UPDATE 
id=@id, orderId=@orderId, parentId=@parentId, orderTime=@orderTime, finishTime=@finishTime, modifyTime=@modifyTime, orderEmt=@orderEmt, plus=@plus, unionId=@unionId, skuId=@skuId, skuName=@skuName, skuNum=@skuNum, skuReturnNum=@skuReturnNum, skuFrozenNum=@skuFrozenNum, price=@price, commissionRate=@commissionRate, subSideRate=@subSideRate, subsidyRate=@subsidyRate, finalRate=@finalRate, estimateCosPrice=@estimateCosPrice, estimateFee=@estimateFee, actualCosPrice=@actualCosPrice, actualFee=@actualFee, validCode=@validCode, traceType=@traceType, positionId=@positionId, siteId=@siteId, pid=@pid, cid1=@cid1, cid2=@cid2, cid3=@cid3, subUnionId=@subUnionId, unionTag=@unionTag, popId=@popId, payMonth=@payMonth, cpActId=@cpActId, unionRole=@unionRole, giftCouponOcsAmount=@giftCouponOcsAmount, giftCouponKey=@giftCouponKey, balanceExt=@balanceExt;");
                            MySqlHelperByH.ExecuteNonQuery(CommandType.Text, updateSql, ps);
                        }

                        if (jdRowOrder.hasMore)
                        {
                            synd_jd_order_old(pageNo + 1, pageSize, startTime, endTime);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(@"synd_jd_order\r\n" + ex.Message + @"\r\n" + ex.StackTrace);
            }
        }

        void synd_jd_order(int pageNo, int pageSize, string startTime, string endTime, string key)
        {
            try
            {
                #region 京东订单处理
                jd_union_open_order_row_query_response jdRowOrder = jDApi.GetRowOrder(1, 20, 3, startTime, endTime, key);
                if (jdRowOrder != null)
                {
                    if (jdRowOrder.data != null)
                    {
                        foreach (JDRowOrder item in jdRowOrder.data)
                        {
                            CommonOrderEntity commonOrderEntity = new CommonOrderEntity();
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
                            commonOrderEntity.order_createtime = item.orderTime;
                            commonOrderEntity.order_settlement_at = item.finishTime;
                            commonOrderEntity.order_pay_time = item.orderTime;
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
                            synd_jd_order(pageNo + 1, pageSize, startTime, endTime, key);
                        }
                        else
                        {
                            InsertCommonOrder(commonOrderList);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(@"synd_jd_order\r\n" + ex.Message + @"\r\n" + ex.StackTrace);
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
        #endregion

        #region 同步拼多多订单
        private HashHelper hashHelper = new HashHelper();
        void synd_pdd_order_old(int pageNo, int pageSize, string startTime, string endTime)
        {
            try
            {
                if (pDDApi != null)
                {
                    #region 拼多多订单处理
                    List<pdd_order> orderList = pDDApi.GetOrderList(startTime, endTime, pageNo, pageSize, false);
                    if (orderList != null)
                    {
                        foreach (pdd_order item in orderList)
                        {
                            string hash_id = hashHelper.HashString(item.order_sn + item.goods_id);

                            MySqlParameter[] ps = new MySqlParameter[]{
                        new MySqlParameter("@hashid",hash_id),
                        new MySqlParameter("@order_sn",item.order_sn),
                        new MySqlParameter("@goods_id",item.goods_id),
                        new MySqlParameter("@goods_name",item.goods_name),
                        new MySqlParameter("@goods_thumbnail_url",item.goods_thumbnail_url),
                        new MySqlParameter("@goods_quantity",item.goods_quantity),
                        new MySqlParameter("@goods_price",item.goods_price),
                        new MySqlParameter("@order_amount",item.order_amount),
                        new MySqlParameter("@promotion_rate",item.promotion_rate),
                        new MySqlParameter("@promotion_amount",item.promotion_amount),
                        new MySqlParameter("@batch_no",item.batch_no),
                        new MySqlParameter("@order_status",item.order_status),
                        new MySqlParameter("@order_status_desc",item.order_status_desc),
                        new MySqlParameter("@order_create_time",item.order_create_time),
                        new MySqlParameter("@order_pay_time",item.order_pay_time),
                        new MySqlParameter("@order_group_success_time",item.order_group_success_time),
                        new MySqlParameter("@order_receive_time",item.order_receive_time),
                        new MySqlParameter("@order_verify_time",item.order_verify_time),
                        new MySqlParameter("@order_settle_time",item.order_settle_time),
                        new MySqlParameter("@order_modify_at",item.order_modify_at),
                        new MySqlParameter("@type",item.type),
                        new MySqlParameter("@group_id",item.group_id),
                        new MySqlParameter("@auth_duo_id",item.auth_duo_id),
                        new MySqlParameter("@zs_duo_id",item.zs_duo_id),
                        new MySqlParameter("@custom_parameters",item.custom_parameters),
                        new MySqlParameter("@cps_sign",item.cps_sign),
                        new MySqlParameter("@url_last_generate_time",item.url_last_generate_time),
                        new MySqlParameter("@point_time",item.point_time),
                        new MySqlParameter("@return_status",item.return_status),
                        new MySqlParameter("@p_id",item.p_id),
                        new MySqlParameter("@cpa_new",item.cpa_new)
                    };


                            string updateSql = @"insert into dm_pdd_order(hashid,order_sn,goods_id,goods_name,goods_thumbnail_url,goods_quantity,goods_price,order_amount,promotion_rate,promotion_amount,batch_no,order_status,order_status_desc,order_create_time,order_pay_time,order_group_success_time,order_receive_time,order_verify_time,order_settle_time,order_modify_at,type,group_id,auth_duo_id,zs_duo_id,custom_parameters,cps_sign,url_last_generate_time,point_time,return_status,p_id,cpa_new)
VALUES(@hashid, @order_sn, @goods_id, @goods_name, @goods_thumbnail_url, @goods_quantity, @goods_price, @order_amount, @promotion_rate, @promotion_amount, @batch_no, @order_status, @order_status_desc, @order_create_time, @order_pay_time, @order_group_success_time, @order_receive_time, @order_verify_time, @order_settle_time, @order_modify_at, @type, @group_id, @auth_duo_id, @zs_duo_id, @custom_parameters, @cps_sign, @url_last_generate_time, @point_time, @return_status, @p_id, @cpa_new) ON DUPLICATE KEY UPDATE
hashid = @hashid,order_sn = @order_sn,goods_id = @goods_id,goods_name = @goods_name,goods_thumbnail_url = @goods_thumbnail_url,goods_quantity = @goods_quantity,goods_price = @goods_price,order_amount = @order_amount,promotion_rate = @promotion_rate,promotion_amount = @promotion_amount,batch_no = @batch_no,order_status = @order_status,order_status_desc = @order_status_desc,order_create_time = @order_create_time,order_pay_time = @order_pay_time,order_group_success_time = @order_group_success_time,order_receive_time = @order_receive_time,order_verify_time = @order_verify_time,order_settle_time = @order_settle_time,order_modify_at = @order_modify_at,type = @type,group_id = @group_id,auth_duo_id = @auth_duo_id,zs_duo_id = @zs_duo_id,custom_parameters = @custom_parameters,cps_sign = @cps_sign,url_last_generate_time = @url_last_generate_time,point_time = @point_time,return_status = @return_status,p_id = @p_id,cpa_new = @cpa_new
"; MySqlHelperByH.ExecuteNonQuery(CommandType.Text, updateSql, ps);
                        }
                        if (orderList.Count >= 20)
                        {
                            synd_pdd_order_old(pageNo + 1, pageSize, startTime, endTime);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(@"synd_jd_order\r\n" + ex.Message + @"\r\n" + ex.StackTrace);
            }
        }

        void synd_pdd_order(int pageNo, int pageSize, string startTime, string endTime)
        {
            try
            {
                if (pDDApi != null)
                {
                    #region 拼多多订单处理
                    List<pdd_order> orderList = pDDApi.GetOrderList(startTime, endTime, pageNo, pageSize, false);
                    if (orderList != null)
                    {
                        foreach (pdd_order item in orderList)
                        {
                            CommonOrderEntity commonOrderEntity = new CommonOrderEntity();
                            commonOrderEntity.order_sn = item.order_sn;//订单id
                            commonOrderEntity.sub_order_sn = item.order_sn;//子订单id
                            commonOrderEntity.origin_id = item.goods_id;//商品id
                            commonOrderEntity.id = hashHelper.HashString(commonOrderEntity.order_sn + commonOrderEntity.sub_order_sn + commonOrderEntity.origin_id);//id通过
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
                            commonOrderEntity.order_createtime = TimeSpanConvert.TimeSpanToDateTime(long.Parse(item.order_create_time)).ToString("yyyy-MM-dd HH:mm:ss");
                            commonOrderEntity.order_settlement_at = TimeSpanConvert.TimeSpanToDateTime(long.Parse(item.order_settle_time)).ToString("yyyy-MM-dd HH:mm:ss");
                            commonOrderEntity.order_pay_time = TimeSpanConvert.TimeSpanToDateTime(long.Parse(item.order_pay_time)).ToString("yyyy-MM-dd HH:mm:ss");
                            commonOrderEntity.order_group_success_time = TimeSpanConvert.TimeSpanToDateTime(long.Parse(item.order_group_success_time)).ToString("yyyy-MM-dd HH:mm:ss");
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
                            synd_pdd_order(pageNo + 1, pageSize, startTime, endTime);
                        }
                        else
                        {
                            InsertCommonOrder(commonOrderList);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(@"synd_jd_order\r\n" + ex.Message + @"\r\n" + ex.StackTrace);
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

        #region 同步淘宝订单
        void synd_tb_order_old(int queryType, int pageNo, int pageSize, string postion_index, string startTime, string endTime)
        {
            try
            {
                if (tbApi != null)
                {
                    #region 淘宝订单处理
                    OrderData orderData = tbApi.GetOrder(queryType.ToString(), postion_index, pageSize.ToString(), "", startTime, endTime, "1", pageNo.ToString(), "2");

                    if (orderData != null)
                    {
                        if (orderData.results.publisher_order_dto != null)
                        {
                            foreach (tb_order item in orderData.results.publisher_order_dto)
                            {
                                string hash_id = hashHelper.HashString(item.trade_id + item.trade_parent_id + item.item_id);

                                MySqlParameter[] ps = new MySqlParameter[]{
                        new MySqlParameter("@hashid",hash_id),
                        new MySqlParameter("@tb_paid_time",item.tb_paid_time),

    new MySqlParameter("@tk_paid_time",item.tk_paid_time),

    new MySqlParameter("@pay_price",item.pay_price),

    new MySqlParameter("@pub_share_fee",item.pub_share_fee),

    new MySqlParameter("@trade_id",item.trade_id),

    new MySqlParameter("@tk_order_role",item.tk_order_role),

    new MySqlParameter("@tk_earning_time",item.tk_earning_time),

    new MySqlParameter("@adzone_id",item.adzone_id),

    new MySqlParameter("@pub_share_rate",item.pub_share_rate),

    new MySqlParameter("@refund_tag",item.refund_tag),

    new MySqlParameter("@subsidy_rate",item.subsidy_rate),

    new MySqlParameter("@tk_total_rate",item.tk_total_rate),

    new MySqlParameter("@item_category_name",item.item_category_name),

    new MySqlParameter("@seller_nick",item.seller_nick),

    new MySqlParameter("@pub_id",item.pub_id),

    new MySqlParameter("@alimama_rate",item.alimama_rate),

    new MySqlParameter("@subsidy_type",item.subsidy_type),

    new MySqlParameter("@item_img",item.item_img),

    new MySqlParameter("@pub_share_pre_fee",item.pub_share_pre_fee),

    new MySqlParameter("@alipay_total_price",item.alipay_total_price),

    new MySqlParameter("@item_title",item.item_title),

    new MySqlParameter("@site_name",item.site_name),

    new MySqlParameter("@item_num",item.item_num),

    new MySqlParameter("@subsidy_fee",item.subsidy_fee),

    new MySqlParameter("@alimama_share_fee",item.alimama_share_fee),

    new MySqlParameter("@trade_parent_id",item.trade_parent_id),

    new MySqlParameter("@order_type",item.order_type),

    new MySqlParameter("@tk_create_time",item.tk_create_time),

    new MySqlParameter("@flow_source",item.flow_source),

    new MySqlParameter("@terminal_type",item.terminal_type),

    new MySqlParameter("@click_time",item.click_time),

    new MySqlParameter("@tk_status",item.tk_status),

    new MySqlParameter("@item_price",item.item_price),

    new MySqlParameter("@item_id",item.item_id),

    new MySqlParameter("@adzone_name",item.adzone_name),

    new MySqlParameter("@total_commission_rate",item.total_commission_rate),

    new MySqlParameter("@item_link",item.item_link),

    new MySqlParameter("@site_id",item.site_id),

    new MySqlParameter("@seller_shop_title",item.seller_shop_title),

    new MySqlParameter("@income_rate",item.income_rate),

    new MySqlParameter("@total_commission_fee",item.total_commission_fee),

    new MySqlParameter("@tk_commission_pre_fee_for_media_platform",item.tk_commission_pre_fee_for_media_platform),

    new MySqlParameter("@tk_commission_fee_for_media_platform",item.tk_commission_fee_for_media_platform),

    new MySqlParameter("@tk_commission_rate_for_media_platform",item.tk_commission_rate_for_media_platform),

    new MySqlParameter("@special_id",item.special_id),

    new MySqlParameter("@relation_id",item.relation_id),

    new MySqlParameter("@tk_deposit_time",item.tk_deposit_time),

    new MySqlParameter("@tb_deposit_time",item.tb_deposit_time),

    new MySqlParameter("@deposit_price",item.deposit_price),

    new MySqlParameter("@app_key",item.app_key)
                    };

                                string updateSql = @"INSERT INTO dm_pdd_order (
	hashid,
	tb_paid_time,
	tk_paid_time,
	pay_price,
	pub_share_fee,
	trade_id,
	tk_order_role,
	tk_earning_time,
	adzone_id,
	pub_share_rate,
	refund_tag,
	subsidy_rate,
	tk_total_rate,
	item_category_name,
	seller_nick,
	pub_id,
	alimama_rate,
	subsidy_type,
	item_img,
	pub_share_pre_fee,
	alipay_total_price,
	item_title,
	site_name,
	item_num,
	subsidy_fee,
	alimama_share_fee,
	trade_parent_id,
	order_type,
	tk_create_time,
	flow_source,
	terminal_type,
	click_time,
	tk_status,
	item_price,
	item_id,
	adzone_name,
	total_commission_rate,
	item_link,
	site_id,
	seller_shop_title,
	income_rate,
	total_commission_fee,
	tk_commission_pre_fee_for_media_platform,
	tk_commission_fee_for_media_platform,
	tk_commission_rate_for_media_platform,
	special_id,
	relation_id,
	tk_deposit_time,
	tb_deposit_time,
	deposit_price,
	app_key
)
VALUES
	(
		@hashid,
		@tb_paid_time,
		@tk_paid_time,
		@pay_price,
		@pub_share_fee,
		@trade_id,
		@tk_order_role,
		@tk_earning_time,
		@adzone_id,
		@pub_share_rate,
		@refund_tag,
		@subsidy_rate,
		@tk_total_rate,
		@item_category_name,
		@seller_nick,
		@pub_id,
		@alimama_rate,
		@subsidy_type,
		@item_img,
		@pub_share_pre_fee,
		@alipay_total_price,
		@item_title,
		@site_name,
		@item_num,
		@subsidy_fee,
		@alimama_share_fee,
		@trade_parent_id,
		@order_type,
		@tk_create_time,
		@flow_source,
		@terminal_type,
		@click_time,
		@tk_status,
		@item_price,
		@item_id,
		@adzone_name,
		@total_commission_rate,
		@item_link,
		@site_id,
		@seller_shop_title,
		@income_rate,
		@total_commission_fee,
		@tk_commission_pre_fee_for_media_platform,
		@tk_commission_fee_for_media_platform,
		@tk_commission_rate_for_media_platform,
		@special_id,
		@relation_id,
		@tk_deposit_time,
		@tb_deposit_time,
		@deposit_price,
		@app_key
	) ON DUPLICATE KEY UPDATE hashid =@hashid,
	tb_paid_time =@tb_paid_time,
	tk_paid_time =@tk_paid_time,
	pay_price =@pay_price,
	pub_share_fee = @pub_share_fee,
	trade_id = @trade_id,
	tk_order_role = @tk_order_role,
	tk_earning_time = @tk_earning_time,
	adzone_id = @adzone_id,
	pub_share_rate = @pub_share_rate,
	refund_tag = @refund_tag,
	subsidy_rate = @subsidy_rate,
	tk_total_rate = @tk_total_rate,
	item_category_name = @item_category_name,
	seller_nick = @seller_nick,
	pub_id = @pub_id,
	alimama_rate = @alimama_rate,
	subsidy_type = @subsidy_type,
	item_img = @item_img,
	pub_share_pre_fee = @pub_share_pre_fee,
	alipay_total_price = @alipay_total_price,
	item_title = @item_title,
	site_name = @site_name,
	item_num = @item_num,
	subsidy_fee = @subsidy_fee,
	alimama_share_fee = @alimama_share_fee,
	trade_parent_id = @trade_parent_id,
	order_type = @order_type,
	tk_create_time = @tk_create_time,
	flow_source = @flow_source,
	terminal_type = @terminal_type,
	click_time = @click_time,
	tk_status = @tk_status,
	item_price = @item_price,
	item_id = @item_id,
	adzone_name = @adzone_name,
	total_commission_rate = @total_commission_rate,
	item_link = @item_link,
	site_id = @site_id,
	seller_shop_title = @seller_shop_title,
	income_rate = @income_rate,
	total_commission_fee = @total_commission_fee,
	tk_commission_pre_fee_for_media_platform = @tk_commission_pre_fee_for_media_platform,
	tk_commission_fee_for_media_platform = @tk_commission_fee_for_media_platform,
	tk_commission_rate_for_media_platform = @tk_commission_rate_for_media_platform,
	special_id = @special_id,
	relation_id = @relation_id,
	tk_deposit_time = @tk_deposit_time,
	tb_deposit_time = @tb_deposit_time,
	deposit_price = @deposit_price,
	app_key = @app_key";
                                MySqlHelperByH.ExecuteNonQuery(CommandType.Text, updateSql, ps);
                            }
                        }
                    }

                    if (orderData.has_next)
                    {
                        tbApi.GetOrder("2", orderData.position_index, pageSize.ToString(), "", startTime, endTime, "1", pageNo.ToString(), "2");
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(@"synd_tb_order\r\n" + ex.Message + @"\r\n" + ex.StackTrace);
            }

        }
        List<CommonOrderEntity> commonOrderList = new List<CommonOrderEntity>();
        void synd_tb_order(int queryType, int pageNo, int pageSize, string tk_status, string postion_index, string startTime, string endTime)
        {
            try
            {
                if (tbApi != null)
                {
                    #region 淘宝订单处理
                    OrderData orderData = tbApi.GetOrder(queryType.ToString(), postion_index, pageSize.ToString(), tk_status, startTime, endTime, "1", pageNo.ToString(), "2");

                    if (orderData != null)
                    {
                        if (orderData.results.publisher_order_dto != null)
                        {
                            foreach (tb_order item in orderData.results.publisher_order_dto)
                            {
                                CommonOrderEntity commonOrderEntity = new CommonOrderEntity();
                                commonOrderEntity.order_sn = item.trade_parent_id;//订单id
                                commonOrderEntity.sub_order_sn = item.trade_id;//子订单id
                                commonOrderEntity.origin_id = item.item_id;//商品id
                                commonOrderEntity.id = hashHelper.HashString(commonOrderEntity.order_sn + commonOrderEntity.sub_order_sn + commonOrderEntity.origin_id);//id通过
                                commonOrderEntity.type_big = 1;
                                commonOrderEntity.order_type = getordertype(item.order_type);
                                commonOrderEntity.type = commonOrderEntity.order_type;
                                commonOrderEntity.title = item.item_title;
                                commonOrderEntity.order_status = item.tk_status;
                                commonOrderEntity.order_type_new = gettbstatus(commonOrderEntity.order_status);
                                commonOrderEntity.rebate_status = 0;
                                commonOrderEntity.image = item.item_img;
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
                                commonOrderEntity.order_createtime = item.tk_create_time;
                                commonOrderEntity.order_settlement_at = item.tk_earning_time;
                                commonOrderEntity.order_pay_time = item.tb_paid_time;
                                commonOrderEntity.createtime = DateTime.Now;
                                commonOrderEntity.updatetime = DateTime.Now;
                                commonOrderEntity.shopname = item.seller_shop_title;
                                commonOrderEntity.category_name = item.item_category_name;
                                commonOrderEntity.media_id = item.site_id;
                                commonOrderEntity.media_name = item.site_name;
                                commonOrderEntity.pid = item.adzone_id;
                                commonOrderEntity.pid_name = item.adzone_name;
                                commonOrderEntity.relation_id = item.relation_id;
                                commonOrderEntity.special_id = item.special_id;
                                commonOrderEntity.protection_status = item.refund_tag;
                                commonOrderEntity.insert_type = 1;
                                commonOrderEntity.order_create_date = ConvertDate(commonOrderEntity.order_createtime);
                                commonOrderEntity.order_create_month = ConvertMonth(commonOrderEntity.order_createtime);
                                commonOrderEntity.order_receive_date = ConvertDate(commonOrderEntity.order_settlement_at);
                                commonOrderEntity.order_receive_month = ConvertMonth(commonOrderEntity.order_settlement_at);

                                commonOrderList.Add(commonOrderEntity);
                            }
                        }
                    }

                    if (orderData.has_next)
                    {
                        tbApi.GetOrder("2", orderData.position_index, pageSize.ToString(), tk_status, startTime, endTime, "1", pageNo.ToString(), "2");
                    }
                    else
                    {
                        InsertCommonOrder(commonOrderList);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(@"synd_tb_order\r\n" + ex.Message + @"\r\n" + ex.StackTrace);
            }
        }

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
        #endregion

        #region 订单统一化管理
        public void InsertCommonOrder(List<CommonOrderEntity> commonOrderList)
        {
            if (commonOrderList.Count <= 0)
                return;
            IEnumerable<string> hashids = commonOrderList.Select(t => t.id);
            string ids = "";
            foreach (string item in hashids)
            {
                if (ids != "")
                    ids += ",";
                ids += "'" + item + "'";
            }

            DataTable existOrderTable = MySqlHelperByH.ExecuteDataTable(CommandType.Text, "select id,order_type_new from dm_order where id in (" + ids + ")");

            string excuteSql = "";
            MySqlParameter[] ps = null;

            foreach (CommonOrderEntity commonOrderEntity in commonOrderList)
            {
                DataRow[] existRecord = existOrderTable.Select(" id='" + commonOrderEntity.id + "'");
                if (existRecord.Length <= 0)
                {
                    if (!string.IsNullOrWhiteSpace(commonOrderEntity.image))
                    {
                        if (!commonOrderEntity.image.StartsWith("http") && !commonOrderEntity.image.StartsWith("https") && commonOrderEntity.image.StartsWith("//"))
                        {
                            commonOrderEntity.image = "https:" + commonOrderEntity.image;
                        }
                    }

                    #region 执行插入语句
                    excuteSql = @"insert into dm_order(id,appid,order_sn,sub_order_sn,origin_id,type_big,type,order_type,title,order_status,rebate_status,image,product_num,product_price,payment_price,estimated_effect
,income_ratio,estimated_income,commission_rate,commission_amount,subsidy_ratio,subsidy_amount,subsidy_type,order_createtime,order_settlement_at,order_pay_time,order_group_success_time,
createtime,shopname,category_name,media_name,media_id,pid_name,pid,relation_id,special_id,protection_status,insert_type,order_type_new,order_create_date,order_create_month,order_receive_date,order_receive_month)
values(@id,@appid,@order_sn,@sub_order_sn,@origin_id,@type_big,@type,@order_type,@title,@order_status,@rebate_status,@image,@product_num,@product_price,@payment_price,@estimated_effect
,@income_ratio,@estimated_income,@commission_rate,@commission_amount,@subsidy_ratio,@subsidy_amount,@subsidy_type,@order_createtime,@order_settlement_at,@order_pay_time,@order_group_success_time,
@createtime,@shopname,@category_name,@media_name,@media_id,@pid_name,@pid,@relation_id,@special_id,@protection_status,@insert_type,@order_type_new,@order_create_date,@order_create_month,@order_receive_date,@order_receive_month)";
                    #endregion

                    #region 插入语句参数
                    ps = new MySqlParameter[]
                    {
                       new MySqlParameter("@id",commonOrderEntity.id),
                       new MySqlParameter("@appid",appid),
                       new MySqlParameter("@order_sn",commonOrderEntity.order_sn),
                       new MySqlParameter("@sub_order_sn",commonOrderEntity.sub_order_sn),
                       new MySqlParameter("@origin_id",commonOrderEntity.origin_id),
                       new MySqlParameter("@type_big",commonOrderEntity.type_big),
                       new MySqlParameter("@type",commonOrderEntity.type),
                       new MySqlParameter("@order_type",commonOrderEntity.order_type),
                       new MySqlParameter("@title",commonOrderEntity.title),
                       new MySqlParameter("@order_status",commonOrderEntity.order_status),
                       new MySqlParameter("@rebate_status",commonOrderEntity.rebate_status),
                       new MySqlParameter("@image",commonOrderEntity.image),
                       new MySqlParameter("@product_num",commonOrderEntity.product_num),
                       new MySqlParameter("@product_price",commonOrderEntity.product_price),
                       new MySqlParameter("@payment_price",commonOrderEntity.payment_price),
                       new MySqlParameter("@estimated_effect",commonOrderEntity.estimated_effect),
                       new MySqlParameter("@income_ratio",commonOrderEntity.income_ratio),
                       new MySqlParameter("@estimated_income",commonOrderEntity.estimated_income),
                       new MySqlParameter("@commission_rate",commonOrderEntity.commission_rate),
                       new MySqlParameter("@commission_amount",commonOrderEntity.commission_amount),
                       new MySqlParameter("@subsidy_ratio",commonOrderEntity.subsidy_ratio),
                       new MySqlParameter("@subsidy_amount",commonOrderEntity.subsidy_amount),
                       new MySqlParameter("@subsidy_type",commonOrderEntity.subsidy_type),
                       new MySqlParameter("@order_createtime",commonOrderEntity.order_createtime),
                       new MySqlParameter("@order_settlement_at",CheckTime(commonOrderEntity.order_settlement_at)),
                       new MySqlParameter("@order_pay_time",CheckTime(commonOrderEntity.order_pay_time)),
                       new MySqlParameter("@order_group_success_time",CheckTime(commonOrderEntity.order_group_success_time)),
                       new MySqlParameter("@createtime",commonOrderEntity.createtime),
                       new MySqlParameter("@shopname",commonOrderEntity.shopname),
                       new MySqlParameter("@category_name",commonOrderEntity.category_name),
                       new MySqlParameter("@media_name",commonOrderEntity.media_name),
                       new MySqlParameter("@media_id",commonOrderEntity.media_id),
                       new MySqlParameter("@pid_name",commonOrderEntity.pid_name),
                       new MySqlParameter("@pid",commonOrderEntity.pid),
                       new MySqlParameter("@relation_id",commonOrderEntity.relation_id),
                       new MySqlParameter("@special_id",commonOrderEntity.special_id),
                       new MySqlParameter("@protection_status",commonOrderEntity.protection_status),
                       new MySqlParameter("@insert_type",commonOrderEntity.insert_type),
                       new MySqlParameter("@order_type_new",commonOrderEntity.order_type_new),
                       new MySqlParameter("@order_create_date",commonOrderEntity.order_create_date),
                       new MySqlParameter("@order_create_month",commonOrderEntity.order_create_month),
                       new MySqlParameter("@order_receive_date",commonOrderEntity.order_receive_date),
                       new MySqlParameter("@order_receive_month",commonOrderEntity.order_receive_month),
                    };
                    #endregion
                }
                else if (existRecord.Length == 1)
                {
                    DataRow dataRow = existRecord[0];
                    int order_type_new = int.Parse(dataRow["order_type_new"].ToString());
                    if (commonOrderEntity.order_type_new != order_type_new)
                    {
                        #region 执行更新语句
                        excuteSql = @"update dm_order set order_status=@order_status,product_num=@product_num,product_price=@product_price,payment_price=@payment_price,estimated_effect=@estimated_effect
,income_ratio=@income_ratio,estimated_income=@estimated_income,commission_rate=@commission_rate,commission_amount=@commission_amount,subsidy_ratio=@subsidy_ratio,subsidy_amount=@subsidy_amount,subsidy_type=@subsidy_type,
order_createtime=@order_createtime,order_settlement_at=@order_settlement_at,order_pay_time=@order_pay_time,order_group_success_time=@order_group_success_time,
protection_status=@protection_status,order_type_new=@order_type_new,order_receive_date=@order_receive_date,order_receive_month=@order_receive_month where id=@id";
                        #endregion

                        #region 更新语句参数
                        ps = new MySqlParameter[]
                        {
                           new MySqlParameter("@id",commonOrderEntity.id),
                           new MySqlParameter("@order_status",commonOrderEntity.order_status),
                           new MySqlParameter("@product_num",commonOrderEntity.product_num),
                           new MySqlParameter("@product_price",commonOrderEntity.product_price),
                           new MySqlParameter("@payment_price",commonOrderEntity.payment_price),
                           new MySqlParameter("@estimated_effect",commonOrderEntity.estimated_effect),
                           new MySqlParameter("@income_ratio",commonOrderEntity.income_ratio),
                           new MySqlParameter("@estimated_income",commonOrderEntity.estimated_income),
                           new MySqlParameter("@commission_rate",commonOrderEntity.commission_rate),
                           new MySqlParameter("@commission_amount",commonOrderEntity.commission_amount),
                           new MySqlParameter("@subsidy_ratio",commonOrderEntity.subsidy_ratio),
                           new MySqlParameter("@subsidy_amount",commonOrderEntity.subsidy_amount),
                           new MySqlParameter("@subsidy_type",commonOrderEntity.subsidy_type),
                           new MySqlParameter("@order_createtime",commonOrderEntity.order_createtime),
                           new MySqlParameter("@order_settlement_at",CheckTime(commonOrderEntity.order_settlement_at)),
                           new MySqlParameter("@order_pay_time",CheckTime(commonOrderEntity.order_pay_time)),
                           new MySqlParameter("@order_group_success_time",CheckTime(commonOrderEntity.order_group_success_time)),
                           new MySqlParameter("@protection_status",commonOrderEntity.protection_status),
                           new MySqlParameter("@order_type_new",commonOrderEntity.order_type_new),
                           new MySqlParameter("@order_receive_date",commonOrderEntity.order_receive_date),
                           new MySqlParameter("@order_receive_month",commonOrderEntity.order_receive_month),
                        };
                        #endregion
                    }
                }
                else
                {
                    LogHelper.WriteLog(commonOrderEntity.order_sn + "该订单存在多条记录!");
                    continue;
                }
                if (excuteSql != "")
                    MySqlHelperByH.ExecuteNonQuery(CommandType.Text, excuteSql, ps);

            }
            commonOrderList.Clear();
        }
        #endregion

        #region 时间转换
        int ConvertDate(string timeStr)
        {
            return string.IsNullOrWhiteSpace(timeStr) ? 0 : int.Parse(DateTime.Parse(timeStr).ToString("yyyyMMdd"));
        }

        int ConvertMonth(string timeStr)
        {
            return string.IsNullOrWhiteSpace(timeStr) ? 0 : int.Parse(DateTime.Parse(timeStr).ToString("yyyyMM"));
        }

        string CheckTime(string timeStr)
        {
            return string.IsNullOrWhiteSpace(timeStr) ? "2000-01-01 00:00:00" : timeStr;
        }
        #endregion
    }
}
