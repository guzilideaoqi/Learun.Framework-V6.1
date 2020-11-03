using HYG.CommonHelper.Common;
using Learun.Application.TwoDevelopment.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-10-28 10:06
    /// 描 述：官推文案
    /// </summary>
    public class dm_friend_circleController : MvcControllerBase
    {
        private dm_friend_circleIBLL dm_friend_circleIBLL = new dm_friend_circleBLL();
        private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();

        #region 视图功能

        /// <summary>
        /// 主页面
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PreviewCircle()
        {
            return View();
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetList(string queryJson)
        {
            var data = dm_friend_circleIBLL.GetList(queryJson);
            return Success(data);
        }
        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = dm_friend_circleIBLL.GetPageList(paginationobj, queryJson);
            var jsonData = new
            {
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取表单数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormData(int keyValue)
        {
            var data = dm_friend_circleIBLL.GetEntity(keyValue);
            return Success(data);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DeleteForm(int keyValue)
        {
            dm_friend_circleIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }

        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult Up(int keyValue)
        {
            dm_friend_circleEntity dm_Friend_CircleEntity = dm_friend_circleIBLL.GetEntity(keyValue);
            dm_Friend_CircleEntity.t_status = 1;
            dm_friend_circleIBLL.SaveEntity(keyValue, dm_Friend_CircleEntity);
            return Success("上架成功!");
        }


        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult Down(int keyValue)
        {
            dm_friend_circleEntity dm_Friend_CircleEntity = dm_friend_circleIBLL.GetEntity(keyValue);
            dm_Friend_CircleEntity.t_status = 2;
            dm_friend_circleIBLL.SaveEntity(keyValue, dm_Friend_CircleEntity);
            return Success("下架成功!");
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(int keyValue, dm_friend_circleEntity entity)
        {
            dm_friend_circleIBLL.SaveEntity(keyValue, entity);
            return Success("保存成功！");
        }
        #endregion


        [HttpPost]
        public ActionResult UploadFile1(int keyValue, string ImgBase64, dm_friend_circleEntity entity)
        {
            string[] files = ImgBase64.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (files.Length > 0)
            {
                UserInfo userInfo = LoginUserInfo.Get();
                userInfo.companyId = "e2b3ec3a-310b-4ab8-aa81-b563ac8f3006";

                List<CircleImage> imageList = new List<CircleImage>();
                for (int i = 0; i < files.Length; i++)
                {
                    string image = OSSHelper.PutBase64(dM_BaseSettingIBLL.GetEntityByCache(userInfo.companyId), "", files[i]);
                    imageList.Add(new CircleImage
                    {
                        Image = image,
                        ThumbnailImage = image + "?x-oss-process=image/resize,w_100,m_lfit"//
                    });
                }

                entity.createcode = userInfo.userId;
                entity.t_status = 1;
                entity.t_type = 1;
                entity.t_images = Newtonsoft.Json.JsonConvert.SerializeObject(imageList);
                dm_friend_circleIBLL.SaveEntity(keyValue, entity);
            }
            return Success("保存成功。");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadFile(int keyValue, string ImgBase64, dm_friend_circleEntity entity)
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
                {
                    return HttpNotFound();
                }

                UserInfo userInfo = LoginUserInfo.Get();
                userInfo.companyId = "e2b3ec3a-310b-4ab8-aa81-b563ac8f3006";
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(userInfo.companyId);

                string[] files_base64 = ImgBase64.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                List<CircleImage> imageList = new List<CircleImage>();
                for (int i = 0; i < files_base64.Length; i++)
                {
                    string image = OSSHelper.PutBase64(dm_BasesettingEntity, "", files_base64[i]);
                    imageList.Add(new CircleImage
                    {
                        Image = image,
                        ThumbnailImage = image + "?x-oss-process=image/resize,w_100,m_lfit"//
                    });
                }

                entity.t_title_page = OSSHelper.PutObject(dm_BasesettingEntity, "", files[0]);

                entity.createcode = userInfo.userId;
                entity.t_status = 1;
                entity.t_type = 1;
                entity.t_images = Newtonsoft.Json.JsonConvert.SerializeObject(imageList);
                dm_friend_circleIBLL.SaveEntity(keyValue, entity);
                return Success("保存成功。");
            }
            else
            {
                return Fail("请上传封面图片!");
            }
        }
    }

    public class CircleImage
    {
        public string Image { get; set; }

        public string ThumbnailImage { get; set; }
    }
}
