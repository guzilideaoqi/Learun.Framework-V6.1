/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2019-11-22 21:57
 * 描  述：淘宝订单列表
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
                    url: top.$.rootUrl + '/Hyg_RobotModule/order_tb/Form',
                    width: 700,
                    height: 400,
                    callBack: function (id) {
                        return top[id].acceptClick(refreshGirdData);
                    }
                });
            });
            // 编辑
            $('#lr_edit').on('click', function () {
                var keyValue = $('#girdtable').jfGridValue('');
                selectedRow = $('#girdtable').jfGridGet('rowdata');
                if (learun.checkrow(keyValue)) {
                    learun.layerForm({
                        id: 'form',
                        title: '编辑',
                        url: top.$.rootUrl + '/Hyg_RobotModule/order_tb/Form?keyValue=' + keyValue,
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
                var keyValue = $('#girdtable').jfGridValue('');
                if (learun.checkrow(keyValue)) {
                    learun.layerConfirm('是否确认删除该项！', function (res) {
                        if (res) {
                            learun.deleteForm(top.$.rootUrl + '/Hyg_RobotModule/order_tb/DeleteForm', { keyValue: keyValue }, function () {
                            });
                        }
                    });
                }
            });
        },
        initGird: function () {
            $('#girdtable').lrAuthorizeJfGrid({
                url: top.$.rootUrl + '/Hyg_RobotModule/order_tb/GetPageList',
                headData: [
                    { label: '创建时间', name: 'tk_paid_time', width: 150, align: "left" },
                    //{ label: '付款金额', name: 'pay_price', width: 80, align: "left" },
                    { label: '订单编号', name: 'trade_id', width: 150, align: "left" },
                    { label: '推广位', name: 'adzone_id', width: 100, align: "left" },
                    { label: '付款预估', name: 'pub_share_pre_fee', width: 80, align: "left" },
                    { label: '结算预估', name: 'pub_share_fee', width: 70, align: "left" },
                    { label: '实付金额', name: 'alipay_total_price', width: 80, align: "left" },
                    {
                        label: '订单状态', name: 'tk_status', width: 80, align: "left", formatter: function (cellValue, rowObject) {
                            var statusText = "";
                            switch (cellValue) {
                                case 3:
                                    statusText = "订单结算";
                                    break; case 12:
                                    statusText = "订单付款";
                                    break; case 13:
                                    statusText = "订单失效";
                                    break; case 14:
                                    statusText = "订单成功";
                                    break;
                            }
                            return statusText;
                        }
                    },
                    { label: '商品id', name: 'item_id', width: 80, align: "left" },
                    { label: '商品标题', name: 'item_title', width: 200, align: "left" },
                    { label: '商品数量', name: 'item_num', width: 80, align: "left" },
                    { label: '佣金金额',  name: 'total_commission_fee', width: 200, align: "left" },
                    { label: '渠道关系id', name: 'relation_id', width: 80, align: "left" },
                    { label: '付款时间', name: 'tb_paid_time', width: 150, align: "left" },
                    { label: '结算时间', name: 'tk_earning_time', width: 150, align: "left" },

                    /*{ label: '二方：佣金收益的第一归属者； 三方：从其他淘宝客佣金中进行分成的推广者', hidden: true, name: 'tk_order_role', width: 200, align: "left" },
                    { label: '从结算佣金中分得的收益比率', hidden: true, name: 'pub_share_rate', width: 200, align: "left" },
                    { label: '维权标签，0 含义为非维权 1 含义为维权订单', hidden: true, name: 'refund_tag', width: 200, align: "left" },
                    { label: '平台给与的补贴比率，如天猫、淘宝、聚划算等', hidden: true, name: 'subsidy_rate', width: 200, align: "left" },
                    { label: '提成=收入比率*分成比率。指实际获得收益的比率', hidden: true, name: 'tk_total_rate', width: 200, align: "left" },
                    { label: '商品所属的根类目，即一级类目的名称', hidden: true, name: 'item_category_name', width: 200, align: "left" },
                    { label: '掌柜旺旺', name: 'seller_nick', hidden: true, width: 200, align: "left" },
                    { label: '推广者的会员id', name: 'pub_id', hidden: true, width: 200, align: "left" },
                    { label: '推广者赚取佣金后支付给阿里妈妈的技术服务费用的比率', hidden: true, name: 'alimama_rate', width: 200, align: "left" },
                    { label: '平台出资方，如天猫、淘宝、或聚划算等', hidden: true, name: 'subsidy_type', width: 200, align: "left" },
                    { label: '商品图片', name: 'item_img', hidden: true, width: 200, align: "left" },
                    { label: '媒体名称', hidden: true, name: 'site_name', width: 200, align: "left" },
                    { label: '补贴金额=结算金额*补贴比率', hidden: true, name: 'subsidy_fee', width: 200, align: "left" },
                    { label: '技术服务费', hidden: true, name: 'alimama_share_fee', width: 200, align: "left" },
                    { label: '买家在淘宝后台显示的订单编号', hidden: true, name: 'trade_parent_id', width: 200, align: "left" },
                    { label: '订单所属平台类型，包括天猫、淘宝、聚划算等', hidden: true, name: 'order_type', width: 200, align: "left" },
                    { label: '订单创建的时间，该时间同步淘宝，可能会略晚于买家在淘宝的订单创建时间', hidden: true, name: 'tk_create_time', width: 200, align: "left" },
                    { label: '产品类型', name: 'flow_source', hidden: true, width: 200, align: "left" },
                    { label: '成交平台', name: 'terminal_type', hidden: true, width: 200, align: "left" },
                    { label: '通过推广链接达到商品、店铺详情页的点击时间', hidden: true, name: 'click_time', width: 200, align: "left" },
                    { label: '商品单价', hidden: true, name: 'item_price', width: 200, align: "left" },
                    { label: '推广位管理下的自定义推广位名称', hidden: true, name: 'adzone_name', width: 200, align: "left" },
                    { label: '佣金比率', hidden: true, name: 'total_commission_rate', width: 200, align: "left" },
                    { label: '商品链接', hidden: true, name: 'item_link', width: 200, align: "left" },
                    { label: '媒体ID', hidden: true, name: 'site_id', width: 200, align: "left" },
                    { label: '店铺名称', hidden: true, name: 'seller_shop_title', width: 200, align: "left" },
                    { label: '订单结算的佣金比率+平台的补贴比率', hidden: true, name: 'income_rate', width: 200, align: "left" },
                    
                    { label: '预估专项服务费', hidden: true, name: 'tk_commission_pre_fee_for_media_platform', width: 200, align: "left" },
                    { label: '结算专项服务费', hidden: true, name: 'tk_commission_fee_for_media_platform', width: 200, align: "left" },
                    { label: '专项服务费率', hidden: true, name: 'tk_commission_rate_for_media_platform', width: 200, align: "left" },
                    { label: '会员运营id', hidden: true, name: 'special_id', width: 200, align: "left" },
                    { label: '预售时期，用户对预售商品支付定金的付款时间，可能略晚于在淘宝付定金时间', hidden: true, name: 'tk_deposit_time', width: 200, align: "left" },
                    { label: '预售时期，用户对预售商品支付定金的付款时间', hidden: true, name: 'tb_deposit_time', width: 200, align: "left" },
                    { label: '预售时期，用户对预售商品支付的定金金额', hidden: true, name: 'deposit_price', width: 200, align: "left" },
                    { label: '开发者调用api的appkey', hidden: true, name: 'app_key', width: 200, align: "left" },*/
                ],
mainId: '',
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
