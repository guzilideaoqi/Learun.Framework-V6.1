using Learun.Application.TwoDevelopment.Common;
using Learun.Application.Web.App_Start._01_Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers.DM_APIControl
{
    public class TencentMeetingController : MvcAPIControllerBase
    {
        /// <summary>
        /// 创建会议房间
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMetting()
        {
            try
            {
                MeetingSettings msettings = new MeetingSettings()
                {
                    mute_enable_join = true,
                    allow_unmute_self = false,
                    mute_all = false,
                    host_video = true,
                    participant_video = false,
                    enable_record = false,
                    play_ivr_on_leave = false,
                    play_ivr_on_join = false,
                    live_url = false
                };

                Dictionary<string, string> user = new Dictionary<string, string>();
                user.Add("userid", "1234567890");
                CreateMeeting createMeeting = new CreateMeeting()
                {
                    userid = "1234567890",
                    instanceid = 1,
                    subject = "test meeting",
                    type = 0,
                    hosts = new List<Dictionary<string, string>>() { user },
                    settings = msettings,
                    start_time = "1590562357",
                    end_time = "1590564097"
                };

                MeetingAPI meetingAPI = new MeetingAPI()
                {
                    AppId = @"223516798",
                    SecretId = @"oNMfqH5G83RSBuIPVj7m9en4Q0sbiLJDychw",
                    Secretkey = @"jzmABOQC2kpWDnsHEdV6JLYb0e41ht3a",
                    SdkId = "2006105282"
                };
                int result = meetingAPI.CreateMeetings(createMeeting, (int resultCode, dynamic resultMsg) =>
                {
                    Console.WriteLine("创建会议结果：\nresultCode：" + resultCode + "：" + resultMsg);
                });

                return Success("创建成功!");
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
        public ActionResult CreateUser() {
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
    }
}