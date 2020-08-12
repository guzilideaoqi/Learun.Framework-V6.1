using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using Learun.Application.TwoDevelopment.Common;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class DM_UserCenterController : MvcAPIControllerBase
    {
        private ICache redisCache = CacheFactory.CaChe();

        private DM_UserIBLL dm_userIBLL = new DM_UserBLL();

        private DM_CertificaRecordIBLL dm_CertificaRecordIBLL = new DM_CertificaRecordBLL();

        private DM_MessageRecordIBLL dm_MessageRecordIBLL = new DM_MessageRecordBLL();

        private DM_AccountDetailIBLL dm_AccountDetailIBLL = new DM_AccountDetailBLL();

        private DM_IntergralDetailIBLL dm_IntegralDetailIBLL = new DM_IntergralDetailBLL();

        private DM_OrderIBLL dm_OrderIBLL = new DM_OrderBLL();

        private DM_Task_Person_SettingIBLL dm_Task_Person_SettingIBLL = new DM_Task_Person_SettingBLL();

        private DM_UserRelationIBLL dm_UserRelationIBLL = new DM_UserRelationBLL();

        private DM_Apply_CashRecordIBLL dm_Apply_CashRecordIBLL = new DM_Apply_CashRecordBLL();

        private DM_BaseSettingIBLL dm_BaseSettingIBLL = new DM_BaseSettingBLL();

        private DM_IntergralChangeGoodIBLL dM_IntergralChangeGoodIBLL = new DM_IntergralChangeGoodBLL();

        private dm_business_cooperationIBLL dm_Business_CooperationIBLL = new dm_business_cooperationBLL();

        #region 用户名密码登陆

        public ActionResult DM_Login(dm_userEntity dm_UserEntity)
        {
            try
            {
                if (dm_UserEntity.phone.IsEmpty() || dm_UserEntity.phone.Length != 11)
                {
                    return Fail("手机号不能为空或格式错误!");
                }
                if (dm_UserEntity.pwd.IsEmpty())
                    return Fail("密码不能为空!");
                string appid = dm_UserEntity.appid = CheckAPPID();
                dm_userEntity loginInfo = dm_userIBLL.Login(dm_UserEntity);
                if (loginInfo == null)
                {
                    return Fail("用户名或密码错误!");
                }
                return Success("登录成功", loginInfo);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 用户注册

        public ActionResult DM_Register(dm_userEntity dm_UserEntity, string VerifiCode, string SmsMessageID, string ParentInviteCode = "EX3LFY")
        {
            try
            {
                if (ParentInviteCode.IsEmpty())
                    ParentInviteCode = ParentInviteCode = "EX3LFY";
                string appid = CheckAPPID();
                /*if (dm_UserEntity.nickname.IsEmpty())
                {
                    return Fail("用户名不能为空!");
                }*/
                if (dm_UserEntity.phone.IsEmpty() || dm_UserEntity.phone.Length != 11)
                {
                    return Fail("手机号不能为空或格式错误!");
                }
                if (ParentInviteCode.IsEmpty())
                {
                    return Fail("邀请码不能为空!");
                }
                /*if (dm_UserEntity.pwd.IsEmpty())
                {
                    return Fail("密码不能为空!");
                }*/
                if (VerifiCode.IsEmpty())
                {
                    return Fail("验证码不能为空!");
                }

                dm_UserEntity.appid = appid;
                dm_UserEntity.nickname = "dlm_" + Time.GetTimeStamp();
                dm_UserEntity.pwd = "123456";
                return Success("注册成功!", dm_userIBLL.Register(dm_UserEntity, VerifiCode, ParentInviteCode, appid, SmsMessageID));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 手机号登录
        /// <summary>
        /// 手机号登录
        /// </summary>
        /// <param name="dm_UserEntity">保留用户手机号</param>
        /// <param name="ParentInviteCode">邀请码</param>
        /// <param name="VerifiCode">验证码</param>
        /// <param name="IsNewUser">是否是新用户  true:新用户  false:老用户</param>
        /// <returns></returns>

        public ActionResult DM_LoginByPhone(dm_userEntity dm_UserEntity, string ParentInviteCode, string VerifiCode, bool IsNewUser, string SmsMessageID)
        {
            try
            {
                string appid = CheckAPPID();

                if (dm_UserEntity.phone.IsEmpty() || dm_UserEntity.phone.Length != 11)
                {
                    return Fail("手机号不能为空或格式错误!");
                }
                if (VerifiCode.IsEmpty())
                {
                    return Fail("验证码不能为空!");
                }

                if (!CommonSMSHelper.IsPassVerification(SmsMessageID, VerifiCode, appid))
                {
                    return Fail("验证码错误!");
                }

                //如果是新用户则重新注册
                if (IsNewUser)
                {
                    if (ParentInviteCode.IsEmpty())
                    {
                        return Fail("邀请码不能为空!");
                    }

                    dm_UserEntity.nickname = "dlm_" + Time.GetTimeStamp();
                    dm_UserEntity.pwd = "123456";

                    return Success("登录成功!", dm_userIBLL.Register(dm_UserEntity, VerifiCode, ParentInviteCode, appid, SmsMessageID));
                }
                else
                {//如果是老用户  直接根据手机号查询出用户信息
                    dm_userEntity dm_new_UserEntity = dm_userIBLL.GetEntityByPhone(dm_UserEntity.phone, appid);
                    return Success("登录成功!", dm_new_UserEntity);
                }
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取验证码
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="Phone">手机号</param>
        /// <param name="Type">验证码类型  1注册  2找回密码  3手机号+验证码登录</param>
        /// <returns></returns>

        public ActionResult GetVerification(string Phone, int Type)
        {
            try
            {
                string appid = CheckAPPID();
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByPhone(Phone, appid);
                switch (Type)
                {
                    case 1:
                        if (dm_UserEntity != null)
                        {
                            return Fail("该手机号已注册!");
                        }
                        break;
                    case 2:
                        if (dm_UserEntity == null)
                        {
                            return Fail("该手机号未注册!");
                        }
                        break;
                    case 3:
                        break;
                    default:
                        return Fail("获取验证码类型不合法!");
                }
                //Random random = new Random();
                //int ranCode = random.Next(1111, 9999);
                string msgid = CommonSMSHelper.SendSms(Phone, appid);

                if (Type != 3)
                {
                    return Success("验证码获取成功!", new
                    {
                        //VerifiCode = ranCode,
                        msgid = msgid
                    });
                }
                else
                {///返回给前端是否为新用户  新用户需要输入邀请码
                    return Success("验证码获取成功!", new
                    {
                        //VerifiCode = ranCode,
                        IsNewUser = dm_UserEntity == null,
                        msgid = msgid
                    });
                }
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 重置密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="Phone">手机号</param>
        /// <param name="Pwd">密码</param>
        /// <param name="VerifiCode">验证码</param>
        /// <returns></returns>

        public ActionResult ResetPwd(string Phone, string Pwd, string VerifiCode, string SmsMessageID)
        {
            try
            {
                if (Phone.IsEmpty() || Phone.Length != 11)
                {
                    return Fail("手机号不能为空或格式错误!");
                }
                if (Pwd.IsEmpty())
                {
                    return Fail("密码不能为空!");
                }
                if (VerifiCode.IsEmpty())
                {
                    return Fail("验证码不能为空!");
                }
                string appid = CheckAPPID();

                if (!CommonSMSHelper.IsPassVerification(SmsMessageID, VerifiCode, appid))
                {
                    return Fail("验证码错误!");
                }

                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByPhone(Phone, appid);

                if (dm_UserEntity == null)
                {
                    return Fail("该手机号未注册!");
                }

                dm_UserEntity.pwd = Md5Helper.Encrypt(Pwd, 16);
                dm_userIBLL.SaveEntity(dm_UserEntity.id.ToInt(), dm_UserEntity);
                return Success("密码修改成功,请重新登录!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取个人信息
        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>

        public ActionResult GetPersonInfo(int User_ID = 0)
        {
            try
            {
                if (User_ID <= 0)
                    return FailNoLogin();
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);
                return Success("获取成功", dm_UserEntity);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取效果收益报表
        /// <summary>
        /// 获取效果收益报表
        /// </summary>
        /// <param name="User_ID">用户ID</param>
        /// <returns></returns>
        public ActionResult GetIncomeReport(int User_ID)
        {
            try
            {
                if (User_ID <= 0)
                    return FailNoLogin();
                return Success("获取成功", dm_UserRelationIBLL.GetIncomeReport(User_ID));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 签到
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public ActionResult SignIn(int User_ID)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                return this.Success("签到成功", (dynamic)dm_userIBLL.SignIn(User_ID));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 加密邀请码

        public ActionResult EncodeInviteCode(int id)
        {
            try
            {
                if (id <= 0) return FailNoLogin();

                string appid = CheckAPPID();
                return Success("获取成功", new
                {
                    InviteCode = dm_userIBLL.EncodeInviteCode(id)
                });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 解析邀请码

        public ActionResult DecodeInviteCode(string InviteCode)
        {
            try
            {
                if (InviteCode.Length != 6)
                    return null;

                string appid = CheckAPPID();

                dm_userEntity dm_UserEntity = dm_userIBLL.DecodeInviteCode(InviteCode);
                if (dm_UserEntity.IsEmpty())
                    return Fail("邀请码错误!");

                return Success("获取成功!", new
                {
                    UserID = dm_UserEntity.id,
                    NickName = dm_UserEntity.nickname,
                    HeadPic = dm_UserEntity.headpic,
                    RealName = dm_UserEntity.realname,
                    Phone = dm_UserEntity.phone
                }); ;
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 实名认证
        /// <summary>
        /// 实名认证提交
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="name"></param>
        /// <param name="cardno"></param>
        /// <param name="CertificaRecordID"></param>
        /// <returns></returns>

        public ActionResult Certification(int User_ID, string name, string cardno, string facecard = "", string frontcard = "")
        {
            try
            {
                string appid = CheckAPPID();
                if (User_ID <= 0) return FailNoLogin();

                if (name.IsEmpty())
                {
                    return Fail("真实姓名不能为空!");
                }
                //dm_certifica_recordEntity dm_Certifica_RecordEntity = new dm_certifica_recordEntity();
                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);

                dm_certifica_recordEntity dm_Certifica_RecordEntity = dm_CertificaRecordIBLL.GetCertificationRecord(User_ID);
                if (dm_Certifica_RecordEntity.IsEmpty())
                    dm_Certifica_RecordEntity = new dm_certifica_recordEntity();

                #region 身份证正面上传
                if (facecard.IsEmpty())
                {
                    HttpPostedFile facecard_file = System.Web.HttpContext.Current.Request.Files["facecard"];
                    if (facecard_file.ContentLength == 0 || string.IsNullOrEmpty(facecard_file.FileName))
                    {
                        return Fail("请上传身份证正面照片!");
                    }
                    dm_Certifica_RecordEntity.facecard = OSSHelper.PutObject(dm_BasesettingEntity, "", facecard_file); ;

                }
                else
                {
                    dm_Certifica_RecordEntity.facecard = facecard;
                }
                #endregion


                #region 身份证反面上传
                if (frontcard.IsEmpty())
                {
                    HttpPostedFile frontcard_file = System.Web.HttpContext.Current.Request.Files["frontcard"];
                    if (frontcard_file.ContentLength == 0 || string.IsNullOrEmpty(frontcard_file.FileName))
                    {
                        return Fail("请上传身份证反面照片!");
                    }

                    dm_Certifica_RecordEntity.frontcard = OSSHelper.PutObject(dm_BasesettingEntity, "", frontcard_file);
                }
                else
                {
                    dm_Certifica_RecordEntity.frontcard = frontcard;
                }
                #endregion

                #region 构造其他信息
                dm_Certifica_RecordEntity.user_id = User_ID;
                dm_Certifica_RecordEntity.realname = name;
                dm_Certifica_RecordEntity.cardno = cardno;
                dm_Certifica_RecordEntity.realstatus = 0;
                dm_Certifica_RecordEntity.appid = appid;
                #endregion

                dm_CertificaRecordIBLL.SaveEntity(dm_Certifica_RecordEntity.id, dm_Certifica_RecordEntity);

                return Success("提交成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        /// <summary>
        /// 获取实名认证记录
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>

        public ActionResult GetCertificationRecord(int User_ID)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                return Success("获取成功", dm_CertificaRecordIBLL.GetCertificationRecord(User_ID));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 修改收货地址

        public ActionResult UpdateAddress(int User_ID, dm_userEntity dm_UserEntity)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                if (dm_UserEntity.province.IsEmpty())
                    return Fail("省份不能为空!");
                if (dm_UserEntity.city.IsEmpty())
                    return Fail("城市不能为空!");
                if (dm_UserEntity.down.IsEmpty())
                    return Fail("区县不能为空!");
                if (dm_UserEntity.address.IsEmpty())
                    return Fail("详细地址不能为空!");

                dm_userIBLL.SaveEntity(User_ID, dm_UserEntity);

                return Success("修改成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 绑定支付宝
        /// <summary>
        /// 绑定支付宝
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="dm_UserEntity"></param>
        /// <returns></returns>
        public ActionResult UpdateZFBInfo(int User_ID, dm_userEntity dm_UserEntity)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                if (dm_UserEntity.zfb.IsEmpty())
                    return Fail("支付宝账号不能为空!");
                if (dm_UserEntity.realname.IsEmpty())
                    return Fail("真实姓名不能为空!");

                dm_userIBLL.SaveEntity(User_ID, dm_UserEntity);

                return Success("修改成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 修改用户昵称
        public ActionResult UpdateNickName(int User_ID, dm_userEntity dm_UserEntity)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                if (dm_UserEntity.nickname.IsEmpty())
                    return Fail("用户昵称不能为空!");

                dm_userIBLL.SaveEntity(User_ID, dm_UserEntity);
                return Success("修改成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 修改微信号
        public ActionResult UpdateWeChatID(int User_ID, dm_userEntity dm_UserEntity)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                if (dm_UserEntity.mywechat.IsEmpty())
                    return Fail("微信号不能为空!");

                dm_userIBLL.SaveEntity(User_ID, dm_UserEntity);
                return Success("修改成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 修改用户头像
        public ActionResult UploadHeadPic(int User_ID, dm_userEntity dm_UserEntity)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();
                if (System.Web.HttpContext.Current.Request.Files.Count > 0)
                {
                    #region 头像上传
                    HttpPostedFile headpic_file = System.Web.HttpContext.Current.Request.Files["headpic"];
                    if (headpic_file.ContentLength == 0 || string.IsNullOrEmpty(headpic_file.FileName))
                    {
                        return HttpNotFound();
                    }
                    #endregion

                    string FileEextension = Path.GetExtension(headpic_file.FileName);
                    string virtualPath = $"/Resource/HeadPic/{Guid.NewGuid().ToString()}{FileEextension}";
                    string fullFileName = base.Server.MapPath("~" + virtualPath);
                    string path = Path.GetDirectoryName(fullFileName);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    headpic_file.SaveAs(fullFileName);
                    dm_UserEntity.headpic = virtualPath;
                }

                dm_userIBLL.SaveEntity(User_ID, dm_UserEntity);
                return Success("修改成功");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 身份证正反面上传专用

        string UpdateCardImage(HttpPostedFile cardInfo)
        {
            string FileEextension = Path.GetExtension(cardInfo.FileName);
            string virtualPath = $"/Resource/CardImage/{Guid.NewGuid().ToString()}{FileEextension}";
            string fullFileName = base.Server.MapPath("~" + virtualPath);
            string path = Path.GetDirectoryName(fullFileName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            cardInfo.SaveAs(fullFileName);

            return virtualPath;
        }
        #endregion

        #region 系统消息
        /// <summary>
        /// 获取系统消息 (做短时间的缓存 10分钟)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSystemMessage(int User_ID, int pageNo = 1, int pageSize = 10)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();
                string cacheKey = Md5Helper.Hash("SystemMessage" + User_ID + pageNo + pageSize);
                IEnumerable<dm_messagerecordEntity> messageRecordList = redisCache.Read<IEnumerable<dm_messagerecordEntity>>(cacheKey, 7);
                if (messageRecordList == null)
                {
                    messageRecordList = dm_MessageRecordIBLL.GetPageList(new Pagination { rows = pageSize, page = pageNo, sidx = "createtime", sord = "desc" }, "{\"user_id\":\"" + User_ID + "\"}");
                    if (messageRecordList != null)
                    {
                        redisCache.Write<IEnumerable<dm_messagerecordEntity>>(cacheKey, messageRecordList, DateTime.Now.AddMinutes(10), 7);
                    }
                }

                return SuccessList("系统消息获取成功!", messageRecordList);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        /// <summary>
        /// 通过用户ID转已读
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public ActionResult MessageToReadByUserID(int User_ID)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();
                dm_MessageRecordIBLL.MessageToReadByUserID(User_ID);
                return Success("状态更改成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        /// <summary>
        /// 通过消息记录转已读
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MessageToReadByID(int id)
        {
            try
            {
                if (id.IsEmpty())
                {
                    return Fail("消息id不能为空!");
                }
                dm_MessageRecordIBLL.MessageToReadByID(id);
                return Success("状态更改成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取明细信息
        /// <summary>
        /// 账户余额明细
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="PageNo">页码</param>
        /// <param name="PageSize">每页显示数量</param>
        /// <param name="MessageType">消息类型 0全部 1订单佣金  2一级粉丝订单  3二级粉丝订单  4团队订单  5下级团队订单  6下级开通代理  7下下级开通代理 8团队成员  9下级团队成员</param>
        /// <returns></returns>
        public ActionResult GetAccountDetailList(int User_ID, int pageNo = 1, int pageSize = 20, int MessageType = 0)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                string cacheKey = Md5Helper.Hash("AccountDetail" + User_ID + pageNo + pageSize + MessageType);

                IEnumerable<dm_accountdetailEntity> accountDetailList = redisCache.Read<IEnumerable<dm_accountdetailEntity>>(cacheKey, 7);

                if (accountDetailList == null)
                {
                    string queryJson = "{\"user_id\":\"" + User_ID + "\"";
                    if (MessageType == 0)
                        queryJson += "}";
                    else
                        queryJson += ",\"type\":\"" + MessageType + "\"}";

                    accountDetailList = dm_AccountDetailIBLL.GetPageList(new Pagination { rows = pageSize, page = pageNo, sidx = "createtime", sord = "desc" }, queryJson);

                    if (accountDetailList.Count() > 0)
                    {
                        redisCache.Write<IEnumerable<dm_accountdetailEntity>>(cacheKey, accountDetailList, DateTime.Now.AddMinutes(10), 7);
                    }
                }

                return SuccessList("账户明细获取成功!", accountDetailList);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        /// <summary>
        /// 获取积分明细
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="MessageType">类型  1新用户注册  2签到  3邀请好友奖励</param>
        /// <returns></returns>
        public ActionResult GetIntegralDetailList(int User_ID, int pageNo = 1, int pageSize = 20, int MessageType = 0)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                string cacheKey = Md5Helper.Hash("IntegralDetail" + User_ID + pageNo + pageSize + MessageType);

                IEnumerable<dm_intergraldetailEntity> intergralchangegoodList = redisCache.Read<IEnumerable<dm_intergraldetailEntity>>(cacheKey, 7);

                if (intergralchangegoodList == null)
                {
                    string queryJson = "{\"user_id\":\"" + User_ID + "\"";
                    if (MessageType == 0)
                        queryJson += "}";
                    else
                        queryJson += ",\"type\":\"" + MessageType + "\"}";

                    intergralchangegoodList = dm_IntegralDetailIBLL.GetPageList(new Pagination { rows = pageSize, page = pageNo, sidx = "createtime", sord = "desc" }, queryJson);

                    if (intergralchangegoodList.Count() > 0)
                    {
                        redisCache.Write<IEnumerable<dm_intergraldetailEntity>>(cacheKey, intergralchangegoodList, DateTime.Now.AddMinutes(10), 7);
                    }
                }

                return SuccessList("账户明细获取成功!", intergralchangegoodList);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取个人任务进度
        public ActionResult GetPersonProcess(int User_ID)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                string appid = CheckAPPID();

                string cacheKey = "MyPersonTask_" + User_ID;

                IEnumerable<dm_task_person_settingEntity> totalTask = redisCache.Read<IEnumerable<dm_task_person_settingEntity>>(cacheKey, 7);
                if (totalTask == null)
                {
                    totalTask = dm_Task_Person_SettingIBLL.GetPersonProcess(User_ID, appid);

                    redisCache.Write<IEnumerable<dm_task_person_settingEntity>>(cacheKey, totalTask, DateTime.Now.AddMinutes(10), 7);
                }

                List<int?> today_ids = new int?[] { 1, 6 }.ToList();

                var taskDetail = new
                {
                    Today_Task = totalTask.Where(t => today_ids.Contains(t.s_type)),
                    DLM_Task = totalTask.Where(t => !today_ids.Contains(t.s_type)),
                };

                return Success("获取成功!", taskDetail);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 领取任务
        public ActionResult ReceiveAwards(int User_ID, int task_id)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                dm_Task_Person_SettingIBLL.ReceiveAwards(User_ID, task_id);
                return Success("领取成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取升级合伙人条件
        public ActionResult GetPartnersProcess(int User_ID)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                string appid = CheckAPPID();
                return Success("领取成功!", dm_Task_Person_SettingIBLL.GetPartnersProcess(User_ID, appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 申请合伙人
        public ActionResult ApplyPartners(int User_ID)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                string appid = CheckAPPID();
                dm_Task_Person_SettingIBLL.ApplyPartners(User_ID, appid);

                return Success("申请成为合伙人成功,我们会在7个工作日审核,请耐心等待!", new { });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 找回订单
        public ActionResult BindOrder(int User_ID, string OrderSn)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                string appid = CheckAPPID();

                dm_OrderIBLL.BindOrder(User_ID, appid, OrderSn);
                return Success("订单绑定成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 我的订单
        public ActionResult GetMyOrder(int User_ID, int PlaformType = 1, int Status = 0, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                string cacheKey = Md5Helper.Hash("OrderList" + User_ID + PlaformType + Status + PageNo + PageSize);
                IEnumerable<dm_orderEntity> dm_OrderEntities = redisCache.Read<IEnumerable<dm_orderEntity>>(cacheKey, 7);
                if (dm_OrderEntities == null)
                {
                    dm_OrderEntities = dm_OrderIBLL.GetMyOrder(User_ID, PlaformType, Status, new Pagination { page = PageNo, rows = PageSize, sidx = "createtime", sord = "desc" });

                    if (dm_OrderEntities.Count() > 0)
                    {
                        redisCache.Write(cacheKey, dm_OrderEntities, DateTime.Now.AddMinutes(1), 7);
                    }
                }

                return SuccessList("获取成功!", dm_OrderEntities);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 我的推广
        /// <summary>
        /// 获取推广图片
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public ActionResult GetShareImage(int User_ID)
        {
            try
            {
                string appid = CheckAPPID();

                if (User_ID <= 0) return FailNoLogin();

                return SuccessList("获取成功", dm_userIBLL.GetShareImage(User_ID, appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取粉丝数据统计
        public ActionResult GetFansStaticInfo(int User_ID)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                string cacheKey = Md5Helper.Hash("FansStaticInfo" + User_ID);
                FansStaticInfoEntity fansStaticInfo = redisCache.Read<FansStaticInfoEntity>(cacheKey, 7);
                if (fansStaticInfo == null)
                {
                    fansStaticInfo = dm_userIBLL.GetFansStaticInfo(User_ID);
                    redisCache.Write<FansStaticInfoEntity>(cacheKey, fansStaticInfo, DateTime.Now.AddMinutes(5), 7);
                }
                return Success("获取成功!", fansStaticInfo);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取粉丝--直属粉丝
        public ActionResult GetChildDetail(int User_ID, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);
                if (dm_UserEntity.IsEmpty())
                    return Fail("用户信息异常!");
                else
                {
                    string cacheKey = Md5Helper.Hash("ChildDetail" + User_ID + PageNo + PageSize);
                    DataTable dataTable = redisCache.Read(cacheKey, 7);
                    if (dataTable == null)
                    {
                        dataTable = dm_UserRelationIBLL.GetMyChildDetail(User_ID, PageNo, PageSize);
                        if (dataTable.Rows.Count >= PageSize)
                            redisCache.Write(cacheKey, dataTable, DateTime.Now.AddHours(3), 7);
                        else
                            redisCache.Write(cacheKey, dataTable, DateTime.Now.AddMinutes(5), 7);
                    }
                    return SuccessList("获取成功!", dataTable);
                }
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取粉丝--二级粉丝
        public ActionResult GetSonChildDetail(int User_ID, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);
                if (dm_UserEntity.IsEmpty())
                    return Fail("用户信息异常!");
                else
                {
                    string cacheKey = Md5Helper.Hash("SonChildDetail" + User_ID + PageNo + PageSize);
                    DataTable dataTable = redisCache.Read(cacheKey, 7);
                    if (dataTable == null)
                    {
                        dataTable = dm_UserRelationIBLL.GetMySonChildDetail(User_ID, PageNo, PageSize);
                        if (dataTable.Rows.Count >= PageSize)
                            redisCache.Write(cacheKey, dataTable, DateTime.Now.AddHours(3), 7);
                        else
                            redisCache.Write(cacheKey, dataTable, DateTime.Now.AddMinutes(5), 7);
                    }
                    return SuccessList("获取成功!", dataTable);
                }
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取粉丝--团队粉丝
        public ActionResult GetPartnersChildDetail(int User_ID, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);
                if (dm_UserEntity.IsEmpty())
                    return Fail("用户信息异常!");
                if (dm_UserEntity.partnersstatus == 0)
                    return Fail("当前非合伙人，无法看团队粉丝");
                else
                {
                    string cacheKey = Md5Helper.Hash("PartnersChildDetail" + User_ID + PageNo + PageSize);
                    DataTable dataTable = redisCache.Read(cacheKey, 7);
                    if (dataTable == null)
                    {
                        dataTable = dm_UserRelationIBLL.GetPartnersChildDetail(dm_UserEntity.partners, PageNo, PageSize);
                        if (dataTable.Rows.Count >= PageSize)
                            redisCache.Write(cacheKey, dataTable, DateTime.Now.AddHours(3), 7);
                        else
                            redisCache.Write(cacheKey, dataTable, DateTime.Now.AddMinutes(5), 7);
                    }
                    return SuccessList("获取成功!", dataTable);
                }
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 申请提现
        public ActionResult ApplyAccountCash(int User_ID, decimal Price = 0, string Remark = "")
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();
                if (Price < 10)
                    return Fail("提现金额不能小于10元!");
                dm_Apply_CashRecordIBLL.ApplyAccountCash(User_ID, Price, Remark);
                return Success("提现成功!", new { });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取我的提现记录
        public ActionResult GetMyCashRecord(int User_ID, int PageNo = 1, int PageSize = 10)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();

                return SuccessList("获取成功", dm_Apply_CashRecordIBLL.GetMyCashRecord(User_ID, new Pagination { page = PageNo, rows = PageSize, sidx = "createtime", sord = "desc" }));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 商务合作
        public ActionResult BusinessCooperation(dm_business_cooperationEntity dm_Business_CooperationEntity)
        {
            try
            {
                if (dm_Business_CooperationEntity.user_id <= 0)
                    return FailNoLogin();

                dm_Business_CooperationIBLL.SaveEntity(0, dm_Business_CooperationEntity);

                return Success("提交成功,工作人员稍后会与您联系!", new { });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 生成融云token
        public ActionResult GeneralRongTokne(int User_ID)
        {
            try
            {
                string appid = CheckAPPID();
                if (User_ID <= 0) return FailNoLogin();

                return SuccessList("获取成功", dm_userIBLL.GeneralRongTokne(User_ID, appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取签到数据
        public ActionResult GetSignData(int User_ID)
        {
            try
            {
                string appid = CheckAPPID();
                if (User_ID <= 0) return FailNoLogin();
                int sign_day = 0, finishsign = 0;

                List<SignRecord> signRecord = dm_userIBLL.GetSignData(User_ID, ref sign_day, ref finishsign);

                dynamic dy = new
                {
                    SignData = signRecord,
                    SignDay = sign_day,
                    TodaySign = finishsign,
                    SignRule = "首次签到可得10积分，连续签到每次加3积分，20积分封顶",
                    IntegralGood = dM_IntergralChangeGoodIBLL.GetPageListByCache(new Pagination
                    {
                        page = 1,
                        rows = 20
                    }, appid)
                };

                return Success("数据获取成功!", dy);
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
