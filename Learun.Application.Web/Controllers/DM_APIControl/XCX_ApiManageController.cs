using Hyg.Common.OtherTools;
using Learun.Application.Web.App_Start._01_Handler;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class XCX_ApiManageController : MvcAPIControllerBase
    {
        /*
         * 小程序模块接口管理
         */

        #region 小程序相关的配置信息
        public const string _appid = "wx11c6e38e612db994";//appid
        public const string appsecret = "3d2654d8c9b021a1223136ff31130255";//小程序密钥
        private const string _mch_id = "1607146586";//商户号
        private const string _key = "893e48b232e0443ca4686fa63a3610ac";//商户平台设置的密钥key
        #endregion


        #region 支付回调地址
        [NoNeedLogin]
        public ActionResult Pay_CallBack()
        {
            return Success("回调成功!");
        }
        #endregion

        #region 生成小程序支付参数
        [NoNeedLogin]
        public ActionResult General_Pay_Param(string openid = "of_-85QWeJbeqAkODjpVlNuM7XQs", decimal amount = 0.01M)
        {
            try
            {
                string resultContent = GetAll(openid, amount);

                //              string param = "{\"time_expire\": \"" + DateTime.Now.AddMinutes(10).ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                //  "\"amount\": {\"total\": 100,\"currency\": \"CNY\"}," +
                //  "\"mchid\": \""+ _mch_id + "\"," +
                //  "\"description\": \"Image形象店-深圳腾大-QQ公仔\"," +
                //  "\"notify_url\": \"https://www.weixin.qq.com/wxpay/pay.php\"," +
                //  "\"payer\": {" +
                //                  "\"openid\": \"oUpF8uMuAJO_M2pxb1Q9zNjWeS6o\"" +
                //  "}," +
                //  "\"out_trade_no\": \"1217752501201407033233368018\"," +
                //  "\"goods_tag\": \"WXG\"," +
                //  "\"appid\": \""+_appid+"\"," +
                //  "\"attach\": \"自定义数据说明\"," +
                //  "\"detail\": {" +
                //                  "\"invoice_id\": \"wx123\"," +
                //      "\"goods_detail\": [{" +
                //          "\"goods_name\": \"iPhoneX 256G\"," +
                //          "\"wechatpay_goods_id\": \"1001\"," +
                //          "\"quantity\": 1," +
                //          "\"merchant_goods_id\": \"商品编码\"," +
                //          "\"unit_price\": 828800" +
                //      "}, {" +
                //          "\"goods_name\": \"iPhoneX 256G\"," +
                //          "\"wechatpay_goods_id\": \"1001\"," +
                //          "\"quantity\": 1," +
                //          "\"merchant_goods_id\": \"商品编码\"," +
                //          "\"unit_price\": 828800" +
                //      "}]," +
                //      "\"cost_price\": 608800" +
                //  "}," +
                //  "\"scene_info\": {" +
                //      "\"store_info\": {" +
                //          "\"address\": \"广东省深圳市南山区科技中一道10000号\"," +
                //          "\"area_code\": \"440305\"," +
                //          "\"name\": \"腾讯大厦分店\"," +
                //          "\"id\": \"0001\"" +
                //      "}," +
                //      "\"device_id\": \"013467007045764\"," +
                //      "\"payer_client_ip\": \"14.23.150.211\"" +
                //  "}," +
                //"\"settle_info\": {" +
                //  "\"profit_sharing\": false" +
                //"}}";
                //              string resultContent = Hyg.Common.OtherTools.AjaxRequest.HttpPost("https://api.mch.weixin.qq.com/v3/pay/transactions/jsapi", param);

                return Success("获取成功", resultContent);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        /// <summary>
        /// 模拟wx统一下单
        /// </summary>
        /// <param name="openid">前台获取用户标识</param>
        /// <returns></returns>
        public string GetAll(string openid, decimal amount)
        {
            if (openid == null)
            {
                return "";
            }
            return Getprepay_id(_appid, "支付测试", "JSAPI支付测试", _mch_id, GetRandomString(30), "http://www.weixin.qq.com/wxpay/pay.php", openid, getRandomTime(), Convert.ToInt32(amount * 100));
        }

        //微信统一下单获取prepay_id & 再次签名返回数据
        private static string Getprepay_id(string appid, string attach, string body, string mch_id, string nonce_str, string notify_url, string openid, string bookingNo, int total_fee)
        {
            var url = "https://api.mch.weixin.qq.com/pay/unifiedorder";//微信统一下单请求地址
            string strA = "appid=" + appid + "&attach=" + attach + "&body=" + body + "&mch_id=" + mch_id + "&nonce_str=" + nonce_str + "&notify_url=" + notify_url + "&openid=" + openid + "&out_trade_no=" + bookingNo + "&spbill_create_ip=61.50.221.43&total_fee=" + total_fee + "&trade_type=JSAPI";
            string strk = strA + "&key=" + _key;  //key为商户平台设置的密钥key(假)
            string strMD5 = Md5Helper.Md5(strk).ToUpper();//MD5签名

            //签名
            var formData = "<xml>";
            formData += "<appid>" + appid + "</appid>";//appid  
            formData += "<attach>" + attach + "</attach>"; //附加数据(描述)
            formData += "<body>" + body + "</body>";//商品描述
            formData += "<mch_id>" + mch_id + "</mch_id>";//商户号  
            formData += "<nonce_str>" + nonce_str + "</nonce_str>";//随机字符串，不长于32位。  
            formData += "<notify_url>" + notify_url + "</notify_url>";//通知地址
            formData += "<openid>" + openid + "</openid>";//openid
            formData += "<out_trade_no>" + bookingNo + "</out_trade_no>";//商户订单号
            formData += "<spbill_create_ip>61.50.221.43</spbill_create_ip>";//终端IP
            formData += "<total_fee>" + total_fee + "</total_fee>";//支付金额单位为（分）
            formData += "<trade_type>JSAPI</trade_type>";//交易类型(JSAPI--公众号支付)
            formData += "<sign>" + strMD5 + "</sign>"; //签名
            formData += "</xml>";

            //请求数据
            var getdata = AjaxRequest.HttpPost(url, formData);

            //获取xml数据
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(getdata);
            //xml格式转json
            string json = doc.ToJsonStr();

            JObject jo = json.ToJsonObject<JObject>();
            string prepay_id = jo["xml"]["prepay_id"]["#cdata-section"].ToString();

            //时间戳
            string _time = getTime().ToString();

            //再次签名返回数据至小程序
            string strB = "appId=" + appid + "&nonceStr=" + nonce_str + "&package=prepay_id=" + prepay_id + "&signType=MD5&timeStamp=" + _time + "&key=" + _key;

            //wx自己写的一个类
            PaymentEntity payment = new PaymentEntity();
            payment.timeStamp = _time;
            payment.nonceStr = nonce_str;
            payment.package = "prepay_id=" + prepay_id;
            payment.paySign = Md5Helper.Md5(strB).ToUpper();
            payment.signType = "MD5";

            //向小程序返回json数据
            return payment.ToJsonStr();
        }

        /// <summary>
        /// 生成随机串    
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        private static string GetRandomString(int length)
        {
            const string key = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            if (length < 1)
                return string.Empty;

            Random rnd = new Random();
            byte[] buffer = new byte[8];

            ulong bit = 31;
            ulong result = 0;
            int index = 0;
            StringBuilder sb = new StringBuilder((length / 5 + 1) * 5);

            while (sb.Length < length)
            {
                rnd.NextBytes(buffer);

                buffer[5] = buffer[6] = buffer[7] = 0x00;
                result = BitConverter.ToUInt64(buffer, 0);

                while (result > 0 && sb.Length < length)
                {
                    index = (int)(bit & result);
                    sb.Append(key[index]);
                    result = result >> 5;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        private static string getRandomTime()
        {
            Random rd = new Random();//用于生成随机数
            string DateStr = DateTime.Now.ToString("yyyyMMddHHmmssMM");//日期
            string str = DateStr + rd.Next(10000).ToString().PadLeft(4, '0');//带日期的随机数
            return str;
        }

        /// <summary>
        /// wx统一下单请求数据
        /// </summary>
        /// <param name="URL">请求地址</param>
        /// <param name="urlArgs">参数</param>
        /// <returns></returns>
        //private static string sendPost(string URL, string urlArgs)
        //{

        //    WebClient wCient = new WebClient();
        //    wCient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        //    byte[] postData = System.Text.Encoding.UTF8.GetBytes(urlArgs);
        //    byte[] responseData = wCient.UploadData(URL, "POST", postData);

        //    string returnStr = System.Text.Encoding.UTF8.GetString(responseData);//返回接受的数据 
        //    return returnStr;
        //}

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private static long getTime()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        #endregion

        #region 获取ACCESS_TOKEN
        public string GetAccessToken()
        {
            string JsCode2SessionUrl = string.Format(@"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", _appid, appsecret);
            JObject jObject = AjaxRequest.HttpGet(JsCode2SessionUrl, "").ToJsonObject<JObject>();
            if (!jObject["access_token"].IsEmpty())
            {
                return jObject["access_token"].ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion
    }

    public class PaymentEntity
    {
        public string timeStamp { get; set; }

        public string nonceStr { get; set; }

        public string package { get; set; }

        public string paySign { get; set; }

        public string signType { get; set; }
    }
}