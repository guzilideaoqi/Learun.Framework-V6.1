/*===================================Copyright© 2021 xxx Ltd. All rights reserved.==============================

文件类名 : AliPayHelper.cs
创建人员 : Mr.Hu
创建时间 : 2021-01-29 11:26:53 
备注说明 : 

 =====================================End=======================================================*/
using Aop.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Learun.Application.TwoDevelopment.Common
{
    /// <summary>
    /// AliPayHelper
    /// </summary>
    public class AliPayHelper
    {
        public static CertParams GetCertParams(string appid, HttpServerUtilityBase httpServerUtilityBase)
        {
            string alipayCertPublicKey_RSA2 = httpServerUtilityBase.MapPath("~" + $"/Cert/alipayCertPublicKey_RSA2.crt");
            string alipayRootCert = httpServerUtilityBase.MapPath("~" + $"/Cert/alipayRootCert.crt");
            string appCertPublicKey = httpServerUtilityBase.MapPath("~" + $"/Cert/appCertPublicKey_" + appid + ".crt");

            CertParams certParams = new CertParams
            {
                AlipayPublicCertPath = alipayCertPublicKey_RSA2,
                AppCertPath = appCertPublicKey,
                RootCertPath = alipayRootCert
            };

            return certParams;
        }
    }
}
