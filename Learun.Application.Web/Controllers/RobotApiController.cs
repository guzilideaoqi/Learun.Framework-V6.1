using HYG.CommonHelper.JDModel;
using HYG.CommonHelper.PDDModel;
using HYG.CommonHelper.ShoppingAPI;
using JDModel;
using Learun.Application.Organization;
using Learun.Application.TwoDevelopment.Hyg_RobotModule;
using Learun.Util;
using PDDModel;
using ShoppingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBModel;

namespace Learun.Application.Web.Controllers
{
    [HandlerLogin(FilterMode.Ignore)]
    public class RobotApiController : MvcControllerBase
    {
        #region 模块对象
        private string tb_appkey = "25552805", tb_appsecret = "7341a330d97862f21447f34c0fc326c9";
        private UserIBLL userBll = new UserBLL();
        /// <summary>
        /// 应用商信息
        /// </summary>
        private AgentManageIBLL agentManageBLL = new AgentManageBLL();
        /// <summary>
        /// 应用商设置
        /// </summary>
        private Application_SettingIBLL application_SettingIBLL = new Application_SettingBLL();
        #endregion

        #region 机器人客户端登录
        /// <summary>
        /// 客户端登录
        /// </summary>
        /// <param name="Account">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <returns></returns>
        public ActionResult Login(string Account, string PassWord)
        {
            try
            {
                s_data_agentEntity s_Data_AgentEntity = agentManageBLL.Login(Account, PassWord);
                if (s_Data_AgentEntity.IsEmpty())
                    throw new Exception("用户名或密码错误!");
                if(s_Data_AgentEntity.F_AllowEndTime<DateTime.Now)
                    throw new Exception("该账户授权已过期!");
                s_application_settingEntity s_Application_SettingEntity= application_SettingIBLL.GetEntityByApplicationId(s_Data_AgentEntity.F_ApplicationId);
                if (s_Application_SettingEntity.IsEmpty())
                    throw new Exception("该用户已废弃!");
                dynamic dy = new
                {
                    Account_Info = s_Data_AgentEntity,//机器人账户信息
                    Agent_Setting = s_Application_SettingEntity
                };

                return Success("登录成功!", dy);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
        #endregion

        #region 获取所有服务商列表
        /// <summary>
        /// 获取所有服务商列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetApplicationList() {
            try
            {
                return Success("获取成功!", application_SettingIBLL.GetList(""));
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
        #endregion

        #region 淘宝模块
        /// <summary>
        /// 淘宝转链接口
        /// </summary>
        /// <param name="AppID">应用商ID</param>
        /// <param name="ItemID">商品ID</param>
        /// <param name="PID">PID</param>
        /// <param name="RelationID">渠道ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConvertLinkByTB(string AppID, string ItemID, string PID, string RelationID)
        {
            try
            {
                s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(AppID);
                TBApi tBApi = new TBApi(tb_appkey, tb_appsecret, s_Application_SettingEntity.F_TB_SessionKey);
                string resultContent = tBApi.ConvertLink(ItemID, PID, RelationID);
                return Success("转链成功!", resultContent);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 同步淘宝订单
        /// </summary>
        /// <param name="AppID">应用商ID</param>
        /// <param name="QueryType">1创建时间  2订单付款时间  3订单结算时间</param>
        /// <param name="Position_Index">除第一页之外，其他都要传</param>
        /// <param name="StartTime">订单查询开始时间</param>
        /// <param name="EndTime">订单查询结束时间</param>
        /// <param name="Jump_Type">向前或向后翻页  -1向前  1向后</param>
        /// <param name="PageNo">页码</param>
        /// <param name="PageSize">每页显示数量</param>
        /// <param name="TK_Status">订单状态 12-付款  13-关闭  14-确认收货  3-结算成功  不传表示所有状态</param>
        /// <param name="Order_Scene">订单类型  1常规订单  2渠道订单  3会员运营订单</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Sync_TB_Order(string AppID, string QueryType, string Position_Index, string StartTime, string EndTime, string Jump_Type = "1", int PageNo = 1, int PageSize = 20, string TK_Status = "", string Order_Scene = "1")
        {
            try
            {
                s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(AppID);
                TBApi tBApi = new TBApi(tb_appkey, tb_appsecret, s_Application_SettingEntity.F_TB_SessionKey);
                OrderData resultContent = tBApi.GetOrder(QueryType, Position_Index, PageSize.ToString(), TK_Status, StartTime, EndTime, Jump_Type, PageNo.ToString(), Order_Scene);

                return Success("订单获取成功!", resultContent);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 查询淘宝商品
        /// </summary>
        /// <param name="AppID">应用商ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search_TB_Good(string AppID)
        {
            try
            {
                s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(AppID);
                TBApi tBApi = new TBApi(tb_appkey, tb_appsecret, s_Application_SettingEntity.F_TB_SessionKey);
                string resultContent = tBApi.SearchGood();

                return Success("商品查询成功!", resultContent);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
        #endregion

        #region 京东模块
        /// <summary>
        /// 京东转链
        /// </summary>
        /// <param name="AppID">应用商ID</param>
        /// <param name="ItemID">商品ID</param>
        /// <param name="SiteID">网站/媒体ID</param>
        /// <param name="PositionID">推广位</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConvertLinkByJD(string AppID, string ItemID, string SiteID, string PositionID)
        {
            try
            {
                s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(AppID);
                JDApi jDApi = new JDApi(s_Application_SettingEntity.F_JD_AppKey, s_Application_SettingEntity.F_JD_Secret, s_Application_SettingEntity.F_JD_SessionKey);
                return Success("转链成功!", jDApi.ConvertUrl(ItemID, SiteID, PositionID));
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 同步京东订单
        /// </summary>
        /// <param name="AppID">应用商ID</param>
        /// <param name="PageNo">当前页码</param>
        /// <param name="PageSize">每页显示的数量</param>
        /// <param name="Type">1下单时间  2完成时间  3更新时间</param>
        /// <param name="Time">查询时间</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Sync_JD_Order(string AppID, int OrderType, string UpdateTime, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(AppID);
                JDApi jDApi = new JDApi(s_Application_SettingEntity.F_JD_AppKey, s_Application_SettingEntity.F_JD_Secret, s_Application_SettingEntity.F_JD_SessionKey);
                JDOrder jDOrder = jDApi.GetOrder(PageNo, PageSize, OrderType, UpdateTime);

                return Success("订单获取成功!", jDOrder);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 京东商品查询
        /// </summary>
        /// <param name="AppID">应用商ID</param>
        /// <param name="EliteID">频道id：1-好券商品,2-超级大卖场,10-9.9专区,22-热销爆品,24-数码家电,25-超市,26-母婴玩具,27-家具日用,28-美妆穿搭,29-医药保健,30-图书文具,31-今日必推,32-王牌好货,33-秒杀商品,34-拼购商品</param>
        /// <param name="PageNo">页码</param>
        /// <param name="PageSize">每页显示的数量</param>
        /// <param name="SortName">排序的字段</param>
        /// <param name="Sort">排序的方式</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search_JD_Good(string AppID, int EliteID, int PageNo = 1, int PageSize = 20, string SortName = "price", string Sort = "desc")
        {
            try
            {
                s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(AppID);
                JDApi jDApi = new JDApi(s_Application_SettingEntity.F_JD_AppKey, s_Application_SettingEntity.F_JD_Secret, s_Application_SettingEntity.F_JD_SessionKey);
                JFGoodsResp[] jDGoods = jDApi.GetGoodList(EliteID, PageNo, PageSize, SortName, Sort);

                return Success("商品查询成功!", jDGoods);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
        #endregion

        #region 拼多多模块
        /// <summary>
        /// 拼多多商品转链
        /// </summary>
        /// <param name="AppID">应用商ID</param>
        /// <param name="ItemID">商品ID</param>
        /// <param name="PID">联盟PID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConvertLinkByPDD(string AppID, string ItemID, string PID)
        {
            try
            {
                s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(AppID);
                PDDApi pDDApi = new PDDApi(s_Application_SettingEntity.F_PDD_ClientID, s_Application_SettingEntity.F_PDD_ClientSecret, "");
                return Success("转链成功!", pDDApi.GeneralUrl(ItemID, PID));
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 同步拼多多订单
        /// </summary>
        /// <param name="AppID">应用商ID</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="PageNo">页码</param>
        /// <param name="PageSize">每页显示的数量</param>
        /// <param name="ReturnCount">是否返回订单总数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Sync_PDD_Order(string AppID, string StartTime, string EndTime, int PageNo = 1, int PageSize = 20, bool ReturnCount = true)
        {
            try
            {
                s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(AppID);
                PDDApi pDDApi = new PDDApi(s_Application_SettingEntity.F_PDD_ClientID, s_Application_SettingEntity.F_PDD_ClientSecret, "");

                List<pdd_order> pdd_Orders= pDDApi.GetOrderList(StartTime, EndTime, PageNo, PageSize, ReturnCount);
                return Success("订单获取成功!", pdd_Orders);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        /// <summary>
        /// 查询拼多多商品
        /// </summary>
        /// <param name="AppID">应用商ID</param>
        /// <param name="KeyWord">关键词</param>
        /// <param name="PageNo">页码</param>
        /// <param name="PageSize">每页显示数量</param>
        /// <param name="SortType">排序方式:0-综合排序;1-按佣金比率升序;2-按佣金比例降序;3-按价格升序;4-按价格降序;5-按销量升序;6-按销量降序;7-优惠券金额排序升序;8-优惠券金额排序降序;9-券后价升序排序;10-券后价降序排序;11-按照加入多多进宝时间升序;12-按照加入多多进宝时间降序;13-按佣金金额升序排序;14-按佣金金额降序排序;15-店铺描述评分升序;16-店铺描述评分降序;17-店铺物流评分升序;18-店铺物流评分降序;19-店铺服务评分升序;20-店铺服务评分降序;27-描述评分击败同类店铺百分比升序，28-描述评分击败同类店铺百分比降序，29-物流评分击败同类店铺百分比升序，30-物流评分击败同类店铺百分比降序，31-服务评分击败同类店铺百分比升序，32-服务评分击败同类店铺百分比降序</param>
        /// <param name="WithCoupon">是否只返回优惠券的商品，false返回所有商品，true只返回有优惠券的商品</param>
        /// <returns></returns>
        public ActionResult Search_PDD_Good(string AppID,string KeyWord, int PageNo = 1, int PageSize = 20,int SortType=0,bool WithCoupon=false) {
            try
            {
                s_application_settingEntity s_Application_SettingEntity = application_SettingIBLL.GetEntityByApplicationId(AppID);
                PDDApi pDDApi = new PDDApi(s_Application_SettingEntity.F_PDD_ClientID, s_Application_SettingEntity.F_PDD_ClientSecret, "");

                List<GoodItem> goodItems = pDDApi.SearchGood(KeyWord, PageNo, PageSize, SortType, WithCoupon);
                return Success("商品获取成功!", goodItems);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
        #endregion

        #region 绑定订单
        [HttpPost]
        public ActionResult BindOrder(string AppID, string OrderNo)
        {
            return Success("绑定成功!", "");
        }
        #endregion
    }
}