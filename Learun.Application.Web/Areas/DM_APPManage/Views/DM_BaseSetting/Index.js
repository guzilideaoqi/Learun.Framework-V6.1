/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-14 16:36
 * 描  述：基础信息配置
 */
var keyValue = "";
var bootstrap = function ($, learun) {
    "use strict";
    var page = {
        init: function () {
            page.initleft();
            page.initData();
            page.bind();
        },
        bind: function () {
            // 保存
            $('#lr_save_btn').on('click', function () {
                if (!$('#lr_layout').lrValidform()) {
                    return false;
                }
                var postData = $('#lr_layout').lrGetFormData();
                $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_BaseSetting/SaveForm?keyValue=' + keyValue, postData, function (res) {
                    // 保存成功后才回调
                    //if (!!callBack) {
                    //    callBack();
                    //}
                });
            });

            $("#OpenTBAuthor").click(function () {
                learun.layerForm({
                    id: 'form',
                    title: '淘宝授权',
                    url: top.$.rootUrl + '/Hyg_RobotModule/Application_Setting/AuthorilizeForm',
                    width: 900,
                    height: 600,
                    btn: []
                });
            })
        },
        initData: function () {
            debugger;
            $.lrSetForm(top.$.rootUrl + '/DM_APPManage/DM_BaseSetting/GetFormData?keyValue=' + keyValue, function (data) {//
                console.log(JSON.stringify(data));
                if (data != null) {
                    $('#lr_layout').lrSetFormData(data.BaseSetting);
                    $('#lr_layout').lrSetFormData(data.BaseSetting_Tip);
                    keyValue = data.BaseSetting.appid;
                }
            });
        },
        initleft: function () {
            $('#lr_left_list li').on('click', function () {
                var $this = $(this);
                if (!$this.hasClass('active')) {
                    var $parent = $this.parent();
                    $parent.find('.active').removeClass('active');
                    $this.addClass('active');
                    var _type = $this.attr('data-value');
                    $('.lr-layout-wrap-item').removeClass('active');
                    $('#lr_layout_item' + _type).addClass('active');
                }
            });

            $("#openchecked").lrselect();//上架审核模式

            $("#taskchecked").lrselect();//任务审核模式

            $("#goodsource").lrselect();//商品搜索来源

            $("#showcommission").lrselect();//显示佣金类型

            $("#miquan_allowclickpraise").lrselect();//米圈普通用户点赞开关

            //商品类型
            $("#goodtype").lrselect({
                url: top.$.rootUrl + "/DM_Good/GetGoodTypeByCache",
                value: "cid",
                text: "cname"
            });

            //超级券商品类型
            $("#super_coupon_goodtype").lrselect({
                url: top.$.rootUrl + "/DM_Good/GetGoodTypeByCache",
                value: "cid",
                text: "cname"
            });
        },
    };
    page.init();
}
