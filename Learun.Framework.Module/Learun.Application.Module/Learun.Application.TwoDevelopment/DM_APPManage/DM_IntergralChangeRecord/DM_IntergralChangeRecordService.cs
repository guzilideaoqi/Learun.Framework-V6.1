using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_IntergralChangeRecordService : RepositoryFactory
	{
		private string fieldSql;

		public DM_IntergralChangeRecordService()
		{
			fieldSql = "\r\n                t.id,\r\n                t.user_id,\r\n                t.goodid,\r\n                t.createtime,\r\n                t.updatetime,\r\n                t.sendstatus,\r\n                t.expresscode,\r\n                t.remark,\r\n                t.appid,\r\n                t.province,\r\n                t.city,\r\n                t.down,\r\n                t.address,\r\n                t.phone,\r\n                t.username\r\n            ";
		}

		public IEnumerable<dm_intergralchangerecordEntity> GetList(string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_intergralchangerecord t ");
				return BaseRepository("dm_data").FindList<dm_intergralchangerecordEntity>(strSql.ToString());
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

		public IEnumerable<dm_intergralchangerecordEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_intergralchangerecord t");
				return BaseRepository("dm_data").FindList<dm_intergralchangerecordEntity>(strSql.ToString(), pagination);
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

		public dm_intergralchangerecordEntity GetEntity(int keyValue)
		{
			try
			{
				return BaseRepository("dm_data").FindEntity<dm_intergralchangerecordEntity>(keyValue);
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
				BaseRepository("dm_data").Delete((dm_intergralchangerecordEntity t) => t.id == (int?)keyValue);
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

		public void SaveEntity(int keyValue, dm_intergralchangerecordEntity entity)
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
					entity.Create();
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

		public void ApplyChangeGood(dm_intergralchangerecordEntity dm_IntergralchangerecordEntity, dm_userEntity dm_UserEntity)
		{
			IRepository db = BaseRepository("dm_data").BeginTrans();
			try
			{
				dm_intergralchangegoodEntity dm_IntergralchangegoodEntity = new DM_IntergralChangeGoodService().GetEntity(dm_IntergralchangerecordEntity.goodid.ToInt());
				if (dm_IntergralchangegoodEntity == null)
				{
					throw new Exception("该商品不存在!");
				}
				if (!(dm_UserEntity.integral >= dm_IntergralchangegoodEntity.needintergral))
				{
					throw new Exception("账户积分不足!");
				}
				db.Insert(dm_IntergralchangerecordEntity);
				dm_UserEntity.integral -= dm_IntergralchangegoodEntity.needintergral;
				dm_UserEntity.Modify(dm_UserEntity.id);
				db.Update(dm_UserEntity);
				db.Commit();
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
	}
}
