/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-16 16:01
 * 描  述：任务中心
 */
var acceptClick;
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.initData();
        },
        bind: function () {
        },
        initData: function () {
            console.log(111)
            $.lrSetForm(top.$.rootUrl + "/DM_APPManage/DM_Task_Revice/GetFormData?keyValue=" + keyValue, function (res) {
                var submitdata = JSON.parse(res.submit_data);
                console.log(submitdata);
                var html = "";
                for (var i = 0; i < submitdata.step.length; i++) {
                    var submitItem = submitdata.step[i];
                    html += "    <div class=\"dataitem\">" +
                        "<div class=\"remark\">" + submitItem.textContent + "</div>" +
                        "<div class=\"imagelist\">" +
                        page.generalimage(submitItem.images)
                        + "</div> " +
                        "</div>";
                }
                console.log(html)
                $("#dataList").html(html);
            })
        },
        generalimage: function (images) {
            var imgstr = "";
            for (var j = 0; j < images.length; j++) {
                imgstr += "<img src=\"" + images[j] + "\"/>"
            }

            return imgstr;
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        var postData = $('#form').lrGetFormData();
        $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_Task_Revice/AuditTask?keyValue=' + keyValue, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    page.init();
}
