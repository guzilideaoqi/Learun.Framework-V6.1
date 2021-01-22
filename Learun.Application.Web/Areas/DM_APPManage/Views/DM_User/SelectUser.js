/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-14 16:47
 * 描  述：会员信息
 */
var acceptClick;
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            page.initGird();
        },
        bind: function () {
            $('#btn_Search').on('click', function () {
                page.search();
            });
        },
        initGird: function () {
            $('#girdtable').jfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_User/GetPageListByDataTable',
                headData: [
                    //{ label: '用户id', name: 'id', width: 200, align: "left" },
                    { label: '手机号', name: 'phone', width: 100, align: "left" },
                    { label: '用户昵称', name: 'nickname', width: 100, align: "left" },
                    { label: '账户余额', name: 'accountprice', width: 80, align: "left" },
                    { label: '账户积分', name: 'integral', width: 80, align: "left" },
                    { label: '邀请码', name: 'invitecode', width: 100, align: "left" },
                    { label: '真是姓名', name: 'realname', width: 100, align: "left" },
                    //{ label: '身份证号', name: 'identitycard', width: 100, align: "left" },
                    {
                        label: '实名状态', name: 'isreal', width: 100, align: "left", formatter: function (cellvalue, rows, option) {
                            switch (cellvalue) {
                                case 0:
                                    return "未实名";
                                    break;
                                case 1:
                                    return "已实名";
                                    break;
                            }
                        }
                    },
                    //{ label: '用户登录token', name: 'token', width: 200, align: "left" },
                    //{ label: '密码', name: 'pwd', width: 200, align: "left" },
                    //{ label: '合作伙伴id', name: 'partners', width: 200, align: "left" },
                    {
                        label: '合伙人状态', name: 'partnersstatus', width: 100, align: "left", formatter: function (cellvalue, rows, option) {
                            switch (cellvalue) {
                                case 0:
                                    return "非合伙人";
                                    break;
                                case 1:
                                    return "申请中";
                                    break;
                                case 2:
                                    return "合伙人";
                                    break;
                            }
                        }
                    },

                    {
                        label: '用户等级', name: 'userlevel', width: 100, align: "left", formatter: function (cellvalue, rows, option) {
                            switch (cellvalue) {

                                case 0:
                                    return "普通用户";
                                    break;
                                case 1:
                                    return "中级用户";
                                    break;
                                case 2:
                                    return "高级用户";
                                    break;
                            }
                        }
                    },
                    { label: '创建时间', name: 'createtime', width: 135, align: "left" },
                    { label: '最后登录时间', name: 'last_logintime', width: 135, align: "left" }
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true,
                isMultiselect: false
            });
            page.search();
        },
        search: function (param) {
            console.log(selectedRow)
            param = param || { txt_phone: $("#txt_phone").val(), txt_nickname: $("#txt_nickname").val(), txt_realname: $("#txt_realname").val(), txt_invitecode: $("#txt_invitecode").val(), txt_partners: selectedRow.partners_id };
            console.log(param)
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        var keyValue = $('#girdtable').jfGridValue('id');
        $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_UserRelation/UpdateUserParent', { UserID: selectedRow.id, ParentID: keyValue }, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    page.init();
}
