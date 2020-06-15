using Learun.Util;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Learun.Application.TwoDevelopment.DM_APPManage

{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-06 21:10
    /// 描 述：文案管理
    /// </summary>
    public class dm_articleEntity 
    {
        #region 实体成员
        /// <summary>
        /// id
        /// </summary>
        /// <returns></returns>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        [Column("ID")]
        public int? id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        [Column("TITLE")]
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string content { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public int? parentid { get; set; }
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
        /// 排序
        /// </summary>
        /// <returns></returns>
        [Column("SORT")]
        public int? sort { get; set; }

        [Column("APPID")]
        public string appid { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Column("A_IMAGE")]
        public string a_image { get; set; }

        /// <summary>
        /// 是否为软件  1是  0否
        /// </summary>
        [Column("ISSOFTARTICLE")]
        public string issoftarticle { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            UserInfo userInfo = LoginUserInfo.Get();
            createtime = DateTime.Now;
            appid = userInfo.IsEmpty()? "e2b3ec3a-310b-4ab8-aa81-b563ac8f3006" : userInfo.companyId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(int? keyValue)
        {
            this.id = keyValue;
        }
        #endregion
    }
}

