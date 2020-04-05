/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2019-11-26 20:21
 * 描  述：京东订单主表
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
                    url: top.$.rootUrl + '/Hyg_RobotModule/order_jd/Form',
                    width: 700,
                    height: 400,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('orderId');
                selectedRow = $('#girdtable').jfGridGet('rowdata');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/Hyg_RobotModule/order_jd/Form?keyValue=' + keyValue,
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
                var keyValue = $('#girdtable').jfGridValue('orderId');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/Hyg_RobotModule/order_jd/DeleteForm', { keyValue: keyValue}, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/Hyg_RobotModule/order_jd/GetPageList',
                headData: [
                        { label: '订单完成时间(时间戳，毫秒)', name: 'finishTime', width: 200, align: "left" },
                        { label: '下单设备(1:PC,2:无线)', name: 'orderEmt', width: 200, align: "left" },
                        { label: '订单ID', name: 'orderId', width: 200, align: "left" },
                        { label: '下单时间(时间戳，毫秒)', name: 'orderTime', width: 200, align: "left" },
                        { label: '父单的订单ID，仅当发生订单拆分时返回， 0：未拆分，有值则表示此订单为子订单', name: 'parentId', width: 200, align: "left" },
                        { label: '订单维度预估结算时间,不建议使用，可以用订单行sku维度paymonth字段参考，（格式：yyyyMMdd），0：未结算，订单'预估结算时间'仅供参考。账号未通过资质审核或订单发生售后，会影响订单实际结算时间。', name: 'payMonth', width: 200, align: "left" },
                        { label: '下单用户是否为PLUS会员 0：否，1：是', name: 'plus', width: 200, align: "left" },
                        { label: '订单维度商家ID，不建议使用，可以用订单行sku维度popId参考', name: 'popId', width: 200, align: "left" },
                        { label: '推客的联盟ID', name: 'unionId', width: 200, align: "left" },
                        { label: '订单维度的推客生成推广链接时传入的扩展字段，不建议使用，可以用订单行sku维度ext1参考,（需要联系运营开放白名单才能拿到数据）', name: 'ext1', width: 200, align: "left" },
                        { label: '订单维度的有效码，不建议使用，可以用订单行sku维度validCode参考,（-1：未知,2.无效-拆单,3.无效-取消,4.无效-京东帮帮主订单,5.无效-账号异常,6.无效-赠品类目不返佣,7.无效-校园订单,8.无效-企业订单,9.无效-团购订单,10.无效-开增值税专用发票订单,11.无效-乡村推广员下单,12.无效-自己推广自己下单,13.无效-违规订单,14.无效-来源与备案网址不符,15.待付款,16.已付款,17.已完成,18.已结算（5.9号不再支持结算状态回写展示）,19.无效-佣金比例为0）注：自2018/7/13起，自己推广自己下单已经允许返佣，故12无效码仅针对历史数据有效', name: 'validCode', width: 200, align: "left" },
                ],
                mainId:'orderId',
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
