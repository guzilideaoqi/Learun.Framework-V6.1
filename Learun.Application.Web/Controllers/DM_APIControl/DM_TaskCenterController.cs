using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Application.Web.App_Start._01_Handler;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using OracleInternal.Secure.Network;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class DM_TaskCenterController : MvcAPIControllerBase
    {
        private ICache redisCache = CacheFactory.CaChe();

        private DM_ReadTaskIBLL dM_ReadTaskIBLL = new DM_ReadTaskBLL();//阅赚任务模块

        private DM_BaseSettingIBLL dM_BaseSettingIBLL = new DM_BaseSettingBLL();//基础配置信息

        private DM_TaskIBLL dm_TaskIBLL = new DM_TaskBLL();//任务模块

        private DM_Task_ReviceIBLL dm_Task_ReviceIBLL = new DM_Task_ReviceBLL();//任务接受记录

        private DM_Task_TypeIBLL dm_Task_TypeIBLL = new DM_Task_TypeBLL();//任务类型

        private DM_Task_ReportIBLL dm_Task_ReportIBLL = new DM_Task_ReportBLL();//任务举报

        private DM_Task_TemplateIBLL dm_Task_TemplateIBLL = new DM_Task_TemplateBLL();//任务模板

        #region 获取任务类型
        public ActionResult GetTaskType()
        {
            try
            {
                string appid = CheckAPPID();
                return SuccessList("获取成功!", dm_Task_TypeIBLL.GetList("{\"appid\":\"" + appid + "\"}"));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取任务列表
        public ActionResult GetTaskList(int PageNo = 1, int PageSize = 20, string TaskType = "-1")
        {
            try
            {
                string appid = CheckAPPID();

                string queryDiction = "";
                if (TaskType == "-1")
                {
                    queryDiction = "{\"appid\":\"" + appid + "\"}";
                }
                else
                {
                    queryDiction = "{\"appid\":\"" + appid + "\",\"task_type\":\"" + TaskType + "\"}";
                }

                return SuccessList("获取成功!", dm_TaskIBLL.GetPageList(new Pagination
                {
                    page = PageNo,
                    rows = PageSize,
                    sidx = "sort",
                    sord = "desc"
                }, queryDiction));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 发布任务
        public ActionResult ReleaseTask(dm_taskEntity dm_TaskEntity)
        {
            try
            {
                string appid = CheckAPPID();
                dm_TaskEntity.appid = appid;
                dm_TaskIBLL.ReleaseTask(dm_TaskEntity);
                return Success("发布成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 取消任务(发布方)
        public ActionResult CancelByReleasePerson(int TaskID)
        {
            try
            {
                dm_TaskIBLL.CancelByReleasePerson(TaskID);
                return Success("取消成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 取消任务(接受方)
        public ActionResult CancelByRevicePerson(int ReviceID)
        {
            try
            {
                dm_Task_ReviceIBLL.CancelByRevicePerson(ReviceID);
                return Success("取消成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 接受任务
        public ActionResult ReviceTask(dm_task_reviceEntity dm_Task_ReviceEntity)
        {
            try
            {
                string appid = CheckAPPID();

                dm_Task_ReviceIBLL.ReviceTask(dm_Task_ReviceEntity, appid);
                return Success("接受成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 提交资料
        public ActionResult SubmitMeans(dm_task_reviceEntity dm_Task_ReviceEntity)
        {
            try
            {
                dm_Task_ReviceIBLL.SubmitMeans(dm_Task_ReviceEntity);
                return Success("资料提交成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 任务审核
        public ActionResult AuditTask(int ReviceID)
        {
            try
            {
                dm_Task_ReviceIBLL.AuditTask(ReviceID);
                return Success("审核成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取我的发布
        public ActionResult GetMyReleaseTask(int user_id, int PageNo = 1, int PageSize = 20, int TaskStatus = -1)
        {
            try
            {
                string cacheKey = "MyReleaseTask" + Md5Helper.Hash(user_id + "" + PageNo + "" + PageSize + "" + TaskStatus);
                IEnumerable<dm_taskEntity> dm_TaskEntities = redisCache.Read<IEnumerable<dm_taskEntity>>(cacheKey, 7);
                if (dm_TaskEntities == null)
                {
                    if (TaskStatus == -1)
                        dm_TaskEntities = dm_TaskIBLL.GetPageList(new Pagination { page = PageNo, rows = PageSize, sidx = "createtime", sord = "desc" }, "{\"user_id\":\"" + user_id + "\"}");
                    else
                        dm_TaskEntities = dm_TaskIBLL.GetPageList(new Pagination { page = PageNo, rows = PageSize, sidx = "createtime", sord = "desc" }, "{\"user_id\":\"" + user_id + "\",\"taskstatus\":\"" + TaskStatus + "\"}");

                    if (dm_TaskEntities.Count() > 0)
                    {
                        if (dm_TaskEntities.Count() < PageSize)
                            redisCache.Write<IEnumerable<dm_taskEntity>>(cacheKey, dm_TaskEntities, DateTime.Now.AddMinutes(5), 7);
                        else
                            redisCache.Write<IEnumerable<dm_taskEntity>>(cacheKey, dm_TaskEntities, DateTime.Now.AddHours(2), 7);
                    }
                }
                return SuccessList("获取成功!", dm_TaskEntities);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取我的接受
        public ActionResult GetMyReviceTask(int user_id, int PageNo = 1, int PageSize = 20, int TaskStatus = -1)
        {
            try
            {
                string cacheKey = "MyReviceTask" + Md5Helper.Hash(user_id + "" + PageNo + "" + PageSize + "" + TaskStatus);
                DataTable dataTable = redisCache.Read(cacheKey, 7);
                if (dataTable == null)
                {
                    dataTable = dm_Task_ReviceIBLL.GetMyReviceTask(user_id, TaskStatus, new Pagination { page = PageNo, rows = PageSize, sidx = "createtime", sord = "desc" });
                    int datarow = dataTable.Rows.Count;
                    if (datarow > 0)
                    {
                        if (datarow < PageSize)
                        {
                            redisCache.Write(cacheKey, dataTable, DateTime.Now.AddMinutes(5), 7);
                        }
                        else
                        {
                            redisCache.Write(cacheKey, dataTable, DateTime.Now.AddHours(2), 7);
                        }
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

        #region 获取任务详情
        public ActionResult GetTaskDetail(int task_id, int user_id)
        {
            try
            {
                var obj = new { TaskInfo = dm_TaskIBLL.GetEntity(task_id), ReviceInfo = dm_Task_ReviceIBLL.GetReviceEntity(user_id, task_id) };
                return Success("获取成功!", obj);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 任务举报
        public ActionResult SubmitTaskReport(dm_task_reportEntity dm_Task_ReportEntity)
        {
            try
            {
                string appid = CheckAPPID();
                dm_Task_ReportEntity.appid = appid;
                dm_Task_ReportIBLL.SubmitTaskReport(dm_Task_ReportEntity);
                return Success("提交成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 新增任务模板
        public ActionResult CreateTaskTemplate(dm_task_templateEntity dm_Task_TemplateEntity)
        {
            try
            {
                dm_Task_TemplateIBLL.SaveEntity(0, dm_Task_TemplateEntity);

                #region 清除任务模板的缓存
                string cacheKey = "TaskTemplate" + dm_Task_TemplateEntity.user_id;
                redisCache.Remove(cacheKey, 7);
                #endregion

                return Success("保存成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 修改任务模板
        public ActionResult UpdateTaskTemplate(int TemplateID, dm_task_templateEntity dm_Task_TemplateEntity)
        {
            try
            {
                dm_Task_TemplateIBLL.SaveEntity(TemplateID, dm_Task_TemplateEntity);

                #region 清除任务模板的缓存
                string cacheKey = "TaskTemplate" + dm_Task_TemplateEntity.user_id;
                redisCache.Remove(cacheKey, 7);
                #endregion

                return Success("修改成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取任务模板
        public ActionResult GetTaskTemplate(int User_ID)
        {
            try
            {
                string appid = CheckAPPID();
                string cacheKey = "TaskTemplate" + User_ID;
                IEnumerable<dm_task_templateEntity> dm_Task_TemplateEntities = redisCache.Read<IEnumerable<dm_task_templateEntity>>(cacheKey, 7);
                if (dm_Task_TemplateEntities == null)
                {
                    dm_Task_TemplateEntities = dm_Task_TemplateIBLL.GetList("{\"User_ID\":\"" + User_ID + "\"}");
                    if (dm_Task_TemplateEntities.Count() > 0)
                    {
                        redisCache.Write<IEnumerable<dm_task_templateEntity>>(cacheKey, dm_Task_TemplateEntities, 7);
                    }
                }

                return SuccessList("获取成功!", dm_Task_TemplateEntities);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 删除任务模板
        public ActionResult DeleteTaskTemplate(int TemplateID, int User_ID)
        {
            try
            {
                dm_Task_TemplateIBLL.DeleteEntity(TemplateID);

                #region 清除任务模板的缓存
                string cacheKey = "TaskTemplate" + User_ID;
                redisCache.Remove(cacheKey, 7);
                #endregion

                return Success("删除成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 获取阅赚任务
        public ActionResult GetReadEarnTaskList(int PageNo, int PageSize)
        {
            try
            {
                string appid = CheckAPPID();
                return SuccessList("获取成功", dM_ReadTaskIBLL.GetPageListByCache(new Pagination
                {
                    page = PageNo,
                    rows = PageSize
                }, appid), new { HeadImage = "http://dlm-appmanage.oss-cn-beijing.aliyuncs.com/20200523/81ce199c-a046-494f-8101-57a9da4e0724.png" });
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }
        #endregion

        #region 增加任务点击次数
        public ActionResult AddClickReadEarnTask(int id)
        {
            try
            {
                string appid = CheckAPPID();
                dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingIBLL.GetEntityByCache(appid);
                Random random = new Random();
                int clickCount = random.Next(dm_BasesettingEntity.readtask_min.ToInt(), dm_BasesettingEntity.readtask_max.ToInt());
                dM_ReadTaskIBLL.AddClickReadEarnTask(id, clickCount, appid);
                return Success("点击成功", new
                {
                    ClickCount = clickCount
                });
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
