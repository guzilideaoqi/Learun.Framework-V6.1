/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-02-05 15:33
 * 描  述：活动管理
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
                    url: top.$.rootUrl + '/DM_APPManage/dm_activity_manage/Form',
                    width: 800,
                    height: 600,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('f_id');
                selectedRow = $('#girdtable').jfGridGet('rowdata');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/DM_APPManage/dm_activity_manage/Form?keyValue=' + keyValue,
                        width: 800,
                        height: 600,
                        callBack: function (id) {
                            return top[id].acceptClick(refreshGirdData);
                        }
                    });
                }
            });
            // 删除
            $('#lr_delete').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('f_id');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/dm_activity_manage/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/dm_activity_manage/GetPageList',
                headData: [
                    //{ label: '主键ID', name: 'f_id', width: 200, align: "left" },
                    //{ label: '红包背景图片', name: 'APP_RedPaper_Image', width: 200, align: "left" },
                    { label: '红包上的文本信息', name: 'APP_RedPaper_Text', width: 200, align: "left" },
                    //{ label: '摇晃红包图片地址', name: 'APP_Rock_RedPaper_Image', width: 200, align: "left" },
                    //{ label: '开红包跳转地址', name: 'APP_To_ActivityUrl', width: 200, align: "left" },
                    { label: '前端刘海栏显示文本', name: 'ActivityTitle', width: 200, align: "left" },
                    { label: '活动类型', name: 'ActivityType', width: 100, align: "left" },
                    { label: '活动开始时间', name: 'ActivityStartTime', width: 150, align: "left" },
                    { label: '活动结束时间', name: 'ActivityEndTime', width: 150, align: "left" },
                    { label: '活动编号', name: 'ActivityCode', width: 100, align: "left" },
                    {
                        label: '活动状态', name: 'ActivityStatus', width: 200, align: "left", formatter: function (cellvalue, rowdata, options) {
                            if (cellvalue == 0)
                                return "已暂停";
                            else
                                return "进行中";
                        }
                    },
                    //{ label: '活动描述', name: 'ActivityRemark', width: 200, align: "left" },
                    { label: '初始红包最小金额', name: 'InitRedPaper_MinPrice', width: 120, align: "left" },
                    { label: '初始红包最大金额', name: 'InitRedPaper_MaxPrice', width: 120, align: "left" },
                    { label: '奖励金额', name: 'RewardPrice', width: 100, align: "left" },
                    { label: '创建时间', name: 'CreateTime', width: 150, align: "left" },
                    { label: '修改时间', name: 'UpdateTime', width: 150, align: "left" },
                ],
                mainId: 'f_id',
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
