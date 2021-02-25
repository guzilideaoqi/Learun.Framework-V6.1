/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-06 21:08
 * 描  述：身份证实名
 */
var selectedRow;
var refreshGirdData;
var CheckCertifica;
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_CertificaRecord/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_CertificaRecord/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_CertificaRecord/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_CertificaRecord/GetPageList',
                headData: [
                    //{ label: '记录id', name: 'id', width: 200, align: "left" },
                    { label: '用户id', name: 'user_id', width: 200, align: "left" },
                    { label: '真实姓名', name: 'realname', width: 200, align: "left" },
                    { label: '身份证号', name: 'cardno', width: 200, align: "left" },
                    { label: '身份证正面', name: 'facecard', width: 200, align: "left" },
                    { label: '身份证反面', name: 'frontcard', width: 200, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 200, align: "left" },
                    { label: '修改时间', name: 'updatetime', width: 200, align: "left" },
                    //{ label: '实名失败原因', name: 'remark', width: 200, align: "left" },
                    {
                        label: '审核状态', name: 'realstatus', width: 200, align: "left", formatter: function (cellvalue, rowData, options) {
                            if (cellvalue == 2)
                                return "审核驳回";
                            else if (cellvalue == 1)
                                return "审核通过";
                            else {
                                var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'")
                                return "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;\" onclick=\"CheckCertifica(" + tempJsonStr + ");\"><i class=\"fa fa-plus\"></i>&nbsp;审核</a>";
                            }
                        }
                    },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true,
                sidx: "createtime",
                sord: "desc"
            });
            page.search();
        },
        search: function (param) {
            param = param || { txt_realname: $("#txt_realname").val(), txt_cardno: $("#txt_cardno").val(), txt_user_id: $("#txt_user_id").val() };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    CheckCertifica = function (rowData) {
        selectedRow = rowData;
        learun.layerForm({
            id: 'form',
            title: '实名信息审核',
            url: top.$.rootUrl + '/DM_APPManage/DM_CertificaRecord/CheckCertificaRecord?keyValue=' + rowData.id,
            width: 550,
            height: 650,
            btn: ["审核通过", "关闭"],
            callBack: function (id) {
                return top[id].acceptClick(refreshGirdData);
            }
        });
    }
    page.init();
}
