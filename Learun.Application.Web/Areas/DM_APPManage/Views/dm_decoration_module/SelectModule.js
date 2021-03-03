/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-03-03 10:54
 * 描  述：模块管理
 */
var acceptClick;
var keyValue = request('keyValue');
var selectItem;
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            $('#fun_type').lrselect({
                text: 'module_name',
                value: 'module_type',
                url: top.$.rootUrl + '/DM_APPManage/dm_decoration_module/GetSelectData',
                maxHeight: 180,
                allowSearch: true,
                select: function (item) {
                    selectItem = item;
                }
            })
        },
        initData: function () {
            if (!!selectedRow) {
                $('#form').lrSetFormData(selectedRow);
            }
        }
    };
    // 保存数据
    acceptClick = function () {
        return selectItem;
    };
    page.init();
}
