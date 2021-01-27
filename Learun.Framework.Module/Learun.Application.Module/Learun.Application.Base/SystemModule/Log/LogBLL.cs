using Learun.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.Base.SystemModule.Log
{
    public static class LogBLL
    {
        private static LogService service = new LogService();
        public static IEnumerable<LogEntity> GetPageList(Pagination pagination, string queryJson, string userId)
        {
            IEnumerable<LogEntity> pageList;
            try
            {
                pageList = LogBLL.service.GetPageList(pagination, queryJson, userId);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex, "");
            }
            return pageList;
        }
        public static void RemoveLog(int categoryId, string keepTime)
        {
            try
            {
                LogBLL.service.RemoveLog(categoryId, keepTime);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex, "");
            }
        }
        public static void WriteLog(this LogEntity logEntity)
        {
            try
            {
                LogBLL.service.WriteLog(logEntity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex, "");
            }
        }

        public static LogEntity LookLogDetail(string F_LogId) {
            try
            {
               return service.GetEntity(F_LogId);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex, "");
            }
        }
    }
}
