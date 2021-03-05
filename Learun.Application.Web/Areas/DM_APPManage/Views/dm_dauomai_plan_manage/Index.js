/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-03-03 10:50
 * 描  述：多麦计划
 */
var selectedRow;
var refreshGirdData;
var disable_fun;
var enable_fun;
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
                    title: '同步推广计划',
                    url: top.$.rootUrl + '/DM_APPManage/dm_dauomai_plan_manage/SyncPlan',
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
                        url: top.$.rootUrl + '/DM_APPManage/dm_dauomai_plan_manage/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/dm_dauomai_plan_manage/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/dm_dauomai_plan_manage/GetPageList',
                headData: [
                    //{ label: 'f_id', name: 'f_id', width: 200, align: "left" },
                    {
                        label: '计划logo', name: 'ads_logo', width: 100, align: "left", formatter: function (cellvalue, rowdata, options) {
                            return "<img src=\"" + cellvalue+"\" style=\"height:30px;\" />"
                        }
                    },
                    { label: '计划ID', name: 'ads_id', width: 80, align: "left" },
                    {
                        label: '推广计划名称', name: 'ads_name', width: 200, align: "left", formatter: function (cellvalue, rowdata, options) {
                            return "<a href=\"" + rowdata.url + "\" target=\"_blank\">" + cellvalue + "</a>"
                        }
                    },
                    {
                        label: '激活状态', name: 'use_status', width: 70, align: "left", formatter: function (cellvalue, rowdata, options) {
                            if (cellvalue == 1) {
                                return "<span class=\"label label-success\" style=\"cursor: pointer;\" onclick=\"disable_fun('" + rowdata.id + "')\">激活中</span>";
                            } else {
                                return "<span class=\"label label-default\" style=\"cursor: pointer;\" onclick=\"enable_fun('" + rowdata.id + "')\">未激活</span>";
                            }
                        }
                    },
                    { label: '所属商场名称', name: 'store_name', width: 100, align: "left" },
                    {
                        label: '活动类型', name: 'channel', width: 100, align: "left", formatter: function (cellvalue, rowdata, options) {
                            var typename = "";
                            switch (cellvalue) {
                                case "0":
                                    typename = "web";
                                    break;
                                case "1":
                                    typename = "wap";
                                    break;
                                case "3":
                                    typename = "ROI";
                                    break;
                                case "4":
                                    typename = "小程序";
                                    break;
                            }
                            return typename;
                        }
                    },
                    { label: 'RD有效期', name: 'rddays', width: 80, align: "left" },
                    //{ label: '当category=1 海外商家时，此字段表示计划允许的跟单地区', name: 'category_area', width: 200, align: "left" },
                    {
                        label: '商家类型', name: 'category', width: 100, align: "left", formatter: function (cellvalue, rowdata, options) {
                            var typename = "";
                            switch (cellvalue) {
                                case "0":
                                    typename = "国内商家";
                                    break;
                                case "1":
                                    typename = "海外商家";
                                    break;
                                case "2":
                                    typename = "跨境电商";
                                    break;
                            }
                            return typename;
                        }
                    },
                    {
                        label: '审核方式', name: 'apply_mode', width: 100, align: "left", formatter: function (cellvalue, rowdata, options) {
                            var typename = "";
                            switch (cellvalue) {
                                case "1":
                                    typename = "人工审核";
                                    break;
                                case "2":
                                    typename = "自动通过";
                                    break;
                            }
                            return typename;
                        }
                    },
                    { label: '计划开始时间', name: 'stime', width: 150, align: "left" },
                    { label: '计划截止时间', name: 'etime', width: 150, align: "left" },
                    //{ label: '默认url', name: 'url', width: 150, align: "left" },
                    {
                        label: '计划状态', name: 'status', width: 80, align: "left", formatter: function (cellvalue, rowdata, options) {
                            var typename = "";
                            switch (cellvalue) {
                                case "0":
                                    typename = "待提交";
                                    break;
                                case "1":
                                    typename = "待审核";
                                    break;
                                case "2":
                                    typename = "已拒绝";
                                    break;
                                case "3":
                                    typename = "运行中";
                                    break;
                                case "4":
                                    typename = "修改待审核";
                                    break;
                                case "7":
                                    typename = "已终止";
                                    break;
                                case "8":
                                    typename = "已挂起";
                                    break;
                            }
                            return typename;
                        }
                    },
                    {
                        label: '申请状态', name: 'ads_apply_status', width: 100, align: "left", formatter: function (cellvalue, rowdata, options) {
                            var typename = "";
                            switch (cellvalue) {
                                case "1":
                                    typename = "审核通过";
                                    break;
                                case "0":
                                    typename = "未审核通过";
                                    break;
                                case "-1":
                                    typename = "未申请";
                                    break;
                            }
                            return typename;
                        }
                    },
                    { label: '普通佣金说明', name: 'commission', width: 150, align: "left" },
                    { label: '创建时间', name: 'createtime', width: 140, align: "left" },
                    { label: '修改时间', name: 'updatetime', width: 140, align: "left" },
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
    //启用
    enable_fun = function (plan_id) {
        learun.layerConfirm('是否确认要【激活】计划！', function (res) {
            if (res) {
                learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/dm_dauomai_plan_manage/StartPlan', { keyValue: plan_id }, function () {
                    page.search();
                });
            }
        });
    }
    //禁用
    disable_fun = function (plan_id) {
        learun.layerConfirm('是否确认要【停用】计划！', function (res) {
            if (res) {
                learun.excuteOperate(top.$.rootUrl + '/DM_APPManage/dm_dauomai_plan_manage/StopPlan', { keyValue: plan_id }, function () {
                    page.search();
                });
            }
        });
    }
    page.init();
}
