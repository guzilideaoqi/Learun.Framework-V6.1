using Aliyun.OSS;
using Aliyun.OSS.Common;
using Learun.Application.Base.SystemModule;
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
using System.Linq;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class DM_ExtendOperateController : MvcAPIControllerBase
    {
        private ICache redisCache = CacheFactory.CaChe();

        private DM_AnnouncementIBLL dm_AnnouncementIBLL = new DM_AnnouncementBLL();

        private DM_BannerIBLL dm_BannerIBLL = new DM_BannerBLL();

        private DM_BaseSettingIBLL dm_BaseSettingIBLL = new DM_BaseSettingBLL();

        private DM_ArticleIBLL dm_ArticleIBLL = new DM_ArticleBLL();

        private dm_level_remarkIBLL dm_Level_RemarkIBLL = new dm_level_remarkBLL();

        private AreaIBLL areaIBLL = new AreaBLL();

        private DM_OrderIBLL dM_OrderIBLL = new DM_OrderBLL();

        private dm_versionIBLL dm_VersionIBLL = new dm_versionBLL();

        private dm_activity_manageIBLL dm_Activity_ManageIBLL = new dm_activity_manageBLL();

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
        /// <summary>
        /// 获取banner图列表
        /// </summary>
        /// <param name="type">0首页导航  1京东导航  2拼多多导航</param>
        /// <returns></returns>
        public ActionResult GetBannerList(int type = 0)
        {
            try
            {
                string appid = CheckAPPID();

                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);
                int status = 1;

                if (type == 0)
                {
                    GetPreviewVersion(dm_BasesettingEntity, ref status);
                }

                return SuccessList("获取成功", dm_BannerIBLL.GetPageListByCache(new Pagination
                {
                    page = 1,
                    rows = 50
                }, type, status, appid));
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
                int Status = 0;
                if (dm_BasesettingEntity.openchecked == "1")
                { //开启审核模式
                    string version = CheckVersion();
                    string platform = CheckPlaform();
                    if ((platform == "ios" && version == dm_BasesettingEntity.previewversion) || (platform == "android" && version == dm_BasesettingEntity.previewversionandroid))
                        Status = 1;
                }

                dm_activity_manageEntity dm_Activity_ManageEntity = dm_Activity_ManageIBLL.GetActivityInfo();
                if (dm_Activity_ManageEntity.IsEmpty())
                    dm_Activity_ManageEntity = new dm_activity_manageEntity { ActivityStatus = 0 };

                return Success("获取成功", new
                {
                    //isAppStorePreview = ((base.Request.Headers["version"].ToString() == dm_BasesettingEntity.previewversion) ? 1 : 0)
                    //previewversion = dm_BasesettingEntity.previewversion,
                    ischecked = Status,  //dm_BasesettingEntity.openchecked,
                    welcomenewperson = dm_BasesettingEntity.welcomenewperson,
                    showcommission = dm_BasesettingEntity.showcommission,
                    miquan_remark = dm_BasesettingEntity.miquan_remark,
                    task_remark = "http://dlaimi.cn/dm_appmanage/dm_article/lookarticle?id=16",
                    task_submit_remark_title = "任务提交小建议",
                    task_submit_remark = dm_BasesettingEntity.task_submit_remark,
                    nodatatip = CommonConfig.NoDataTip,
                    sign_rule = dm_BasesettingEntity.sign_rule,
                    cashrecord_fee = dm_BasesettingEntity.cashrecord_fee,
                    cashrecord_remark = dm_BasesettingEntity.cashrecord_remark,
                    activitysetting = dm_Activity_ManageEntity
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

        #region 获取合伙人页面各等级说明
        public ActionResult GetLevelRemark()
        {
            try
            {
                string appid = CheckAPPID();

                string cacheKey = "LevelRemark";

                IEnumerable<dm_level_remarkEntity> dm_Level_RemarkEntities = redisCache.Read<IEnumerable<dm_level_remarkEntity>>(cacheKey, 7);
                if (dm_Level_RemarkEntities.IsEmpty() || dm_Level_RemarkEntities.Count() <= 0)
                {
                    dm_Level_RemarkEntities = dm_Level_RemarkIBLL.GetList("");
                    if (dm_Level_RemarkEntities.Count() > 0)
                    {
                        redisCache.Write(cacheKey, dm_Level_RemarkEntities, DateTime.Now.AddMinutes(5));
                    }
                }

                /*List<LevelRemark> JuniorRemark = new List<LevelRemark> {
                    new LevelRemark{Remark="自购分享加倍",SubRemark="",RemarkImage="" },
                    new LevelRemark{Remark="来米收益",SubRemark="" },
                    new LevelRemark{Remark="徒弟分红",SubRemark="" },
                    new LevelRemark{Remark="积分特权",SubRemark="" }
                };

                List<LevelRemark> SeniorRemark = new List<LevelRemark> {
                    new LevelRemark{Remark="收益提升62%",SubRemark="" },
                    new LevelRemark{Remark="来米收益",SubRemark="" },
                    new LevelRemark{Remark="徒弟分红",SubRemark="" },
                    new LevelRemark{Remark="积分特权",SubRemark="" },
                    new LevelRemark{Remark="发布任务",SubRemark="" },
                    new LevelRemark{Remark="合伙人权益",SubRemark="" }
                };

                List<LevelRemark> PartnerRemark = new List<LevelRemark> {
                    new LevelRemark{Remark="收益提升35倍",SubRemark="" },
                    new LevelRemark{Remark="粉丝出单",SubRemark="收益提升160%" },
                    new LevelRemark{Remark="粉丝分红",SubRemark="收益提升120%" },
                    new LevelRemark{Remark="团队收益",SubRemark="额外收益提升20倍" },
                    new LevelRemark{Remark="团队数据系统",SubRemark="独立数据 清晰运营" },
                    new LevelRemark{Remark="靓号邀请码",SubRemark="可自定义邀请码" },
                    new LevelRemark{Remark="线下活动",SubRemark="公司制订境内外旅行" },
                    new LevelRemark{Remark="免手续费",SubRemark="提现0手续费" },
                    new LevelRemark{Remark="线下体验店",SubRemark="创立线下分公司" }
                };*/

                return Success("获取成功", new { JuniorRemark = dm_Level_RemarkEntities.Where(t => t.RemarkType == 0), SeniorRemark = dm_Level_RemarkEntities.Where(t => t.RemarkType == 1), PartnerRemark = dm_Level_RemarkEntities.Where(t => t.RemarkType == 2) });
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
                dM_OrderIBLL.ExcuteSubCommission(AppID);
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
                return Fail("该接口已停用,请查看新的接口!");
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

                return Success("上传成功", new { ImageUrl = virtualPath });
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
            return Fail("该接口已停用,请查看新的接口!");
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


                    images.Add(virtualPath);
                }

                return Success("上传成功", new { ImageUrls = images });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 上传单张图片(阿里云上传)
        public ActionResult UploadImageByAliSingle()
        {
            try
            {
                string appid = CheckAPPID();
                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);

                if (System.Web.HttpContext.Current.Request.Files.Count != 1)
                    return Fail("请上传一张图片!");

                HttpPostedFile pic_file = System.Web.HttpContext.Current.Request.Files[0];
                if (pic_file.ContentLength == 0 || string.IsNullOrEmpty(pic_file.FileName))
                {
                    return HttpNotFound();
                }

                string key = DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid().ToString() + Path.GetExtension(pic_file.FileName);

                string putUrl = OSSHelper.PutObject(dm_BasesettingEntity, "", pic_file);
                //string putUrl = PutObject(dm_BasesettingEntity.oss_accesskeyid, dm_BasesettingEntity.oss_accesskeysecret, dm_BasesettingEntity.oss_endpoint, dm_BasesettingEntity.oss_buketname, key, pic_file.InputStream);

                return Success("上传成功", new { ImageUrl = putUrl });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 上传多张图片(阿里云上传)
        public ActionResult UploadImageByAliMutiple()
        {
            List<string> images = new List<string>();
            try
            {
                string appid = CheckAPPID();
                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);

                if (System.Web.HttpContext.Current.Request.Files.Count <= 0)
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


                    //string putUrl = PutObject(dm_BasesettingEntity.oss_accesskeyid, dm_BasesettingEntity.oss_accesskeysecret, dm_BasesettingEntity.oss_endpoint, dm_BasesettingEntity.oss_buketname, key, pic_file.InputStream);
                    string putUrl = OSSHelper.PutObject(dm_BasesettingEntity, "", pic_file);

                    images.Add(putUrl);
                }

                return Success("上传成功", new { ImageUrls = images });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="accessKeyId">开发者秘钥对，通过阿里云控制台的秘钥管理页面创建与管理</param>
        /// <param name="accessKeySecret">开发者秘钥对，通过阿里云控制台的秘钥管理页面创建与管理</param>
        /// <param name="endpoint">Endpoint，创建Bucket时对应的Endpoint</param>
        /// <param name="bucketName">Bucket名称，创建Bucket时对应的Bucket名称</param>
        /// <param name="key">文件标识</param>
        /// <param name="file">需要上传文件的文件路径</param>
        public static string PutObject(string accessKeyId, string accessKeySecret, string endpoint, string bucketName, string key, Stream stream)
        {
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            try
            {
                PutObjectResult putObjectResult = client.PutObject(bucketName, key, stream);
                //var uri = client.GeneratePresignedUri(bucketName, key);
                //return uri.ToString();
                return string.Format("http://{0}.{2}/{1}", bucketName, key, endpoint);
            }
            catch (OssException ex)
            {
                throw new Exception("阿里云请求异常", ex);
                //LogHelper.LogException<OssException>($"Msg:{ex.Message};Code:{ex.ErrorCode};RequestID:{ex.RequestId};HostID:{ex.HostId}");
            }
        }

        /// <summary>
        /// 获取图片地址
        /// </summary>
        /// <param name="accessKeyId">开发者秘钥对，通过阿里云控制台的秘钥管理页面创建与管理</param>
        /// <param name="accessKeySecret">开发者秘钥对，通过阿里云控制台的秘钥管理页面创建与管理</param>
        /// <param name="endpoint">Endpoint，创建Bucket时对应的Endpoint</param>
        /// <param name="bucketName">Bucket名称，创建Bucket时对应的Bucket名称</param>
        /// <param name="key">文件标识</param>
        /// <param name="width">设置图片的宽度</param>
        /// <param name="height">设置图片的高度</param>
        /// <returns></returns>
        public static string GetIamgeUri(string accessKeyId, string accessKeySecret, string endpoint, string bucketName, string key, float width = 100, float height = 100)
        {
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            try
            {
                var process = $"image/resize,m_fixed,w_{width},h_{height}";
                var req = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get)
                {
                    Expiration = DateTime.Now.AddHours(1),
                    Process = process
                };
                var uri = client.GeneratePresignedUri(req);
                return uri.ToString();
            }
            catch (OssException ex)
            {
                throw new Exception("阿里云请求异常", ex);
                //LogHelper.LogException<OssException>($"Msg:{ex.Message};Code:{ex.ErrorCode};RequestID:{ex.RequestId};HostID:{ex.HostId}");
            }
        }

        #region 获取省份/城市/区域
        /// <summary>
        /// 0获取默认省份  其他数据传入相应编号
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public ActionResult GetArea(string parentID)
        {
            try
            {
                ; string cacheKey = "AreaInfo" + parentID;
                List<AreaEntity> areaEntities = redisCache.Read<List<AreaEntity>>(cacheKey, 7);

                if (areaEntities == null)
                {
                    areaEntities = areaIBLL.GetList(parentID, true);
                    if (areaEntities.Count > 0)
                    {
                        redisCache.Write<List<AreaEntity>>(cacheKey, areaEntities, 7);
                    }
                }

                return SuccessList("获取成功!", areaEntities);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取APP
        public ActionResult GetVersionRecord()
        {
            try
            {
                dm_versionEntity dm_VersionEntity = dm_VersionIBLL.GetVersionRecord(CheckPlaform());
                return Success("获取成功!", dm_VersionEntity);
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
                throw new Exception("ceshi");
                string appid = CheckAPPID();

                return SuccessList("获取成功", jsonData);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 检查head数据
        public string CheckAPPID()
        {
            if (base.Request.Headers["appid"].IsEmpty())
            {
                throw new Exception("缺少参数appid");
            }
            return base.Request.Headers["appid"].ToString();
        }

        public string CheckPlaform()
        {
            if (base.Request.Headers["platform"].IsEmpty())
            {
                throw new Exception("缺少参数platform");
            }
            return base.Request.Headers["platform"].ToString();
        }

        public string CheckVersion()
        {
            if (base.Request.Headers["version"].IsEmpty())
            {
                throw new Exception("缺少参数version");
            }
            return base.Request.Headers["version"].ToString();
        }

        public void GetPreviewVersion(dm_basesettingEntity dm_BasesettingEntity, ref int Status)
        {
            if (dm_BasesettingEntity.openchecked == "1")
            { //开启审核模式
                string version = CheckVersion();
                string platform = CheckPlaform();
                if ((platform == "ios" && version == dm_BasesettingEntity.previewversion) || (platform == "android" && version == dm_BasesettingEntity.previewversionandroid))
                    Status = 2;
            }
        }
        #endregion
    }

    public class LevelRemark
    {
        public string Remark { get; set; }
        public string SubRemark { get; set; }

        public string RemarkImage { get; set; }
    }
}
