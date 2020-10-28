/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-10-28 10:06
 * 描  述：官推文案
 */
var selectedRow;
var refreshGirdData;
var LookCircleDetail;
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.bind();
            page.initGird();
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
                    url: top.$.rootUrl + '/DM_APPManage/dm_friend_circle/Form?keyValue=0',
                    width: 700,
                    height: 600,
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
                        url: top.$.rootUrl + '/DM_APPManage/dm_friend_circle/Form?keyValue=' + keyValue,
                        width: 700,
                        height: 600,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/dm_friend_circle/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });

            //上架文章
            $('#lr_up').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认上架该文章！', function (res) {
                        if (res) {
                            learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/dm_friend_circle/Up', { loading: "正在上架文章...", keyValue: keyValue }, function () {
                                page.search();
                            })
                        }
                    })
                }
            });

            //下架文章
            $('#lr_down').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('id');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认下架该文章！', function (res) {
                        if (res) {
                            learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/dm_friend_circle/Down', { loading: "正在下架文章...", keyValue: keyValue }, function () {
                                page.search();
                            })
                        }
                    })
                }
            });

            $('#t_status').lrselect({
                width: 200, placeholder: "请选择状态"
            });

            $('#t_type').lrselect({
                width: 200, placeholder: "请选择文案类型"
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/dm_friend_circle/GetPageList',
                headData: [
                    //{ label: '朋友圈文案id', name: 'id', width: 200, align: "left" },
                    { label: '文案内容', name: 't_content', width: 200, align: "left" },
                    {
                        label: '文案类型', name: 't_type', width: 200, align: "left", formatter: function (cellvalue, rowData, options) {
                            if (cellvalue == 0)
                                return "普通文案";
                            else if (cellvalue == 1) {
                                return "官推文案"
                            }
                        }
                    },
                    {
                        label: '文案类型', name: 't_status', width: 200, align: "left", formatter: function (cellvalue, rowData, options) {
                            if (cellvalue == 0)
                                return "<span style='color:red;'>未上架</span>";
                            else if (cellvalue == 1) {
                                return "<span style='color:green;'>上架成功</span>";
                            } else if (cellvalue == 2) {
                                return "<span style='color:#c3c3c3;'>已下架</span>";
                            }
                        }
                    },
                    //{ label: '图片链接数组', name: 't_images', width: 200, align: "left" },
                    { label: '排序', name: 't_sort', width: 200, align: "left" },
                    { label: '创建人', name: 'createcode', width: 200, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 200, align: "left" },
                    {
                        label: '操作', name: 'id', width: 200, align: "left", formatter: function (cellvalue, rowData, options) {
                            var tempJsonStr = JSON.stringify(rowData).replace(/\"/g, "'");
                            return "<a id=\"lr_add\"  class=\"btn btn-success\" style=\"padding:1px 6px;font-size:12px;\" onclick=\"LookCircleDetail(" + tempJsonStr + ");\"><i class=\"fa fa-search\"></i>&nbsp;文案预览</a>"
                        }
                    },
                ],
                mainId: 'id',
                reloadSelected: true,
                isPage: true
            });
            page.search();
        },
        search: function (param) {
            param = param || { t_type: $("#t_type").lrselectGet(), t_status: $("#t_status").lrselectGet() };
            $('#girdtable').jfGridSet('reload', { param: { queryJson: JSON.stringify(param) } });
        }
    };
    refreshGirdData = function () {
        page.search();
    };

    LookCircleDetail = function (rowdata) {
        selectedRow = rowdata;
        learun.layerForm({
            id: 'form',
            title: '文案预览',
            url: top.$.rootUrl + '/DM_APPManage/dm_friend_circle/PreviewCircle',
            width: 700,
            height: 600
        });
    }
    page.init();
}
