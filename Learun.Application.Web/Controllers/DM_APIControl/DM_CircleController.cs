using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class DM_CircleController : MvcAPIControllerBase
    {
        private ICache redisCache = CacheFactory.CaChe();

        private dm_friend_circleIBLL dm_Friend_CircleIBLL = new dm_friend_circleBLL();

        #region 获取哆米圈官推文章
        public ActionResult GetCircleByGovernment(int PageNo = 1, int PageSize = 20)
        {
            try
            {
                Pagination pagination = new Pagination
                {
                    page = PageNo,
                    rows = PageSize,
                    sidx = "createtime",
                    sord = "desc"
                };
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("GetCircleByGovernment" + pagination.ToJson() + appid);
                IEnumerable<dm_friend_circleEntity> dm_Friend_CircleEntities = redisCache.Read<IEnumerable<dm_friend_circleEntity>>(cacheKey, 7);
                if (dm_Friend_CircleEntities == null)
                {
                    dm_Friend_CircleEntities = dm_Friend_CircleIBLL.GetCircleByGovernment(pagination, appid);
                    if (dm_Friend_CircleEntities.Count() > 0)
                    {
                        redisCache.Write<IEnumerable<dm_friend_circleEntity>>(cacheKey, dm_Friend_CircleEntities, DateTime.Now.AddMinutes(10), 7);
                    }
                }

                return SuccessList("获取成功!", dm_Friend_CircleEntities);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取哆米圈普通文章
        public ActionResult GetCircleByGeneral(int PageNo = 1, int PageSize = 20)
        {
            try
            {
                Pagination pagination = new Pagination
                {
                    page = PageNo,
                    rows = PageSize,
                    sidx = "createtime",
                    sord = "desc"
                };
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("GetCircleByGeneral" + pagination.ToJson() + appid);
                DataTable dataTable = redisCache.Read(cacheKey, 7);
                if (dataTable == null)
                {
                    dataTable = dm_Friend_CircleIBLL.GetCircleByGeneral(pagination, appid);
                    if (dataTable.Rows.Count > 0)
                    {
                        redisCache.Write(cacheKey, dataTable, DateTime.Now.AddMinutes(2), 7);
                    }
                }

                return SuccessList("获取成功!", dataTable);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取单条哆米圈文章
        public ActionResult GetSingleCircle(int id)
        {
            try
            {
                string cacheKey = "SingleCircle" + id;
                dm_friend_circleEntity dm_Friend_CircleEntity = redisCache.Read<dm_friend_circleEntity>(cacheKey, 7);
                if (dm_Friend_CircleEntity.IsEmpty())
                {
                    dm_Friend_CircleEntity = dm_Friend_CircleIBLL.GetSingleCircle(id);
                    if (!dm_Friend_CircleEntity.IsEmpty())
                        redisCache.Write<dm_friend_circleEntity>(cacheKey, dm_Friend_CircleEntity, DateTime.Now.AddMinutes(2), 7);
                }

                return Success("获取成功!", dm_Friend_CircleEntity);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 发布哆米圈文章
        public ActionResult PubCircle(string Content, string Images, string User_ID)
        {
            try
            {
                string appid = CheckAPPID();
                dm_Friend_CircleIBLL.PubCircle(appid, Content, Images, User_ID);

                return Success("发布成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取我的哆米圈文章
        public ActionResult GetMyCircle(string User_ID, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                Pagination pagination = new Pagination
                {
                    page = PageNo,
                    rows = PageSize,
                    sidx = "createtime",
                    sord = "desc"
                };
                string appid = CheckAPPID();
                string cacheKey = Md5Helper.Hash("GetMyCircle" + pagination.ToJson() + appid);
                DataTable dataTable = redisCache.Read(cacheKey, 7);
                if (dataTable == null)
                {
                    dataTable = dm_Friend_CircleIBLL.GetMyCircle(pagination, appid);
                    if (dataTable.Rows.Count > 0)
                    {
                        redisCache.Write(cacheKey, dataTable, DateTime.Now.AddMinutes(2), 7);
                    }
                }

                return SuccessList("获取成功!", dataTable);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
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