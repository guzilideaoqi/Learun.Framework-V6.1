/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-17 22:06
 * 描  述：导航图管理
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_Banner/Form?keyValue=0',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_Banner/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_Banner/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Banner/GetPageList',
                headData: [
                    { label: '导航图id', name: 'id', width: 200, align: "left" },
                    {
                        label: '导航图类型', name: 'b_type', width: 200, align: "left", formatter: function (cellvalue, rowdata, options) {
                            var returnValue = "";
                            switch (cellvalue) {

                                case 0:
                                    returnValue= "首页导航";
                                    break;
                                case 1:
                                    returnValue = "京东导航";
                                    break;
                                case 2:
                                    returnValue = "拼多多导航";
                                    break;
                            }
                            return returnValue;
                        }
                    },
                    { label: '导航图标题', name: 'b_title', width: 200, align: "left" },
                    { label: '导航图图片地址', name: 'b_image', width: 200, align: "left" },
                    { label: '导航图参数', name: 'b_param', width: 200, align: "left" },
                    { label: 'appid', name: 'appid', width: 200, align: "left" },
                    { label: 'createtime', name: 'createtime', width: 200, align: "left" },
                    { label: 'createcode', name: 'createcode', width: 200, align: "left" },
                ],
                mainId: 'id',
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
