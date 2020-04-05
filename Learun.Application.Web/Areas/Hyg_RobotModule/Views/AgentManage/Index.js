/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2019-11-19 21:15
 * 描  述：代理商管理
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
                    url: top.$.rootUrl + '/Hyg_RobotModule/AgentManage/Form',
                    width: 700,
                    height: 400,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('F_AgentId');
                selectedRow = $('#girdtable').jfGridGet('rowdata');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/Hyg_RobotModule/AgentManage/Form?keyValue=' + keyValue,
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
                var keyValue = $('#girdtable').jfGridValue('F_AgentId');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/Hyg_RobotModule/AgentManage/DeleteForm', { keyValue: keyValue }, function () {
                                location.reload();
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/Hyg_RobotModule/AgentManage/GetPageList',
                headData: [
                        { label: '代理商ID', name: 'F_AgentId', width: 150, align: "left" },
                        { label: '代理商账户', name: 'F_Account', width: 150, align: "left" },
                        { label: '代理商密码', name: 'F_Password', width: 200, align: "left" },
                        { label: '代理商昵称', name: 'F_NickName', width:150, align: "left" },
                        { label: '使用开始时间', name: 'F_AllowStartTime', width: 120, align: "left" },
                        { label: '使用结束时间', name: 'F_AllowEndTime', width: 120, align: "left" },
                        { label: '拼多多佣金比例', name: 'F_PDD_ComissionRate', width: 110, align: "left" },
                        { label: '京东佣金比例', name: 'F_JD_ComissionRate', width: 90, align: "left" },
                        { label: '淘宝佣金比例', name: 'F_TB_ComissionRate', width: 90, align: "left" },
                        { label: '淘宝渠道ID', name: 'F_TB_RelationId', width: 80, align: "left" },
                        { label: '淘宝PID', name: 'F_TB_PID', width: 80, align: "left" },
                        { label: '京东PID', name: 'F_JD_PID', width: 80, align: "left" },
                        { label: '拼多多PID', name: 'F_PDD_PID', width: 80, align: "left" },
                        { label: '应用商ID', name: 'F_ApplicationId', width: 200, align: "left" },
                        { label: '有效标志', name: 'F_EnabledMark', width: 70, align: "left" },
                        { label: '创建日期', name: 'F_CreateDate', width: 140, align: "left" },
                        { label: '创建用户主键', name: 'F_CreateUserId', width: 200, align: "left" },
                        { label: '创建用户', name: 'F_CreateUserName', width: 200, align: "left" },
                        { label: '修改日期', name: 'F_ModifyDate', width: 200, align: "left" },
                        { label: '修改用户主键', name: 'F_ModifyUserId', width: 200, align: "left" },
                        { label: '修改用户', name: 'F_ModifyUserName', width: 200, align: "left" },
                ],
                mainId:'F_AgentId',
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
