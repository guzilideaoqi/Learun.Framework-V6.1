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

        #region 用户名密码登陆

        public ActionResult DM_Login(dm_userEntity dm_UserEntity)
        {
            try
            {
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

        public ActionResult DM_Register(dm_userEntity dm_UserEntity, string ParentInviteCode, string VerifiCode, string SmsMessageID)
        {
            try
            {
                string appid = CheckAPPID();
                if (dm_UserEntity.nickname.IsEmpty())
                {
                    return Fail("用户名不能为空!");
                }
                if (dm_UserEntity.phone.IsEmpty() || dm_UserEntity.phone.Length != 11)
                {
                    return Fail("手机号不能为空或格式错误!");
                }
                if (ParentInviteCode.IsEmpty())
                {
                    return Fail("邀请码不能为空!");
                }
                if (dm_UserEntity.pwd.IsEmpty())
                {
                    return Fail("密码不能为空!");
                }
                if (VerifiCode.IsEmpty())
                {
                    return Fail("验证码不能为空!");
                }
                if (!CommonSMSHelper.IsPassVerification(SmsMessageID, VerifiCode, appid))
                {
                    return Fail("验证码错误!");
                }
                dm_UserEntity.appid = appid;
                return Success("注册成功!", dm_userIBLL.Register(dm_UserEntity, ParentInviteCode, appid));
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

                    dm_UserEntity.nickname = "昵称";
                    dm_UserEntity.pwd = "123456";

                    return Success("登录成功!", dm_userIBLL.Register(dm_UserEntity, ParentInviteCode, appid));
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

        public ActionResult GetPersonInfo(int id)
        {
            try
            {
                return Success("获取成功", dm_userIBLL.GetEntityByCache(id));
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
        public ActionResult SignIn(int userid)
        {
            try
            {
                return this.Success("签到成功", (dynamic)dm_userIBLL.SignIn(userid));
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
                string appid = CheckAPPID();
                return Success("获取成功!", new
                {
                    UserID = dm_userIBLL.DecodeInviteCode(InviteCode)
                });
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

        public ActionResult Certification(int user_id, string name, string cardno, int CertificaRecordID = 0)
        {
            try
            {
                string appid = CheckAPPID();
                if (user_id.IsEmpty())
                {
                    return Fail("用户id不能为空!");
                }

                if (name.IsEmpty())
                {
                    return Fail("真实姓名不能为空!");
                }

                #region 身份证正面上传
                HttpPostedFile facecard_file = System.Web.HttpContext.Current.Request.Files["facecard"];
                if (facecard_file.ContentLength == 0 || string.IsNullOrEmpty(facecard_file.FileName))
                {
                    return HttpNotFound();
                }
                #endregion

                #region 身份证反面上传
                HttpPostedFile frontcard_file = System.Web.HttpContext.Current.Request.Files["frontcard"];
                if (frontcard_file.ContentLength == 0 || string.IsNullOrEmpty(frontcard_file.FileName))
                {
                    return HttpNotFound();
                }
                #endregion

                dm_certifica_recordEntity dm_Certifica_RecordEntity = new dm_certifica_recordEntity();

                #region 开始执行上传图片
                dm_Certifica_RecordEntity.facecard = UpdateCardImage(facecard_file);
                dm_Certifica_RecordEntity.frontcard = UpdateCardImage(frontcard_file);
                #endregion

                #region 构造其他信息
                dm_Certifica_RecordEntity.user_id = user_id;
                dm_Certifica_RecordEntity.realname = name;
                dm_Certifica_RecordEntity.cardno = cardno;
                dm_Certifica_RecordEntity.realstatus = 0;
                dm_Certifica_RecordEntity.appid = appid;
                #endregion

                dm_CertificaRecordIBLL.SaveEntity(CertificaRecordID, dm_Certifica_RecordEntity);

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

        public ActionResult GetCertificationRecord(int user_id)
        {
            try
            {
                if (user_id.IsEmpty())
                {
                    return Fail("用户id不能为空!");
                }

                return Success("获取成功", dm_CertificaRecordIBLL.GetCertificationRecord(user_id));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 修改收货地址

        public ActionResult UpdateAddress(int user_id, dm_userEntity dm_UserEntity)
        {
            try
            {
                if (dm_UserEntity.province.IsEmpty())
                    return Fail("省份不能为空!");
                if (dm_UserEntity.city.IsEmpty())
                    return Fail("城市不能为空!");
                if (dm_UserEntity.down.IsEmpty())
                    return Fail("区县不能为空!");
                if (dm_UserEntity.address.IsEmpty())
                    return Fail("详细地址不能为空!");

                dm_userIBLL.SaveEntity(user_id, dm_UserEntity);

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
        public ActionResult UpdateZFBInfo(int user_id, dm_userEntity dm_UserEntity)
        {
            try
            {
                if (dm_UserEntity.zfb.IsEmpty())
                    return Fail("支付宝账号不能为空!");
                if (dm_UserEntity.realname.IsEmpty())
                    return Fail("真实姓名不能为空!");

                dm_userIBLL.SaveEntity(user_id, dm_UserEntity);

                return Success("修改成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 修改用户昵称
        public ActionResult UpdateNickName(int user_id, dm_userEntity dm_UserEntity)
        {
            try
            {
                if (dm_UserEntity.nickname.IsEmpty())
                    return Fail("用户昵称不能为空!");

                dm_userIBLL.SaveEntity(user_id, dm_UserEntity);
                return Success("修改成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 修改用户头像
        public ActionResult UploadHeadPic(int user_id, dm_userEntity dm_UserEntity)
        {
            try
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

                dm_userIBLL.SaveEntity(user_id, dm_UserEntity);
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
        public ActionResult GetSystemMessage(int user_id, int pageNo = 1, int pageSize = 10)
        {
            try
            {
                if (user_id.IsEmpty())
                {
                    return Fail("用户id不能为空!");
                }
                string cacheKey = Md5Helper.Hash("SystemMessage" + user_id + pageNo + pageSize);
                IEnumerable<dm_messagerecordEntity> messageRecordList = redisCache.Read<IEnumerable<dm_messagerecordEntity>>(cacheKey, 7);
                if (messageRecordList == null)
                {
                    messageRecordList = dm_MessageRecordIBLL.GetPageList(new Pagination { rows = pageSize, page = pageNo, sidx = "createtime", sord = "desc" }, "{\"user_id\":\"" + user_id + "\"}");
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
        public ActionResult MessageToReadByUserID(int user_id)
        {
            try
            {
                if (user_id.IsEmpty())
                {
                    return Fail("用户id不能为空!");
                }
                dm_MessageRecordIBLL.MessageToReadByUserID(user_id);
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
        public ActionResult GetAccountDetailList(int user_id, int pageNo = 1, int pageSize = 20, int MessageType = 0)
        {
            try
            {
                if (user_id.IsEmpty())
                {
                    return Fail("用户id不能为空!");
                }

                string cacheKey = Md5Helper.Hash("AccountDetail" + user_id + pageNo + pageSize + MessageType);

                IEnumerable<dm_accountdetailEntity> accountDetailList = redisCache.Read<IEnumerable<dm_accountdetailEntity>>(cacheKey, 7);

                if (accountDetailList == null)
                {
                    string queryJson = "{\"user_id\":\"" + user_id + "\"";
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
        public ActionResult GetIntegralDetailList(int user_id, int pageNo = 1, int pageSize = 20, int MessageType = 0)
        {
            try
            {
                if (user_id.IsEmpty())
                {
                    return Fail("用户id不能为空!");
                }

                string cacheKey = Md5Helper.Hash("IntegralDetail" + user_id + pageNo + pageSize + MessageType);

                IEnumerable<dm_intergraldetailEntity> intergralchangegoodList = redisCache.Read<IEnumerable<dm_intergraldetailEntity>>(cacheKey, 7);

                if (intergralchangegoodList == null)
                {
                    string queryJson = "{\"user_id\":\"" + user_id + "\"";
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
        public ActionResult GetPersonProcess(int user_id)
        {
            try
            {
                string appid = CheckAPPID();
                return SuccessList("获取成功!", dm_Task_Person_SettingIBLL.GetPersonProcess(user_id, appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 领取任务
        public ActionResult ReceiveAwards(int user_id, int task_id)
        {
            try
            {
                dm_Task_Person_SettingIBLL.ReceiveAwards(user_id, task_id);
                return Success("领取成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取升级合伙人条件
        public ActionResult GetPartnersProcess(int user_id)
        {
            try
            {
                string appid = CheckAPPID();
                return Success("领取成功!", dm_Task_Person_SettingIBLL.GetPartnersProcess(user_id, appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 申请合伙人
        public ActionResult ApplyPartners(int user_id)
        {
            try
            {
                string appid = CheckAPPID();
                dm_Task_Person_SettingIBLL.ApplyPartners(user_id, appid);

                return Success("申请成为合伙人成功,我们会在7个工作日审核,请耐心等待!", new { });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 找回订单
        public ActionResult BindOrder(int user_id, string OrderSn)
        {
            try
            {
                string appid = CheckAPPID();
                if (user_id.IsEmpty())
                {
                    return Fail("用户id不能为空!");
                }
                dm_OrderIBLL.BindOrder(user_id, appid, OrderSn);
                return Success("订单绑定成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 我的订单
        public ActionResult GetMyOrder(int user_id, int PlaformType = 1, int Status = 0, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                if (user_id.IsEmpty())
                {
                    return Fail("用户id不能为空!");
                }

                string cacheKey = Md5Helper.Hash("OrderList" + user_id + PlaformType + Status + PageNo + PageSize);
                IEnumerable<dm_orderEntity> dm_OrderEntities = redisCache.Read<IEnumerable<dm_orderEntity>>(cacheKey, 7);
                if (dm_OrderEntities == null)
                {
                    dm_OrderEntities = dm_OrderIBLL.GetMyOrder(user_id, PlaformType, Status, new Pagination { page = PageNo, rows = PageSize, sidx = "createtime", sord = "desc" });

                    if (dm_OrderEntities.Count() > 0)
                    {
                        redisCache.Write(cacheKey, dm_OrderEntities, DateTime.Now.AddMinutes(10), 7);
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
        public ActionResult GetShareImage(int user_id)
        {
            try
            {
                string appid = CheckAPPID();
                return SuccessList("获取成功", dm_userIBLL.GetShareImage(user_id, appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取粉丝--直属粉丝
        public ActionResult GetChildDetail(int user_id, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);
                if (dm_UserEntity.IsEmpty())
                    return Fail("用户信息异常!");
                else
                {
                    string cacheKey = Md5Helper.Hash("ChildDetail" + user_id + PageNo + PageSize);
                    DataTable dataTable = redisCache.Read(cacheKey, 7);
                    if (dataTable == null)
                    {
                        dataTable = dm_UserRelationIBLL.GetMyChildDetail(user_id, PageNo, PageSize);
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
        public ActionResult GetSonChildDetail(int user_id, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);
                if (dm_UserEntity.IsEmpty())
                    return Fail("用户信息异常!");
                else
                {
                    string cacheKey = Md5Helper.Hash("SonChildDetail" + user_id + PageNo + PageSize);
                    DataTable dataTable = redisCache.Read(cacheKey, 7);
                    if (dataTable == null)
                    {
                        dataTable = dm_UserRelationIBLL.GetMySonChildDetail(user_id, PageNo, PageSize);
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
        public ActionResult GetPartnersChildDetail(int user_id, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(user_id);
                if (dm_UserEntity.IsEmpty())
                    return Fail("用户信息异常!");
                if (dm_UserEntity.partnersstatus == 0)
                    return Fail("当前非合伙人，无法看团队粉丝");
                else
                {
                    string cacheKey = Md5Helper.Hash("PartnersChildDetail" + user_id + PageNo + PageSize);
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
        public ActionResult ApplyAccountCash(int user_id, decimal Price = 0, string Remark = "")
        {
            try
            {
                if (user_id <= 0)
                    return Fail("用户信息异常!");
                if (Price < 10)
                    return Fail("提现金额不能小于10元!");
                dm_Apply_CashRecordIBLL.ApplyAccountCash(user_id, Price, Remark);
                return Success("提现成功!", new { });
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
