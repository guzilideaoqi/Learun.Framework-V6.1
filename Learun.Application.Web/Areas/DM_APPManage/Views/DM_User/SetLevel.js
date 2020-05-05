/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-17 22:06
 * 描  述：导航图管理
 */
var acceptClick;
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            console.log(typeof (selectedRow));
        },
        bind: function () {
            // 是否需要运费
            $('#user_level').lrselect();
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData();
        console.log(selectedRow.length);
        var user_ids = "";
        if (selectedRow.length == undefined) {
            user_ids = selectedRow.id;
        } else {
            for (var i = 0; i < selectedRow.length; i++) {
                if (user_ids != "")
                    user_ids += ",";
                user_ids += selectedRow[i].id;
            }
        }
        postData["userids"] = user_ids;
        $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_User/SetUserLevel', postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    page.init();
}
