/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-26 16:01
 * 描  述：订单管理
 */
var selectedRow;
var refreshGirdData;
var SelectRowIndx = 0;
var bootstrap = function ($, learun) {
    "use strict";
    var startTime, endTime;
    var page = {
        init: function () {
            page.bind();

            page.initGird();
        },
        bind: function () {
            // 时间搜索框
            $('#datesearch').lrdate({
                dfdata: [
                    { name: '今天', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00') }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
                    { name: '近7天', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00', 'd', -6) }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
                    { name: '近1个月', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00', 'm', -1) }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } },
                    { name: '近3个月', begin: function () { return learun.getDate('yyyy-MM-dd 00:00:00', 'm', -3) }, end: function () { return learun.getDate('yyyy-MM-dd 23:59:59') } }
                ],
                // 月
                mShow: false,
                premShow: false,
                // 季度
                jShow: false,
                prejShow: false,
                // 年
                ysShow: false,
                yxShow: false,
                preyShow: false,
                yShow: false,
                // 默认
                dfvalue: '1',
                selectfn: function (begin, end) {
                    startTime = begin;
                    endTime = end;
                }
            });
            $('#multiple_condition_query').lrMultipleQuery(function (queryJson) {
                // 调用后台查询
                // queryJson 查询条件
                page.search();
            }, 220);

            //平台类型下拉框初始化
            $('#txt_type_big').lrselect({ placeholder: "请选择平台类型" });

            $('#txt_status').lrselect({ placeholder: "请选择订单状态" });

            // 查询
            $('#btn_Search').on('click', function () {
                page.search();
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_Order/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_Order/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_Order/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Order/GetPageList',
                headData: [
                    //{ label: '主键', name: 'id', width: 200, align: "left" },
                    //{ label: '总平台id', name: 'appid', width: 200, align: "left" },
                    { label: '订单编号', name: 'order_sn', width: 150, align: "left" },
                    { label: '子订单编号', name: 'sub_order_sn', width: 150, align: "left" },
                    { label: '商品ID', name: 'origin_id', width: 100, align: "left" },
                    //{
                    //    label: '大平台类型:1=淘宝和天猫,3=京东,4=拼多多', name: 'type_big', width: 200, align: "left", formatter: function (cellValue, rowData, options) {
                    //        var statusStr = "";
                    //        switch (cellValue) {
                    //            case 1:
                    //                statusStr = "未返";
                    //                break;
                    //            case 2:
                    //                statusStr = "已返";
                    //                break;
                    //            case 3:
                    //                statusStr = "已返";
                    //                break;
                    //            case 4:
                    //                statusStr = "已返";
                    //                break;
                    //        }
                    //        return statusStr;
                    //    }
                    //},
                    {
                        label: '平台类型', name: 'type_big', width: 60, align: "left", formatter: function (cellValue, rowData, options) {
                            var statusStr = "";
                            switch (cellValue) {
                                case 1:
                                    statusStr = "淘宝";
                                    break;
                                case 2:
                                    statusStr = "天猫";
                                    break;
                                case 3:
                                    statusStr = "京东";
                                    break;
                                case 4:
                                    statusStr = "拼多多";
                                    break;
                            }
                            return statusStr;
                        }
                    },
                    /*{
                        label: '订单类型', name: 'order_type', width: 60, align: "left", formatter: function (cellValue, rowData, options) {
                            var statusStr = "";
                            switch (cellValue) {
                                case 1:
                                    statusStr = "天猫";
                                    break;
                                case 2:
                                    statusStr = "淘宝";
                                    break;
                                case 3:
                                    statusStr = "聚划算";
                                    break;
                            }
                            return statusStr;
                        }},*/
                    { label: '商品标题', name: 'title', width: 200, align: "left" },
                    //{ label: '订单原始状态', name: 'order_status', width: 200, align: "left" },
                    {
                        label: '返佣状态', name: 'rebate_status', width: 60, align: "left", formatter: function (cellValue, rowData, options) {
                            var statusStr = "";
                            switch (cellValue) {
                                case 0:
                                    statusStr = "未返";
                                    break;
                                case 1:
                                    statusStr = "已返";
                                    break;
                            }
                            return statusStr;
                        }
                    },
                    //{ label: '商品图片地址', name: 'image', width: 200, align: "left" },
                    //{ label: '数量', name: 'product_num', width: 50, align: "left" },
                    //{ label: '原价', name: 'product_price', width: 50, align: "left" },
                    { label: '实付金额', name: 'payment_price', width: 80, align: "left" },
                    { label: '结算预估佣金', name: 'estimated_effect', width: 80, align: "left" },
                    //{ label: '收入比率', name: 'income_ratio', width: 80, align: "left" },
                    { label: '付款预估佣金', name: 'estimated_income', width: 80, align: "left" },
                    //{ label: '佣金比例', name: 'commission_rate', width: 60, align: "left" },
                    //{ label: '实结佣金金额', name: 'commission_amount', width: 80, align: "left" },
                    //{ label: '补贴比率', name: 'subsidy_ratio', width: 60, align: "left" },
                    //{ label: '补贴金额', name: 'subsidy_amount', width: 60, align: "left" },
                    //{ label: '补贴类型', name: 'subsidy_type', width: 60, align: "left" },
                    //{ label: '订单创建时间', name: 'order_createtime', width: 150, align: "left" },
                    { label: '订单结算时间', name: 'order_settlement_at', width: 150, align: "left" },
                    { label: '订单付款时间', name: 'order_pay_time', width: 150, align: "left" },
                    //{ label: '订单成团时间', name: 'order_group_success_time', width: 200, align: "left" },
                    { label: '记录创建时间', name: 'createtime', width: 150, align: "left" },
                    //{ label: '记录修改时间', name: 'updatetime', width: 200, align: "left" },
                    //{ label: '店铺名称', name: 'shopname', width: 200, align: "left" },
                    //{ label: '类目名称', name: 'category_name', width: 200, align: "left" },
                    //{ label: '来源媒体名称', name: 'media_name', width: 200, align: "left" },
                    //{ label: '媒体id', name: 'media_id', width: 200, align: "left" },
                    //{ label: '广告位名称', name: 'pid_name', width: 200, align: "left" },
                    { label: '推广位id', name: 'pid', width: 60, align: "left" },
                    { label: '渠道ID', name: 'relation_id', width: 60, align: "left" },
                    //{ label: '会员ID', name: 'special_id', width: 200, align: "left" },
                    //{ label: '维权状态', name: 'protection_status', width: 200, align: "left" },
                    //{ label: '订单来源  1同步服务  2后台', name: 'insert_type', width: 200, align: "left" },
                    {
                        label: '订单状态', name: 'order_type_new', width: 70, align: "left", formatter: function (cellValue, rowData, options) {
                            var statusStr = "";
                            switch (cellValue) {
                                case 0:
                                    statusStr = "未处理";
                                    break;
                                case 1:
                                    statusStr = "订单付款";
                                    break;
                                case 2:
                                    statusStr = "收货未结";
                                    break;
                                case 3:
                                    statusStr = "订单失效";
                                    break;
                                case 4:
                                    statusStr = "结算至余额";
                                    break;
                            }
                            return statusStr;
                        }
                    },
                    //{ label: '订单创建日期', name: 'order_create_date', width: 200, align: "left" },
                    //{ label: '订单创建月份', name: 'order_create_month', width: 200, align: "left" },
                    //{ label: '订单确认收货日期', name: 'order_receive_date', width: 200, align: "left" },
                    //{ label: '订单确认收货月份', name: 'order_receive_month', width: 200, align: "left" },
                    { label: '用户ID', name: 'userid', width: 60, align: "left" },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true,
                isMultiselect: false
            });
            page.search();
        },
        search: function (param) {
            param = param || { StartTime: startTime, EndTime: endTime, txt_OrderSN: $("#txt_OrderSN").val(), txt_UserID: $("#txt_UserID").val(), txt_status: $("#txt_status").lrselectGet(), txt_type_big: $("#txt_type_big").lrselectGet() };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    page.init();
}
 