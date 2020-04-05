using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Cache.Redis;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class DM_UserCenterController : MvcAPIControllerBase
    {
        private ICache redisCache = CacheFactory.CaChe();

        #region 构造用户数据访问类
        DM_UserIBLL dm_userIBLL = new DM_UserBLL();
        #endregion

        /// <summary>
        /// dm_data用户信息类API
        /// Copyright (c) 2013-2017 上海力软信息技术有限公司
        /// 创建人：胡亚广
        /// 日 期：2020.03.13
        /// </summary>
        // GET: DM_UserCenter
        #region 用户登录
        public ActionResult DM_Login(dm_userEntity dm_UserEntity)
        {
            try
            {
                string appid = CheckAPPID();
                dm_UserEntity.appid = appid;

                dm_userEntity loginInfo = dm_userIBLL.Login(dm_UserEntity);
                if (loginInfo == null)
                    return Fail("用户名或密码错误!");
                else
                {
                    return Success("登录成功", loginInfo);
                }
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 用户注册
        //1、(注册时直接分配京东和拼多多商品ID)
        //2、用户注册必须包含验证码
        //3、必须包含上级邀请码
        public ActionResult DM_Register(dm_userEntity dm_UserEntity,string ParentInviteCode, string VerifiCode)
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

        #region 获取验证码
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="Phone">手机号</param>
        /// <param name="Type">1注册  2重置密码</param>
        /// <returns></returns>
        public ActionResult GetVerification(string Phone, int Type)
        {
            try
            {
                string appid = CheckAPPID();
                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByPhone(Phone, appid);
                if (Type == 1)
                {
                    if (dm_UserEntity != null)
                        return Fail("该手机号已注册!");
                }
                else if (Type == 2)
                {
                    if (dm_UserEntity == null)
                        return Fail("该手机号未注册!");
                }
                else
                {
                    return Fail("获取验证码类型不合法!");
                }

                Random random = new Random();
                int ranCode = random.Next(1111, 9999);
                return Success("验证码获取成功!", ranCode);
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 重置密码
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
                    return Fail("该手机号未注册!");
                else
                {
                    dm_UserEntity.pwd = Md5Helper.Encrypt(Pwd, 16);
                    dm_userIBLL.SaveEntity(dm_UserEntity.id.ToInt(), dm_UserEntity);

                    return Success("密码修改成功,请重新登录!");
                }
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }

        }
        #endregion

        #region 获取个人信息(缓存中获取)
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

        #region 生成邀请码
        public ActionResult EncodeInviteCode(int id) {
            try
            {
                string appid = CheckAPPID();
                return Success("获取成功", dm_userIBLL.EncodeInviteCode(id));
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 解析邀请码
        public ActionResult DecodeInviteCode(string InviteCode) {
            try
            {
                string appid = CheckAPPID();
                return Success("获取成功", dm_userIBLL.DecodeInviteCode(InviteCode));
            }
            catch (Exception ex)
            {
                return Fail(ex.InnerException.Message);
            }
        }
        #endregion

        #region 申请成为合伙人
        public ActionResult ApplyPartners()
        {
            return View();
        }
        #endregion

        #region 检测头部是否包含APPID
        public string CheckAPPID()
        {
            if (Request.Headers["appid"].IsEmpty())
                throw new Exception("缺少参数appid");
            else
            {
                return Request.Headers["appid"].ToString();
            }
        }
        #endregion
    }
}