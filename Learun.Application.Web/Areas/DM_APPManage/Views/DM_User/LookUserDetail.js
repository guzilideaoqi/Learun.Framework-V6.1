/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-14 16:47
 * 描  述：会员信息
 */
var acceptClick;
var keyValue = request('keyValue');
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            // 代码查看
            $('#nav_tabs').lrFormTabEx();

            $("#lr_clear_auth").on('click', function () {
                var tip = "清除授权后用户在APP端将无法正常淘宝购物,是否继续？"
                learun.layerConfirm(tip, function (res) {
                    if (res) {
                        learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/DM_User/Clear_TB_Relation_Auth', { User_ID: selectedRow.id }, function () {
                            learun.layerClose("LookUserDetail", "");
                        });
                    }
                });
            })
        },
        initData: function () {
            if (!!selectedRow) {
                $.lrSetForm(top.$.rootUrl + '/DM_APPManage/DM_User/GetFormData?keyValue=' + selectedRow.id, function (data) {//
                    $('#form').lrSetFormData(data);

                    if (!!data.tb_relationid)
                        $("#clearauthbyn").css("display", "block");
                })

                page.accountpriceGrid(selectedRow.id);
                page.initIntegralGird(selectedRow.id);
                page.initMyChild(selectedRow.id);
            }
        },
        accountpriceGrid: function (user_id) {
            $('#accountpriceRecord').jfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_AccountDetail/GetPageList',
                height: 440,
                headData: [
                    { label: '当前账户余额', name: 'currentvalue', width: 100, align: "left" },
                    { label: '变动余额', name: 'stepvalue', width: 100, align: "left" },
                    { label: '变更说明', name: 'title', width: 200, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 150, align: "left" },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true,
                isShowNum: true,
                sidx: "createtime",
                sord: "desc"
            });
            page.search_price({ queryJson: JSON.stringify({ user_id: user_id }) });
        },
        initIntegralGird: function (user_id) {
            $('#integralRecord').jfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_IntergralDetail/GetPageList',
                height: 440,
                headData: [
                    //{ label: 'id', name: 'id', width: 40, align: "left" },
                    { label: '当前积分', name: 'currentvalue', width: 100, align: "left" },
                    { label: '变动积分', name: 'stepvalue', width: 100, align: "left" },
                    //{ label: '用户id', name: 'user_id', width: 200, align: "left" },
                    { label: '变更说明', name: 'title', width: 200, align: "left" },
                    //{ label: '类型(待定)', name: 'type', width: 200, align: "left" },
                    //{ label: '描述', name: 'remark', width: 200, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 150, align: "left" },
                    //{ label: '创建人', name: 'createcode', width: 200, align: "left" },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true,
                isShowNum: true,
                sidx: "createtime",
                sord: "desc"
            });
            page.search({ queryJson: JSON.stringify({ user_id: user_id }) });
        },
        initMyChild: function (user_id) {
            $('#mysubuser').jfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_User/GetChildUser',
                height: 440,
                headData: [
                    //{ label: 'id', name: 'id', width: 40, align: "left" },
                    { label: '用户昵称', name: 'nickname', width: 100, align: "left" },
                    {
                        label: '手机号', name: 'stepvalue', width: 100, align: "left", formatter: function (cellvalue, rowData, options) {
                            return cellvalue;
                            if (cellvalue.length == 11) {
                                var str2 = cellvalue.substr(0, 3) + "****" + cellvalue.substr(7);
                                return str2;
                            }
                            else {
                                return cellvalue;
                            }
                        }
                    },
                    //{ label: '用户id', name: 'user_id', width: 200, align: "left" },
                    { label: '真实姓名', name: 'realname', width: 200, align: "left" },
                    //{ label: '类型(待定)', name: 'type', width: 200, align: "left" },
                    //{ label: '描述', name: 'remark', width: 200, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 150, align: "left" },
                    //{ label: '创建人', name: 'createcode', width: 200, align: "left" },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: false,
                isShowNum: true,
                sidx: "createtime",
                sord: "desc"
            });
            page.search_mysub(user_id);
        },
        search: function (param) {
            $('#integralRecord').jfGridSet('reload', { param: param });
        },
        search_price: function (param) {
            $('#accountpriceRecord').jfGridSet('reload', { param: param });
        },
        search_mysub: function (param) {
            $('#mysubuser').jfGridSet('reload', { param: { userid: param } });
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        learun.layerClose("LookUserDetail", "");
    };
    page.init();
}
