using Learun.Application.Excel;
using System.Collections.Generic;
using System.Web.Mvc;
using Learun.Util;
using System.Data;
using Learun.Application.Base.SystemModule;
using System;
using System.Drawing;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Cache.Base;
using Learun.Cache.Factory;

namespace Learun.Application.Web.Areas.LR_SystemModule.Controllers
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2017.04.01
    /// 描 述：Excel导入管理
    /// </summary>
    public class ExcelImportController : MvcControllerBase
    {
        private ExcelImportIBLL excelImportIBLL = new ExcelImportBLL();
        private AnnexesFileIBLL annexesFileIBLL = new AnnexesFileBLL();

        private DM_UserIBLL dm_UserBLL = new DM_UserBLL();
        #region 缓存定义
        private ICache cache = CacheFactory.CaChe();
        private string cacheKey = "learun_adms_excelError_";       // +公司主键
        #endregion
        #region 视图功能
        /// <summary>
        /// 导入模板管理页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 导入模板管理表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 设置字段属性
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetFieldForm()
        {
            return View();
        }

        /// <summary>
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportForm()
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetPageList(string pagination, string queryJson)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = excelImportIBLL.GetPageList(paginationobj, queryJson);
            var jsonData = new
            {
                rows = data,
                total = paginationobj.total,
                page = paginationobj.page,
                records = paginationobj.records,
            };
            return Success(jsonData);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="moduleId">功能模块主键</param>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetList(string moduleId)
        {
            var data = excelImportIBLL.GetList(moduleId);
            return Success(data);
        }
        /// <summary>
        /// 获取表单数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult GetFormData(string keyValue)
        {
            ExcelImportEntity entity = excelImportIBLL.GetEntity(keyValue);
            IEnumerable<ExcelImportFieldEntity> list = excelImportIBLL.GetFieldList(keyValue);
            var data = new
            {
                entity = entity,
                list = list
            };
            return Success(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string strEntity, string strList)
        {
            ExcelImportEntity entity = strEntity.ToObject<ExcelImportEntity>();
            List<ExcelImportFieldEntity> filedList = strList.ToObject<List<ExcelImportFieldEntity>>();
            excelImportIBLL.SaveEntity(keyValue, entity, filedList);
            return Success("保存成功！");
        }
        /// <summary>
        /// 删除表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            excelImportIBLL.DeleteEntity(keyValue);
            return Success("删除成功！");
        }
        /// <summary>
        /// 更新表单数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateForm(string keyValue, ExcelImportEntity entity)
        {
            excelImportIBLL.UpdateEntity(keyValue, entity);
            return Success("操作成功！");
        }
        #endregion

        #region 扩展方法
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DownSchemeFile(string keyValue)
        {
            ExcelImportEntity templateInfo = excelImportIBLL.GetEntity(keyValue);
            IEnumerable<ExcelImportFieldEntity> fileds = excelImportIBLL.GetFieldList(keyValue);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.FileName = Server.UrlDecode(templateInfo.F_Name) + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            //表头
            DataTable dt = new DataTable();
            foreach (var col in fileds)
            {
                if (col.F_RelationType != 1 && col.F_RelationType != 4 && col.F_RelationType != 5 && col.F_RelationType != 6 && col.F_RelationType != 7)
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = col.F_Name,
                        ExcelColumn = col.F_ColName,
                        Alignment = "center",
                    });
                    dt.Columns.Add(col.F_Name, typeof(string));
                }
            }
            ExcelHelper.ExcelDownload(dt, excelconfig);
        }

        /// <summary>
        /// excel文件导入（通用）
        /// </summary>
        /// <param name="templateId">模板Id</param>
        /// <param name="fileId">文件主键</param>
        /// <param name="chunks">分片数</param>
        /// <param name="ext">文件扩展名</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExecuteImportExcel(string templateId, string fileId, int chunks, string ext)
        {
            string path = annexesFileIBLL.SaveAnnexes(fileId, fileId + "." + ext, chunks);
            if (!string.IsNullOrEmpty(path))
            {
                DataTable dt = ExcelHelper.ExcelImport(path);
                dm_user_relationEntity dm_User_RelationEntity = new dm_user_relationEntity();
                if (templateId == "a21c6d88-16bf-489d-a019-96047431f0b4")
                {
                    int snum = 0;
                    int fnum = 0;
                    DataTable failDt = new DataTable();
                    dt.Columns.Add("导入错误", typeof(string));
                    foreach (DataColumn dc in dt.Columns)
                    {
                        failDt.Columns.Add(dc.ColumnName, dc.DataType);
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dataRow = dt.Rows[i];

                        if (dataRow[0].IsEmpty())
                            continue;
                        string Phone = dataRow[0].ToString();//手机号
                        string RealName = ConvertEmpty(dataRow[1]);//真实姓名
                        string NickName = ConvertEmpty(dataRow[2]);//用户昵称
                        string CardNo = ConvertEmpty(dataRow[3]);//身份证号
                        string UserLevel = ConvertEmpty(dataRow[4]);//用户等级
                        string InviteCode = ConvertEmpty(dataRow[5]);//邀请码
                        string Province = ConvertEmpty(dataRow[6]);//省份
                        string City = ConvertEmpty(dataRow[7]);//城市
                        string Down = ConvertEmpty(dataRow[8]);//区域
                        string DetailAddress = ConvertEmpty(dataRow[9]);//详细地址
                        string WeChat = ConvertEmpty(dataRow[10]);//微信号

                        int? partner_id = 0;
                        dm_userEntity dm_UserEntity = dm_UserBLL.GetEntityByInviteCode(InviteCode, ref dm_User_RelationEntity);
                        if (!dm_UserEntity.IsEmpty() && !dm_User_RelationEntity.IsEmpty())
                        {
                            if (dm_UserEntity.partnersstatus == 2)
                            {
                                partner_id = dm_UserEntity.partners;//如果邀请码对应的用户是合伙人 则合伙人编号直接用该用户的
                            }
                            else
                            {
                                partner_id = dm_User_RelationEntity.partners_id;//如果上级非合伙人则继承上级合伙人
                            }

                            UserInfo userInfo = LoginUserInfo.Get();
                            if (dm_UserBLL.ImportUserInfo(userInfo.userId, Phone, RealName, NickName, CardNo, UserLevel, Province, City, Down, DetailAddress, WeChat, dm_User_RelationEntity.parent_id.ToString(), dm_User_RelationEntity.parent_nickname, partner_id.ToString()))
                            {
                                snum++;
                            }
                            else
                            {
                                fnum++;
                                failDt.Rows.Add(dataRow.ItemArray);
                            }
                        }
                        else
                            continue;
                    }

                    var data = new
                    {
                        Success = snum,
                        Fail = fnum
                    };

                    // 写入缓存如果有未导入的数据
                    if (failDt.Rows.Count > 0)
                    {
                        string errordt = failDt.ToJson();

                        cache.Write<string>(cacheKey + fileId, errordt, CacheId.excel);
                    }

                    return Success(data);
                }
                else
                {
                    string res = excelImportIBLL.ImportTable(templateId, fileId, dt);
                    var data = new
                    {
                        Success = res.Split('|')[0],
                        Fail = res.Split('|')[1]
                    };

                    return Success(data);
                }
            }
            else
            {
                return Fail("导入数据失败!");
            }
        }

        /// <summary>
        /// 下载文件(导入文件未被导入的数据)
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DownImportErrorFile(string fileId, string fileName)
        {
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.FileName = Server.UrlDecode("未导入错误数据【" + fileName + "】") + ".xls";
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            //表头
            DataTable dt = excelImportIBLL.GetImportError(fileId);
            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == "导入错误")
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = col.ColumnName,
                        ExcelColumn = col.ColumnName,
                        Alignment = "center",
                        Background = Color.Red
                    });
                }
                else
                {
                    excelconfig.ColumnEntity.Add(new ColumnModel()
                    {
                        Column = col.ColumnName,
                        ExcelColumn = col.ColumnName,
                        Alignment = "center",
                    });
                }


            }
            ExcelHelper.ExcelDownload(dt, excelconfig);
        }
        #endregion

        #region 非空数据转换
        string ConvertEmpty(object obj)
        {
            return obj.IsEmpty() ? "" : obj.ToString();
        }
        #endregion
    }
}