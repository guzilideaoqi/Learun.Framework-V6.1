/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2019-11-22 22:10
 * 描  述：拼多多订单列表
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
                    url: top.$.rootUrl + '/Hyg_RobotModule/order_pdd/Form',
                    width: 700,
                    height: 400,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('order_sn');
                selectedRow = $('#girdtable').jfGridGet('rowdata');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/Hyg_RobotModule/order_pdd/Form?keyValue=' + keyValue,
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
                var keyValue = $('#girdtable').jfGridValue('order_sn');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/Hyg_RobotModule/order_pdd/DeleteForm', { keyValue: keyValue}, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/Hyg_RobotModule/order_pdd/GetPageList',
                headData: [
                        { label: '订单编号', name: 'order_sn', width: 150, align: "left" },
                        { label: '商品id', name: 'goods_id', width: 100, align: "left" },
                        { label: '商品名称', name: 'goods_name', width: 200, align: "left" },
                    { label: '商品数量', name: 'goods_quantity', width: 80, align: "left" },
                    {
                        label: '订单价格', name: 'order_amount', width: 80, align: "left", formattre: function (cellValue, rowObject) {
                            return cellValue / 100;
                        }
                    },
                    {
                        label: '佣金比例', name: 'promotion_rate', width: 80, align: "left", formattre: function (cellValue, rowObject) {
                            return cellValue / 10;
                        } },
                    {
                        label: '佣金', name: 'promotion_amount', width: 80, align: "left", formattre: function (cellValue, rowObject) {
                            return cellValue / 100;
                        } },
                    { label: '订单状态', name: 'order_status_desc', width: 80, align: "left" },
                    {
                        label: '支付时间', name: 'order_pay_time', width: 100, align: "left", formatter: function (cellValue, rowObject) {
                            return learun.formatDate(cellValue * 1000 + "/Date(", "yyyy-MM-dd HH:mm:ss");
                        }
                    },
                    {
                        label: '成团时间', name: 'order_group_success_time', width: 100, align: "left", formatter: function (cellValue, rowObject) {
                            return learun.formatDate(cellValue * 1000 + "/Date(", "yyyy-MM-dd HH:mm:ss");
                        }},
                    {
                        label: '确认收货时间', name: 'order_receive_time', width: 100, align: "left", formatter: function (cellValue, rowObject) {
                            return learun.formatDate(cellValue * 1000 + "/Date(", "yyyy-MM-dd HH:mm:ss");
                        }},
                    {
                        label: '审核时间', name: 'order_verify_time', width: 100, align: "left", formatter: function (cellValue, rowObject) {
                            return learun.formatDate(cellValue * 1000 + "/Date(", "yyyy-MM-dd HH:mm:ss");
                        }},
                    {
                        label: '结算时间', name: 'order_settle_time', width: 100, align: "left", formatter: function (cellValue, rowObject) {
                            return learun.formatDate(cellValue * 1000 + "/Date(", "yyyy-MM-dd HH:mm:ss");
                        }},
                    {
                        label: '最后更新时间', name: 'order_modify_at', width: 100, align: "left", formatter: function (cellValue, rowObject) {
                            return learun.formatDate(cellValue * 1000 + "/Date(", "yyyy-MM-dd HH:mm:ss");
                        } },
                    { label: '推广位id', name: 'p_id', width: 100, align: "left" },
                    /*{ label: '订单创建时间', hidden: true, name: 'order_create_time', width: 200, align: "left" },
                    { label: '订单状态', hidden: true, name: 'order_status', width: 200, align: "left" },
                    { label: '结算批次号', hidden: true, name: 'batch_no', width: 200, align: "left" },
                    { label: '商品价格（分）', hidden: true, name: 'goods_price', width: 200, align: "left" },
                    { label: '商品缩略图', hidden: true, name: 'goods_thumbnail_url', width: 200, align: "left" },
                    { label: '订单类型：0：领券页面， 1： 红包页， 2：领券页， 3： 题页', hidden: true, name: 'type', width: 200, align: "left" },
                    { label: '成团编号', hidden: true,name: 'group_id', width: 200, align: "left" },
                    { label: '多多客工具id', hidden: true, name: 'auth_duo_id', width: 200, align: "left" },
                    { label: '招商多多客id', hidden: true, name: 'zs_duo_id', width: 200, align: "left" },
                    { label: '自定义参数', hidden: true,name: 'custom_parameters', width: 200, align: "left" },
                    { label: 'CPS_Sign', hidden: true,name: 'cps_sign', width: 200, align: "left" },
                    { label: '链接最后一次生产时间', hidden: true,name: 'url_last_generate_time', width: 200, align: "left" },
                    { label: '打点时间', hidden: true, name: 'point_time', width: 200, align: "left" },
                    { label: '售后状态：0：无，1：售后中，2：售后完成', hidden: true,name: 'return_status', width: 200, align: "left" },
                    { label: '是否是 cpa 新用户，1表示是，0表示否', hidden: true, name: 'cpa_new', width: 200, align: "left" },*/
                ],
                mainId:'order_sn',
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
