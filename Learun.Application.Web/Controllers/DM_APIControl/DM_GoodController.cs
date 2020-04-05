using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    /// <summary>
    /// dm_data商品类API
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创建人：胡亚广
    /// 日 期：2020.03.13
    /// 描 述：
    /// </summary>
    public class DM_GoodController : MvcAPIControllerBase
    {
        #region 构造数据访问类 
        private DM_IntergralChangeGoodIBLL dM_IntergralChangeGoodIBLL = new DM_IntergralChangeGoodBLL();
        private DM_IntergralChangeRecordIBLL dM_IntergralChangeRecordIBLL = new DM_IntergralChangeRecordBLL();
        private DM_UserIBLL dm_userIBLL = new DM_UserBLL();
        #endregion

        #region 获取商品类别
        public ActionResult GetGoodType() {
            return Content("男装，女装");
        }
        #endregion

        #region 获取淘宝商品(根据用户等级计算佣金)
        // GET: DM_Good
        /// <summary>
        /// 获取淘宝商品
        /// </summary>
        public ActionResult Get_TB_GoodList() {
            return View();
        }
        #endregion

        #region 淘宝商品转链
        public ActionResult ConvertLinkByTB() {
            return View();
        }
        #endregion

        #region 同步淘宝订单
        public ActionResult Get_TB_Order() {
            return View();
        }
        #endregion

        #region 获取京东商品
        public ActionResult Get_JD_GoodList() {
            return View();
        }
        #endregion

        #region 京东商品转链
        public ActionResult ConvertLinkByJD() {
            return View();
        }
        #endregion

        #region 同步京东订单
        public ActionResult Get_JD_Order() {
            return View();
        }
        #endregion

        #region 获取拼多多商品
        public ActionResult Get_PDD_GoodList() {
            return View();
        }
        #endregion

        #region 拼多多商品转链
        public ActionResult ConvertLinkByPDD() {
            return View();
        }
        #endregion

        #region 同步拼多多订单
        public ActionResult Get_PDD_Order() {
            return View();
        }
        #endregion

        #region 获取积分兑换商品
        public ActionResult GetIntergralChangeGood(int PageNo,int PageSize) {
            try
            {
                string appid = CheckAPPID();

                return Success("获取成功", dM_IntergralChangeGoodIBLL.GetPageListByCache(new Pagination { page = PageNo, rows = PageSize }, appid));
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
        #endregion

        #region 申请兑换商品
        public ActionResult ApplyChangeGood(dm_intergralchangerecordEntity dm_IntergralchangerecordEntity) {
            try
            {
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(dm_IntergralchangerecordEntity.user_id.ToInt());
                if (dm_UserEntity == null)
                    return Fail("用户信息异常!");
                dM_IntergralChangeRecordIBLL.ApplyChangeGood(dm_IntergralchangerecordEntity, dm_UserEntity);

                return Success("商品兑换成功,我们将会在7个工作日内发货!");
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 检测头部是否包含APPID
        public string CheckAPPID()
        {
            if (Request.Headers["appid"].IsEmpty())
                throw new Exception("缺少参数appid");
            else
            {
                return Request.Headers["appid"].ToString();
            }
        }
        #endregion
    }
}