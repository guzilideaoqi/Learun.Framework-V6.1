/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-14 17:35
 * 描  述：提现申请记录
 */
var selectedRow;
var refreshGirdData;
var CheckCashRecord;
var CheckCashRecordByAli;
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
                url: top.$.rootUrl + '/DM_APPManage/DM_Apply_CashRecord/GetPageListByDataTable',
                headData: [
                    //{ label: '记录id', name: 'id', width: 200, align: "left" },
                    //{ label: '用户id', name: 'user_id', width: 200, align: "left" },
                    { label: '用户昵称', name: 'nickname', width: 100, align: "left" },
                    { label: '真实姓名', name: 'realname', width: 100, align: "left" },
                    { label: '手机号', name: 'phone', width: 150, align: "left" },
                    { label: '支付宝', name: 'zfb', width: 200, align: "left" },
                    { label: '提现金额', name: 'price', width: 200, align: "left" },
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
                    { label: '审核时间', name: 'checktime', width: 200, align: "left" },
                    {
                        label: '提现状态', name: 'status', width: 200, align: "left", formatter: function (cellValue, rowData, options) {
                            if (cellValue == 0) {
                                var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'")
                                var btn = "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;\" onclick=\"CheckCashRecord(" + tempJsonStr + ");\"><i class=\"fa fa-plus\"></i>&nbsp;手动转账</a>";
                                btn += "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;\" onclick=\"CheckCashRecordByAli(" + tempJsonStr + ");\"><i class=\"fa fa-plus\"></i>&nbsp;自动转账</a>";
                                return btn;

                            } else if (cellValue == 1)
                                return "审核成功";
                            else if (cellValue == 2)
                                return "审核驳回";
                            else
                                return "未知请求";
                        }
                    },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true
            });
            page.search();
        },
        search: function (param) {
            param = param || { txt_user_id: $("#txt_user_id").val(), txt_nickname: $("#txt_nickname").val(), txt_realname: $("#txt_realname").val(), txt_phone: $("#txt_phone").val() };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    CheckCashRecord = function (rowData) {
        var tip = "请确认是否已经人工打款,一旦操作无法驳回,是否继续？"
        learun.layerConfirm(tip, function (res) {
            if (res) {
                learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/DM_Apply_CashRecord/CheckApplyCashRecord', { id: rowData.id }, function () {
                    location.reload();
                });
            }
        });
    };
    CheckCashRecordByAli= function(rowData) {
        var tip = "确认打款后将直接转账到用户支付宝，是否继续？"
        learun.layerConfirm(tip, function (res) {
            if (res) {
                learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/DM_Apply_CashRecord/CheckApplyCashRecordByAli', { id: rowData.id }, function () {
                    location.reload();
                });
            }
        });
    }
    page.init();
}
