/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-03-06 11:16
 * 描  述：dm_duomai_order商品
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
                    url: top.$.rootUrl + '/DM_APPManage/dm_duomai_order_good/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/dm_duomai_order_good/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/dm_duomai_order_good/DeleteForm', { keyValue: keyValue}, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/dm_duomai_order_good/GetPageList',
                headData: [
                        { label: 'id', name: 'id', width: 200, align: "left" },
                        { label: '商品类目，以实际商家平台结果为准', name: 'goods_cate', width: 200, align: "left" },
                        { label: '商品类目名称，有可能是数字标识、文字说明，以实际商家平台结果为准', name: 'goods_cate_name', width: 200, align: "left" },
                        { label: '商品编号', name: 'goods_id', width: 200, align: "left" },
                        { label: '商品名称', name: 'goods_name', width: 200, align: "left" },
                        { label: '商品单价', name: 'goods_price', width: 200, align: "left" },
                        { label: '商品件数', name: 'goods_ta', width: 200, align: "left" },
                        { label: '订单金额，例：1.00', name: 'orders_price', width: 200, align: "left" },
                        { label: '商家平台原始订单状态描述，有可能是英文、中文、数值等，以实际结果为准', name: 'order_status', width: 200, align: "left" },
                        { label: '预估站长佣金，非结算站长佣金，例：1.00', name: 'order_commission', width: 200, align: "left" },
                        { label: '订单号', name: 'order_sn', width: 200, align: "left" },
                        { label: '父订单号/子订单号', name: 'parent_order_sn', width: 200, align: "left" },
                        { label: '商品图片地址', name: 'goods_img', width: 200, align: "left" },
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
