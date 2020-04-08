using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Learun.Application.Web.App_Start._01_Handler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Learun.Util;
using Learun.Application.TwoDevelopment.DM_APPManage;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class A_PayController : MvcAPIControllerBase
    {
        private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();

        #region 生成支付参数
        /// <summary>
        /// 生成支付参数
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="PackageType">套餐ID</param>
        /// <returns></returns>
        // GET: A_Pay
        public ActionResult GeneralPayParam(int user_id, int PackageType)
        {
            try
            {
                string appid = CheckAPPID();

                if (PackageType <= 0)
                {
                    return Fail("套餐信息异常!");
                }

                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);

                IAopClient client = new DefaultAopClient("https://openapi.alipaydev.com/gateway.do", dm_BasesettingEntity.alipay_appid, dm_BasesettingEntity.merchant_private_key, "json", "1.0", "RSA2", dm_BasesettingEntity.alipay_public_key, "GBK", false);
                AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
                request.SetNotifyUrl(dm_BasesettingEntity.alipay_notifyurl);
                request.SetReturnUrl(dm_BasesettingEntity.alipay_notifyurl);

                request.BizContent = "{" +
    "\"timeout_express\":\"90m\"," +
    "\"total_amount\":\"9.00\"," +
    "\"product_code\":\"QUICK_MSECURITY_PAY\"," +
    "\"body\":\"服务开通\"," +
    "\"subject\":\"高级代理\"," +
    "\"out_trade_no\":\"70501111111S001111119\"," +
    "\"time_expire\":\"2020-04-09 10:05\"," +
    "\"goods_type\":\"1\"," +
    //"\"promo_params\":\"{\\\"storeIdType\\\":\\\"1\\\"}\"," +
    "\"passback_params\":\"" + HttpUtility.UrlEncode("guzilideaoqi") + "\"," +
    "\"extend_params\":{" +
    "\"sys_service_provider_id\":\"2088511833207846\"," +
    "\"hb_fq_num\":\"3\"," +
    "\"hb_fq_seller_percent\":\"100\"," +
    "\"industry_reflux_info\":\"{\\\\\\\"scene_code\\\\\\\":\\\\\\\"metro_tradeorder\\\\\\\",\\\\\\\"channel\\\\\\\":\\\\\\\"xxxx\\\\\\\",\\\\\\\"scene_data\\\\\\\":{\\\\\\\"asset_name\\\\\\\":\\\\\\\"ALIPAY\\\\\\\"}}\"," +
    "\"card_type\":\"S0JP0000\"" +
    "    }," +
    "\"merchant_order_no\":\"20201008001\"," +
    "\"enable_pay_channels\":\"pcredit,moneyFund,debitCardExpress\"," +
    "\"store_id\":\"NJ_001\"," +
    "\"specified_channel\":\"pcredit\"," +
    "\"disable_pay_channels\":\"pcredit,moneyFund,debitCardExpress\"," +
    "      \"goods_detail\":[{" +
    "        \"goods_id\":\"apple-01\"," +
    "\"alipay_goods_id\":\"20010001\"," +
    "\"goods_name\":\"ipad\"," +
    "\"quantity\":1," +
    "\"price\":2000," +
    "\"goods_category\":\"34543238\"," +
    "\"categories_tree\":\"124868003|126232002|126252004\"," +
    "\"body\":\"开通代理\"," +
    "\"show_url\":\"http://www.alipay.com/xxx.jpg\"" +
    "        }]," +
    "\"ext_user_info\":{" +
    "\"name\":\"老王\"," +
    "\"mobile\":\"16587658765\"," +
    "\"cert_type\":\"IDENTITY_CARD\"," +
    "\"cert_no\":\"362334768769238881\"," +
    "\"min_age\":\"18\"," +
    "\"fix_buyer\":\"F\"," +
    "\"need_check_info\":\"F\"" +
    "    }," +
    "\"business_params\":\"{\\\"data\\\":\\\"123\\\"}\"," +
    "\"agreement_sign_params\":{" +
    "\"personal_product_code\":\"CYCLE_PAY_AUTH_P\"," +
    "\"sign_scene\":\"INDUSTRY|DIGITAL_MEDIA\"," +
    "\"external_agreement_no\":\"test20190701\"," +
    "\"external_logon_id\":\"13852852877\"," +
    "\"access_params\":{" +
    "\"channel\":\"ALIPAYAPP\"" +
    "      }," +
    "\"sub_merchant\":{" +
    "\"sub_merchant_id\":\"2088123412341234\"," +
    "\"sub_merchant_name\":\"哆来米\"," +
    "\"sub_merchant_service_name\":\"能省钱更能赚钱\"," +
    "\"sub_merchant_service_description\":\"副业才有未来\"" +
    "      }," +
    "\"period_rule_params\":{" +
    "\"period_type\":\"DAY\"," +
    "\"period\":3," +
    "\"execute_time\":\"2019-01-23\"," +
    "\"single_amount\":10.99," +
    "\"total_amount\":600," +
    "\"total_payments\":12" +
    "      }" +
    "    }" +
    "  }";

                AlipayTradeAppPayResponse response = client.SdkExecute(request);

                return Success("支付参数获取成功!", new { PayParam = response.Body });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 支付结果回调
        public ActionResult PayCallBack()
        {
            try
            {
                int i = 0;
                IDictionary<string, string> sArray = new Dictionary<string, string>();
                NameValueCollection coll;
                //Load Form variables into NameValueCollection variable.
                coll = Request.Form;

                // Get names of all forms into a string array.
                String[] requestItem = coll.AllKeys;

                for (i = 0; i < requestItem.Length; i++)
                {
                    sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
                }

                Logger.Info(JsonConvert.SerializeObject(sArray));
                return Success("成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        public string CheckAPPID()
        {
            if (base.Request.Headers["appid"].IsEmpty())
            {
                throw new Exception("缺少参数appid");
            }
            return base.Request.Headers["appid"].ToString();
        }
    }
}