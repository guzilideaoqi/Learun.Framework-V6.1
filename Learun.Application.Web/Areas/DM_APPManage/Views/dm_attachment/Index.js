/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-03-03 10:49
 * 描  述：附件管理
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
                    url: top.$.rootUrl + '/DM_APPManage/dm_attachment/Form?keyValue=0',
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
                        url: top.$.rootUrl + '/DM_APPManage/dm_attachment/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/dm_attachment/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/dm_attachment/GetPageList',
                headData: [
                    //{ label: 'id', name: 'id', width: 200, align: "left" },
                    {
                        label: '文件地址', name: 'file_url', width: 200, align: "left", formatter: function (cellvalue, rowdata, options) {
                            if (rowdata.file_type == 1) {
                                return "<img src=\"" + cellvalue + "\" style=\"width:25px;height:25px;\" />";
                            } else {
                                return cellvalue;
                            }
                        }
                    },
                    { label: '文件大小', name: 'file_size', width: 200, align: "left" },
                    { label: '文件名称', name: 'file_name', width: 200, align: "left" },
                    { label: '简称', name: 'file_custom_name', width: 200, align: "left" },
                    {
                        label: '文件类型', name: 'file_type', width: 200, align: "left", formatter: function (cellvalue, rowdata, options) {
                            var typeName = "";
                            switch (cellvalue) {
                                case 1:
                                    typeName = "图片";
                                    break;
                                case 2:
                                    typeName = "视频";
                                    break;
                                case 3:
                                    typeName = "文件";
                                    break;
                                case 4:
                                    typeName = "其他";
                                    break;
                            }
                            return typeName;
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
            var keyword = $('#txt_Keyword').val();

            param = param || { keyword: keyword };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };
    page.init();
}
