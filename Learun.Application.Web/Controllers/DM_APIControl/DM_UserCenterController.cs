using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
	public class DM_UserCenterController : MvcAPIControllerBase
	{
		private ICache redisCache = CacheFactory.CaChe();

		private DM_UserIBLL dm_userIBLL = new DM_UserBLL();

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
				return Success("验证码获取成功!", new
				{
					VerifiCode = ranCode
				});
			}
			catch (Exception ex)
			{
				return Fail(ex.InnerException.Message);
			}
		}

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

		public ActionResult DecodeInviteCode(string InviteCode)
		{
			try
			{
				string appid = CheckAPPID();
				return Success("获取成功", new
				{
					UserID = dm_userIBLL.DecodeInviteCode(InviteCode)
				});
			}
			catch (Exception ex)
			{
				return Fail(ex.InnerException.Message);
			}
		}

		public ActionResult ApplyPartners()
		{
			return View();
		}

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
