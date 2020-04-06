using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.DataBase.Repository;
using Learun.Loger;
//using Learun.Loger;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_UserService : RepositoryFactory
	{
		private ICache redisCache = CacheFactory.CaChe();

		private DM_UserRelationService dm_UserRelationService = new DM_UserRelationService();

		private DM_BaseSettingService dm_BaseSettingService = new DM_BaseSettingService();

		private DM_IntergralDetailService dM_IntergralDetailService = new DM_IntergralDetailService();

		private string fieldSql;

		private static object lockObject = new object();

		private static char[] r = new char[32]
		{
			'Q',
			'W',
			'E',
			'8',
			'A',
			'S',
			'2',
			'D',
			'Z',
			'9',
			'C',
			'7',
			'P',
			'5',
			'I',
			'K',
			'3',
			'M',
			'J',
			'U',
			'F',
			'R',
			'4',
			'V',
			'Y',
			'L',
			'T',
			'N',
			'6',
			'B',
			'G',
			'H'
		};

		private static char b = 'X';

		private static int binLen = r.Length;

		private static int s = 6;

		public DM_UserService()
		{
			fieldSql = "    t.id,    t.realname,    t.identitycard,    t.isreal,    t.phone,    t.token,    t.pwd,    t.nickname,    t.accountprice,    t.invitecode,    t.partners,    t.partnersstatus,    t.tb_pid,    t.tb_relationid,    t.tb_orderrelationid,    t.jd_pid,    t.pdd_pid,    t.userlevel,    t.createtime,    t.updatetime,    t.appid,    t.province,    t.city,    t.down,    t.address";
		}

		public IEnumerable<dm_userEntity> GetList(string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_user t ");
				return BaseRepository("dm_data").FindList<dm_userEntity>(strSql.ToString());
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public IEnumerable<dm_userEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_user t ");
				return BaseRepository("dm_data").FindList<dm_userEntity>(strSql.ToString(), pagination);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public dm_userEntity GetEntity(int keyValue)
		{
			try
			{
				return BaseRepository("dm_data").FindEntity<dm_userEntity>(keyValue);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public dm_userEntity GetEntityByPhone(string phone, string appid)
		{
			try
			{
				return BaseRepository("dm_data").FindEntity((dm_userEntity t) => t.phone == phone && t.appid == appid);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public dm_userEntity GetEntityByCache(int id)
		{
			try
			{
				string cacheKey = "UserInfo" + id.ToString();
				dm_userEntity dm_UserEntity = redisCache.Read<dm_userEntity>(cacheKey, 7L);
				if (dm_UserEntity == null)
				{
					dm_UserEntity = GetEntity(id);
					if (dm_UserEntity != null)
					{
						redisCache.Write(cacheKey, dm_UserEntity, 7L);
					}
				}
				return dm_UserEntity;
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public void DeleteEntity(int keyValue)
		{
			try
			{
				BaseRepository("dm_data").Delete((dm_userEntity t) => t.id == (int?)keyValue);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public void SaveEntity(int keyValue, dm_userEntity entity)
		{
			try
			{
				if (keyValue > 0)
				{
					entity.Modify(keyValue);
					BaseRepository("dm_data").Update(entity);
				}
				else
				{
					BaseRepository("dm_data").Insert(entity);
				}
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public dm_userEntity Login(dm_userEntity entity)
		{
			try
			{
				entity.pwd = Md5Helper.Encrypt(entity.pwd, 16);
				return BaseRepository("dm_data").FindEntity((dm_userEntity t) => t.phone == entity.phone && t.appid == entity.appid && t.pwd == entity.pwd);
			}
			catch (Exception ex)
			{
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public dm_userEntity Register(dm_userEntity dm_UserEntity, string VerifiCode, string appid)
		{
			lock (lockObject)
			{
				IRepository db = BaseRepository("dm_data").BeginTrans();
				int? parent_id = 0;
				int? id = 0;
				try
				{
					parent_id = DecodeInviteCode(VerifiCode);
					if (parent_id <= 0)
					{
						throw new Exception("邀请码错误!");
					}
					dm_userEntity parent_user_entity = GetEntity(parent_id.ToInt());
					if (parent_user_entity == null)
					{
						throw new Exception("您的邀请人用户异常!");
					}
					dm_user_relationEntity dm_Parent_User_RelationEntity = dm_UserRelationService.GetEntityByParentID(parent_id);
					dm_userEntity dm_UserEntity_exist = GetEntityByPhone(dm_UserEntity.phone, appid);
					if (dm_UserEntity_exist != null)
					{
						throw new Exception("该手机号已注册!");
					}
					dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingService.GetEntityByCache(appid);
					dm_UserEntity.pwd = Md5Helper.Encrypt(dm_UserEntity.pwd, 16);
					dm_UserEntity.token = Guid.NewGuid().ToString();
					BaseRepository("dm_data").Insert(dm_UserEntity);
					dm_userEntity updateEntity = new dm_userEntity();
					id = (updateEntity.id = BaseRepository("dm_data").FindObject("SELECT LAST_INSERT_ID();").ToInt());
					updateEntity.invitecode = EncodeInviteCode(updateEntity.id);
					updateEntity.integral = dm_BasesettingEntity.new_people;
					updateEntity.Create();
					db.Update(updateEntity);
					db.Insert(new dm_intergraldetailEntity
					{
						createtime = DateTime.Now,
						currentvalue = updateEntity.integral,
						title = "新用户注册奖励",
						stepvalue = dm_BasesettingEntity.new_people,
						type = 1,
						user_id = id
					});
					parent_user_entity.integral += dm_BasesettingEntity.new_people_parent;
					db.Update(parent_user_entity);
					db.Insert(new dm_intergraldetailEntity
					{
						createtime = DateTime.Now,
						currentvalue = parent_user_entity.integral,
						title = "邀请好友奖励",
						stepvalue = dm_BasesettingEntity.new_people_parent,
						type = 3,
						user_id = parent_id
					});
					dm_user_relationEntity dm_User_RelationEntity = new dm_user_relationEntity
					{
						user_id = id,
						parent_id = parent_id,
						partners_id = dm_Parent_User_RelationEntity.partners_id,
						createtime = DateTime.Now
					};
					db.Insert(dm_User_RelationEntity);
					db.Commit();
					return GetEntity(id.ToInt());
				}
				catch (Exception ex)
				{
					db.Rollback();
					Log log = LogFactory.GetLogger("workflowapi");
					string[] obj = new string[6]
					{
						"上下级绑定失败,当前用户",
						null,
						null,
						null,
						null,
						null
					};
					int? num2 = id;
					obj[1] = num2.ToString();
					obj[2] = ",上级用户";
					num2 = parent_id;
					obj[3] = num2.ToString();
					obj[4] = ex.Message;
					obj[5] = ex.StackTrace;
					log.Error(string.Concat(obj));
					if (ex is ExceptionEx)
					{
						throw;
					}
					throw ExceptionEx.ThrowServiceException(ex);
				}
			}
		}

		public dynamic SignIn(int userid)
		{
			IRepository db = BaseRepository("dm_data").BeginTrans();
			try
			{
				dm_userEntity dm_UserEntity = GetEntityByCache(userid);
				if (dm_UserEntity.IsEmpty())
				{
					throw new Exception("用户信息异常!");
				}
				dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingService.GetEntityByCache(dm_UserEntity.appid);
				if (dm_BasesettingEntity.IsEmpty())
				{
					throw new Exception("获取基础配置信息异常!");
				}
				int? currentIntegral = 0;
				int signCount = 0;
				dm_intergraldetailEntity dm_IntergraldetailEntity = dM_IntergralDetailService.GetLastSignData(userid);
				if (dm_IntergraldetailEntity == null)
				{
					currentIntegral = dm_BasesettingEntity.firstsign;
					signCount = 1;
				}
				else
				{
					if (dm_IntergraldetailEntity.createtime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
					{
						throw new Exception("今日已签到!");
					}
					if (dm_IntergraldetailEntity.createtime.ToString("yyyy-MM-dd") == DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd"))
					{
						int? todayIntegral = dm_IntergraldetailEntity.stepvalue + dm_BasesettingEntity.signscrement;
						currentIntegral = ((todayIntegral > dm_BasesettingEntity.signcapping) ? dm_BasesettingEntity.signcapping : todayIntegral);
						signCount = int.Parse(dm_IntergraldetailEntity.remark) + 1;
					}
					else
					{
						currentIntegral = dm_BasesettingEntity.firstsign;
						signCount = 1;
					}
				}
				dm_UserEntity.integral += currentIntegral;
				dm_UserEntity.Modify(dm_UserEntity.id);
				db.Update(dm_UserEntity);
				db.Insert(new dm_intergraldetailEntity
				{
					currentvalue = dm_UserEntity.integral,
					stepvalue = currentIntegral,
					user_id = userid,
					title = "签到奖励",
					remark = signCount.ToString(),
					type = 2,
					createtime = DateTime.Now
				});
				db.Commit();
				return new
				{
					CurrentIntegral = currentIntegral,
					SignCount = signCount
				};
			}
			catch (Exception ex)
			{
				db.Rollback();
				if (ex is ExceptionEx)
				{
					throw;
				}
				throw ExceptionEx.ThrowServiceException(ex);
			}
		}

		public string EncodeInviteCode(int? id)
		{
			char[] buf = new char[32];
			int charPos = 32;
			while (id / binLen > 0)
			{
				int ind = (id % binLen).Value;
				buf[--charPos] = r[ind];
				id /= binLen;
			}
			buf[--charPos] = r[(id % binLen).Value];
			string str = new string(buf, charPos, 32 - charPos);
			if (str.Length < s)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(b);
				Random rnd = new Random();
				for (int i = 1; i < s - str.Length; i++)
				{
					sb.Append(r[rnd.Next(binLen)]);
				}
				str += sb.ToString();
			}
			return str;
		}

		public int? DecodeInviteCode(string InviteCode)
		{
			char[] chs = InviteCode.ToCharArray();
			int? res = 0;
			for (int i = 0; i < chs.Length; i++)
			{
				int ind = 0;
				for (int j = 0; j < binLen; j++)
				{
					if (chs[i] == r[j])
					{
						ind = j;
						break;
					}
				}
				if (chs[i] == b)
				{
					break;
				}
				res = ((i <= 0) ? new int?(ind) : (res * binLen + ind));
			}
			return res;
		}
	}
}
