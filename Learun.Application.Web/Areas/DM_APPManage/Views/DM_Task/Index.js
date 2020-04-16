/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-04-16 16:01
 * 描  述：任务中心
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
                    url: top.$.rootUrl + '/DM_APPManage/DM_Task/Form',
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
                        url: top.$.rootUrl + '/DM_APPManage/DM_Task/Form?keyValue=' + keyValue,
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
                            learun.deleteForm(top.$.rootUrl + '/DM_APPManage/DM_Task/DeleteForm', { keyValue: keyValue}, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/DM_APPManage/DM_Task/GetPageList',
                headData: [
                        { label: 'id', name: 'id', width: 200, align: "left" },
                        { label: '任务编号', name: 'task_no', width: 200, align: "left" },
                        { label: 'task_title', name: 'task_title', width: 200, align: "left" },
                        { label: '任务类型', name: 'task_type', width: 200, align: "left" },
                        { label: '任务状态 0未进行  1进行中  2已完成  3已取消', name: 'task_status', width: 200, align: "left" },
                        { label: '任务描述', name: 'task_description', width: 200, align: "left" },
                        { label: '任务操作', name: 'task_operate', width: 200, align: "left" },
                        { label: '0pc  1移动', name: 'plaform', width: 200, align: "left" },
                        { label: '排序值', name: 'sort', width: 200, align: "left" },
                        { label: '创建时间', name: 'createtime', width: 200, align: "left" },
                        { label: '总佣金(发布任务所需要的总金额)', name: 'totalcommission', width: 200, align: "left" },
                        { label: '服务费(每一单的服务费)', name: 'servicefee', width: 200, align: "left" },
                        { label: '初级佣金(每一单的佣金)', name: 'juniorcommission', width: 200, align: "left" },
                        { label: '高级佣金(每一单的佣金)', name: 'seniorcommission', width: 200, align: "left" },
                        { label: '需求人数', name: 'needcount', width: 200, align: "left" },
                        { label: '完成人数', name: 'finishcount', width: 200, align: "left" },
                        { label: '用户ID', name: 'user_id', width: 200, align: "left" },
                        { label: '平台ID', name: 'appid', width: 200, align: "left" },
                ],
                mainId:'id',
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
