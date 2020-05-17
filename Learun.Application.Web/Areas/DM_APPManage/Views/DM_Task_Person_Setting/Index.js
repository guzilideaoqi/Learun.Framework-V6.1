/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-10 13:55
 * 描  述：进度任务设置
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_Task_Person_Setting/Form?keyValue=0',
                    width: 700,
                    height: 500,
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_Task_Person_Setting/Form?keyValue=' + keyValue,
                        width: 700,
                        height: 500,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_Task_Person_Setting/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Task_Person_Setting/GetPageList',
                headData: [
                    //{ label: 'id', name: 'id', width: 200, align: "left" },
                    { label: '任务标题', name: 'title', width: 200, align: "left" },
                    { label: '任务描述', name: 'remark', width: 200, align: "left" },
                    {
                        label: '任务类型', name: 's_type', width: 80, align: "left", formatter: function (cellvalue, options, rowData) {
                            //1每日签到任务  2邀请粉丝任务 3团队粉丝任务  4购物任务  5团队购物任务
                            var typeName = "未知";
                            switch (cellvalue) {

                                case 1:
                                    typeName = "每日签到任务";
                                    break;
                                case 2:
                                    typeName = "邀请粉丝任务";
                                    break;
                                case 3:
                                    typeName = "团队粉丝任务";
                                    break;
                                case 4:
                                    typeName = "购物任务";
                                    break;
                                case 5:
                                    typeName = "团队购物任务";
                                    break;
                                case 6:
                                    typeName = "浏览商品";
                                    break;
                            }

                            return typeName;
                        }
                    },
                    { label: '所需人数', name: 'needcount', width: 80, align: "left" },
                    {
                        label: '是否为合伙人任务', name: 'ispartners', width: 200, align: "left", formatter: function (cellvalue, options, rowData) {
                            var typeName = "未知";
                            switch (cellvalue) {
                                case 0:
                                    typeName = "否";
                                    break;
                                case 1:
                                    typeName = "是";
                                    break;
                            }

                            return typeName;
                        }
                    },
                    {
                        label: '启用状态', name: 'isenabled', width: 80, align: "left", formatter: function (cellvalue, options, rowData) {
                            var typeName = "未知";
                            switch (cellvalue) {
                                case 0:
                                    typeName = "禁用";
                                    break;
                                case 1:
                                    typeName = "启用";
                                    break;
                            }

                            return typeName;
                        }
                    },
                    { label: '创建时间', name: 'createtime', width: 150, align: "left" },
                    { label: '修改时间', name: 'updatetime', width: 150, align: "left" },
                    //{ label: '平台id', name: 'appid', width: 200, align: "left" },
                    {
                        label: '奖励类型', name: 'rewardtype', width: 80, align: "left", formatter: function (cellvalue, options, rowData) {
                            var typeName = "未知";
                            switch (cellvalue) {
                                case 0:
                                    typeName = "积分奖励";
                                    break;
                                case 1:
                                    typeName = "余额奖励";
                                    break;
                            }

                            return typeName;
                        }
                    },
                    { label: '奖励值', name: 'rewardcount', width: 80, align: "left" },
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
