/*
 * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.03.16
 * 描 述：弹层（基于layer.js-3.0.3）	
 */
(function ($, learun) {
    "use strict";
    $.extend(learun, {
        // 询问框
        layerConfirm: function (msg, callback) {
            top.layer.confirm(msg, {
                btn: ['确认', '取消'],
                title: "哆来米提示",
                icon: 0,
                skin: 'lr-layer',
            }, function (index) {
                callback(true, index);
            }, function (index) {
                callback(false, index);
                top.layer.close(index); //再执行关闭  
            });
        },
        // 自定义表单弹层
        layerForm: function (op) {
            var dfop = {
                id: null,
                title: '系统窗口',
                width: 550,
                height: 400,
                url: 'error',
                btn: ['确认', '关闭'],
                callBack: false,
                maxmin: false,
                end: false,
            };
            $.extend(dfop, op || {});

            /*适应窗口大小*/
            dfop.width = dfop.width > $(window).width() ? $(window).width() - 10 : dfop.width;
            dfop.height = dfop.height > $(window).height() ? $(window).height() - 10 : dfop.height;

            var r = top.layer.open({
                id: dfop.id,
                maxmin: dfop.maxmin,
                type: 2,//0（信息框，默认）1（页面层）2（iframe层）3（加载层）4（tips层）
                title: dfop.title,
                area: [dfop.width + 'px', dfop.height + 'px'],
                btn: dfop.btn,
                content: op.url,
                skin: dfop.btn == null ? 'lr-layer-nobtn' : 'lr-layer',
                success: function (layero, index) {
                    top['layer_' + dfop.id] = learun.iframe($(layero).find('iframe').attr('id'), top.frames);
                    layero[0].learun_layerid = 'layer_' + dfop.id;
                    //如果底部有按钮添加-确认并关闭窗口勾选按钮
                    if (!!dfop.btn && layero.find('.lr-layer-btn-cb').length == 0) {
                        layero.find('.layui-layer-btn').append('<div class="checkbox lr-layer-btn-cb" myIframeId="layer_' + dfop.id + '" ><label><input checked="checked" type="checkbox" >确认并关闭窗口</label></div>');
                    }

                },
                yes: function (index) {
                    var flag = true;
                    if (!!dfop.callBack) {
                        flag = dfop.callBack('layer_' + dfop.id);
                    }
                    if (!!flag) {
                        learun.layerClose('', index);
                    }
                },
                end: function () {
                    top['layer_' + dfop.id] = null;
                    if (!!dfop.end) {
                        dfop.end();
                    }
                }
            });
        },
        // 关闭弹层
        layerClose: function (name, index) {
            var _index;
            if (!!index) {
                _index = index;
            }
            else {
                _index = top.layer.getFrameIndex(name);
            }
            var layero = top.$("#layui-layer" + _index);
            var $IsClose = layero.find('.layui-layer-btn').find(".lr-layer-btn-cb input");
            var IsClose = $IsClose.is(":checked");
            if ($IsClose.length == 0) {
                IsClose = true;
            }
            if (IsClose) {
                top.layer.close(_index); //再执行关闭  
            } else {
                top[layero[0].learun_layerid].location.reload();
            }

        },
        layerMutipleBtnConfirm: function (op) {
            top.layer.open({
                content: op.content,
                btn: op.btn || ['确认', '取消'],
                title: "哆来米提示",
                icon: 0,
                skin: 'lr-layer',
                yes: function (index, layero) {
                    //按钮【按钮一】的回调
                    op.yes(index, layero);
                }
                , btn2: function (index, layero) {
                    //按钮【按钮二】的回调
                    op.btn2(index, layero);
                    //return false; //开启该代码可禁止点击该按钮关闭
                }
                , btn3: function (index, layero) {
                    //按钮【按钮三】的回调
                    op.btn3(index, layero);
                    //return false;// 开启该代码可禁止点击该按钮关闭
                }
                , cancel: function () {
                    //右上角关闭回调

                    //return false; //开启该代码可禁止点击该按钮关闭
                }
            });
        },
        layerMutipleBtnForm: function (op) {
            /*适应窗口大小*/
            var width = op.width > $(window).width() ? $(window).width() - 10 : op.width;
            var height = op.height > $(window).height() ? $(window).height() - 10 : op.height;
            var dfop = {
                id: null,
                type: 2,
                title: op.title || '哆来米提示',
                area: [width + 'px', height + 'px'],
                content: op.url,
                btn: op.btn || ['确认', '关闭'],
                callBack: false,
                maxmin: false,
                end: false,
                yes: function (index, layero) {
                    //按钮【按钮一】的回调
                    op.yes(index, layero);
                }
                , btn2: function (index, layero) {
                    //按钮【按钮二】的回调
                    op.btn2(index, layero);
                    //return false; //开启该代码可禁止点击该按钮关闭
                }
                , btn3: function (index, layero) {
                    //按钮【按钮三】的回调
                    op.btn3(index, layero);
                    //return false;// 开启该代码可禁止点击该按钮关闭
                }
                , cancel: function () {
                    //右上角关闭回调

                    //return false; //开启该代码可禁止点击该按钮关闭
                }
            };

            var r = top.layer.open(dfop);
        }
    });


})(window.jQuery, top.learun);