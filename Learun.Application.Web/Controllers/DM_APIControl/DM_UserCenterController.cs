using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class DM_UserCenterController : MvcAPIControllerBase
    {
        private ICache redisCache = CacheFactory.CaChe();

        private DM_UserIBLL dm_userIBLL = new DM_UserBLL();

        #region 用户名密码登陆
        [HttpPost]
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
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 用户注册
        [HttpPost]
        public ActionResult DM_Register(dm_userEntity dm_UserEntity, string ParentInviteCode, string VerifiCode)
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
                return Success("注册成功!", dm_userIBLL.Register(dm_UserEntity, ParentInviteCode, appid));
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
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
        [HttpPost]
        public ActionResult DM_LoginByPhone(dm_userEntity dm_UserEntity, string ParentInviteCode, string VerifiCode, bool IsNewUser)
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
                return Fail(ex.InnerException.Message);
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
        [HttpPost]
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
                    default:
                        return Fail("获取验证码类型不合法!");
                    case 3:
                        break;
                }
                Random random = new Random();
                int ranCode = random.Next(1111, 9999);

                if (Type != 3)
                {
                    return Success("验证码获取成功!", new
                    {
                        VerifiCode = ranCode
                    });
                }
                else
                {///返回给前端是否为新用户  新用户需要输入邀请码
                    return Success("验证码获取成功!", new
                    {
                        VerifiCode = ranCode,
                        IsNewUser = dm_UserEntity == null
                    });
                }
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
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
        [HttpPost]
        public ActionResult ResetPwd(string Phone, string Pwd, string VerifiCode)
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
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByPhone(Phone, appid);

                if (dm_UserEntity == null)
                {
                    return Fail("该手机号未注册!");
                }
                dm_UserEntity.pwd = Md5Helper.Encrypt(Pwd, 16);
                dm_userIBLL.SaveEntity(dm_UserEntity.id.ToInt(), dm_UserEntity);
                return Success("密码修改成功,请重新登录!", new
                {

                });
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 获取个人信息
        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPersonInfo(int id)
        {
            try
            {
                return Success("获取成功", dm_userIBLL.GetEntityByCache(id));
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 签到
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignIn(int userid)
        {
            try
            {
                return this.Success("签到成功", (dynamic)dm_userIBLL.SignIn(userid));
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 加密邀请码
        [HttpPost]
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
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 解析邀请码
        [HttpPost]
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
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 实名认证
        [HttpPost]
        public ActionResult Certification(int user_id, string name, string cardno)
        {
            try
            {
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

                dm_userEntity dm_UserEntity = new dm_userEntity();

                #region 开始执行上传图片
                dm_UserEntity.facecard = UpdateCardImage(facecard_file);
                dm_UserEntity.frontcard = UpdateCardImage(frontcard_file);
                #endregion

                #region 构造其他信息
                dm_UserEntity.id = user_id;
                dm_UserEntity.realname = name;
                dm_UserEntity.identitycard = cardno;
                #endregion

                dm_userIBLL.SaveEntity(user_id, dm_UserEntity);

                return Success("提交成功!");
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 修改收货地址
        [HttpPost]
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
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 身份证正反面上传专用
        [HttpPost]
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

        #region 申请合伙人
        public ActionResult ApplyPartners()
        {
            return View();
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
