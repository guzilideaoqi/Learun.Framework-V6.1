using Dapper;
using HYG.CommonHelper.Common;
using Learun.Application.TwoDevelopment.Common;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-06-13 15:08
    /// 描 述：直播房间列表
    /// </summary>
    public class DM_MeetingListService : RepositoryFactory
    {
        #region 构造函数和属性

        private string fieldSql;
        public DM_MeetingListService()
        {
            fieldSql = @"
                t.id,
                t.subject,
                t.meeting_id,
                t.meeting_code,
                t.password,
                t.hosts,
                t.participants,
                t.start_time,
                t.end_time,
                t.join_url,
                t.settings,
                t.join_image,
                t.createtime,
                t.updatetime,
t.page_image
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_meetinglistEntity> GetList(string queryJson)
        {
            try
            {
                //参考写法
                //var queryParam = queryJson.ToJObject();
                // 虚拟参数
                //var dp = new DynamicParameters(new { });
                //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_meetinglist t ");
                return this.BaseRepository("dm_data").FindList<dm_meetinglistEntity>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_meetinglistEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_meetinglist t ");
                return this.BaseRepository("dm_data").FindList<dm_meetinglistEntity>(strSql.ToString(), pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public dm_meetinglistEntity GetEntity(int? keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_meetinglistEntity>(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void DeleteEntity(int? keyValue)
        {
            try
            {
                this.BaseRepository("dm_data").Delete<dm_meetinglistEntity>(t => t.id == keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveEntity(int? keyValue, dm_meetinglistEntity entity)
        {
            try
            {
                if (keyValue > 0)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("dm_data").Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository("dm_data").Insert(entity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #endregion

        #region 批量保存直播间列表
        public void CreateMetting(List<dm_meetinglistEntity> dm_Meetinglist)
        {
            try
            {
                this.BaseRepository("dm_data").Insert(dm_Meetinglist);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion

        #region 获取进行中的直播间
        public IEnumerable<dm_meetinglistEntity> GetMeetingList(Pagination pagination, string keyWord, int User_ID)
        {
            try
            {
                Expression<Func<dm_meetinglistEntity, bool>> expression = LinqExtensions.True<dm_meetinglistEntity>();
                expression = LinqExtensions.And<dm_meetinglistEntity>(expression, (dm_meetinglistEntity t) => t.end_time > DateTime.Now);
                if (!keyWord.IsEmpty())
                    expression = LinqExtensions.And<dm_meetinglistEntity>(expression, (dm_meetinglistEntity t) => t.subject.Contains(keyWord) || t.meeting_code.Contains(keyWord));
                if (User_ID > 0)
                    expression = LinqExtensions.And<dm_meetinglistEntity>(expression, (dm_meetinglistEntity t) => t.user_id == User_ID);

                return this.BaseRepository("dm_data").FindList<dm_meetinglistEntity>(expression, pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        #endregion

        #region 生成直播间推广二维码
        public string GeneralMeetingImage(dm_basesettingEntity dm_BasesettingEntity, string Join_Url)
        {
            try
            {
                string basePath = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd("\\".ToCharArray());
                string path1 = basePath + @"/Resource/ShareImage/MeetingDefault.png";
                string path2 = basePath + @"/Resource/ShareImage/MeetingDefault2.png";
                string path3 = basePath + @"/Resource/ShareImage/MeetingDefault3.png";

                //生成推广二维码
                Bitmap qrCode = QRCodeHelper.GenerateQRCode(Join_Url, 300, 300);

                List<string> imageList = new List<string>();
                imageList.Add(GeneralShareImage(dm_BasesettingEntity, path1, qrCode));
                imageList.Add(GeneralShareImage(dm_BasesettingEntity, path2, qrCode, 260, 1050));
                imageList.Add(GeneralShareImage(dm_BasesettingEntity, path3, qrCode, 260, 600));

                return JsonConvert.JsonSerializerIO(imageList.ToArray());
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        string GeneralShareImage(dm_basesettingEntity dm_BasesettingEntity, string bj_image_path, Bitmap qrCode, int width = 260, int height = 980)
        {
            System.Drawing.Image imgSrc = System.Drawing.Image.FromFile(bj_image_path);
            using (Graphics g = Graphics.FromImage(imgSrc))
            {
                //画专属推广二维码
                g.DrawImage(qrCode, new Rectangle(width,//-450这个数，越小越靠左，可以调整二维码在背景图的位置
                height,//同理-650越小越靠上
                qrCode.Width,
                qrCode.Height),
                0, 0, qrCode.Width, qrCode.Height, GraphicsUnit.Pixel);

                //画头像
                //g.DrawImage(titleImage, 8, 8, titleImage.Width, titleImage.Height);
            }

            string basePath = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd("\\".ToCharArray());
            string path1 = basePath + @"/Resource/ShareImage/MeetingDefault1.png";
            using (System.IO.FileStream fs = new System.IO.FileStream(path1, FileMode.Create))
            {
                imgSrc.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            string oss_url = OSSHelper.PutObject(dm_BasesettingEntity, "", path1);

            return oss_url;
            //MemoryStream mStream = new MemoryStream();
            //imgSrc.Save(mStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            //imgSrc.Save(newPath, System.Drawing.Imaging.ImageFormat.Jpeg);

        }
        #endregion
    }
}
