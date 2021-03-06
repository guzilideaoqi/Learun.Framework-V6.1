/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-03-06 11:15
 * 描  述：dm_duomai_order
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
                    url: top.$.rootUrl + '/DM_APPManage/dm_duomai_order/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/dm_duomai_order/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/dm_duomai_order/DeleteForm', { keyValue: keyValue}, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/dm_duomai_order/GetPageList',
                headData: [
                        { label: 'id', name: 'id', width: 200, align: "left" },
                        { label: '计划ID', name: 'ads_id', width: 200, align: "left" },
                        { label: '推广位ID', name: 'site_id', width: 200, align: "left" },
                        { label: '推广计划链接ID', name: 'link_id', width: 200, align: "left" },
                        { label: '反馈ID', name: 'euid', width: 200, align: "left" },
                        { label: '订单编号', name: 'order_sn', width: 200, align: "left" },
                        { label: '针对部分联盟存在父订单号', name: 'parent_order_sn', width: 200, align: "left" },
                        { label: '订单下单时间，格式:yyyy-MM-dd HH:mm:ss', name: 'order_time', width: 200, align: "left" },
                        { label: '订单金额，例：1.00', name: 'orders_price', width: 200, align: "left" },
                        { label: '订单佣金，例：1.00', name: 'siter_commission', width: 200, align: "left" },
                        { label: '多麦联盟结算状态：-1=无效 0=未确认 1=确认 2=结算', name: 'status', width: 200, align: "left" },
                        { label: '确认订单金额，1.00', name: 'confirm_price', width: 200, align: "left" },
                        { label: '确认订单佣金，1.00', name: 'confirm_siter_commission', width: 200, align: "left" },
                        { label: '结算时间，格式:yyyy-MM-dd HH:mm:ss', name: 'charge_time', width: 200, align: "left" },
                        { label: '创建时间', name: 'createtime', width: 200, align: "left" },
                        { label: '修改时间', name: 'updatetime', width: 200, align: "left" },
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
