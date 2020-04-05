/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2019-11-19 20:34
 * 描  述：应用商信息设置
 */
var acceptClick;
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            $.lrSetForm(top.$.rootUrl + '/Hyg_RobotModule/Application_Setting/GetFormDataByApplicationId', function (data) {
                if (!!data) {
                    keyValue = data.F_SettingId;
                    $('#form').lrSetFormData(data);
                }
            });

            $("#OpenTBAuthor").click(function () {
                learun.layerForm({
                    id: 'form',
                    title: '淘宝授权',
                    url: top.$.rootUrl + '/Hyg_RobotModule/Application_Setting/AuthorilizeForm',
                    width: 900,
                    height: 600,
                    btn:[]
                });
            })
        },
        bind: function () {
        },
        initData: function () {
            if (!!selectedRow) {
                $('#form').lrSetFormData(selectedRow);
            }
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData();
        $.lrSaveForm(top.$.rootUrl + '/Hyg_RobotModule/Application_Setting/SaveForm?keyValue=' + keyValue, postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    page.init();
}
