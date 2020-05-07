/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-16 16:02
 * 描  述：任务举报记录
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_Task_Report/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_Task_Report/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_Task_Report/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Task_Report/GetPageListByDataTable',
                headData: [
                    //{ label: 'id', name: 'id', width: 200, align: "left" },
                    //{ label: '举报用户', name: 'user_id', width: 200, align: "left" },
                    { label: '举报人手机号', name: 'phone', width: 120, align: "left" },
                    { label: '昵称', name: 'nickname', width: 100, align: "left" },
                    { label: '真实姓名', name: 'realname', width: 100, align: "left" },
                    { label: '任务编号', name: 'task_no', width: 200, align: "left" },
                    { label: '任务标题', name: 'task_title', width: 200, align: "left" },
                    { label: '举报内容', name: 'report_content', width: 200, align: "left" },
                    { label: '举报时间', name: 'report_time', width: 135, align: "left" },
                    //{ label: '平台id', name: 'appid', width: 200, align: "left" },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true
            });
            page.search();
        },
        search: function (param) {
            param = param || { txt_phone: $("#txt_phone").val(), txt_nickname: $("#txt_nickname").val(), txt_realname: $("#txt_realname").val(), txt_task_no: $("#txt_task_no").val() };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    page.init();
}
