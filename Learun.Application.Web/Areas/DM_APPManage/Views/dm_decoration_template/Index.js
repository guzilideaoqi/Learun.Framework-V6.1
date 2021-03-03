/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-03-03 10:56
 * 描  述：装修模板
 */
var selectedRow;
var refreshGirdData;
var decoration;
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
                    url: top.$.rootUrl + '/DM_APPManage/dm_decoration_template/Form?keyValue=0',
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
                        url: top.$.rootUrl + '/DM_APPManage/dm_decoration_template/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/dm_decoration_template/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/dm_decoration_template/GetPageList',
                headData: [
                    { label: '模板名称', name: 'template_name', width: 200, align: "left" },
                    { label: '模板备注', name: 'template_remark', width: 200, align: "left" },
                    {
                        label: '主色', name: 'main_color', width: 200, align: "left", formatter: function (cellvalue, rowdata, options) {
                            return "<div style=\"background-color:" + cellvalue + ";\">" + cellvalue + "</div>";
                        }
                    },
                    {
                        label: '辅色', name: 'secondary_color', width: 200, align: "left", formatter: function (cellvalue, rowdata, options) {
                            return "<div style=\"background-color:" + cellvalue + ";\">" + cellvalue + "</div>";
                        }
                    },
                    {
                        label: '模板状态', name: 'template_status', width: 200, align: "left", formatter: function (cellvalue, rowdata, options) {
                            var typeName = "";
                            switch (cellvalue) {
                                case 0:
                                    typeName = "禁用";
                                    break;
                                case 1:
                                    typeName = "使用中";
                                    break;
                            }
                            return typeName;
                        }
                    },
                    {
                        label: '装修', name: 'id', width: 200, align: "left", formatter: function (cellvalue, rowdata, options) {
                            return "<span class=\"label label-success\" style=\"cursor: pointer;\" onclick=\"decoration('" + cellvalue + "','" + rowdata.template_name + "')\">装修</span>";
                        }
                    },
                    { label: '创建时间', name: 'createtime', width: 200, align: "left" },
                    { label: '修改时间', name: 'updatetime', width: 200, align: "left" },
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
    decoration = function (id, name) {
        learun.layerForm({
            id: 'decoration',
            title: name + '--装修',
            url: top.$.rootUrl + '/DM_APPManage/dm_decoration_template/DecorationTemplate',
            width: 600,
            height: 700,
            callBack: function (id) {
                return top[id].acceptClick(refreshGirdData);
            }
        });
    }
    page.init();
}
