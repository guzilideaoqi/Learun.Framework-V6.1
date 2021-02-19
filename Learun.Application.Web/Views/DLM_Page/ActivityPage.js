// 菜单操作
var ReviceActivityTask;
var meuns = {
    config: {
        TaskList: [],
        task_ids: ""
    },
    init: function () {
        meuns.LoadTask();

        $("#changetask").on("click", function () {
            var isallow = true;
            for (var i = 0; i < meuns.config.TaskList.length; i++) {
                var taskItem = meuns.config.TaskList[i];
                if (taskItem.revicestatus != 0) {
                    isallow = false; break;
                }
            }
            if (isallow)
                meuns.LoadTask();
            else {
                meuns.Toast("当前已有接受任务,无法重新获取任务!");
            }
        })

        $("#loadmore").on("click", function () {
            meuns.NativeToApp(1, 0, 0);
        })

        $("#tixian").on("click", function () {
            if ($(".finish").length >= 3) {
                meuns.NativeToApp(2, 0, 0);
            } else {
                meuns.Toast("完成以下三个小任务才能提现!");
            }
        })

        $("#inviteuser").on("click", function () {
            if ($(".finish").length >= 3) {
                meuns.NativeToApp(5, 0, 0);
            } else {
                meuns.Toast("请先完成以上小任务!");
            }
        })
    },
    LoadTask: function () {
        $.ajax({
            url: "/DLM_Page/GetRandActivityTaskList",
            type: "GET",
            dataType: "json",
            async: true,
            cache: false,
            data: { token: $("#token").val() },
            success: function (res) {
                if (res.code == 200) {
                    if (res.data.length > 0) {
                        meuns.config.TaskList = res.data;
                        meuns.config.task_ids = "";
                        meuns.LoadTaskHtmlData();
                    } else {
                        meuns.Toast("当前没有活动任务或活动已结束!");
                    }
                } else {
                    meuns.Toast(res.info);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                meuns.Toast("接口请求异常!");
            }
        });
    },
    LoadTaskHtmlData: function () {
        $("#task_list").html("");
        var html = "";
        var stepName = "第一步";
        var isrunning = false;
        var btnclass = "red";
        var btnText = "领取任务";
        for (var i = 0; i < meuns.config.TaskList.length; i++) {
            var taskItem = meuns.config.TaskList[i];
            if (meuns.config.task_ids != "")
                meuns.config.task_ids += ",";
            meuns.config.task_ids += taskItem.id;
            if (i == 1)
                stepName = "第二步";
            else if (i == 2)
                stepName = "第三步";

            if (isrunning) {
                btnclass = "gray";
                btnText = "待完成";
            } else {
                if (taskItem.revicestatus == 0)
                    isrunning = true;
                else if (taskItem.revicestatus == 1) {
                    btnText = "去完成";
                    btnclass = "red"
                    isrunning = true;
                } else if (taskItem.revicestatus == 2) {
                    btnText = "审核中";
                    isrunning = true;
                    btnclass = "red"
                } else if (taskItem.revicestatus == 3) {
                    btnText = "已完成";
                    btnclass = "finish";
                } else if (taskItem.revicestatus == 5) {
                    isrunning = true;
                    btnText = "未通过";
                    btnclass = "nopass";
                }
            }

            html += "<div class=\"task_item\">" +
                "<div class=\"headicon\">" +
                "<img src=\"../Content/dlm/activity_image/headicon" + (i + 1) + ".png\" />" +
                "</div > " +
                "    <div class=\"task_content\">" +
                "   <div class=\"task_title\">" + taskItem.task_title + "</div>" +
                "  <div class=\"task_remark\">" + taskItem.task_description + "</div>" +
                "             </div > " +
                "<div class=\"btn_info\">" +
                "<div class=\"btn " + btnclass + "\" onclick=\"ReviceActivityTask('" + btnText + "'," + taskItem.id + "," + taskItem.reviceid + ",'" + taskItem.failreason + "')\">" + btnText + "</div>" +
                "           </div>" +
                "      </div>";

            /*html += "            <div class=\"task_item\">" +
                "<div class=\"task_content\">" + stepName + " " + taskItem.task_title + "  " + taskItem.singlecommission + "元</div>" +
                "<div class=\"btn " + btnclass + "\" onclick=\"ReviceActivityTask('" + btnText + "'," + taskItem.reviceid + ",'" + taskItem.failreason + "')\">" + btnText + "</div>" +
                "</div>";*/

        }
        $("#task_list").html(html);
    },
    Toast: function (text) {
        showMessage(text, 3000, true);
    }, NativeToApp: function (type, taskId, receiveId) {
        var platform = $("#platform").val();
        if (platform == "android") {
            window.app.onNativeToAPP(type, taskId, receiveId);
        } else if (platform == "ios") {
            window.webkit.messageHandlers.onNativeToAPP.postMessage({ type: type, taskId: taskId, receiveId: receiveId });
        } else {
            meuns.Toast("未检测到合法平台!");
        }
    }
}

ReviceActivityTask = function (btntext, taskid, reviceid, failreason) {
    if (btntext == "领取任务") {
        $.ajax({
            url: "/DLM_Page/ReviceActivityTask",
            type: "Post",
            dataType: "json",
            async: true,
            cache: false,
            data: { token: $("#token").val(), taskids: meuns.config.task_ids },
            success: function (res) {
                meuns.Toast(res.info);
                meuns.LoadTask();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                meuns.Toast("接口请求异常!");
            }
        });
    } else if (btntext == "去完成") {
        var tip = "任务ID：" + taskid + "接受ID：" + reviceid;
        meuns.NativeToApp(4, taskid, reviceid);
    } else if (btntext == "未通过") {
        meuns.Toast(failreason)
    }
}

meuns.init();
