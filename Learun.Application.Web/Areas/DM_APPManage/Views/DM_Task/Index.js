/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-16 16:01
 * 描  述：任务中心
 */
var selectedRow;
var refreshGirdData;
var LookReviceDetail;
var CheckTask;
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.bind();

            page.initGird();
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_Task/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_Task/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_Task/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });

            // 是否需要运费
            $('#txt_status').lrselect({
                width: 200, placeholder: "请选择状态"
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Task/GetPageListByDataTable',
                headData: [
                    //{ label: 'id', name: 'id', width: 200, align: "left" },
                    { label: '任务编号', name: 'task_no', width: 200, align: "left" },
                    { label: 'task_title', name: 'task_title', width: 200, align: "left" },
                    //{ label: '任务类型', name: 'task_type', width: 200, align: "left" },
                    {
                        label: '任务状态', name: 'task_status', width: 80, align: "left", formatter: function (cellvalue, rowData, options) {
                            var status = "";
                            switch (cellvalue) {
                                case -2:
                                    var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'")
                                    status = "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;margin-left:8px;\" onclick=\"CheckTask(" + tempJsonStr + ");\"><i class=\"fa fa-edit\"></i>&nbsp;审核</a>";
                                    break;
                                case 0:
                                    status = "未进行";
                                    break;
                                case 1:
                                    status = "进行中";
                                    break;
                                case 2:
                                    status = "已完成";
                                    break;
                                case 3:
                                    status = "已取消";
                                    break;
                            }

                            return status;
                        }
                    },
                    { label: '任务描述', name: 'task_description', width: 200, align: "left" },
                    //{ label: '任务操作', name: 'task_operate', width: 200, align: "left" },
                    {
                        label: '任务来源', name: 'plaform', width: 80, align: "left", formatter: function (cellvalue, rowData, options) {
                            var status = "";
                            switch (cellvalue) {
                                case 0:
                                    status = "PC";
                                    break;
                                case 1:
                                    status = "移动";
                                    break;
                            }
                            return status;
                        }
                    },
                    { label: '排序值', name: 'sort', width: 80, align: "left" },
                    { label: '总佣金', name: 'totalcommission', width: 80, align: "left" },
                    { label: '服务费', name: 'servicefee', width: 80, align: "left" },
                    { label: '初级佣金', name: 'juniorcommission', width: 80, align: "left" },
                    { label: '高级佣金', name: 'seniorcommission', width: 80, align: "left" },
                    { label: '需求人数', name: 'needcount', width: 80, align: "left" },
                    { label: '完成人数', name: 'finishcount', width: 80, align: "left" },
                    { label: '发布人昵称', name: 'nickname', width: 120, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 150, align: "left" },
                    {
                        label: '操作', name: 'id', width: 200, align: "left", formatter: function (cellvalue, rowData, options) {
                            var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'");
                            //var btnList = "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;\" onclick=\"LookUserDetail(" + tempJsonStr + ");\"><i class=\"fa fa-search\"></i>&nbsp;任务详情</a>";
                            var btnList = "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;margin-left:8px;\" onclick=\"LookReviceDetail(" + tempJsonStr + ");\"><i class=\"fa fa-edit\"></i>&nbsp;接单人信息</a>";

                            return btnList;
                        }
                    }
                    //{ label: '用户ID', name: 'user_id', width: 200, align: "left" },
                    //{ label: '平台ID', name: 'appid', width: 200, align: "left" },
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
            param = param || { txt_title: $("#txt_title").val(), txt_realname: $("#txt_realname").val(), txt_nickname: $("#txt_nickname").val(), txt_phone: $("#txt_phone").val(), txt_status: $('#txt_status').lrselectGet() };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    LookReviceDetail = function (rowData) {
        selectedRow = rowData;
        learun.layerForm({
            id: 'LookReviceDetail',
            title: '接受详情',
            url: top.$.rootUrl + '/DM_APPManage/DM_Task/LookReviceDetail',
            width: 1000,
            height: 550,
            btn: ["关闭"],
            callBack: function (id) {
                return top[id].acceptClick(refreshGirdData);
            }
        });
    };
    CheckTask = function (rowData) {
        selectedRow = rowData;
        learun.layerConfirm('审核通过之后任务将会在APP端展示,如果任务需要取消,请联系发布者在APP端任务详情中操作,是否继续？', function (res) {
            if (res) {
                learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/DM_Task/CheckTaskByWeb', { keyValue: selectedRow.id }, function () {
                    refreshGirdData();
                });
            }
        })
    };
    page.init();
}
