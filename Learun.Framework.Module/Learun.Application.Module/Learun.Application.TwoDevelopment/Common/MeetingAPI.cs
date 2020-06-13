using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.Common
{
    /// <summary>
    /// 腾讯会议API调用
    /// </summary>
    public class MeetingAPI
    {
        private string action;
        private string region;
        private string secretkey;
        private string version;
        private string token;
        private string appId;
        private string sdkId;
        private string secretId;
        private int registered = -1;
        /// <summary>
        /// 申请的安全凭证密钥对中的 secretkey ，用于签名
        /// </summary>
        public string Secretkey { get => secretkey; set => secretkey = value; }
        /// <summary>
        /// 腾讯会议分配给三方开发应用的 App ID
        /// </summary>
        public string AppId { get => appId; set => appId = value; }
        /// <summary>
        /// 用户子账号或开发的应用 ID，未分配可不填
        /// </summary>
        public string SdkId { get => sdkId; set => sdkId = value; }
        /// <summary>
        /// 申请的安全凭证密钥对中的 SecretId
        /// </summary>
        public string SecretId { get => secretId; set => secretId = value; }
        /// <summary>
        /// 非必填字段，表示是否启用了腾讯会议的企业用户管理功能。
        /// 请求头不带该字段或者该字段值为0，表示未启用企业用户管理功能。
        /// 用户使用未注册的 userid 创建的会议，在会议客户端中无法看到会议列表，可以正常使用会议短链接或会议号加入会议。
        /// 以下两种场景，请求头必须带该字段且值为1
        /// 企业用户通过 SSO 接入了腾讯会议账号体系
        /// 企业用户通过腾讯会议企业用户管理创建用户
        /// </summary>
        public int Registered { get => registered; set => registered = value; }

        public MeetingAPI()
        {
        }

        #region 
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns></returns>
        private int GetNonce()
        {
            Random rd = new Random();
            return rd.Next(10000, 99999);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private long GetTimestamp()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
            return (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数
        }
        #endregion

        public delegate void MAResult(int resultCode, dynamic resultMsg);

        #region 企业会议管理

        /// <summary>
        /// 创建会议
        /// </summary>
        /// <param name="createMeeting">创建会议对象</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int CreateMeetings(CreateMeeting createMeeting, MAResult result)
        {
            if (createMeeting == null)
                return 1;
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                string httpMethod = @"POST";
                string nonce = GetNonce().ToString();
                string timeStamp = GetTimestamp().ToString();
                Console.WriteLine("时间戳：" + timeStamp);
                string uri = @"/v1/meetings";
                //去除空值
                JsonSerializerSettings jss = new JsonSerializerSettings();
                jss.NullValueHandling = NullValueHandling.Ignore;
                string body = JsonConvert.SerializeObject(createMeeting, Formatting.Indented, jss);
                string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                    nonce, timeStamp, uri, body);

                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });

            return 0;
        }

        public string CreateMeetings(CreateMeeting createMeeting)
        {
            string httpMethod = @"POST";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            Console.WriteLine("时间戳：" + timeStamp);
            string uri = @"/v1/meetings";
            //去除空值
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.NullValueHandling = NullValueHandling.Ignore;
            string body = JsonConvert.SerializeObject(createMeeting, Formatting.Indented, jss);
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);

            return m_HttpRequestSubmit(uri, httpMethod, signature, nonce, timeStamp, body);
        }

        /// <summary>
        /// 通过会议 ID 查询会议详情
        /// </summary>
        /// <param name="meetingId">会议ID</param>
        /// <param name="userid">账号</param>
        /// <param name="instanceid">终端设备类型</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int GetMeetingsWithID(string meetingId, string userid, int instanceid, MAResult result)
        {
            if (!(meetingId != null && userid != null))
                return 1;
            string httpMethod = @"GET";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/meetings/" + meetingId + "?userid=" + userid + "&instanceid=" + instanceid;
            string body = @"";
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            Task.Run(() =>
            {
                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });

            return 0;
        }

        /// <summary>
        /// 通过会议 Code 查询
        /// </summary>
        /// <param name="meetingcode">会议 Code</param>
        /// <param name="userid">账号</param>
        /// <param name="instanceid">终端设备类型</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int GetMeetingsWithCode(string meetingcode, string userid, int instanceid, MAResult result)
        {
            if (!(meetingcode != null && userid != null))
                return 1;
            string httpMethod = @"GET";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/meetings?meeting_code=" + meetingcode + "&userid=" + userid + "&instanceid=" + instanceid;
            string body = @"";
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            Task.Run(() =>
            {
                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });

            return 0;
        }

        /// <summary>
        /// 取消会议
        /// </summary>
        /// <param name="cancelMeeting">参数</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int CancelMeetings(CancelMeeting cancelMeeting, MAResult result)
        {
            if (cancelMeeting == null)
                return 1;
            string httpMethod = @"POST";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/meetings/" + cancelMeeting.meetingId + "/cancel";
            //去除空值
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.NullValueHandling = NullValueHandling.Ignore;
            string body = JsonConvert.SerializeObject(cancelMeeting, Formatting.Indented, jss);
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            Task.Run(() =>
            {
                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });
            return 0;
        }

        /// <summary>
        /// 修改会议
        /// </summary>
        /// <param name="modifyMeeting">参数</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int ModifyMeetings(ModifyMeeting modifyMeeting, MAResult result)
        {
            if (modifyMeeting == null)
                return 1;
            string httpMethod = @"POST";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/meetings/" + modifyMeeting.meetingId;
            //去除空值
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.NullValueHandling = NullValueHandling.Ignore;
            string body = JsonConvert.SerializeObject(modifyMeeting, Formatting.Indented, jss);
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            Task.Run(() =>
            {
                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });
            return 0;
        }

        /// <summary>
        /// 获取参会成员列表
        /// </summary>
        /// <param name="meeting_id">会议的唯一 ID</param>
        /// <param name="userid">调用方用于标示用户的唯一 ID</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int GetMeetingsParticipants(string meeting_id, string userid, MAResult result)
        {
            if (meeting_id == null || userid == null)
                return 1;
            string httpMethod = @"GET";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/meetings/" + meeting_id + "/participants?userid=" + userid;
            string body = @"";
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            Task.Run(() =>
            {
                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });
            return 0;
        }

        /// <summary>
        /// 查询用户的会议列表
        /// </summary>
        /// <param name="userid">调用方用于标示用户的唯一 ID</param>
        /// <param name="instanceid">用户的终端设备类型</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int GetUserMeetings(string userid, int instanceid, MAResult result)
        {
            if (userid == null)
                return 1;
            string httpMethod = @"GET";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/meetings?userid=" + userid + "&instanceid=" + instanceid;
            string body = @"";
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            Task.Run(() =>
            {
                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });
            return 0;
        }

        #endregion

        #region 企业用户管理

        /// <summary>
        /// 创建企业用户，暂只支持国内手机用户
        /// </summary>
        /// <param name="meetingUser">用户对象</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int CreateUser(MeetingUser meetingUser, MAResult result)
        {
            if (meetingUser == null)
                return 1;
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                string httpMethod = @"POST";
                string nonce = GetNonce().ToString();
                string timeStamp = GetTimestamp().ToString();
                string uri = @"/v1/users";
                //去除空值
                JsonSerializerSettings jss = new JsonSerializerSettings();
                jss.NullValueHandling = NullValueHandling.Ignore;
                string body = JsonConvert.SerializeObject(meetingUser, Formatting.Indented, jss);
                string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                    nonce, timeStamp, uri, body);

                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });

            return 0;
        }

        public string CreateUser(MeetingUser meetingUser) {
                string httpMethod = @"POST";
                string nonce = GetNonce().ToString();
                string timeStamp = GetTimestamp().ToString();
                string uri = @"/v1/users";
                //去除空值
                JsonSerializerSettings jss = new JsonSerializerSettings();
                jss.NullValueHandling = NullValueHandling.Ignore;
                string body = JsonConvert.SerializeObject(meetingUser, Formatting.Indented, jss);
                string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                    nonce, timeStamp, uri, body);

                return m_HttpRequestSubmit(uri, httpMethod, signature,nonce, timeStamp, body);
        }

        /// <summary>
        /// 更新企业用户
        /// </summary>
        /// <param name="meetingUser">用户对象</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int UpdateUser(MeetingUser meetingUser, MAResult result)
        {
            if (meetingUser == null)
                return 1;
            string httpMethod = @"PUT";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/users/" + meetingUser.userid;
            //去除空值
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.NullValueHandling = NullValueHandling.Ignore;
            string body = JsonConvert.SerializeObject(meetingUser, Formatting.Indented, jss);
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            Task.Run(() =>
            {
                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });

            return 0;
        }

        public string UpdateUser(MeetingUser meetingUser)
        {
            if (meetingUser == null)
                return "";
            string httpMethod = @"PUT";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/users/" + meetingUser.userid;
            //去除空值
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.NullValueHandling = NullValueHandling.Ignore;
            string body = JsonConvert.SerializeObject(meetingUser, Formatting.Indented, jss);
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);

            return m_HttpRequestSubmit(uri, httpMethod, signature, nonce, timeStamp, body);
        }

        /// <summary>
        /// 获取企业用户详情
        /// </summary>
        /// <param name="userid">调用方用于标示用户的唯一 ID</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int GetUserDetail(string userid, MAResult result)
        {
            if (userid == null)
                return 1;
            string httpMethod = @"GET";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/users/" + userid;
            string body = @"";
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            m_HttpRequestSubmit(uri, httpMethod, signature,
               nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
               {
                   if (result != null)
                       result(resultCode, resultMsg);
               });

            return 0;
        }

        public string GetUserDetail(string userid)
        {
            if (userid == null)
                return "";
            string httpMethod = @"GET";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/users/" + userid;
            string body = @"";
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            return m_HttpRequestSubmit(uri, httpMethod, signature, nonce, timeStamp, body);
        }

        /// <summary>
        /// 获取企业用户列表
        /// </summary>
        /// <param name="page">当前页，默认为1</param>
        /// <param name="page_size">分页大小，默认为10，最大为20</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int GetUserList(int page, int page_size, MAResult result)
        {
            if (page < 1)
                return 1;
            if (page_size < 1 || page_size > 20)
                return 1;
            string httpMethod = @"GET";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/users/list?page=" + page + "&page_size=" + page_size;
            string body = @"";
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            Task.Run(() =>
            {
                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });

            return 0;
        }

        /// <summary>
        /// 删除企业用户
        /// </summary>
        /// <param name="userid">调用方用于标示用户的唯一 ID</param>
        /// <param name="result">结果</param>
        /// <returns>0为成功，其他失败</returns>
        public int DeleteUser(string userid, MAResult result)
        {
            if (userid == null)
                return 1;
            string httpMethod = @"DELETE";
            string nonce = GetNonce().ToString();
            string timeStamp = GetTimestamp().ToString();
            string uri = @"/v1/users/" + userid;
            string body = @"";
            string signature = MeetingSignature.MeetingSign(secretId, secretkey, httpMethod,
                nonce, timeStamp, uri, body);
            Task.Run(() =>
            {
                m_HttpRequestSubmit(uri, httpMethod, signature,
                    nonce, timeStamp, body, (int resultCode, dynamic resultMsg) =>
                    {
                        result?.Invoke(resultCode, resultMsg);
                    });
            });

            return 0;
        }

        #endregion

        /// <summary>
        /// HTTP请求
        /// </summary>
        /// <param name="strUri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="signature"></param>
        /// <param name="nonce"></param>
        /// <param name="timestamp"></param>
        /// <param name="requestBody"></param>
        /// <param name="result"></param>
        private void m_HttpRequestSubmit(string strUri, string httpMethod, string signature,
            string nonce, string timestamp,
            string requestBody, MAResult result)
        {
            string strResult = "error";
            try
            {
                string strUrl = @"https://api.meeting.qq.com" + strUri;
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                req.Host = "api.meeting.qq.com";

                byte[] bs = Encoding.UTF8.GetBytes(requestBody);
                string responseData = System.String.Empty;

                req.Method = httpMethod;
                if (httpMethod == @"POST" || httpMethod == @"PUT")
                {
                    req.ContentLength = bs.Length;
                }

                req.Headers.Add("AppId", appId ?? @"");
                req.ContentType = "application/json";
                if (sdkId != null && sdkId.Length > 0)
                    req.Headers.Add("SdkId", sdkId);
                req.Headers.Add("X-TC-Key", secretId ?? @"");
                req.Headers.Add("X-TC-Nonce", nonce.ToString());
                if (registered > -1)
                    req.Headers.Add("X-TC-Registered", registered.ToString());
                req.Headers.Add("X-TC-Signature", signature);
                req.Headers.Add("X-TC-Timestamp", timestamp.ToString());

                //Console.WriteLine("Headers：/n" + req.Headers);

                if (httpMethod == @"POST" || httpMethod == @"PUT")
                {
                    using (Stream reqStream = req.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                        reqStream.Close();
                    }
                }
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {

                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = reader.ReadToEnd().ToString();
                        strResult = responseData;
                    }
                }

                if (result != null)
                {
                    result(200, strResult);
                }
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(data))
                        {
                            strResult = reader.ReadToEnd();
                            if (result != null)
                            {
                                result((int)httpResponse.StatusCode, strResult);
                            }
                        }
                    }
                }
            }
            //return strResult;
        }

        private string m_HttpRequestSubmit(string strUri, string httpMethod, string signature, string nonce, string timestamp, string requestBody)
        {
            string strResult = "error";
            try
            {
                string strUrl = @"https://api.meeting.qq.com" + strUri;
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                req.Host = "api.meeting.qq.com";

                byte[] bs = Encoding.UTF8.GetBytes(requestBody);
                string responseData = System.String.Empty;

                req.Method = httpMethod;
                if (httpMethod == @"POST" || httpMethod == @"PUT")
                {
                    req.ContentLength = bs.Length;
                }

                req.Headers.Add("AppId", appId ?? @"");
                req.ContentType = "application/json";
                if (sdkId != null && sdkId.Length > 0)
                    req.Headers.Add("SdkId", sdkId);
                req.Headers.Add("X-TC-Key", secretId ?? @"");
                req.Headers.Add("X-TC-Nonce", nonce.ToString());
                if (registered > -1)
                    req.Headers.Add("X-TC-Registered", registered.ToString());
                req.Headers.Add("X-TC-Signature", signature);
                req.Headers.Add("X-TC-Timestamp", timestamp.ToString());

                //Console.WriteLine("Headers：/n" + req.Headers);

                if (httpMethod == @"POST" || httpMethod == @"PUT")
                {
                    using (Stream reqStream = req.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                        reqStream.Close();
                    }
                }
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {

                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = reader.ReadToEnd().ToString();
                        strResult = responseData;
                    }
                }

            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(data))
                        {
                            strResult = reader.ReadToEnd();
                        }
                    }
                }
            }
            return strResult;
        }
    }

    #region 签名类

    /// <summary>
    /// 签名类
    /// </summary>
    class MeetingSignature
    {
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="secretId">secretId</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="httpMethod">httpMethod</param>
        /// <param name="headerNonce">随机数</param>
        /// <param name="headerTimestamp">时间戳</param>
        /// <param name="requestUri">Uri</param>
        /// <param name="requestBody">body</param>
        /// <returns></returns>
        public static string MeetingSign(String secretId, String secretKey, String httpMethod, String headerNonce, String headerTimestamp, String requestUri, String requestBody)
        {
            string tobesig = httpMethod + "\nX-TC-Key=" + (secretId ?? @"") + "&X-TC-Nonce=" +
                headerNonce + "&X-TC-Timestamp=" + headerTimestamp + "\n"
                + requestUri + "\n" + requestBody;
            Console.WriteLine("签名串：" + tobesig);

            string signRet = string.Empty;
            using (HMACSHA256 mac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey ?? @"")))
            {
                byte[] hash = mac.ComputeHash(Encoding.UTF8.GetBytes(tobesig));
                string hexHash = bytesToHex(hash);
                signRet = Convert.ToBase64String(Encoding.UTF8.GetBytes(hexHash));
            }

            //Console.WriteLine("签名：" + signRet);
            return signRet;
        }
        private static String bytesToHex(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; ++i)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }

    #endregion

    #region 调用腾讯会议API所需的各种类

    /// <summary>
    /// 修改会议
    /// </summary>
    public class ModifyMeeting
    {
        /// <summary>
        /// 会议的唯一 ID
        /// </summary>
        public string meetingId;
        /// <summary>
        /// 会议创建者 ID
        /// </summary>
        public string userid;
        /// <summary>
        /// 设备类型 ID 
        /// </summary>
        public int instanceid;
        /// <summary>
        /// 会议主题
        /// </summary>
        public string subject;
        /// <summary>
        /// 会议主持人的用户 ID，如果没有指定，主持人将被设定为上文的 userid，即 API 调用者
        /// </summary>
        public List<Dictionary<string, string>> hosts;
        /// <summary>
        /// 邀请的参会者，可为空
        /// </summary>
        public List<Dictionary<string, string>> invitees;
        /// <summary>
        /// 会议开始时间戳（单位秒）
        /// </summary>
        public string start_time;
        /// <summary>
        /// 会议结束时间戳（单位秒）
        /// </summary>
        public string end_time;
        /// <summary>
        /// 会议密码，可不填
        /// </summary>
        public string password;
        /// <summary>
        /// 会议的配置，可为缺省配置
        /// </summary>
        public MeetingSettings settings;
    }

    /// <summary>
    /// 创建会议
    /// </summary>
    public class CreateMeeting
    {
        /// <summary>
        /// 调用方用于标示用户的唯一 ID
        /// </summary>
        public string userid;
        /// <summary>
        /// 用户的终端设备类型
        /// </summary>
        public int instanceid;
        /// <summary>
        /// 会议主题
        /// </summary>
        public string subject;
        /// <summary>
        /// 会议类型 
        /// 0 - 预约会议 
        /// 1 - 快速会议
        /// </summary>
        public int type;
        /// <summary>
        /// 会议主持人的用户 ID，如果没有指定，主持人被设定为参数 userid 的用户，即 API 调用者
        /// </summary>
        public List<Dictionary<string, string>> hosts;
        /// <summary>
        /// 会议邀请的参会者，可为空
        /// </summary>
        public List<Dictionary<string, string>> invitees;
        /// <summary>
        /// 会议开始时间戳（单位秒）
        /// </summary>
        public string start_time;
        /// <summary>
        /// 会议结束时间戳（单位秒）
        /// </summary>
        public string end_time;
        /// <summary>
        /// 会议密码，可不填
        /// </summary>
        public string password;
        /// <summary>
        /// 会议媒体参数配置
        /// </summary>
        public MeetingSettings settings;
    }

    /// <summary>
    /// 会议媒体参数配置
    /// </summary>
    public class MeetingSettings
    {
        /// <summary>
        /// 入会时静音
        /// </summary>
        public bool mute_enable_join;
        /// <summary>
        /// 允许参会者取消静音
        /// </summary>
        public bool allow_unmute_self;
        /// <summary>
        /// 全体静音
        /// </summary>
        public bool mute_all;
        /// <summary>
        /// 主持人视频
        /// </summary>
        public bool host_video;
        /// <summary>
        /// 参会者视频 
        /// </summary>
        public bool participant_video;
        /// <summary>
        /// 开启录播，暂不支持
        /// </summary>
        public bool enable_record;
        /// <summary>
        /// 参会者离开时播放提示音
        /// </summary>
        public bool play_ivr_on_leave;
        /// <summary>
        /// 有新的与会者加入时播放提示音
        /// </summary>
        public bool play_ivr_on_join;
        /// <summary>
        /// 开启直播
        /// </summary>
        public bool live_url;
    }

    /// <summary>
    /// 用户对象
    /// </summary>
    public class MeetingUser
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string email;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone;
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string username;
        /// <summary>
        /// 调用方用于标示用户的唯一 ID
        /// </summary>
        public string userid;

        /// <summary>
        /// 用户头像
        /// </summary>
        public string avatar_url;
    }

    /// <summary>
    /// 取消会议
    /// </summary>
    public class CancelMeeting
    {
        /// <summary>
        /// 会议的唯一 ID
        /// </summary>
        public string meetingId;
        /// <summary>
        /// 用户的终端设备类型
        /// </summary>
        public int instanceid;
        /// <summary>
        /// 调用方用于标示用户的唯一 ID
        /// </summary>
        public string userid;
        /// <summary>
        /// 原因代码，可为用户自定义
        /// </summary>
        public int reason_code;
        /// <summary>
        /// 详细取消原因描述
        /// </summary>
        public string reason_detail;
    }

    #endregion
}
