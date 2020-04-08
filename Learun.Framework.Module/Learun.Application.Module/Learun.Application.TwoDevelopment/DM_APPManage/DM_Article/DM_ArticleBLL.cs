using Learun.Util;
using System;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-06 21:10
    /// 描 述：文案管理
    /// </summary>
    public class DM_ArticleBLL : DM_ArticleIBLL
    {
        private DM_ArticleService dM_ArticleService = new DM_ArticleService();

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_articleEntity> GetList(string queryJson)
        {
            try
            {
                return dM_ArticleService.GetList(queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_articleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_ArticleService.GetPageList(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public dm_articleEntity GetEntity(int keyValue)
        {
            try
            {
                return dM_ArticleService.GetEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        #endregion

        #region 获取文章类树形结构
        /// <summary>
        /// 获取分类树形数据
        /// </summary>
        /// <returns></returns>
        public List<TreeModel> GetArticleTree()
        {
            try
            {
                UserInfo userInfo = LoginUserInfo.Get();
                IEnumerable<dm_articleEntity> classifyList = GetList("{\"appid\":\"" + userInfo.companyId + "\"}");
                List<TreeModel> treeList = new List<TreeModel>();
                foreach (var item in classifyList)
                {
                    TreeModel node = new TreeModel();
                    node.id = item.id.ToString();
                    node.text = item.title;
                    node.value = item.content.ToString();
                    node.showcheck = false;
                    node.checkstate = 0;
                    node.isexpand = true;
                    node.parentId = item.parentid.ToString();
                    treeList.Add(node);
                }
                return treeList.ToTree();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
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
        public void DeleteEntity(int keyValue)
        {
            try
            {
                dM_ArticleService.DeleteEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveEntity(int keyValue, dm_articleEntity entity)
        {
            try
            {
                dM_ArticleService.SaveEntity(keyValue, entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        #endregion


        #region 获取子类别
        /// <summary>
        /// 获取文章子类别
        /// </summary>
        /// <param name="appid">平台id</param>
        /// <param name="ModeType">1新手教程</param>
        /// <returns></returns>
        public IEnumerable<dm_articleEntity> GetChildrenArticle(string appid, int ModeType) {
            try
            {
                return dM_ArticleService.GetChildrenArticle(appid,ModeType);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        #endregion

        #region 获取文章详情
        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="appid">平台id</param>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public dm_articleEntity GetArticleDetail(string appid, int id) {
            try
            {
                return dM_ArticleService.GetArticleDetail(appid, id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }
        #endregion

    }
}
