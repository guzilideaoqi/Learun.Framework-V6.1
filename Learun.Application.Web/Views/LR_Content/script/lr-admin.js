/*
 * 版 本 Learun-ADMS V6.1.6.0 力软 敏捷 开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：力 软- 前端开发组
 * 日 期：2017.03.16
 * 描 述：admin顶层页面操作方法
 */

var loaddfimg;
(function ($, learun) {
    "use strict";

    var page = {
        init: function () {
            /*判断当前浏览器是否是IE浏览器*/
            if ($('body').hasClass('IE') || $('body').hasClass('InternetExplorer')) {
                $('#lr_loadbg').append('<img data-img="imgdw" src="' + top.$.rootUrl + '/Content/images/ie-loader.gif" style="position: absolute;top: 0;left: 0;right: 0;bottom: 0;margin: auto;vertical-align: middle;">');
                Pace.stop();
                $.imServer.init();
            }
            else {
                Pace.on('done', function () {
                    $('#lr_loadbg').fadeOut();
                    Pace.options.target = '#learunpacenone';
                    $.imServer.init();
                });
            }

            // 通知栏插件初始化设置
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": true,
                "progressBar": false,
                "positionClass": "toast-top-center",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "3000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
            // 打开首页模板
            learun.frameTab.open({ F_ModuleId: '0', F_Icon: 'fa fa-desktop', F_FullName: '控制台', F_UrlAddress: '/Home/AdminDesktopTemp' }, true);
            learun.clientdata.init(function () {
                page.userInit();
                // 初始页面特例
                bootstrap($, learun);
                //$.imServer.init();
                if ($('body').hasClass('IE') || $('body').hasClass('InternetExplorer')) {
                    $('#lr_loadbg').fadeOut();
                }
            });

            // 加载数据进度
            page.loadbarInit();
            // 全屏按钮
            page.fullScreenInit();
            // 主题选择初始化
            page.uitheme();

            //加载数据统计
            page.datastatistic();
        },

        // 登录头像和个人设置
        userInit: function () {
            var loginInfo = learun.clientdata.get(['userinfo']);
            var headimg;
            if (loginInfo.gender != 0) {
                headimg = top.$.rootUrl + '/Content/images/head/on-boy.jpg';
            }
            else {
                headimg = top.$.rootUrl + '/Content/images/head/on-girl.jpg';
            }
            loaddfimg = function () {
                document.getElementById('userhead').src = headimg;
            }
            var _html = '<div class="lr-frame-personCenter"><a href="javascript:void(0);" class="dropdown-toggle" data-toggle="dropdown">';
            _html += '<img id="userhead" src="' + top.$.rootUrl + loginInfo.headIcon + '" alt="用户头像" onerror="loaddfimg()" >';
            _html += '<span>' + loginInfo.realName + '</span>';
            _html += '</a>';
            _html += '<ul class="dropdown-menu pull-right">';
            _html += '<li><a href="javascript:void(0);" id="lr_userinfo_btn"><i class="fa fa-user"></i>个人信息</a></li>';
            _html += '<li><a href="javascript:void(0);" id="lr_schedule_btn"><i class="fa fa-calendar"></i>我的日程</a></li>';
            if (loginInfo.isSystem) {
                _html += '<li><a href="javascript:void(0);" id="lr_clearredis_btn"><i class="fa fa-refresh"></i>清空缓存</a></li>';
            }
            _html += '<li><a href="javascript:void(0);" id="lr_loginout_btn"><i class="fa fa-power-off"></i>安全退出</a></li>';
            _html += '</ul></div>';
            $('body').append(_html);

            $('#lr_loginout_btn').on('click', page.loginout);
            $('#lr_userinfo_btn').on('click', page.openUserCenter);
            $('#lr_clearredis_btn').on('click', page.clearredis);
        },
        loginout: function () { // 安全退出
            learun.layerConfirm("注：您确定要安全退出本次登录吗？", function (r) {
                if (r) {
                    learun.loading(true, '退出系统中...');
                    learun.httpAsyncPost($.rootUrl + '/Login/OutLogin', {}, function (data) {
                        window.location.href = $.rootUrl + "/Login/Index";
                    });
                }
            });
        },
        clearredis: function () {
            learun.layerConfirm("注：您确定要清空全部后台缓存数据吗？", function (r) {
                if (r) {
                    learun.loading(true, '清理缓存数据中...');
                    learun.httpAsyncPost($.rootUrl + '/Home/ClearRedis', {}, function (data) {
                        window.location.href = $.rootUrl + "/Login/Index";
                    });
                }
            });
        },
        openUserCenter: function () {
            // 打开个人中心
            learun.frameTab.open({ F_ModuleId: '1', F_Icon: 'fa fa-user', F_FullName: '个人中心', F_UrlAddress: '/UserCenter/Index' });
        },

        // 全屏按钮
        fullScreenInit: function () {
            var _html = '<div class="lr_frame_fullscreen"><a href="javascript:void(0);" id="lr_fullscreen_btn" title="全屏"><i class="fa fa-arrows-alt"></i></a></div>';
            $('body').append(_html);
            $('#lr_fullscreen_btn').on('click', function () {
                if (!$(this).attr('fullscreen')) {
                    $(this).attr('fullscreen', 'true');
                    page.requestFullScreen();
                } else {
                    $(this).removeAttr('fullscreen');
                    page.exitFullscreen();
                }
            });
        },
        requestFullScreen: function () {
            var de = document.documentElement;
            if (de.requestFullscreen) {
                de.requestFullscreen();
            } else if (de.mozRequestFullScreen) {
                de.mozRequestFullScreen();
            } else if (de.webkitRequestFullScreen) {
                de.webkitRequestFullScreen();
            }
        },
        exitFullscreen: function () {
            var de = document;
            if (de.exitFullscreen) {
                de.exitFullscreen();
            } else if (de.mozCancelFullScreen) {
                de.mozCancelFullScreen();
            } else if (de.webkitCancelFullScreen) {
                de.webkitCancelFullScreen();
            }
        },

        // 加载数据进度
        loadbarInit: function () {
            var _html = '<div class="lr-loading-bar" id="lr_loading_bar" >';
            _html += '<div class="lr-loading-bar-bg"></div>';
            _html += '<div class="lr-loading-bar-message" id="lr_loading_bar_message"></div>';
            _html += '</div>';
            $('body').append(_html);
        },

        // 皮肤主题设置
        uitheme: function () {
            var uitheme = top.$.cookie('Learn_ADMS_V6.1_UItheme') || '1';
            var $setting = $('<div class="lr-theme-setting"></div>');
            var $btn = $('<button class="btn btn-default"><i class="fa fa-spin fa-gear"></i></button>');
            var _html = '<div class="panel-heading">界面风格</div>';
            _html += '<div class="panel-body">';
            _html += '<div><label><input type="radio" name="ui_theme" value="1" ' + (uitheme == '1' ? 'checked' : '') + '>经典版</label></div>';
            _html += '<div><label><input type="radio" name="ui_theme" value="2" ' + (uitheme == '2' ? 'checked' : '') + '>风尚版</label></div>';
            _html += '<div><label><input type="radio" name="ui_theme" value="3" ' + (uitheme == '3' ? 'checked' : '') + '>炫动版</label></div>';
            _html += '<div><label><input type="radio" name="ui_theme" value="4" ' + (uitheme == '4' ? 'checked' : '') + '>飞扬版</label></div>';
            _html += '</div>';
            $setting.append($btn);
            $setting.append(_html);
            $('body').append($setting);

            $btn.on('click', function () {
                var $parent = $(this).parent();
                if ($parent.hasClass('opened')) {
                    $parent.removeClass('opened');
                }
                else {
                    $parent.addClass('opened');
                }
            });
            $setting.find('input').click(function () {
                var value = $(this).val();
                top.$.cookie('Learn_ADMS_V6.1_UItheme', value, { path: "/" });
                window.location.href = $.rootUrl + '/Home/Index';
            });

        },
        datastatistic: function () {
            var uitheme = top.$.cookie('Learn_ADMS_V6.1_UItheme') || '1';
            var $setting = $('<div class="lr-theme-setting opened" style="top:55%;"></div>');
            var $btn = $('<button class="btn btn-default"><i class="fa fa-spin fa-snowflake-o"></i></button>');
            var _html = '<div class="panel-heading">代办事项<span style="font-size:8px;"></span></div>';
            _html += '<div class="panel-body">';
            _html += '<div style="padding-left:0px;" char="nocheck_certifica" title="点击去审核">待实名认证(<span id="nocheck_certifica" style="color:red;">0</span>)</div>';
            _html += '<div style="padding-left:0px;" char="nocheck_cashrecord" title="点击去审核">等待提现(<span id="nocheck_cashrecord" style="color:red;">0</span>)</div>';
            _html += '<div style="padding-left:0px;" char="nocheck_apply_partners" title="点击去审核">合伙人申请(<span id="nocheck_apply_partners" style="color:red;">0</span>)</div>';
            _html += '<div style="padding-left:0px;" char="nocheck_friend_circle" title="点击去审核">米圈待审核(<span id="nocheck_friend_circle" style="color:red;">0</span>)</div>';
            _html += '<div style="padding-left:0px;" char="nocheck_task" title="点击去审核">任务待审核(<span id="nocheck_task" style="color:red;">0</span>)</div>';
            _html += '</div>';
            $setting.append($btn);
            $setting.append(_html);
            $('body').append($setting);

            $btn.on('click', function () {
                var $parent = $(this).parent();
                if ($parent.hasClass('opened')) {
                    $parent.removeClass('opened');
                }
                else {
                    $parent.addClass('opened');
                }
            });
            $(".panel-body>div").click(function () {
                var value = $(this).attr("char");
                var moduleid = "", f_icon = "", f_fullname = "", f_urladdress = "";
                switch (value) {
                    case "nocheck_certifica":
                        moduleid = "c1a33c94-f460-4782-92be-5014fbbf8663";
                        f_icon = "fa fa-address-book";
                        f_fullname = "身份证实名";
                        f_urladdress = "/DM_APPManage/DM_CertificaRecord/Index";
                        break;
                    case "nocheck_cashrecord":
                        moduleid = "df3d15b7-f41e-4586-b130-4042218d3095";
                        f_icon = "fa fa-building";
                        f_fullname = "提现申请记录";
                        f_urladdress = "/DM_APPManage/DM_Apply_CashRecord/Index";
                        break;
                    case "nocheck_apply_partners":
                        moduleid = "75db761e-3f2f-4efc-9818-c5cb80819004";
                        f_icon = "fa fa-user-circle-o";
                        f_fullname = "合伙人申请";
                        f_urladdress = "/DM_APPManage/DM_APP_Partners_Record/Index";
                        break;
                    case "nocheck_friend_circle":
                        moduleid = "b49d8d30-ef5b-40b5-8502-6061969230c8";
                        f_icon = "fa fa-comments";
                        f_fullname = "文案管理";
                        f_urladdress = "/DM_APPManage/dm_friend_circle/Index";
                        break;
                    case "nocheck_task":
                        moduleid = "2a1c42af-19cc-4863-b7c5-76037adf0391";
                        f_icon = "fa fa-meetup";
                        f_fullname = "任务中心";
                        f_urladdress = "/DM_APPManage/DM_Task/Index";
                        break;
                }
                learun.frameTab.open({ F_ModuleId: moduleid, F_Icon: f_icon, F_FullName: f_fullname, F_UrlAddress: f_urladdress });
            });
            page.loadstatistic();
            var step_time = 60000 * 5;
            setInterval(function () {
                page.loadstatistic();
            }, step_time)
        },
        loadstatistic: function () {
            learun.httpAsyncPost($.rootUrl + '/DM_User/NoCheckDataStatistic', {}, function (data) {
                var list = data.data;
                if (list.length > 0) {
                    var statisticItem = list[0];
                    $("#nocheck_cashrecord").text(statisticItem.nocheck_cashrecord);
                    $("#nocheck_certifica").text(statisticItem.nocheck_certifica);
                    $("#nocheck_apply_partners").text(statisticItem.nocheck_apply_partners);
                    $("#nocheck_friend_circle").text(statisticItem.nocheck_friend_circle);
                    $("#nocheck_task").text(statisticItem.nocheck_task);
                }
                console.log(data);
            });
        }
    };

    $(function () {
        page.init();
    });
})(window.jQuery, top.learun);