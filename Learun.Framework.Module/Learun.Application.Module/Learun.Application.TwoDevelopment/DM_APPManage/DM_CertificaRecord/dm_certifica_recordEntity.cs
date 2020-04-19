using Learun.Application.TwoDevelopment.Common;
using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-06 21:08
    /// 描 述：身份证实名
    /// </summary>
    public class dm_certifica_recordEntity
    {
        #region 实体成员
        /// <summary>
        /// 记录id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        /// <returns></returns>
        [Column("USER_ID")]
        public int? user_id { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        /// <returns></returns>
        [Column("REALNAME")]
        public string realname { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        /// <returns></returns>
        [Column("CARDNO")]
        public string cardno { get; set; }

        private string _facecard;
        /// <summary>
        /// 身份证正面
        /// </summary>
        /// <returns></returns>
        [Column("FACECARD")]
        public string facecard { get { if (!string.IsNullOrEmpty(_facecard)) return CommonConfig.ImageQianZhui + _facecard; return _facecard; } set { _facecard = value; } }

        private string _frontcard;
        /// <summary>
        /// 身份证反面
        /// </summary>
        /// <returns></returns>
        [Column("FRONTCARD")]
        public string frontcard { get { if (!string.IsNullOrEmpty(_frontcard)) return CommonConfig.ImageQianZhui + _frontcard; return _frontcard; } set { _frontcard = value; } }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? createtime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("UPDATETIME")]
        public DateTime? updatetime { get; set; }
        /// <summary>
        /// 实名失败原因
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string remark { get; set; }
        /// <summary>
        /// 记录审核状态  0未审核  1审核通过  2审核驳回
        /// </summary>
        /// <returns></returns>
        [Column("REALSTATUS")]
        public int? realstatus { get; set; }

        /// <summary>
        /// 平台id
        /// </summary>
        [Column("APPID")]
        public string appid { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.createtime = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(int? keyValue)
        {
            this.id = keyValue;
            this.updatetime = DateTime.Now;
        }
        #endregion
    }
}

