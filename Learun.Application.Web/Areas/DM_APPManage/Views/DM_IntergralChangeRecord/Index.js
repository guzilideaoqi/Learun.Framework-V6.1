/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-14 16:45
 * 描  述：积分兑换记录
 */
var selectedRow;
var refreshGirdData;
var SendExpressNumber;
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_IntergralChangeRecord/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_IntergralChangeRecord/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_IntergralChangeRecord/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_IntergralChangeRecord/GetPageList',
                headData: [
                    //{ label: 'id', name: 'id', width: 200, align: "left"},
                    //{ label: '用户id', name: 'user_id', width: 200, align: "left" },
                    { label: '收件人', name: 'username', width: 100, align: "left" },
                    { label: '手机号', name: 'phone', width: 100, align: "left" },
                    { label: '商品标题', name: 'goodtitle', width: 200, align: "left" },
                    { label: '快递单号', name: 'expresscode', width: 100, align: "left" },
                    { label: '描述(平台描述)', name: 'remark', width: 200, align: "left" },
                    { label: '省份', name: 'province', width: 100, align: "left" },
                    { label: '城市', name: 'city', width: 100, align: "left" },
                    { label: '区县', name: 'down', width: 100, align: "left" },
                    { label: '详细地址', name: 'address', width: 100, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 200, align: "left" },
                    { label: '修改时间', name: 'updatetime', width: 200, align: "left" },
                    {
                        label: '发货状态', name: 'sendstatus', width: 100, align: "left", formatter: function (cellvalue, rowData, options) {
                            if (cellvalue == 1)
                                return "已发货";
                            else {
                                var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'")
                                return "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;\" onclick=\"SendExpressNumber(" + tempJsonStr + ");\"><i class=\"fa fa-plus\"></i>&nbsp;发货</a>";
                            }
                        }
                    },
                    //{ label: '平台ID', name: 'appid', width: 200, align: "left" },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true
            });
            page.search();
        },
        search: function (param) {
            param = param || { txt_phone: $("#txt_phone").val(), txt_username: $("#txt_username").val(), txt_expresscode: $("#txt_expresscode").val() };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    SendExpressNumber = function (rowData) {
        selectedRow = rowData;
        learun.layerForm({
            id: 'form',
            title: '请输入发货单号',
            url: top.$.rootUrl + '/DM_APPManage/DM_IntergralChangeRecord/SendExpressNumber?keyValue=' + rowData.id,
            width: 350,
            height: 140,
            callBack: function (id) {
                return top[id].acceptClick(refreshGirdData);
            }
        });
    };
    page.init();
}
