/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2020-10-28 10:06
 * 描  述：官推文案
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

            $("li.weui-uploader__file").addClass("weui-uploader__file_status");
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
            }

            uploaderFilesLoad("uploaderFiles", "uploaderInput", 9, function () {
                //console.log("回调函数");
                //console.log(getImgFilesData("uploaderFiles"));//多张图片中以逗号分隔
            });


            ///初始化图片控件
            $('.file').prepend('<img src="' + top.$.rootUrl + headimg + '" id="uploadPreview" onerror="loaddfimg()"  style="max-height:200px;max-width:400px;">');
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
        var ImgBase64 = getImgFilesData("uploaderFiles");
        if (!!ImgBase64) {
            postData["ImgBase64"] = ImgBase64;
            //learun.loading(true, '正在保存...');
            //$.lrSaveForm(top.$.rootUrl + '/DM_APPManage/dm_friend_circle/UploadFile?keyValue=' + keyValue, postData, function (res) {
            //    // 保存成功后才回调
            //    if (!!callBack) {
            //        callBack();
            //    }
            //});

            $.ajaxFileUpload({
                url: "/DM_APPManage/dm_friend_circle/UploadFile?keyValue=" + keyValue,
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
            learun.alert.error('请选择需要上传的图片');
        }
    };
    page.init();
}
