using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class dm_basesettingEntity
    {
        [Column("APPID")]
        public string appid
        {
            get;
            set;
        }

        [Column("TB_ACCOUNTID")]
        public int? tb_accountid
        {
            get;
            set;
        }

        [Column("TB_APPKEY")]
        public string tb_appkey
        {
            get;
            set;
        }

        [Column("TB_APPSECRET")]
        public string tb_appsecret
        {
            get;
            set;
        }

        [Column("TB_SESSIONKEY")]
        public string tb_sessionkey
        {
            get;
            set;
        }

        [Column("TB_RELATION_PID")]
        public string tb_relation_pid
        {
            get;
            set;
        }

        [Column("TB_AUTHORENDTIME")]
        public DateTime? tb_authorendtime
        {
            get;
            set;
        }

        /// <summary>
        /// �Ա�����������
        /// </summary>
        [Column("TB_RELATION_INVITECODE")]
        public string tb_relation_invitecode { get; set; }

        [Column("JD_ACCOUNTID")]
        public int? jd_accountid
        {
            get;
            set;
        }

        [Column("JD_APPKEY")]
        public string jd_appkey
        {
            get;
            set;
        }

        [Column("JD_APPSECRET")]
        public string jd_appsecret
        {
            get;
            set;
        }

        [Column("JD_SESSIONKEY")]
        public string jd_sessionkey
        {
            get;
            set;
        }

        [Column("PDD_ACCOUNTID")]
        public int? pdd_accountid
        {
            get;
            set;
        }

        [Column("PDD_CLIENTID")]
        public string pdd_clientid
        {
            get;
            set;
        }

        [Column("PDD_CLIENTSECRET")]
        public string pdd_clientsecret
        {
            get;
            set;
        }

        [Column("OPENAGENT_ONE")]
        public int openagent_one
        {
            get;
            set;
        }

        [Column("OPENAGENT_TWO")]
        public int openagent_two
        {
            get;
            set;
        }

        [Column("OPENAGENT_ONE_PARTNERS")]
        public int openagent_one_partners
        {
            get;
            set;
        }

        [Column("OPENAGENT_TWO_PARTNERS")]
        public int openagent_two_partners
        {
            get;
            set;
        }

        /// <summary>
        /// ������Ӷ��(�߼�)
        /// </summary>
		[Column("TASK_DO_SENIOR")]
        public int task_do_senior
        {
            get;
            set;
        }

        /// <summary>
        /// ������Ӷ��(����)
        /// </summary>
        [Column("TASK_DO_JUNIOR")]
        public int task_do_junior { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        [Column("TASK_SERVICEFEE")]
        public int task_servicefee { get; set; }

        /// <summary>
        /// ������һ������Ӷ��
        /// </summary>
		[Column("TASK_ONE")]
        public int task_one
        {
            get;
            set;
        }
        /// <summary>
        /// �������������Ӷ��
        /// </summary>
		[Column("TASK_TWO")]
        public int task_two
        {
            get;
            set;
        }
        /// <summary>
        /// ������һ���ϻ���Ӷ��
        /// </summary>
		[Column("TASK_ONE_PARTNERS")]
        public int task_one_partners
        {
            get;
            set;
        }
        /// <summary>
        /// ����������ϻ���Ӷ��
        /// </summary>
		[Column("TASK_TWO_PARTNERS")]
        public int task_two_partners
        {
            get;
            set;
        }

        [Column("SHOPPING_PAY_JUNIOR")]
        public int shopping_pay_junior
        {
            get;
            set;
        }

        [Column("SHOPPING_PAY_MIDDLE")]
        public int shopping_pay_middle
        {
            get;
            set;
        }

        [Column("SHOPPING_PAY_SENIOR")]
        public int shopping_pay_senior
        {
            get;
            set;
        }

        [Column("SHOPPING_ONE")]
        public int shopping_one
        {
            get;
            set;
        }

        [Column("SHOPPING_TWO")]
        public int shopping_two
        {
            get;
            set;
        }

        [Column("SHOPPING_ONE_PARTNERS")]
        public int shopping_one_partners
        {
            get;
            set;
        }

        [Column("SHOPPING_TWO_PARTNERS")]
        public int shopping_two_partners
        {
            get;
            set;
        }

        [Column("FIRSTSIGN")]
        public int? firstsign
        {
            get;
            set;
        }

        [Column("SIGNSCREMENT")]
        public int? signscrement
        {
            get;
            set;
        }

        [Column("SIGNCAPPING")]
        public int? signcapping
        {
            get;
            set;
        }

        [Column("READTASK_MIN")]
        public int? readtask_min
        {
            get;
            set;
        }

        [Column("READTASK_MAX")]
        public int? readtask_max
        {
            get;
            set;
        }

        [Column("NEW_PEOPLE")]
        public int? new_people
        {
            get;
            set;
        }

        [Column("NEW_PEOPLE_PARENT")]
        public int? new_people_parent
        {
            get;
            set;
        }

        [Column("DTK_APPKEY")]
        public string dtk_appkey
        {
            get;
            set;
        }

        [Column("DTK_APPSECRET")]
        public string dtk_appsecret
        {
            get;
            set;
        }

        [Column("QIANZHUI_IMAGE")]
        public string qianzhui_image
        {
            get;
            set;
        }

        /// <summary>
        /// ��ǰ�ϼܰ汾��(IOS)
        /// </summary>
        [Column("PREVIEWVERSION")]
        public string previewversion
        {
            get;
            set;
        }

        /// <summary>
        /// ��ǰ�ϼܰ汾��(��׿)
        /// </summary>
        [Column("PREVIEWVERSIONANDROID")]
        public string previewversionandroid { get; set; }

        /// <summary>
        /// ����˽Կ
        /// </summary>
        [Column("MERCHANT_PRIVATE_KEY")]
        public string merchant_private_key { get; set; }

        /// <summary>
        /// ֧������Կ
        /// </summary>
        [Column("ALIPAY_PUBLIC_KEY")]
        public string alipay_public_key { get; set; }

        /// <summary>
        /// ֧����APPID
        /// </summary>
        [Column("ALIPAY_APPID")]
        public string alipay_appid { get; set; }

        /// <summary>
        /// ֧���ص���ַ
        /// </summary>
        [Column("ALIPAY_NOTIFYURL")]
        public string alipay_notifyurl { get; set; }

        /// <summary>
        /// ����APPKEY
        /// </summary>
        [Column("JG_APPKEY")]
        public string jg_appkey { get; set; }

        /// <summary>
        /// ����APPSECRET
        /// </summary>
        [Column("JG_APPSECRET")]
        public string jg_appsecret { get; set; }

        /// <summary>
        /// ����ǩ��ID
        /// </summary>
        [Column("SMS_SIGN_ID")]
        public string sms_sign_id { get; set; }

        /// <summary>
        /// ����ģ��ID
        /// </summary>
        [Column("SMS_TEMPLATE_ID")]
        public string sms_template_id { get; set; }

        /// <summary>
        /// ����������������
        /// </summary>
        [Column("REVICETASKCOUNTLIMIT")]
        public int revicetaskcountlimit { get; set; }

        /// <summary>
        /// OSS����ID
        /// </summary>
        [Column("OSS_ACCESSKEYID")]
        public string oss_accesskeyid { get; set; }

        /// <summary>
        /// OSS����Secret
        /// </summary>
        [Column("OSS_ACCESSKEYSECRET")]
        public string oss_accesskeysecret { get; set; }

        /// <summary>
        /// OSS����ڵ�
        /// </summary>
        [Column("OSS_ENDPOINT")]
        public string oss_endpoint { get; set; }

        /// <summary>
        /// Bucket����
        /// </summary>
        [Column("OSS_BUKETNAME")]
        public string oss_buketname { get; set; }

        /// <summary>
        /// ���ģʽ 2�ϼ�ģʽ 1����  0�ر�
        /// </summary>
        [Column("OPENCHECKED")]
        public string openchecked { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        [Column("TASK_RULE")]
        public string task_rule { get; set; }

        /// <summary>
        /// �������  1����  0�ر�
        /// </summary>
        [Column("TASKCHECKED")]
        public int taskchecked { get; set; }

        /// <summary>
        /// ����APPKEY
        /// </summary>
        [Column("RONGCLOUD_APPKEY")]
        public string rongcloud_appkey { get; set; }

        /// <summary>
        /// ����APPSECRET
        /// </summary>
        [Column("RONGCLOUD_APPSECRET")]
        public string rongcloud_appsecret { get; set; }

        /// <summary>
        /// ��ƷԴ  0�Ա�����  1���Կ�
        /// </summary>
        [Column("GOODSOURCE")]
        public int? goodsource { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        [Column("GOODTYPE")]
        public int? goodtype { get; set; }

        /// <summary>
        /// 9.9��Ʒ��С���
        /// </summary>
        [Column("MIN_PRICE")]
        public int? min_price { get; set; }

        /// <summary>
        /// 9.9��Ʒ�����
        /// </summary>
        [Column("MAX_PRICE")]
        public int? max_price { get; set; }
        /// <summary>
        /// ��СӶ�����
        /// </summary>
        [Column("MIN_TK_RATE")]
        public int? min_tk_rate { get; set; }

        /// <summary>
        /// ���Ӷ�����
        /// </summary>
        [Column("MAX_TK_RATE")]
        public int? max_tk_rate { get; set; }

        /// <summary>
        /// ����ȯ��Ʒ���
        /// </summary>
        [Column("SUPER_COUPON_GOODTYPE")]
        public int? super_coupon_goodtype { get; set; }

        /// <summary>
        /// ����ȯ��С���
        /// </summary>
        [Column("SUPER_COUPON_MIN_PRICE")]
        public decimal? super_coupon_min_price { get; set; }

        /// <summary>
        /// ����ȯ�����
        /// </summary>
        [Column("SUPER_COUPON_MAX_PRICE")]
        public decimal? super_coupon_max_price { get; set; }

        /// <summary>
        /// ����ȯ��СӶ�����
        /// </summary>
        [Column("SUPER_COUPON_MIN_TK_RATE")]
        public decimal? super_coupon_min_tk_rate { get; set; }

        /// <summary>
        /// ����ȯ��С���
        /// </summary>
        [Column("SUPER_COUPON_COUPONPRICELOWERLIMIT")]
        public int? super_coupon_couponPriceLowerLimit { get; set; }

        /// <summary>
        /// ��Ѷ����AppID
        /// </summary>
        [Column("MEETING_APPID")]
        public string meeting_appid { get; set; }

        /// <summary>
        /// ��Ѷ����SecretID
        /// </summary>
        [Column("MEETING_SECRETID")]
        public string meeting_secretid { get; set; }

        /// <summary>
        /// ��Ѷ����SecretKey
        /// </summary>
        [Column("MEETING_SECRETKEY")]
        public string meeting_secretkey { get; set; }

        /// <summary>
        /// ��Ѷ����sdkid
        /// </summary>
        [Column("MEETING_SDKID")]
        public string meeting_sdkid { get; set; }

        /// <summary>
        /// ���˻�ӭ��
        /// </summary>
        [Column("WELCOMENEWPERSON")]
        public string welcomenewperson { get; set; }

        /// <summary>
        /// ��ʾӶ������  0��ʾ��Ӷ��   1��ʾ��Ӷ��
        /// </summary>
        public int? showcommission { get; set; }

        /// <summary>
        /// ��Ȧ�������ӻ�������
        /// </summary>
        public int miquan_integral { get; set; }

        /// <summary>
        /// ������ͨ�û����޼ӻ��� 0���ӻ���  1�ӻ���
        /// </summary>
        public int miquan_allowclickpraise { get; set; }
        public void Create()
        {
            UserInfo userInfo = LoginUserInfo.Get();
            appid = userInfo.companyId;
        }

        public void Modify(string keyValue)
        {
            appid = keyValue;
        }
    }
}
