using Learun.Application.TwoDevelopment.Common;
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

        private dm_friend_thumb_recordIBLL dm_Friend_Thumb_RecordIBLL = new dm_friend_thumb_recordBLL();

        #region 获取哆米圈官推文章
        public ActionResult GetCircleByGovernment(int PageNo = 1, int PageSize = 20)
        {
            try
            {
                #region 获取哆米圈官方文章
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
                #endregion

                return SuccessList("获取成功!", GeneralPraise(dm_Friend_CircleEntities));
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
        public ActionResult PubCircle(string Content, string Images, int User_ID)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();
                string appid = CheckAPPID();
                dm_Friend_CircleIBLL.PubCircle(appid, Content, Images, User_ID.ToString());

                return Success("发布成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取我的哆米圈文章
        public ActionResult GetMyCircle(int User_ID, int PageNo = 1, int PageSize = 20)
        {
            try
            {
                if (User_ID <= 0) return FailNoLogin();
                Pagination pagination = new Pagination
                {
                    page = PageNo,
                    rows = PageSize,
                    sidx = "createtime",
                    sord = "desc"
                };
                string cacheKey = Md5Helper.Hash("GetMyCircle" + pagination.ToJson() + User_ID);
                DataTable dataTable = redisCache.Read(cacheKey, 7);
                if (dataTable == null)
                {
                    dataTable = dm_Friend_CircleIBLL.GetMyCircle(pagination, User_ID.ToString());
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

        #region 构造点赞信息
        List<dynamic> GeneralPraise(IEnumerable<dm_friend_circleEntity> dm_Friend_CircleEntities)
        {
            //获取用户信息
            dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfo(base.Request.Headers);

            #region 构造点赞信息
            List<int> friend_ids = dm_Friend_CircleEntities.Select(t => t.id).ToList();
            DataTable dataTable = null;
            IEnumerable<dm_friend_thumb_recordEntity> dm_friend_thumb_recordList = null;
            if (friend_ids.Count > 0)
            {
                #region 获取哆米圈文章的点赞记录
                dataTable = dm_Friend_Thumb_RecordIBLL.GetPraiseRecord(friend_ids);
                #endregion

                #region 获取我的点赞情况
                dm_friend_thumb_recordList = dm_Friend_Thumb_RecordIBLL.GetPraiseRecord(friend_ids, (int)dm_UserEntity.id);
                #endregion
            }

            List<dynamic> dyList = new List<dynamic>();
            foreach (var item in dm_Friend_CircleEntities)
            {
                List<string> headPicList = new List<string>();
                int MyPariseStatus = 0;
                if (!dataTable.IsEmpty())
                {
                    DataRow[] dataRows = dataTable.Select(" friend_id=" + item.id);
                    foreach (DataRow itemRow in dataRows)
                    {
                        headPicList.Add(itemRow["headpic"].IsEmpty() ? "" : itemRow["headpic"].ToString());
                    }
                }

                if (!dm_friend_thumb_recordList.IsEmpty())
                {
                    dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity = dm_friend_thumb_recordList.Where(t => t.user_id == dm_UserEntity.id && t.friend_id == item.id).FirstOrDefault();
                    MyPariseStatus = dm_Friend_Thumb_RecordEntity.IsEmpty() ? 0 : (int)dm_Friend_Thumb_RecordEntity.status;
                }
                dyList.Add(new
                {
                    TemplateDetail = item,
                    PraiseRecord = headPicList,
                    MyPariseStatus = MyPariseStatus,
                    PubUserInfo = new { NickName = "哆来米", HeadPic = "" }
                });
            }
            #endregion

            return dyList;
        }
        #endregion

        #region 分享之后增加分享次数

        #endregion

        #region 点赞
        public ActionResult ClickPraise(dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity)
        {
            try
            {
                dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfo(base.Request.Headers);

                dm_Friend_Thumb_RecordEntity.user_id = dm_UserEntity.id;
                dm_Friend_Thumb_RecordIBLL.ClickPraise(dm_Friend_Thumb_RecordEntity);
                return Success("点赞成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 取消点赞
        public ActionResult CanclePraise(dm_friend_thumb_recordEntity dm_Friend_Thumb_RecordEntity)
        {
            try
            {
                dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfo(base.Request.Headers);

                dm_Friend_Thumb_RecordEntity.user_id = dm_UserEntity.id;

                dm_Friend_Thumb_RecordIBLL.CanclePraise(dm_Friend_Thumb_RecordEntity);
                return Success("已取消!");
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