/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-16 16:01
 * 描  述：任务中心
 */
var acceptClick;
var selectedRow;
var refreshGirdData;
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.initGird();
            page.bind();
        },
        bind: function () {

        },
        initGird: function () {
            $('#reviceTaskList').jfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Task_Revice/GetPageListByDataTable',
                headData: [
                    //{ label: 'id', name: 'id', width: 200, align: "left" },
                    //{ label: '任务id', name: 'task_id', width: 40, align: "left" },
                    { label: '用户昵称', name: 'nickname', width: 80, align: "left" },
                    { label: '真实姓名', name: 'realname', width: 80, align: "left" },
                    { label: '手机号', name: 'phone', width: 100, align: "left" },
                    //{ label: '用户id', name: 'user_id', width: 200, align: "left" },
                    { label: '接受时间', name: 'revice_time', width: 140, align: "left" },
                    {
                        label: '接收状态', name: 'status', width: 80, align: "left", formatter: function (cellvalue, rowData, options) {
                            var status = "";
                            switch (cellvalue) {
                                case 1:
                                    status = "进行中";
                                    break;
                                case 2:
                                    status = "待审核";
                                    break;
                                case 3:
                                    status = "已完成";
                                    break;
                                case 4:
                                    status = "取消接受";
                                    break;
                            }
                            return status;
                        }
                    },
                    { label: '提交时间', name: 'submit_time', width: 140, align: "left" },
                    //{ label: '提交数据', name: 'submit_data', width: 200, align: "left" },
                    { label: '审核时间', name: 'check_time', width: 140, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 140, align: "left" },
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
            param = param || { Task_ID: selectedRow.id };
            $('#reviceTaskList').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    // 保存数据
    acceptClick = function (callBack) {
        learun.layerClose("LookReviceDetail", "");
    };
    page.init();
}
