/*
 * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.04.05
 * 描 述：分类管理	
 */

var keyValue = request('keyValue');
var acceptClick;
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var ue;
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            // 上级
            $('#parentid').lrselect({
                url: top.$.rootUrl + '/DM_APPManage/DM_Article/GetClassifyTree',
                type: 'tree',
                allowSearch: true,
                maxHeight: 225
            });

            ue = UE.getEditor('editor');
        },
        initData: function () {
            if (!!selectedRow) {
                keyValue = selectedRow.id || '';
                $('#form').lrSetFormData(selectedRow);
                console.log(selectedRow.content);

                setTimeout(function () {
                    ue.setContent(selectedRow.content);
                }, 300);
            }
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData(keyValue);
        if (postData["parentid"] == '' || postData["parentid"] == '&nbsp;') {
            postData["parentid"] = '0';
        }
        else if (postData["parentid"] == keyValue) {
            learun.alert.error('上级不能是自己本身');
            return false;
        }
        postData["content"] = ue.getContent(null, null, true);
        $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_Article/SaveForm?keyValue=' + keyValue, postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };

    page.init();
}