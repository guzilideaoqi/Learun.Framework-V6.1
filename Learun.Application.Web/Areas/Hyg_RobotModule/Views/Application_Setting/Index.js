/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2019-11-19 20:34
 * 描  述：应用商信息设置
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
                    url: top.$.rootUrl + '/Hyg_RobotModule/Application_Setting/Form',
                    width: 700,
                    height: 400,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('F_SettingId');
                selectedRow = $('#girdtable').jfGridGet('rowdata');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/Hyg_RobotModule/Application_Setting/Form?keyValue=' + keyValue,
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
                var keyValue = $('#girdtable').jfGridValue('F_SettingId');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/Hyg_RobotModule/Application_Setting/DeleteForm', { keyValue: keyValue}, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/Hyg_RobotModule/Application_Setting/GetPageList',
                headData: [
                        { label: '配置表ID', name: 'F_SettingId', width: 200, align: "left" },
                        { label: '应用ID', name: 'F_ApplicationId', width: 200, align: "left" },
                        { label: '联盟账户ID', name: 'F_TB_AccountId', width: 200, align: "left" },
                        { label: '淘宝AppKey', name: 'F_TB_AppKey', width: 200, align: "left" },
                        { label: '淘宝AppSecret', name: 'F_TB_Secret', width: 200, align: "left" },
                        { label: '淘宝SessionKey', name: 'F_TB_SessionKey', width: 200, align: "left" },
                        { label: '淘宝授权到期时间', name: 'F_TB_AuthorEndTime', width: 200, align: "left" },
                        { label: '京东联盟账户ID', name: 'F_JD_AccountId', width: 200, align: "left" },
                        { label: '京东AppKey', name: 'F_JD_AppKey', width: 200, align: "left" },
                        { label: '京东Secret', name: 'F_JD_Secret', width: 200, align: "left" },
                        { label: '京东SessionKey', name: 'F_JD_SessionKey', width: 200, align: "left" },
                        { label: '多多进宝账户ID', name: 'F_PDD_AccountId', width: 200, align: "left" },
                        { label: '拼多多ClientID', name: 'F_PDD_ClientID', width: 200, align: "left" },
                        { label: '拼多多ClientSecret', name: 'F_PDD_ClientSecret', width: 200, align: "left" },
                        { label: '应用商名称', name: 'F_ApplicationName', width: 200, align: "left" },
                        { label: '应用商Logo', name: 'F_ApplicationLogo', width: 200, align: "left" },
                        { label: '公司名称', name: 'F_CompanyName', width: 200, align: "left" },
                        { label: '联系方式', name: 'F_Telephone', width: 200, align: "left" },
                        { label: 'QQ号', name: 'F_OICQ', width: 200, align: "left" },
                        { label: '微信号', name: 'F_WeChat', width: 200, align: "left" },
                        { label: '描述信息', name: 'F_Description', width: 200, align: "left" },
                        { label: '创建日期', name: 'F_CreateDate', width: 200, align: "left" },
                        { label: '创建用户主键', name: 'F_CreateUserId', width: 200, align: "left" },
                        { label: '创建用户', name: 'F_CreateUserName', width: 200, align: "left" },
                        { label: '修改日期', name: 'F_ModifyDate', width: 200, align: "left" },
                        { label: '修改用户主键', name: 'F_ModifyUserId', width: 200, align: "left" },
                        { label: '修改用户', name: 'F_ModifyUserName', width: 200, align: "left" },
                ],
                mainId:'F_SettingId',
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
