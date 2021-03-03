/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-03-03 10:50
 * 描  述：多麦计划
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
                    url: top.$.rootUrl + '/DM_APPManage/dm_dauomai_plan_manage/Form',
                    width: 700,
                    height: 400,
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
                var keyValue = $('#girdtable').jfGridValue('f_id');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/dm_dauomai_plan_manage/DeleteForm', { keyValue: keyValue}, function () {
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
                        { label: 'f_id', name: 'f_id', width: 200, align: "left" },
                        { label: '推广计划ID', name: 'ads_id', width: 200, align: "left" },
                        { label: '推广计划名称', name: 'ads_name', width: 200, align: "left" },
                        { label: '所属商场名称', name: 'store_name', width: 200, align: "left" },
                        { label: '活动类型：0 web 1 wap 3 ROI 4 小程序', name: 'channel', width: 200, align: "left" },
                        { label: 'RD有效期', name: 'rddays', width: 200, align: "left" },
                        { label: '普通佣金说明，具体佣金说明及政策请访问', name: 'commission', width: 200, align: "left" },
                        { label: '当category=1 海外商家时，此字段表示计划允许的跟单地区', name: 'category_area', width: 200, align: "left" },
                        { label: '商家类型：0 国内商家 1 海外商家 2 跨境电商', name: 'category', width: 200, align: "left" },
                        { label: '审核方式 1人工审核 2自动通过', name: 'apply_mode', width: 200, align: "left" },
                        { label: '计划开始时间，格式:yyyy-MM-dd HH:mm:ss', name: 'stime', width: 200, align: "left" },
                        { label: '计划截止时间，格式:yyyy-MM-dd HH:mm:ss', name: 'etime', width: 200, align: "left" },
                        { label: '默认url', name: 'url', width: 200, align: "left" },
                        { label: '计划logo', name: 'ads_logo', width: 200, align: "left" },
                        { label: '计划状态：0 待提交 1 待审核 2 已拒绝 3 运行中 4 修改待审核 7 已终止 8已挂起', name: 'status', width: 200, align: "left" },
                        { label: '申请状态：1审核通过，0未审核通过，-1未申请', name: 'ads_apply_status', width: 200, align: "left" },
                        { label: '创建时间', name: 'createtime', width: 200, align: "left" },
                        { label: '修改时间', name: 'updatetime', width: 200, align: "left" },
                ],
                mainId:'f_id',
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
