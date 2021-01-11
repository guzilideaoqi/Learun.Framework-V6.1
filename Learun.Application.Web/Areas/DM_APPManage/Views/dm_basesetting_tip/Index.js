/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-01-11 11:36
 * 描  述：明细说明
 */
var selectedRow;
var refreshGirdData;
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.initGird();
            page.bind();
        },
        bind: function () {
            // 查询
            $('#btn_Search').on('click', function () {
                var keyword = $('#txt_Keyword').val();
                page.search({ keyword: keyword });
            });
            // 刷新
            $('#lr_refresh').on('click', function () {
                location.reload();
            });
            // 新增
            $('#lr_add').on('click', function () {
                selectedRow = null;
                learun.layerForm({
                    id: 'form',
                    title: '新增',
                    url: top.$.rootUrl + '/DM_APPManage/dm_basesetting_tip/Form',
                    width: 700,
                    height: 400,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('id');
                selectedRow = $('#girdtable').jfGridGet('rowdata');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/DM_APPManage/dm_basesetting_tip/Form?keyValue=' + keyValue,
                        width: 700,
                        height: 400,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }
            });
            // 删除
            $('#lr_delete').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/dm_basesetting_tip/DeleteForm', { keyValue: keyValue}, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/dm_basesetting_tip/GetPageList',
                headData: [
                        { label: 'id', name: 'id', width: 200, align: "left" },
                        { label: '站长id', name: 'appid', width: 200, align: "left" },
                        { label: 'task_do', name: 'task_do', width: 200, align: "left" },
                        { label: '任务(一级)', name: 'task_one', width: 200, align: "left" },
                        { label: '任务(二级)', name: 'task_two', width: 200, align: "left" },
                        { label: '任务(一级团队)', name: 'task_parners_one', width: 200, align: "left" },
                        { label: '任务(二级团队)', name: 'task_parners_two', width: 200, align: "left" },
                        { label: '购物人提示', name: 'shop_pay', width: 200, align: "left" },
                        { label: '购物(一级)', name: 'shop_one', width: 200, align: "left" },
                        { label: '购物(二级)', name: 'shop_two', width: 200, align: "left" },
                        { label: '购物(一级团队)', name: 'shop_parners_one', width: 200, align: "left" },
                        { label: '购物(二级团队)', name: 'shop_parners_two', width: 200, align: "left" },
                        { label: '开通代理(一级)', name: 'opengent_one', width: 200, align: "left" },
                        { label: '开通代理(二级)', name: 'opengent_two', width: 200, align: "left" },
                        { label: '开通代理(三级)', name: 'opengent_three', width: 200, align: "left" },
                        { label: '开通代理(一级合伙人)', name: 'opengent_parners_one', width: 200, align: "left" },
                        { label: '开通代理(二级合伙人)', name: 'opengent_parners_two', width: 200, align: "left" },
                        { label: '创建时间', name: 'CreateTime', width: 200, align: "left" },
                        { label: '修改时间', name: 'UpdateTime', width: 200, align: "left" },
                ],
                mainId:'id',
                reloadSelected: true,
                isPage: true
            });
            page.search();
        },
        search: function (param) {
            param = param || {};
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    page.init();
}
