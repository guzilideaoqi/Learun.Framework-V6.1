using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_AccountDetailService : RepositoryFactory
	{
		private string fieldSql;

		public DM_AccountDetailService()
		{
			fieldSql = "    t.id,    t.currentvalue,    t.stepvalue,    t.user_id,    t.title,    t.type,    t.remark,    t.createtime,    t.createcode,t.profitLoss";
		}

		public IEnumerable<dm_accountdetailEntity> GetList(string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_accountdetail t ");
				return BaseRepository("dm_data").FindList<dm_accountdetailEntity>(strSql.ToString());
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

		public IEnumerable<dm_accountdetailEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
                var param = queryJson.ToJObject();

				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_accountdetail t where 1=1");

                if (!param["type"].IsEmpty())
                {
                    strSql.Append(" and type='" + param["type"].ToString() + "'");
                }

                if (!param["user_id"].IsEmpty()) {
                    strSql.Append(" and user_id='" + param["user_id"].ToString() + "'");
                }

				return BaseRepository("dm_data").FindList<dm_accountdetailEntity>(strSql.ToString(), pagination);
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

		public dm_accountdetailEntity GetEntity(int? keyValue)
		{
			try
			{
				return BaseRepository("dm_data").FindEntity<dm_accountdetailEntity>(keyValue);
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

		public void DeleteEntity(int? keyValue)
		{
			try
			{
				BaseRepository("dm_data").Delete((dm_accountdetailEntity t) => t.id == keyValue);
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

		public void SaveEntity(int? keyValue, dm_accountdetailEntity entity)
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
	}
}
