/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-14 17:35
 * 描  述：提现申请记录
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_Apply_CashRecord/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_Apply_CashRecord/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_Apply_CashRecord/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Apply_CashRecord/GetPageList',
                headData: [
                    { label: '记录id', name: 'id', width: 200, align: "left" },
                    { label: '用户id', name: 'user_id', width: 200, align: "left" },
                    { label: '提现金额', name: 'price', width: 200, align: "left" },
                    {
                        label: '提现状态', name: 'status', width: 200, align: "left", formatter: function (cellValue, rowData, options) {
                            if (cellValue == 0)
                                return "未审核";
                            else if (cellValue == 1)
                                return "审核成功";
                            else if (cellValue == 2)
                                return "审核驳回";
                            else
                                return "未知请求";
                        }
                    },
                    {
                        label: '打款方式', name: 'paytype', width: 200, align: "left", formatter: function (cellValue, rowData, options) {
                            if (cellValue == 0)
                                return "未打款";
                            else if (cellValue == 1)
                                return "手动打款";
                            else if (cellValue == 2)
                                return "支付宝打款";
                            else
                                return "未知请求";
                        }
                    },
                    { label: '申请提现备注信息', name: 'remark', width: 200, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 200, align: "left" },
                ],
                mainId: 'id',
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
