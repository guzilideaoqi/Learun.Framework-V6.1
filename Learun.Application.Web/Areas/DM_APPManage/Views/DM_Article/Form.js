/*
 * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：力软-前端开发组
 * 日 期：2017.04.05
 * 描 述：分类管理	
 */

var keyValue = request('keyValue');
var issoftarticle = request('issoftarticle');
var acceptClick;
var loaddfimg;
var headimg;
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var ue;
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            ue = UE.getEditor('editor');

            function uploadImg() {
                var f = document.getElementById('uploadFile').files[0]
                var src = window.URL.createObjectURL(f);
                document.getElementById('uploadPreview').src = src;
            };

            $('#uploadFile').on('change', uploadImg);

            // 上级
            $('#parentid').lrselect({
                url: top.$.rootUrl + '/DM_APPManage/DM_Article/GetClassifyTree',
                type: 'tree',
                allowSearch: true,
                maxHeight: 225
            });

        },
        initData: function () {
            console.log(selectedRow);
            if (!!selectedRow) {
                keyValue = selectedRow.id || '';
                $('#form').lrSetFormData(selectedRow);
                setTimeout(function () {
                    ue.setContent(selectedRow.content);

                    headimg = selectedRow.a_image;

                    $('.file').prepend('<img src="' + top.$.rootUrl + headimg + '" id="uploadPreview" onerror="loaddfimg()" >');
                    if (true) {
                        headimg = top.$.rootUrl + '/Content/images/head/on-boy.jpg';
                    }
                    else {
                        headimg = top.$.rootUrl + '/Content/images/head/on-girl.jpg';
                    }

                    loaddfimg = function () {
                        document.getElementById('uploadPreview').src = headimg;
                    }
                }, 300);
            } else {
                $('.file').prepend('<img src="' + top.$.rootUrl + '/Content/images/head/on-boy.jpg" id="uploadPreview" onerror="loaddfimg()" >');


                loaddfimg = function () {
                    document.getElementById('uploadPreview').src = headimg;
                }
            }
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData(keyValue);
        if (postData["parentid"] == '' || postData["parentid"] == '&nbsp;') {
            postData["parentid"] = '0';
        }
        else if (postData["parentid"] == keyValue) {
            learun.alert.error('上级不能是自己本身');
            return false;
        }

        if (!issoftarticle)
            postData["issoftarticle"] = issoftarticle;

        var f = document.getElementById('uploadFile').files[0];

        debugger;
        if (!!f) {
            postData["content"] = encodeURIComponent(ue.getContent(null, null, true));

            learun.loading(true, '正在保存...');
            $.ajaxFileUpload({
                url: "/DM_APPManage/DM_Article/UploadFile?keyValue=" + keyValue,
                secureuri: false,
                fileElementId: 'uploadFile',
                dataType: 'json',
                data: postData,
                success: function (data) {
                    learun.loading(false);
                    if (data.code == 200) {
                        learun.alert.success('保存成功');
                        learun.layerClose('form');
                        if (!!callBack) {
                            callBack();
                        }
                    }
                }
            });
        } else {
            postData["content"] = ue.getContent(null, null, true);

            $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_Article/SaveForm?keyValue=' + keyValue, postData, function (res) {
                // 保存成功后才回调
                if (!!callBack) {
                    callBack();
                }
            });
        }
    };

    page.init();
}