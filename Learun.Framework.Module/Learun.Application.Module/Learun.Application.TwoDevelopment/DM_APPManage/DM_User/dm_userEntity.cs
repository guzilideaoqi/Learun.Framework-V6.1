using Learun.Cache.Factory;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class dm_userEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int? id
        {
            get;
            set;
        }
        /// <summary>
        /// 真实姓名
        /// </summary>
		[Column("REALNAME")]
        public string realname
        {
            get;
            set;
        }

        /// <summary>
        /// 支付宝账号
        /// </summary>
        [Column("ZFB")]
        public string zfb
        {
            get; set;
        }
        /// <summary>
        /// 身份证号
        /// </summary>
		[Column("IDENTITYCARD")]
        public string identitycard
        {
            get;
            set;
        }

        /// <summary>
        /// 身份证反面
        /// </summary>
        [Column("FRONTCARD")]
        public string frontcard { get; set; }

        /// <summary>
        /// 身份证正面
        /// </summary>
        [Column("FACECARD")]
        public string facecard { get; set; }

        /// <summary>
        /// 是否实名 0未实名  1已实名
        /// </summary>
        [Column("ISREAL")]
        public int? isreal
        {
            get;
            set;
        }
        /// <summary>
        /// 手机号
        /// </summary>
		[Column("PHONE")]
        public string phone
        {
            get;
            set;
        }
        /// <summary>
        /// 访问token
        /// </summary>
		[Column("TOKEN")]
        public string token
        {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary>
		[Column("PWD")]
        public string pwd
        {
            get;
            set;
        }
        /// <summary>
        /// 用户昵称
        /// </summary>
		[Column("NICKNAME")]
        public string nickname
        {
            get;
            set;
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        [Column("HEADPIC")]
        public string headpic { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
		[Column("ACCOUNTPRICE")]
        public decimal? accountprice
        {
            get;
            set;
        }
        /// <summary>
        /// 邀请码
        /// </summary>
		[Column("INVITECODE")]
        public string invitecode
        {
            get;
            set;
        }

        /// <summary>
        /// 备用邀请码
        /// </summary>
        [Column("BY_INVITECODE")]
        public string by_invitecode {
            get;set;
        }
        /// <summary>
        /// 合伙人id
        /// </summary>
		[Column("PARTNERS")]
        public int? partners
        {
            get;
            set;
        }
        /// <summary>
        /// 0非合伙人  1合伙人申请中   2已成为合伙人
        /// </summary>
		[Column("PARTNERSSTATUS")]
        public int? partnersstatus
        {
            get;
            set;
        }
        /// <summary>
        /// 淘宝pid
        /// </summary>
		[Column("TB_PID")]
        public string tb_pid
        {
            get;
            set;
        }
        /// <summary>
        /// 渠道id
        /// </summary>
		[Column("TB_RELATIONID")]
        public string tb_relationid
        {
            get;
            set;
        }
        /// <summary>
        /// 跟单渠道id
        /// </summary>
		[Column("TB_ORDERRELATIONID")]
        public string tb_orderrelationid
        {
            get;
            set;
        }
        /// <summary>
        /// 京东网站id
        /// </summary>
        [Column("JD_SITE")]
        public string jd_site { get; set; }
        /// <summary>
        /// 京东pid
        /// </summary>
		[Column("JD_PID")]
        public string jd_pid
        {
            get;
            set;
        }
        /// <summary>
        /// 拼多多pid
        /// </summary>
		[Column("PDD_PID")]
        public string pdd_pid
        {
            get;
            set;
        }
        /// <summary>
        /// 用户等级  0普通用户   1初级用户  2高级用户
        /// </summary>
		[Column("USERLEVEL")]
        public int? userlevel
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
		[Column("CREATETIME")]
        public DateTime? createtime
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
		[Column("UPDATETIME")]
        public DateTime? updatetime
        {
            get;
            set;
        }
        /// <summary>
        /// 平台id
        /// </summary>
		[Column("APPID")]
        public string appid
        {
            get;
            set;
        }
        /// <summary>
        /// 省份
        /// </summary>
		[Column("PROVINCE")]
        public string province
        {
            get;
            set;
        }
        /// <summary>
        /// 城市
        /// </summary>
		[Column("CITY")]
        public string city
        {
            get;
            set;
        }
        /// <summary>
        /// 区县
        /// </summary>
		[Column("DOWN")]
        public string down
        {
            get;
            set;
        }
        /// <summary>
        /// 详细地址
        /// </summary>
		[Column("ADDRESS")]
        public string address
        {
            get;
            set;
        }
        /// <summary>
        /// 账户积分
        /// </summary>
		[Column("INTEGRAL")]
        public int? integral
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有开通语音直播间权限  0无权限  1有权限
        /// </summary>
        [Column("ISVOICE")]
        public int? isvoice { get; set; }

        /// <summary>
        /// 用户启用状态  0异常用户 1正常用户
        /// </summary>
        [Column("ISENABLE")]
        public int? isenable { get; set; }

        /// <summary>
        /// 导师微信
        /// </summary>
        [Column("MYWECHAT")]
        public string mywechat { get; set; }

        /// <summary>
        /// 本月预估
        /// </summary>
        [Column("CURRENTMONTHEFFECT")]
        public decimal? currentmontheffect
        {
            get; set;
        }

        /// <summary>
        /// 本月收货
        /// </summary>
        [Column("CURRENTMONTHRECEIVEEFFECT")]
        public decimal? currentmonthreceiveeffect
        {
            get; set;
        }

        /// <summary>
        /// 上月收货
        /// </summary>
        [Column("UPMONTHRECEIVEEFFECT")]
        public decimal? upmonthreceiveeffect
        {
            get; set;
        }

        /// <summary>
        /// 我的下级数量
        /// </summary>
        [Column("MYCHILDCOUNT")]
        public int? mychildcount { get; set; }

        /// <summary>
        /// 我的二级数量
        /// </summary>
        [Column("MYSONCHILDCOUNT")]
        public int? mysonchildcount { get; set; }

        /// <summary>
        /// 我的团队数量
        /// </summary>
        [Column("MYPARTNERSCOUNT")]
        public int? mypartnerscount { get; set; }

        [Column("TB_NICKNAME")]
        public string tb_nickname { get; set; }

        [Column("ISRELATION_BEIAN")]
        public int isrelation_beian { get; set; }

        /// <summary>
        /// 融云Token
        /// </summary>
        [Column("RONGCLOUD_TOKEN")]
        public string rongcloud_token { get; set; }
        public void Create()
        {
            userlevel = 0;
            createtime = DateTime.Now;
            isreal = 0;
            accountprice = default(decimal);
            isenable = 1;
            isvoice = 0;
            currentmontheffect = 0;
            currentmonthreceiveeffect = 0;
            upmonthreceiveeffect = 0;
            mychildcount = 0;
            mysonchildcount = 0;
            mypartnerscount = 0;
            isrelation_beian = 0;
        }

        public void Modify(int? keyValue)
        {
            id = keyValue;
            int? num = keyValue;
            string cacheKey = "UserInfo" + num.ToString();
            CacheFactory.CaChe().Remove(cacheKey, 7L);
        }
    }
}
