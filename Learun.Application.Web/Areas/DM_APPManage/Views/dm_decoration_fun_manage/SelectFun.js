/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-14 16:47
 * 描  述：会员信息
 */
var acceptClick;
var keyValue = request('keyValue');
var fun_type = 1;  var selectFun;
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            page.initData();
            page.initGird();
        },
        bind: function () {
            // 代码查看
            $('#nav_tabs').lrFormTabEx(function (id) {
                fun_type = id;
                page.search();
            });

            $("#btn_Search").on("click", function () {
                page.search();
            })
        },
        initData: function () {

        },
        initGird: function () {
            $('#girdtable').jfGrid({
                url: top.$.rootUrl + '/DM_APPManage/dm_decoration_fun_manage/GetPageList',
                headData: [
                    //{ label: 'id', name: 'id', width: 200, align: "left" },
                    { label: '功能名称', name: 'fun_name', width: 120, align: "left" },
                    //{
                    //    label: '功能类型', name: 'fun_type', width: 100, align: "left", formatter: function (cellvalue, rowdata, options) {
                    //        var typeName = "";
                    //        switch (cellvalue) {
                    //            case 1:
                    //                typeName = "原生";
                    //                break;
                    //            case 2:
                    //                typeName = "多麦";
                    //                break;
                    //            case 3:
                    //                typeName = "站内H5";
                    //                break;
                    //        }
                    //        return typeName;
                    //    }
                    //},
                    { label: '功能对应参数值', name: 'fun_param', width: 100, align: "left" },
                    { label: '功能描述', name: 'fun_remark', width: 155, align: "left" },
                    { label: '修改时间', name: 'updatetime', width: 200, align: "left" },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true,
                isShowNum: false
            });
            page.search();
        },
        search: function (param) {
            param = param || { fun_type: fun_type, keyword: $("#txt_Keyword").val() };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }

    };
    selectFun = function () {
        var selectedData = $('#girdtable').jfGridGet('rowdata');
        return selectedData;
        //if (!!selectedData)
        //    return selectedData.id;
    }
    page.init();
}