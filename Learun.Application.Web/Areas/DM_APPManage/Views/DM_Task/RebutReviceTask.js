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
    var page = {
        init: function () {

        },
        bind: function () {
        },
        initData: function () {
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData();
        $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_Task_Revice/RebutReciveTaskByWeb?id=' + keyValue, { failreason: $("#remark").val() }, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    page.init();
}
