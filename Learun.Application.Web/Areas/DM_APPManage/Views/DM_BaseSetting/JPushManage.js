/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-14 16:36
 * 描  述：基础信息配置
 */
var acceptClick;
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
            $("#PubTask").bind("click", function () {
                if (!$('#form').lrValidform()) {
                    return false;
                }
                var postData = $('#form').lrGetFormData();
                postData["Plaform"] = $("input[name=jpush_object]:checked").val();
                $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_BaseSetting/ExcutePush', postData, function (res) {
                    // 保存成功后才回调
                    //learun.alert.success('推送成功');
                });
            });
        },
        initData: function () {
            if (!!selectedRow) {
                $('#form').lrSetFormData(selectedRow);
            }
        }
    };
    page.init();
}