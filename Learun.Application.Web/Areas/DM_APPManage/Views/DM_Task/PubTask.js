/* * 版 本 Learun-ADMS
 * Copyright (c) 2013-2017
 * 创建人：超级管理员
 * 日  期：2020-04-16 16:01
 * 描  述：任务中心
 */
var acceptClick;
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var ue;
    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
            //内容编辑器
            ue = UE.getEditor('editor');

            $("#task_type").lrselect({
                url: top.$.rootUrl + '/DM_APPManage/DM_Task_Type/GetListByWeb',
                text: 'name'
            });

            $("#PubTask").bind("click", function (callBack) {
                if (!$('#form').lrValidform()) {
                    return false;
                }
                var postData = $('#form').lrGetFormData();
                postData["task_operate"] = ue.getContent(null, null, true);
                keyValue = (keyValue == null || keyValue == "") ? 0 : keyValue;
                $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_Task/PubNewTask?keyValue=' + keyValue, postData, function (res) {
                    // 保存成功后才回调
                    //if (!!callBack) {
                    //    callBack();
                    //}
                    //learun.alert.success('发布成功');
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