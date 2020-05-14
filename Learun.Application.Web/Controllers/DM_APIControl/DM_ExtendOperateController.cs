using Learun.Application.TwoDevelopment.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class DM_ExtendOperateController : MvcAPIControllerBase
    {
        private ICache redisCache = CacheFactory.CaChe();

        private DM_AnnouncementIBLL dm_AnnouncementIBLL = new DM_AnnouncementBLL();

        private DM_BannerIBLL dm_BannerIBLL = new DM_BannerBLL();

        private DM_BaseSettingIBLL dm_BaseSettingIBLL = new DM_BaseSettingBLL();

        private DM_ArticleIBLL dm_ArticleIBLL = new DM_ArticleBLL();

        #region 获取平台设置
        public ActionResult GetPlaformSetting()
        {
            try
            {
                string appid = CheckAPPID();
                return Success("获取成功", dm_BaseSettingIBLL.GetEntityByCache(appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取公告列表
        public ActionResult GetAnnouncementList()
        {
            try
            {
                string appid = CheckAPPID();
                return SuccessList("获取成功", dm_AnnouncementIBLL.GetPageListByCache(new Pagination
                {
                    page = 1,
                    rows = 50
                }, appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取banner图列表
        public ActionResult GetBannerList(int type = 0)
        {
            try
            {
                string appid = CheckAPPID();
                return SuccessList("获取成功", dm_BannerIBLL.GetPageListByCache(new Pagination
                {
                    page = 1,
                    rows = 50
                }, type, appid));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取公用的配置信息
        public ActionResult GetCommonSetting()
        {
            try
            {
                string appid = CheckAPPID();
                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);
                return Success("获取成功", new
                {
                    isAppStorePreview = ((base.Request.Headers["version"].ToString() == dm_BasesettingEntity.previewversion) ? 1 : 0)
                });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取文章详情
        public ActionResult GetArticleDetail(int id)
        {
            try
            {
                string appid = CheckAPPID();

                return Success("获取成功", dm_ArticleIBLL.GetArticleDetail(appid, id));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取新人教程
        public ActionResult GetNewPersonHelp()
        {
            try
            {
                string appid = CheckAPPID();

                return SuccessList("获取成功", dm_ArticleIBLL.GetChildrenArticle(appid, 1));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 帮助中心
        public ActionResult GetHelpCenter()
        {
            try
            {
                string appid = CheckAPPID();

                return SuccessList("获取成功", dm_ArticleIBLL.GetChildrenArticle(appid, 8));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 执行返利
        /// <summary>
        /// 执行返利
        /// </summary>
        /// <param name="AppID">appid</param>
        /// <returns></returns>
        public ActionResult ExcuteSubCommission(string AppID)
        {
            try
            {
                string appid = CheckAPPID();
                return Success("返利执行成功");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 上传单张图片
        public ActionResult UploadImageBySingle()
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.Count != 1)
                    return Fail("请上传一张图片!");
                #region 头像上传
                HttpPostedFile pic_file = System.Web.HttpContext.Current.Request.Files[0];
                if (pic_file.ContentLength == 0 || string.IsNullOrEmpty(pic_file.FileName))
                {
                    return HttpNotFound();
                }
                #endregion

                string FileEextension = Path.GetExtension(pic_file.FileName);
                string virtualPath = $"/Resource/CommonImage/{Guid.NewGuid().ToString()}{FileEextension}";
                string fullFileName = base.Server.MapPath("~" + virtualPath);
                string path = Path.GetDirectoryName(fullFileName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                pic_file.SaveAs(fullFileName);


                return Success("上传成功", new { ImageUrl = CommonConfig.ImageQianZhui + virtualPath });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 上传多张图片
        public ActionResult UploadImageByMutiple()
        {
            List<string> images = new List<string>();
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.Count <= 1)
                    return Fail("请上传至少一张图片!");

                HttpFileCollection httpFileCollection = System.Web.HttpContext.Current.Request.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    HttpPostedFile pic_file = httpFileCollection[i];

                    #region 头像上传
                    if (pic_file.ContentLength == 0 || string.IsNullOrEmpty(pic_file.FileName))
                    {
                        return HttpNotFound();
                    }
                    #endregion

                    string FileEextension = Path.GetExtension(pic_file.FileName);
                    string virtualPath = $"/Resource/CommonImage/{Guid.NewGuid().ToString()}{FileEextension}";
                    string fullFileName = base.Server.MapPath("~" + virtualPath);
                    string path = Path.GetDirectoryName(fullFileName);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    pic_file.SaveAs(fullFileName);


                    images.Add(CommonConfig.ImageQianZhui + virtualPath);
                }

                return Success("上传成功", new { ImageUrls = images });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 接口测试
        public ActionResult TestApi(string jsonData)
        {
            try
            {
                string appid = CheckAPPID();

                return SuccessList("获取成功", jsonData);
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
