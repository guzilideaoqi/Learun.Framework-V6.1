using Learun.Application.TwoDevelopment.Common;
using Learun.Application.Web.App_Start._01_Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Learun.Util;
using Learun.Application.TwoDevelopment.DM_APPManage;
using HYG.CommonHelper.Common;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    [VerificationAPI(FilterMode.Ignore)]
    public class TencentMeetingController : MvcAPIControllerBase
    {
        private DM_BaseSettingIBLL dm_BaseSettingIBLL = new DM_BaseSettingBLL();

        private DM_UserIBLL dm_userIBLL = new DM_UserBLL();

        private DM_MeetingListIBLL dm_MeetingListIBLL = new DM_MeetingListBLL();

        /// <summary>
        /// 创建会议房间
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMetting(int User_ID, string Subject, DateTime StartTime, DateTime EndTime, string Page_Image = "", string Password = "", int Mute_Enable_Join = 0, int Allow_Unmute_Self = 0, int Mute_All = 0, int Host_Video = 0, int Participant_Video = 0, int Play_Ivr_On_Leave = 0, int Play_Ivr_On_Join = 0)
        {
            try
            {
                if (User_ID <= 0)
                    return FailNoLogin();

                string appid = CheckAPPID();

                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);

                dm_userEntity dm_UserEntity = dm_userIBLL.GetEntityByCache(User_ID);

                if (dm_UserEntity.isvoice != 1)
                    throw new Exception("无创建直播间权限!");

                MeetingSettings msettings = new MeetingSettings()
                {
                    mute_enable_join = Mute_Enable_Join == 1,
                    allow_unmute_self = Allow_Unmute_Self == 1,
                    mute_all = Mute_All == 1,
                    host_video = Host_Video == 1,
                    participant_video = Participant_Video == 1,
                    enable_record = false,
                    play_ivr_on_leave = Play_Ivr_On_Leave == 1,
                    play_ivr_on_join = Play_Ivr_On_Join == 1,
                    live_url = false
                };

                Dictionary<string, string> user = new Dictionary<string, string>();
                user.Add("userid", dm_UserEntity.phone);
                CreateMeeting createMeeting = new CreateMeeting()
                {
                    userid = dm_UserEntity.phone,
                    instanceid = 1,
                    subject = Subject,
                    type = 0,
                    hosts = new List<Dictionary<string, string>>() { user },
                    settings = msettings,
                    start_time = Time.GetTimeStamp(StartTime),
                    end_time = Time.GetTimeStamp(EndTime),
                    password = Password
                };

                MeetingAPI meetingAPI = new MeetingAPI()
                {
                    AppId = dm_BasesettingEntity.meeting_appid,
                    SecretId = dm_BasesettingEntity.meeting_secretid,
                    Secretkey = dm_BasesettingEntity.meeting_secretkey,
                    SdkId = dm_BasesettingEntity.meeting_sdkid,
                    //Registered = 1
                };

                #region 创建用户(不管是否成功  都需要创建房间)
                MeetingUser meetingUser = new MeetingUser()
                {
                    userid = dm_UserEntity.phone,
                    username = dm_UserEntity.nickname,
                    email = dm_UserEntity.phone + "@qq.com",
                    phone = dm_UserEntity.phone,
                    avatar_url = dm_UserEntity.headpic
                };
                string userdetail = meetingAPI.GetUserDetail(dm_UserEntity.phone);
                if (userdetail.Contains("error_info"))
                {//有错误就执行创建
                    MeetingErrorResponse meetingErrorResponse = JsonConvert.JsonDeserialize<MeetingErrorResponse>(userdetail);
                    if (meetingErrorResponse.error_info.error_code == 20002)
                    {//用户已存在
                        meetingAPI.UpdateUser(meetingUser);
                    }
                    else if (meetingErrorResponse.error_info.error_code == 20001)
                    {//用户不存在
                        meetingAPI.CreateUser(meetingUser);
                    }
                }
                else
                {//执行更新
                    meetingAPI.UpdateUser(meetingUser);
                }
                #endregion

                string result = meetingAPI.CreateMeetings(createMeeting);

                if (!result.Contains("error_info"))
                {
                    CreateMeetingResponse createMeetingResponse = JsonConvert.JsonDeserialize<CreateMeetingResponse>(result);
                    if (createMeetingResponse.meeting_number > 0)
                    {
                        List<dm_meetinglistEntity> MeetingEntityList = new List<dm_meetinglistEntity>();
                        foreach (MeetingInfo item in createMeetingResponse.meeting_info_list)
                        {
                            MeetingEntityList.Add(new dm_meetinglistEntity
                            {
                                hosts = User_ID.ToString(),
                                join_url = item.join_url,
                                meeting_code = item.meeting_code,
                                meeting_id = item.meeting_id,
                                start_time = StartTime,
                                end_time = EndTime,
                                participants = "",
                                password = Password,
                                user_id = User_ID,
                                subject = item.subject,
                                createtime = DateTime.Now,
                                settings = "",
                                join_image = dm_MeetingListIBLL.GeneralMeetingImage(dm_BasesettingEntity, item.join_url),
                                page_image= Page_Image
                            });
                        }
                        if (MeetingEntityList.Count > 0)
                        {
                            dm_MeetingListIBLL.CreateMetting(MeetingEntityList);
                        }
                    }
                }
                else
                {
                    MeetingErrorResponse meetingErrorResponse = JsonConvert.JsonDeserialize<MeetingErrorResponse>(result);
                    throw new Exception(meetingErrorResponse.error_info.message);
                }

                return Success("创建成功,请刷新直播列表!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        /// <summary>
        /// 获取直播间权限(只显示未过期的)
        /// </summary>
        /// <param name="User_ID">用户ID，传0时默认是全部</param>
        /// <param name="keyword">关键词  房间名称或房间编号</param>
        /// <returns></returns>
        public ActionResult GetMeetingList(int PageNo=1,int PageSize=10,int User_ID=0,string keyword="")
        {
            try
            {
                return SuccessList("获取成功", dm_MeetingListIBLL.GetMeetingList(new Pagination { rows = PageSize, page = PageNo, sidx = "createtime", sord = "desc" }, keyword, User_ID));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        public ActionResult GetMeetingJoinImage(string Join_Url) {
            try
            {
                string appid = CheckAPPID();
                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);

                return Success("生成成功!", dm_MeetingListIBLL.GeneralMeetingImage(dm_BasesettingEntity, Join_Url));
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }


        public ActionResult GetMeetingUserDetail(string phone) {
            try
            {
                string appid = CheckAPPID();

                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);

                MeetingAPI meetingAPI = new MeetingAPI()
                {
                    AppId = dm_BasesettingEntity.meeting_appid,
                    SecretId = dm_BasesettingEntity.meeting_secretid,
                    Secretkey = dm_BasesettingEntity.meeting_secretkey,
                    SdkId = dm_BasesettingEntity.meeting_sdkid,
                    //Registered = 1
                };

                string userdetail = meetingAPI.GetUserDetail(phone);

                return Success("获取成功!", userdetail);
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        public ActionResult GetMeetingDetail(string code,string userid,int instanceid) {
            try
            {
                string appid = CheckAPPID();

                dm_basesettingEntity dm_BasesettingEntity = dm_BaseSettingIBLL.GetEntityByCache(appid);

                MeetingAPI meetingAPI = new MeetingAPI()
                {
                    AppId = dm_BasesettingEntity.meeting_appid,
                    SecretId = dm_BasesettingEntity.meeting_secretid,
                    Secretkey = dm_BasesettingEntity.meeting_secretkey,
                    SdkId = dm_BasesettingEntity.meeting_sdkid,
                    //Registered = 1
                };


                meetingAPI.GetMeetingsWithCode(code, userid, instanceid, null);
                return Success("");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateUser()
        {
            try
            {
                MeetingUser meetingUser = new MeetingUser()
                {
                    userid = @"10086",
                    username = @"中国移不动",
                    email = @"10086@12306.cn",
                    phone = @"10086123061"
                };
                MeetingAPI meetingAPI = new MeetingAPI()
                {
                    AppId = @"",
                    SecretId = @"",
                    Secretkey = @""
                };
                meetingAPI.CreateUser(meetingUser, (int resultCode, dynamic resultMsg) =>
                {
                    Console.WriteLine("创建用户结果：\nresultCode：" + resultCode + "：" + resultMsg);
                });
                return Success("创建成功!");
            }
            catch (Exception ex)
            {
                return FailException(ex);
            }
        }

        public string CheckAPPID()
        {
            if (base.Request.Headers["appid"].IsEmpty())
            {
                throw new Exception("缺少参数appid");
            }
            return base.Request.Headers["appid"].ToString();
        }
    }

    public class MeetingErrorResponse
    {
        public ErrorInfo error_info { get; set; }
    }

    public class ErrorInfo
    {
        public int error_code { get; set; }
        public string message { get; set; }
    }

    public class CreateMeetingResponse
    {
        /// <summary>
        /// 创建会议的数量
        /// </summary>
        public int meeting_number { get; set; }
        /// <summary>
        /// 会议列表
        /// </summary>
        public List<MeetingInfo> meeting_info_list { get; set; }
    }

    public class MeetingInfo
    {
        /// <summary>
        /// 会议名称
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 会议ID
        /// </summary>
        public string meeting_id { get; set; }
        /// <summary>
        /// 会议编号
        /// </summary>
        public string meeting_code { get; set; }
        /// <summary>
        /// 会议开始时间
        /// </summary>
        public string start_time { get; set; }
        /// <summary>
        /// 会议结束时间
        /// </summary>
        public string end_time { get; set; }
        /// <summary>
        /// 入会链接
        /// </summary>
        public string join_url { get; set; }
    }
}