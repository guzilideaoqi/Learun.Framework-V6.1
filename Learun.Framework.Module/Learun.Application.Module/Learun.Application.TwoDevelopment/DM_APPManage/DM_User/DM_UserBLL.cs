using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_UserBLL : DM_UserIBLL
	{
		private DM_UserService dM_UserService = new DM_UserService();

		public IEnumerable<dm_userEntity> GetList(string queryJson)
		{
			try
			{
				return dM_UserService.GetList(queryJson);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public IEnumerable<dm_userEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				return dM_UserService.GetPageList(pagination, queryJson);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public dm_userEntity GetEntity(int keyValue)
		{
			try
			{
				return dM_UserService.GetEntity(keyValue);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public dm_userEntity GetEntityByPhone(string phone, string appid)
		{
			try
			{
				return dM_UserService.GetEntityByPhone(phone, appid);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public dm_userEntity GetEntityByCache(int id)
		{
			try
			{
				return dM_UserService.GetEntityByCache(id);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public void DeleteEntity(int keyValue)
		{
			try
			{
				dM_UserService.DeleteEntity(keyValue);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public void SaveEntity(int keyValue, dm_userEntity entity)
		{
			try
			{
				dM_UserService.SaveEntity(keyValue, entity);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public dm_userEntity Login(dm_userEntity entity)
		{
			try
			{
				return dM_UserService.Login(entity);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public dm_userEntity Register(dm_userEntity dm_UserEntity, string VerifiCode, string appid)
		{
			try
			{
				return dM_UserService.Register(dm_UserEntity, VerifiCode, appid);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public string EncodeInviteCode(int? id)
		{
			try
			{
				return dM_UserService.EncodeInviteCode(id);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}

		public int? DecodeInviteCode(string InviteCode)
		{
			try
			{
				return dM_UserService.DecodeInviteCode(InviteCode);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowBusinessException(ex);
			}
		}
	}
}
