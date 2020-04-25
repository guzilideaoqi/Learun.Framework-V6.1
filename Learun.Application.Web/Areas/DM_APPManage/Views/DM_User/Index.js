/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-14 16:47
 * 描  述：会员信息
 */
var selectedRow;
var refreshGirdData;
var LookUserDetail;
var UpdateAccountPrice;
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_User/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_User/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_User/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
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
                    { label: '修改时间', name: 'updatetime', width: 100, align: "left" },
                    {
                        label: '操作', name: 'id', width: 300, align: "left", formatter: function (cellvalue, rowData, option) {
                            var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'")
                            var btnList = "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;\" onclick=\"LookUserDetail(" + tempJsonStr + ");\"><i class=\"fa fa-search\"></i>&nbsp;查看会员详情</a>";
                            btnList += "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;margin-left:8px;\" onclick=\"UpdateAccountPrice(" + tempJsonStr + ");\"><i class=\"fa fa-edit\"></i>&nbsp;修改余额</a>";

                            return btnList;
                        }
                    },
                    //{ label: '平台ID', name: 'appid', width: 200, align: "left" },
                    /*{ label: '淘宝pid', name: 'tb_pid', width: 150, align: "left" },
                    { label: '拼多多pid', name: 'pdd_pid', width: 100, align: "left" },
                    { label: '淘宝渠道id', name: 'tb_relationid', width: 80, align: "left" },
                    { label: '跟单渠道id', name: 'tb_orderrelationid', width: 80, align: "left" },
                    { label: '京东pid', name: 'jd_pid', width: 80, align: "left" },
                    { label: '省份', name: 'province', width: 50, align: "left" },
                    { label: '市', name: 'city', width: 50, align: "left" },
                    { label: '县区乡镇', name: 'down', width: 80, align: "left" },
                    { label: '详细地址', name: 'address', width: 200, align: "left" },*/
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true
            });
            page.search();
        },
        search: function (param) {
            param = param || { txt_user_id: $("#txt_user_id").val(), txt_phone: $("#txt_phone").val(), txt_nickname: $("#txt_nickname").val(), txt_realname: $("#txt_realname").val(), txt_invitecode: $("#txt_invitecode").val(), txt_partners: $("#txt_partners").val() };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    UpdateAccountPrice = function (rowData) {
        selectedRow = rowData;
        learun.layerForm({
            id: 'form',
            title: '修改账户余额',
            url: top.$.rootUrl + '/DM_APPManage/DM_User/UpdateAccountPrice',
            width: 600,
            height: 400,
            callBack: function (id) {
                return top[id].acceptClick(refreshGirdData);
            }
        });
    }
    LookUserDetail = function (rowData) {
        selectedRow = rowData;
        learun.layerForm({
            id: 'LookUserDetail',
            title: '用户详情',
            url: top.$.rootUrl + '/DM_APPManage/DM_User/LookUserDetail',
            width: 600,
            height: 550,
            btn:["关闭"],
            callBack: function (id) {
                return top[id].acceptClick(refreshGirdData);
            }
        });
    }
    page.init();
}
