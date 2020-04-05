/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-03-14 16:42
 * 描  述：积分兑换商品
 */
var acceptClick;
var keyValue = request('keyValue');
var loaddfimg;
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            page.initData();
        },
        bind: function () {
            function uploadImg() {
                var f = document.getElementById('uploadFile').files[0]
                var src = window.URL.createObjectURL(f);
                document.getElementById('uploadPreview').src = src;
            };

            $('#uploadFile').on('change', uploadImg);
        },
        initData: function () {
            var headimg;
            if (!!selectedRow) {
                $('#form').lrSetFormData(selectedRow);
                headimg = selectedRow.taskimage;
            }
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
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData();
        var f = document.getElementById('uploadFile').files[0];
        if (!!f) {
            learun.loading(true, '正在保存...');
            $.ajaxFileUpload({
                url: "/DM_APPManage/DM_ReadTask/UploadFile?keyValue=" + keyValue,
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
            $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/DM_ReadTask/SaveForm?keyValue=' + keyValue, postData, function (res) {
                // 保存成功后才回调
                if (!!callBack) {
                    callBack();
                }
            });
        }
    };
    page.init();
}

