using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Learun.Application.TwoDevelopment.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-14 17:35
    /// 描 述：提现申请记录
    /// </summary>
    public class DM_Apply_CashRecordController : MvcControllerBase
    {
        private DM_Apply_CashRecordIBLL dM_Apply_CashRecordIBLL = new DM_Apply_CashRecordBLL();
        private DM_UserIBLL dM_UserIBLL = new DM_UserBLL();
        private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();

        #region 视图功能

        /// <summary>
        /// 主页面
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetList(string queryJson)
        {
            var data = dM_Apply_CashRecordIBLL.GetList(queryJson);
            return Success(data);
        }
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = dM_Apply_CashRecordIBLL.GetPageList(paginationobj, queryJson);
            var jsonData = new
            {
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageListByDataTable(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = dM_Apply_CashRecordIBLL.GetPageListByDataTable(paginationobj, queryJson);
            var jsonData = new
            {
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }

        /// <summary>
        /// 获取表单数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormData(int keyValue)
        {
            var data = dM_Apply_CashRecordIBLL.GetEntity(keyValue);
            return Success(data);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DeleteForm(int keyValue)
        {
            dM_Apply_CashRecordIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }
        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(int keyValue, dm_apply_cashrecordEntity entity)
        {
            dM_Apply_CashRecordIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }
        #endregion

        #region 审核提现记录
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckApplyCashRecord(int id)
        {
            dM_Apply_CashRecordIBLL.CheckApplyCashRecord(id, 1);
            return Success("打款成功！");
        }
        #endregion

        #region 自动打款
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckApplyCashRecordByAli(int id)
        {
            try
            {
                UserInfo userInfo = LoginUserInfo.Get();
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntity(userInfo.companyId);

                dm_apply_cashrecordEntity jlm_Xcx_Apply_CashrecordEntity = dM_Apply_CashRecordIBLL.GetEntity(id);

                if (jlm_Xcx_Apply_CashrecordEntity.IsEmpty())
                    throw new Exception("未找到提现记录!");

                if (jlm_Xcx_Apply_CashrecordEntity.status != 0)
                    throw new Exception("当前提现记录不可操作!");

                dm_userEntity dm_UserEntity = dM_UserIBLL.GetEntity(jlm_Xcx_Apply_CashrecordEntity.user_id);
                if (dm_UserEntity.IsEmpty())
                    throw new Exception("未检测到用户信息!");
                if (dm_UserEntity.realname.IsEmpty())
                    throw new Exception("该用户未实名!");
                if (dm_UserEntity.zfb.IsEmpty())
                    throw new Exception("该用户未绑定支付宝账号!");


                IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", dm_BasesettingEntity.alipay_appid, dm_BasesettingEntity.merchant_private_key, "json", "1.0", "RSA2", "utf-8", false, AliPayHelper.GetCertParams(dm_BasesettingEntity.alipay_appid, base.Server));
                AlipayFundTransUniTransferRequest request = new AlipayFundTransUniTransferRequest();
                request.BizContent = "{" +
                "\"out_biz_no\":\"" + Guid.NewGuid().ToString() + "\"," +
                "\"trans_amount\":" + jlm_Xcx_Apply_CashrecordEntity.price + "," +
                "\"product_code\":\"TRANS_ACCOUNT_NO_PWD\"," +
                "\"biz_scene\":\"DIRECT_TRANSFER\"," +
                "\"order_title\":\"哆来米\"," +
                "\"original_order_id\":\"\"," +
                "\"payee_info\":{" +
                "\"identity\":\"" + dm_UserEntity.zfb + "\"," +
                "\"identity_type\":\"ALIPAY_LOGON_ID\"," +
                "\"name\":\"" + dm_UserEntity.realname + "\"" +
                "    }," +
                "\"remark\":\"余额提现\"," +
                "\"business_params\":\"{\\\"sub_biz_scene\\\":\\\"REDPACKET\\\"}\"" +
                "  }";
                AlipayFundTransUniTransferResponse response = client.CertificateExecute(request);
                if (response.IsError)
                    throw new Exception(response.Msg + "=>" + response.SubMsg);

                jlm_Xcx_Apply_CashrecordEntity.paytype = 2;
                jlm_Xcx_Apply_CashrecordEntity.status = 1;
                jlm_Xcx_Apply_CashrecordEntity.OrderId = response.OrderId;
                jlm_Xcx_Apply_CashrecordEntity.PayFundOrderId = response.PayFundOrderId;
                jlm_Xcx_Apply_CashrecordEntity.TransDate = DateTime.Parse(response.TransDate);
                jlm_Xcx_Apply_CashrecordEntity.checktime = DateTime.Now;

                dM_Apply_CashRecordIBLL.CheckApplyCashRecordByAli(jlm_Xcx_Apply_CashrecordEntity);

                return Success("打款成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion
    }
}
