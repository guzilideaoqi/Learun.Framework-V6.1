/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-16 16:01
 * 描  述：任务中心
 */
var selectedRow;
var refreshGirdData;
var LookReviceDetail;
var LookTaskDetail;
var CheckTask;
var DownTask;
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
                    title: '发布活动任务',
                    url: top.$.rootUrl + '/DM_APPManage/DM_Task/PubTask?isactivity=1',
                    width: 1100,
                    height: 800,
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
                        title: '编辑活动任务',
                        url: top.$.rootUrl + '/DM_APPManage/DM_Task/PubTask?isactivity=1&keyValue=' + keyValue,
                        width: 1100,
                        height: 800,
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
            $('#girdtable').jfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Task/GetPageListByDataTable',
                headData: [
                    //{ label: 'id', name: 'id', width: 200, align: "left" },
                    { label: '任务编号', name: 'task_no', width: 200, align: "left" },
                    {
                        label: '任务标题', name: 'task_title', width: 200, align: "left", formatter: function (cellvalue, rowData, options) {
                            var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'");
                            return "<a onclick=\"LookTaskDetail(" + tempJsonStr + ");\" href=\"#\">" + cellvalue + "</a>";
                        }
                    },
                    //{ label: '任务类型', name: 'task_type', width: 200, align: "left" },
                    {
                        label: '任务状态', name: 'task_status', width: 80, align: "center", formatter: function (cellvalue, rowData, options) {
                            var status = "";
                            switch (cellvalue) {
                                case -2:
                                    var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'");
                                    status = "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;margin-left:8px;\" onclick=\"CheckTask(" + tempJsonStr + ");\"><i class=\"fa fa-edit\"></i>&nbsp;审核</a>";
                                    break;
                                case 0:
                                    var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'");
                                    status = "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;margin-left:8px;\" onclick=\"DownTask(" + tempJsonStr + ");\"><i class=\"fa fa-edit\"></i>&nbsp;下架</a>";
                                    //status = "进行中";
                                    break;
                                case 1:
                                    status = "已完成";
                                    break;
                                case 2:
                                    status = "已取消";
                                    break;
                                case 3:
                                    status = "已下架";
                                    break;
                            }

                            return status;
                        }
                    },
                    {
                        label: '任务描述', name: 'task_description', width: 200, align: "left", formatter: function (cellvalue, rowData, options) {
                            var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'");
                            return "<a onclick=\"LookTaskDetail(" + tempJsonStr + ");\" href=\"#\">" + cellvalue + "</a>";
                        }
                    },
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
                    { label: '权重', name: 'sort', width: 80, align: "left" },
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
                            var btnList = "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;\" onclick=\"LookTaskDetail(" + tempJsonStr + ");\"><i class=\"fa fa-search\"></i>&nbsp;任务详情</a>";
                            btnList += "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;margin-left:8px;\" onclick=\"LookReviceDetail(" + tempJsonStr + ");\"><i class=\"fa fa-edit\"></i>&nbsp;接单人信息</a>";

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
            param = param || { txt_title: $("#txt_title").val(), txt_realname: $("#txt_realname").val(), txt_nickname: $("#txt_nickname").val(), txt_phone: $("#txt_phone").val(), txt_status: $('#txt_status').lrselectGet(), isactivity: 1 };
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
    LookTaskDetail = function (rowData) {
        selectedRow = rowData;
        learun.layerForm({
            id: 'LookTaskDetail',
            title: '任务详情',
            url: top.$.rootUrl + '/DM_APPManage/DM_Task/LookTaskDetail?TaskID=' + selectedRow.id,
            width: 600,
            height: 700,
            btn: ["关闭"],
            callBack: function (id) {
                return top[id].acceptClick(refreshGirdData);
            }
        });
    };
    CheckTask = function (rowData) {
        selectedRow = rowData;
        learun.layerMutipleBtnConfirm({
            btn: ["审核通过", "审核驳回", "关闭"],
            content: "请选择对任务的操作?",
            yes: function (index, layero) {
                learun.layerConfirm('审核通过之后任务将会在APP端展示,如果任务需要取消,请联系发布者在APP端任务详情中操作,是否继续？', function (res) {
                    if (res) {
                        learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/DM_Task/CheckTaskByWeb', { keyValue: selectedRow.id }, function () {
                            refreshGirdData();
                        });
                    }
                })
            }, btn2: function (index, layero) {
                //按钮【按钮二】的回调
                learun.layerForm({
                    id: 'form',
                    title: '驳回原因',
                    url: top.$.rootUrl + '/DM_APPManage/DM_Task/RebutTask?keyValue=' + selectedRow.id,
                    width: 600,
                    height: 400,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
                //return false; //开启该代码可禁止点击该按钮关闭
            }
            , btn3: function (index, layero) {
                //按钮【按钮三】的回调
                //return false 开启该代码可禁止点击该按钮关闭
            }
        })
    };

    DownTask = function (rowData) {
        selectedRow = rowData;
        learun.layerConfirm('任务下架后将无法在APP展示,已经接受任务的不影响返佣,是否继续操作？', function (res) {
            if (res) {
                learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/DM_Task/DownTask', { keyValue: selectedRow.id }, function () {
                    refreshGirdData();
                });
            }
        })
    };
    page.init();
}
