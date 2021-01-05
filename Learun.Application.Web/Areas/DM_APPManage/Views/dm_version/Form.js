/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-01-05 13:46
 * 描  述：版本管理
 */
var acceptClick;
var keyValue = request('keyValue');
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
                //console.log(f);
                //var src = window.URL.createObjectURL(f);
                document.getElementById('app_name').innerHTML = f.name;
            };

            $('#uploadFile').on('change', uploadImg);

            $('#App_Plaform').lrselect().on("change", function () {
                page.initImage();
            });
        },
        initData: function () {
            if (!!selectedRow) {
                $('#form').lrSetFormData(selectedRow);
                document.getElementById('app_name').innerHTML = selectedRow.App_Name;
            } else {
                page.initImage();
            }
        }, initImage: function () {
            var headimg;
            var app_plaform = $("#App_Plaform").lrselectGet();
            if (app_plaform == "2") {
                headimg = top.$.rootUrl + '/Content/images/iOS.png';
            } else {
                headimg = top.$.rootUrl + '/Content/images/Android.png';
            }
            document.getElementById('uploadPreview').src = headimg;
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
                url: "/DM_APPManage/dm_version/UploadFile?keyValue=" + keyValue,
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
            $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/dm_version/SaveForm?keyValue=' + keyValue, postData, function (res) {
                // 保存成功后才回调
                if (!!callBack) {
                    callBack();
                }
            });
        }
    };
    page.init();
}
