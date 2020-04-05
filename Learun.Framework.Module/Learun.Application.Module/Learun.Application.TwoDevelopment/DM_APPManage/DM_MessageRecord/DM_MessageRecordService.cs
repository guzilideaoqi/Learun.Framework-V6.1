using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
	public class DM_MessageRecordService : RepositoryFactory
	{
		private string fieldSql;

		public DM_MessageRecordService()
		{
			fieldSql = "\r\n                t.id,\r\n                t.messagetitle,\r\n                t.messagecontent,\r\n                t.user_id,\r\n                t.messagetype,\r\n                t.createtime,\r\n                t.createcode\r\n            ";
		}

		public IEnumerable<dm_messagerecordEntity> GetList(string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_messagerecord t ");
				return BaseRepository("多米易购").FindList<dm_messagerecordEntity>(strSql.ToString());
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

		public IEnumerable<dm_messagerecordEntity> GetPageList(Pagination pagination, string queryJson)
		{
			try
			{
				StringBuilder strSql = new StringBuilder();
				strSql.Append("SELECT ");
				strSql.Append(fieldSql);
				strSql.Append(" FROM dm_messagerecord t ");
				return BaseRepository("多米易购").FindList<dm_messagerecordEntity>(strSql.ToString(), pagination);
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

		public dm_messagerecordEntity GetEntity(int? keyValue)
		{
			try
			{
				return BaseRepository("多米易购").FindEntity<dm_messagerecordEntity>(keyValue);
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
				BaseRepository("多米易购").Delete((dm_messagerecordEntity t) => t.id == keyValue);
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

		public void SaveEntity(int? keyValue, dm_messagerecordEntity entity)
		{
			try
			{
				if (keyValue > 0)
				{
					entity.Modify(keyValue);
					BaseRepository("多米易购").Update(entity);
				}
				else
				{
					entity.Create();
					BaseRepository("多米易购").Insert(entity);
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
