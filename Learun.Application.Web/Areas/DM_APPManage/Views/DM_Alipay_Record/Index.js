/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-10 13:38
 * 描  述：开通代理记录
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_Alipay_Record/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_Alipay_Record/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_Alipay_Record/DeleteForm', { keyValue: keyValue}, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Alipay_Record/GetPageList',
                headData: [
                        { label: '订单开通记录', name: 'id', width: 200, align: "left" },
                        { label: '用户Id', name: 'out_trade_no', width: 200, align: "left" },
                        { label: 'user_id', name: 'user_id', width: 200, align: "left" },
                        { label: '支付宝交易订单号', name: 'alipay_trade_no', width: 200, align: "left" },
                        { label: '生成支付的金额(此处做好保留，防止后期套餐金额修改)', name: 'total_amount', width: 200, align: "left" },
                        { label: '支付宝交易状态', name: 'alipay_status', width: 200, align: "left" },
                        { label: '1初级代理  2高级代理  3升级代理', name: 'templateid', width: 200, align: "left" },
                        { label: '支付宝交易创建时间', name: 'gmt_create', width: 200, align: "left" },
                        { label: '支付时间', name: 'gmt_payment', width: 200, align: "left" },
                        { label: '回调时间', name: 'notify_time', width: 200, align: "left" },
                        { label: '记录创建时间', name: 'createtime', width: 200, align: "left" },
                        { label: '记录修改时间', name: 'updatetime', width: 200, align: "left" },
                        { label: '支付宝账号id', name: 'seller_id', width: 200, align: "left" },
                        { label: '支付宝回调所得', name: 'notify_id', width: 200, align: "left" },
                        { label: '交易信息(套餐名称)', name: 'subject', width: 200, align: "left" },
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
