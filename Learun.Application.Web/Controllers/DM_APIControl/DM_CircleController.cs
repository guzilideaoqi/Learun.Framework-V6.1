﻿using Learun.Application.TwoDevelopment.Common;
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

        private DM_UserIBLL dM_UserIBLL = new DM_UserBLL();

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
                List<FriendCircleEntity> friendCircleEntities = redisCache.Read<List<FriendCircleEntity>>(cacheKey, 7);
                if (friendCircleEntities == null)
                {
                    IEnumerable<dm_friend_circleEntity> dm_Friend_CircleEntities = dm_Friend_CircleIBLL.GetCircleByGovernment(pagination, appid);
                    friendCircleEntities = GeneralPraise(dm_Friend_CircleEntities, true);
                    if (friendCircleEntities.Count > 0)
                    {
                        redisCache.Write<List<FriendCircleEntity>>(cacheKey, friendCircleEntities, DateTime.Now.AddMinutes(2), 7);
                    }
                }
                #endregion

                return SuccessList("获取成功!", friendCircleEntities);
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
                List<FriendCircleEntity> friendCircleEntities = redisCache.Read<List<FriendCircleEntity>>(cacheKey, 7);
                if (friendCircleEntities == null)
                {
                    IEnumerable<dm_friend_circleEntity> dm_Friend_CircleEntities = dm_Friend_CircleIBLL.GetCircleByGeneral(pagination, appid);
                    friendCircleEntities = GeneralPraise(dm_Friend_CircleEntities);
                    if (friendCircleEntities.Count > 0)
                    {
                        redisCache.Write<List<FriendCircleEntity>>(cacheKey, friendCircleEntities, DateTime.Now.AddMinutes(2), 7);
                    }
                }

                return SuccessList("获取成功!", friendCircleEntities);
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
        public ActionResult PubCircle(string Content, string Images)
        {
            try
            {
                //获取用户信息
                dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfo(base.Request.Headers);
                if (dm_UserEntity.userlevel == 1 || dm_UserEntity.userlevel == 2)
                {
                    string appid = CheckAPPID();
                    dm_Friend_CircleIBLL.PubCircle(appid, Content, Images, dm_UserEntity.id.ToString());

                    return Success("发布成功!");
                }
                else
                {
                    throw new Exception("您的等级不足，无法发布米圈文章,请升级后重新发布!");
                }
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取我的哆米圈文章
        public ActionResult GetMyCircle(int PageNo = 1, int PageSize = 20)
        {
            try
            {
                dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfo(base.Request.Headers);
                Pagination pagination = new Pagination
                {
                    page = PageNo,
                    rows = PageSize,
                    sidx = "createtime",
                    sord = "desc"
                };
                string cacheKey = Md5Helper.Hash("GetMyCircle" + pagination.ToJson() + dm_UserEntity.id);
                List<FriendCircleEntity> friendCircleEntities = redisCache.Read<List<FriendCircleEntity>>(cacheKey, 7);
                if (friendCircleEntities == null)
                {
                    IEnumerable<dm_friend_circleEntity> dm_Friend_CircleEntities = dm_Friend_CircleIBLL.GetMyCircle(pagination, dm_UserEntity.id.ToString());
                    friendCircleEntities = GeneralPraise(dm_Friend_CircleEntities);
                    if (friendCircleEntities.Count > 0)
                    {
                        redisCache.Write<List<FriendCircleEntity>>(cacheKey, friendCircleEntities, DateTime.Now.AddMinutes(2), 7);
                    }
                }

                return SuccessList("获取成功!", friendCircleEntities);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 构造点赞信息
        List<FriendCircleEntity> GeneralPraise(IEnumerable<dm_friend_circleEntity> dm_Friend_CircleEntities, bool IsGovernment = false)
        {
            //获取用户信息
            dm_userEntity dm_UserEntity = CacheHelper.ReadUserInfo(base.Request.Headers);

            #region 构造点赞信息
            List<int> friend_ids = dm_Friend_CircleEntities.Select(t => t.id).ToList();

            DataTable dataTable = null;
            IEnumerable<dm_friend_thumb_recordEntity> dm_friend_thumb_recordList = null;
            IEnumerable<dm_userEntity> dm_UserList = null;
            if (friend_ids.Count > 0)
            {
                #region 获取哆米圈文章的点赞记录
                dataTable = dm_Friend_Thumb_RecordIBLL.GetPraiseRecord(friend_ids);
                #endregion

                #region 获取我的点赞情况
                dm_friend_thumb_recordList = dm_Friend_Thumb_RecordIBLL.GetPraiseRecord(friend_ids, (int)dm_UserEntity.id);
                #endregion

                if (!IsGovernment)
                {
                    List<string> user_ids = dm_Friend_CircleEntities.Select(t => t.createcode).Distinct().ToList();//获取任务创建人
                    dm_UserList = dM_UserIBLL.GetUserListByIDS(user_ids);
                }
            }

            List<FriendCircleEntity> dyList = new List<FriendCircleEntity>();
            foreach (var item in dm_Friend_CircleEntities)
            {
                List<string> headPicList = new List<string>();
                int MyPariseStatus = 0;
                string NickName = "", HeadPic = "";
                if (IsGovernment)
                {
                    NickName = "哆来米";
                    HeadPic = "http://dlaimi.cn/Content/Images/default.png";
                }
                else
                {
                    if (!dm_UserList.IsEmpty())
                    {
                        dm_userEntity Pub_UserEntity = dm_UserList.Where(t => item.createcode == t.id.ToString()).FirstOrDefault();
                        if (!Pub_UserEntity.IsEmpty())
                        {
                            NickName = Pub_UserEntity.nickname;
                            HeadPic = Pub_UserEntity.headpic;
                        }
                    }
                }


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
                dyList.Add(new FriendCircleEntity
                {
                    TemplateDetail = item,
                    PraiseRecord = headPicList,
                    MyPariseStatus = MyPariseStatus,
                    Pub_UserInfo = new PubUserInfo { NickName = NickName, HeadPic = HeadPic, PubTime = TimeConvert(item.createtime) }
                });
            }
            #endregion

            return dyList;
        }
        #endregion

        #region 时间处理
        string TimeConvert(DateTime PubTime)
        {
            DateTime currentTime = DateTime.Now;
            int days = (currentTime - PubTime).Days;
            if (days == 0)
            {
                return PubTime.ToString("HH:mm");
            }
            else if (days > 3)
            {
                return days + "天前";
            }
            else
            {
                return PubTime.ToString("MM月dd日");
            }
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

    public class FriendCircleEntity
    {
        /// <summary>
        /// 朋友圈文章信息
        /// </summary>
        public dm_friend_circleEntity TemplateDetail { get; set; }

        /// <summary>
        /// 点赞记录
        /// </summary>
        public List<string> PraiseRecord { get; set; }

        /// <summary>
        /// 我的点赞状态
        /// </summary>
        public int MyPariseStatus { get; set; }

        /// <summary>
        /// 发布人信息
        /// </summary>
        public PubUserInfo Pub_UserInfo { get; set; }
    }

    public class PubUserInfo
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadPic { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public string PubTime { get; set; }
    }
}