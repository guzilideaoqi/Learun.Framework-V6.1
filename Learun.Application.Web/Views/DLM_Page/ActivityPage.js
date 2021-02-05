﻿// 菜单操作
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
            meuns.NativeToApp(1, 0);
        })

        $("#tixian").on("click", function () {
            meuns.NativeToApp(2, 0);
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
        var html = " <div class=\"remark\">提现攻略</div>";
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
                    isrunning = true;
                } else if (taskItem.revicestatus == 2) {
                    btnText = "审核中";
                    isrunning = true;
                } else if (taskItem.revicestatus == 3) {
                    btnText = "已完成";
                    btnclass = "finish";
                } else if (taskItem.revicestatus == 5) {
                    isrunning = true;
                    btnText = "未通过";
                    btnclass = "nopass";
                }
            }


            html += "            <div class=\"task_item\">" +
                "<div class=\"task_content\">" + stepName + " " + taskItem.task_title + "  " + taskItem.singlecommission + "元</div>" +
                "<div class=\"btn " + btnclass + "\" onclick=\"ReviceActivityTask('" + btnText + "'," + taskItem.reviceid + ",'" + taskItem.failreason + "')\">" + btnText + "</div>" +
                "</div>";

        }
        $("#task_list").html(html);
    },
    Toast: function (text) {
        showMessage(text, 3000, true);
    }, NativeToApp: function (type, id) {
        var platform = $("#platform").val();
        if (platform == "android") {
            window.app.onNativeToAPP(type, id);
        } else if (platform == "ios") {
            meuns.Toast("无法从活动页面跳转到APP，请直接在我的接收中完成任务!");
        } else {
            meuns.Toast("未检测到合法平台!");
        }
    }
}

ReviceActivityTask = function (btntext, reviceid, failreason) {
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
        meuns.NativeToApp(3, reviceid);
    } else if (btntext == "未通过") {
        meuns.Toast(failreason)
    }
}

meuns.init();
