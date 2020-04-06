using Learun.DataBase.Repository;
using Learun.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Learun.Application.Base.SystemModule.Log
{
    public class LogService : RepositoryFactory
    {
        public IEnumerable<LogEntity> GetPageList(Pagination pagination, string queryJson, string userId)
        {
            IEnumerable<LogEntity> result;
            try
            {
                Expression<Func<LogEntity, bool>> expression = LinqExtensions.True<LogEntity>();
                JObject jObject = queryJson.ToJObject();
                if (!jObject["CategoryId"].IsEmpty())
                {
                    int categoryId = jObject["CategoryId"].ToInt();
                    expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_CategoryId == (int?)categoryId);
                }
                if (!jObject["StartTime"].IsEmpty() && !jObject["EndTime"].IsEmpty())
                {
                    DateTime startTime = jObject["StartTime"].ToDate();
                    DateTime endTime = jObject["EndTime"].ToDate();
                    expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_OperateTime >= (DateTime?)startTime && t.F_OperateTime <= (DateTime?)endTime);
                }
                if (!jObject["OperateUserId"].IsEmpty())
                {
                    string OperateUserId = jObject["OperateUserId"].ToString();
                    expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_OperateUserId == OperateUserId);
                }
                if (!jObject["OperateAccount"].IsEmpty())
                {
                    string OperateAccount = jObject["OperateAccount"].ToString();
                    expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_OperateAccount.Contains(OperateAccount));
                }
                if (!jObject["OperateType"].IsEmpty())
                {
                    string operateType = jObject["OperateType"].ToString();
                    expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_OperateType == operateType);
                }
                if (!jObject["Module"].IsEmpty())
                {
                    string module = jObject["Module"].ToString();
                    expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_Module.Contains(module));
                }
                if (!jObject["keyword"].IsEmpty())
                {
                    string keyword = jObject["keyword"].ToString();
                    expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_Module.Contains(keyword) || t.F_OperateType.Contains(keyword) || t.F_IPAddress.Contains(keyword));
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_OperateUserId == userId);
                }
                result = base.BaseRepository().FindList<LogEntity>(expression, pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex, "");
            }
            return result;
        }
        public LogEntity GetEntity(string keyValue)
        {
            return base.BaseRepository().FindEntity<LogEntity>(keyValue);
        }
        public void RemoveLog(int categoryId, string keepTime)
        {
            try
            {
                DateTime operateTime = DateTime.Now;
                if (keepTime == "7")
                {
                    operateTime = DateTime.Now.AddDays(-7.0);
                }
                else
                {
                    if (keepTime == "1")
                    {
                        operateTime = DateTime.Now.AddMonths(-1);
                    }
                    else
                    {
                        if (keepTime == "3")
                        {
                            operateTime = DateTime.Now.AddMonths(-3);
                        }
                    }
                }
                Expression<Func<LogEntity, bool>> expression = LinqExtensions.True<LogEntity>();
                expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_OperateTime <= (DateTime?)operateTime);
                expression = LinqExtensions.And<LogEntity>(expression, (LogEntity t) => t.F_CategoryId == (int?)categoryId);
                base.BaseRepository().Delete<LogEntity>(expression);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex, "");
            }
        }
        public void WriteLog(LogEntity logEntity)
        {
            try
            {
                logEntity.F_LogId = Guid.NewGuid().ToString();
                logEntity.F_OperateTime = new DateTime?(DateTime.Now);
                logEntity.F_DeleteMark = new int?(0);
                logEntity.F_EnabledMark = new int?(1);
                logEntity.F_IPAddress = Net.Ip;
                logEntity.F_Host = Net.Host;
                logEntity.F_Browser = Net.Browser;
                base.BaseRepository().Insert<LogEntity>(logEntity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowServiceException(ex, "");
            }
        }
    }
}
