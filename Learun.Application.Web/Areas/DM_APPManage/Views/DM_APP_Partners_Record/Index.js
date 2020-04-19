/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-06 21:07
 * 描  述：合伙人申请
 */
var selectedRow;
var refreshGirdData;
var CheckAppPartners;
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_APP_Partners_Record/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_APP_Partners_Record/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_APP_Partners_Record/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_APP_Partners_Record/GetPageList',
                headData: [
                    //{ label: 'id', name: 'id', width: 200, align: "left" },
                    { label: '用户id', name: 'user_id', width: 100, align: "left" },
                    { label: '用户昵称', name: 'nickname', width: 100, align: "left" },
                    { label: '真实姓名', name: 'realname', width: 100, align: "left" },
                    { label: '手机号', name: 'phone', width: 100, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 200, align: "left" },
                    { label: '修改时间', name: 'updatetime', width: 200, align: "left" },
                    {
                        label: '审核状态', name: 'status', width: 100, align: "left", formatter: function (cellvalue, rowData, options) {
                            if (cellvalue == 1)
                                return "已审核";
                            else {
                                var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'")
                                return "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;\" onclick=\"CheckAppPartners(" + tempJsonStr + ");\"><i class=\"fa fa-plus\"></i>&nbsp;审核</a>";
                            }
                        }
                    }
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true
            });
            page.search();
        },
        search: function (param) {
            param = param || { txt_user_id: $("#txt_user_id").val(), txt_nickname: $("#txt_nickname").val(), txt_realname: $("#txt_realname").val(), txt_phone: $("#txt_phone").val()};
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    CheckAppPartners = function (rowData) {
        var tip = "用户【" + rowData.nickname + "】即将成为合伙人,一旦确定无法回退,是否继续？"
        learun.layerConfirm(tip, function (res) {
            if (res) {
                learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/DM_APP_Partners_Record/CheckAppPartnersRecord', { id: rowData.id, status: 1 }, function () {
                    location.reload();
                });
            }
        });
    };
    page.init();
}
