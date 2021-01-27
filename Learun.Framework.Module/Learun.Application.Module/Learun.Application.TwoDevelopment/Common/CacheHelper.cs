/*===================================Copyright© 2020 xxx Ltd. All rights reserved.==============================

文件类名 : CacheHelper.cs
创建人员 : Mr.Hu
创建时间 : 2020-11-09 10:53:26 
备注说明 : 缓存公用处理类

 =====================================End=======================================================*/
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Cache.Base;
using Learun.Cache.Factory;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.Common
{
    /// <summary>
    /// 缓存公用处理类
    /// </summary>
    public class CacheHelper
    {
        private static ICache redisCache = CacheFactory.CaChe();
        const string SingleLogin = "SingleLogin_";
        /// <summary>
        /// 写入用户信息
        /// </summary>
        /// <param name="oldToken">原有用户信息的token</param>
        /// <param name="dm_UserEntity">里面的token是最新的</param>
        public static void SaveUserInfo(string oldToken, dm_userEntity dm_UserEntity)
        {
            #region 重新构造用户缓存信息
            if (!dm_UserEntity.IsEmpty())
            {
                string cacheKey = SingleLogin + dm_UserEntity.token;
                redisCache.Write<dm_userEntity>(cacheKey, dm_UserEntity, 7);

                #region 移除用户信息
                if (!oldToken.IsEmpty())
                {
                    string old_cacheKey = SingleLogin + oldToken;
                    redisCache.Remove(old_cacheKey, 7);
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 读取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static dm_userEntity ReadUserInfo(NameValueCollection header)
        {
            if (!header["token"].IsEmpty())
            {
                string token = header["token"].ToString();

                dm_userEntity dm_UserEntity = ReadUserInfoByToken(token);

                return dm_UserEntity;
            }
            else
            {
                return null;
            }
        }

        public static dm_userEntity ReadUserInfoByToken(string token)
        {
            string cacheKey = SingleLogin + token;
            dm_userEntity dm_UserEntity = redisCache.Read<dm_userEntity>(cacheKey, 7);

            return dm_UserEntity;
        }

        /// <summary>
        /// 更新缓存中的用户信息
        /// </summary>
        /// <param name="dm_UserEntity"></param>
        public static void UpdateUserInfo(dm_userEntity dm_UserEntity)
        {
            string cacheKey = SingleLogin + dm_UserEntity.token;
            redisCache.Write<dm_userEntity>(cacheKey, dm_UserEntity, 7);
        }
    }
}
