using Learun.Cache.Base;
using System;
using System.Collections.Generic;
using System.Data;

namespace Learun.Cache.Redis
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创建人：力软-框架开发组
    /// 日 期：2017.03.06
    /// 描 述：定义缓存接口
    /// </summary>
    public class CacheByRedis : ICache
    {
        #region Key-Value
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public T Read<T>(string cacheKey, long dbId = 0) where T : class
        {
            return RedisCache.Get<T>(cacheKey, dbId);
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        public void Write<T>(string cacheKey, T value, long dbId = 0) where T : class
        {
            RedisCache.Set(cacheKey, value, dbId);
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public void Write<T>(string cacheKey, T value, DateTime expireTime, long dbId = 0) where T : class
        {
            RedisCache.Set(cacheKey, value, expireTime, dbId);
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="TimeSpan">缓存时间</param>
        public void Write<T>(string cacheKey, T value, TimeSpan timeSpan, long dbId = 0) where T : class
        {
            RedisCache.Set(cacheKey, value, timeSpan, dbId);
        }

        public void Write(string cacheKey, DataTable dataTable, DateTime expireTime, long dbId)
        {
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();//定义BinaryFormatter以序列化DataSet对象   
            System.IO.MemoryStream ms = new System.IO.MemoryStream();//创建内存流对象   
            formatter.Serialize(ms, dataTable);//把DataSet对象序列化到内存流   
            byte[] buffer = ms.ToArray();//把内存流对象写入字节数组   
            ms.Close();//关闭内存流对象   
            ms.Dispose();//释放资源  

            RedisCache.Set(cacheKey, SetBytesFormT(dataTable), expireTime, dbId);
        }

        public DataTable Read(string cacheKey, long dbId)
        {
            byte[] item = RedisCache.Get<byte[]>(cacheKey, dbId);
            if (item == null)
                return null;
            return GetObjFromBytes(item) as DataTable;
        }
        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public void Remove(string cacheKey, long dbId = 0)
        {
            RedisCache.Remove(cacheKey, dbId);
        }
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public void RemoveAll(long dbId = 0)
        {
            RedisCache.RemoveAll(dbId);
        }
        #endregion

        #region redis中处理DataTable
        byte[] SetBytesFormT(DataTable t)
        {
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();//定义BinaryFormatter以序列化DataSet对象   
            System.IO.MemoryStream ms = new System.IO.MemoryStream();//创建内存流对象   
            formatter.Serialize(ms, t);//把DataSet对象序列化到内存流   
            byte[] buffer = ms.ToArray();//把内存流对象写入字节数组   
            ms.Close();//关闭内存流对象   
            ms.Dispose();//释放资源   
            return buffer;
        }

        object GetObjFromBytes(byte[] buffer)
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer))
            {
                stream.Position = 0;
                System.Runtime.Serialization.IFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Object reobj = bf.Deserialize(stream);
                return reobj;
            }
        }
        #endregion
    }
}
