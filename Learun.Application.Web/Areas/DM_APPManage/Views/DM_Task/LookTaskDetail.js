/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-14 16:47
 * 描  述：会员信息
 */
var acceptClick;
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            // 代码查看
            $('#nav_tabs').lrFormTabEx();
        },
        initData: function () {
            if (!!selectedRow) {
                $('#form').lrSetFormData(selectedRow);
                if (selectedRow.task_status == -2)
                    $("#task_status").val("未审核").css("color", "red");
                else if (selectedRow.task_status == 0) {
                    $("#task_status").val("进行中").css("color", "green");
                } else if (selectedRow.task_status == 1) {
                    $("#task_status").val("已完成").css("color", "yellow");
                } else if (selectedRow.task_status == 2) {
                    $("#task_status").val("已取消").css({ "color": "#c3c3c3" });
                } else if (selectedRow.task_status == 3) {
                    $("#task_status").val("已下架").css({ "color": "#c3c3c3" });
                }


                if (selectedRow.plaform == 0) {
                    $("#task_operate").html(selectedRow.task_operate);
                    $("#submit_data").html(selectedRow.submit_data);
                } else {
                    //任务操作步骤
                    var task_operate_json_data = JSON.parse(selectedRow.task_operate.replace(/'/g, '"'));
                    var task_operate_step = task_operate_json_data.step;
                    var step_html = "";
                    for (var i = 0; i < task_operate_step.length; i++) {
                        var step_detail = task_operate_step[i];
                        step_html += "<div class=\"task_title\">" + step_detail.textContent + "</div><div>";
                        for (var j = 0; j < step_detail.images.length; j++) {
                            step_html += "<img src=\"" + step_detail.images[j] + "\"></img>";
                        }
                        step_html += "</div>";
                    }
                    $("#task_operate").html(step_html);

                    //任务要求
                    var submit_data_json_data = JSON.parse(selectedRow.submit_data.replace(/'/g, '"'));
                    var submit_data_step = submit_data_json_data.step;
                    var submit_data_html = "";

                    submit_data_html += "<div class=\"task_title\">" + submit_data_step.textContent + "</div><div>";
                    for (var j = 0; j < submit_data_step.images.length; j++) {
                        submit_data_html += "<img src=\"" + submit_data_step.images[j] + "\"></img>";
                    }

                    submit_data_html += "</div>";
                    $("#submit_data").html(submit_data_html);
                }
            }
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        learun.layerClose("LookTaskDetail", "");
    };
    page.init();
}