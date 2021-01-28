using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderService.Model
{
    public class BaseSettingEntity
    {
        public long tb_accountid { get; set; }
        public long tb_appkey { get; set; }
        public string tb_appsecret { get; set; }
        public string tb_sessionkey { get; set; }
        public string tb_authorendtime { get; set; }
        public long jd_accountid { get; set; }
        public string jd_appkey { get; set; }
        public string jd_appsecret { get; set; }
        public string jd_sessionkey { get; set; }
        public long pdd_accountid { get; set; }
        public string pdd_clientid { get; set; }
        public string pdd_clientsecret { get; set; }
    }

    public class BaseSettingReponse {
        public int code { get; set; }
        public string info { get; set; }
        public BaseSettingEntity data { get; set; }
    }
}
