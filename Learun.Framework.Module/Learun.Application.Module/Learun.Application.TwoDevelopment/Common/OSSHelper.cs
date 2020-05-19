using Aliyun.OSS;
using Aliyun.OSS.Common;
using Learun.Application.TwoDevelopment.DM_APPManage;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Learun.Application.TwoDevelopment.Common
{
    public class OSSHelper
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="accessKeyId">开发者秘钥对，通过阿里云控制台的秘钥管理页面创建与管理</param>
        /// <param name="accessKeySecret">开发者秘钥对，通过阿里云控制台的秘钥管理页面创建与管理</param>
        /// <param name="endpoint">Endpoint，创建Bucket时对应的Endpoint</param>
        /// <param name="bucketName">Bucket名称，创建Bucket时对应的Bucket名称</param>
        /// <param name="key">文件标识</param>
        /// <param name="file">需要上传文件的文件路径</param>
        public static string PutObject(dm_basesettingEntity dm_BasesettingEntity, string key, HttpPostedFile pic_file)
        {
            var client = new OssClient(dm_BasesettingEntity.oss_endpoint, dm_BasesettingEntity.oss_accesskeyid, dm_BasesettingEntity.oss_accesskeysecret);
            try
            {
                if(key.IsEmpty())
                    key = DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid().ToString() + Path.GetExtension(pic_file.FileName);

                PutObjectResult putObjectResult = client.PutObject(dm_BasesettingEntity.oss_buketname, key, pic_file.InputStream);
                //var uri = client.GeneratePresignedUri(bucketName, key);
                //return uri.ToString();
                return string.Format("http://{0}.{2}/{1}", dm_BasesettingEntity.oss_buketname, key, dm_BasesettingEntity.oss_endpoint);
            }
            catch (OssException ex)
            {
                throw new Exception("阿里云请求异常", ex);
                //LogHelper.LogException<OssException>($"Msg:{ex.Message};Code:{ex.ErrorCode};RequestID:{ex.RequestId};HostID:{ex.HostId}");
            }
        }
    }
}
