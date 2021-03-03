/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-03-03 10:56
 * 描  述：装修模板
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
            // 新增
            $('#lr_add').on('click', function () {
                selectedRow = null;
                learun.layerForm({
                    id: 'selectfun',
                    title: '新增功能',
                    url: top.$.rootUrl + '/DM_APPManage/dm_decoration_module/SelectModule',
                    width: 600,
                    height: 400,
                    callBack: function (id) {
                        var selectItem = top[id].acceptClick();
                        if (!!selectItem) {
                            page.generalDataGrid(selectItem);
                            learun.layerClose("selectfun");
                        }
                    }
                });
            });
        },
        initData: function () {
            if (!!selectedRow) {
                $('#form').lrSetFormData(selectedRow);
            }
        }, generalDataGrid: function (item) {
            $("#container").append("<div>这个是新增的div</div>");
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData();
        $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/dm_decoration_template/SaveForm?keyValue=' + keyValue, postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    page.init();
}
