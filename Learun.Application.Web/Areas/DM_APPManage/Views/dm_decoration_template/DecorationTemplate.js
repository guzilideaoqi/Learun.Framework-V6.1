/* * 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架(http://www.learun.cn)
 * Copyright (c) 2013-2017 上海力软信息技术有限公司
 * 创建人：超级管理员
 * 日  期：2021-03-03 10:56
 * 描  述：装修模板
 */
var acceptClick;
var keyValue = request('keyValue');
var addrow;
var removerow;
var selectattachment;
var selectfun;
var removemodule;
var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            page.bind();
            page.initData();
            $("#main_color").css({ "background-color": selectedRow.main_color, "color": selectedRow.main_color })
            $("#secondary_color").css({ "background-color": selectedRow.secondary_color, "color": selectedRow.secondary_color })
        },
        bind: function () {
            // 新增
            $('#lr_add').on('click', function () {
                selectedRow = null;
                learun.layerForm({
                    id: 'selectmodule',
                    title: '新增功能',
                    url: top.$.rootUrl + '/DM_APPManage/dm_decoration_module/SelectModule',
                    width: 600,
                    height: 400,
                    callBack: function (id) {
                        var selectItem = top[id].acceptClick();
                        if (!!selectItem) {
                            page.generalDataGrid(selectItem);
                            learun.layerClose("selectmodule");
                        }
                    }
                });
            });
        },
        initData: function () {
            if (!!selectedRow) {
                $('#form').lrSetFormData(selectedRow);
            }
        }, generalDataGrid: function (item) {
            console.log(item);
            var gridtable_id = "gridtable" + item.module_type;

            if (item.module_type == 2 || item.module_type == 3) {
                if (page.existgridtable(2) || page.existgridtable(3)) {
                    learun.alert.error("多眼专区和多眼专区加强版只能存在其一!"); return;
                }
            } else if (item.module_type == 1) {
                if (page.existgridtable(item.module_type)) {
                    learun.alert.error("滚动轮播图模块已存在!"); return;
                }
            } else if (item.module_type == 8) {
                if (page.existgridtable(item.module_type)) {
                    learun.alert.error("滚动公告模块已存在!"); return;
                }
            } else {
                gridtable_id += learun.newGuid();
            }


            var html = "<div class=\"module_item\" module_type=\"" + item.module_type + "\">" +
                "<label class=\"module_name\">" + item.module_name + "</label>" +
                "<label class=\"add_fun_item\" onclick=\"addrow('" + gridtable_id + "')\">+</label>" +
                "<label class=\"removemodule\" onclick=\"removemodule(this)\">X</label>" +
                "<div id=\"" + gridtable_id + "\"></div>" +
                "</div>";
            $("#container").append(html);


            page.initgrid(gridtable_id);
        }, initgrid: function (gridtable_id) {
            $('#' + gridtable_id).jfGrid({
                rowdatas: [],
                headData: [
                    {
                        label: '名称', name: 'module_item_name', width: 145, align: "left", formatter: function (cellvalue, rowdata, options) {
                            return "<input id=\"txt_Keyword\" type=\"text\" class=\"form-control\" placeholder=\"名称\" value=\"" + cellvalue + "\" />";
                        }
                    },
                    {
                        label: '图片', name: 'module_item_image', width: 105, align: "center", formatter: function (cellvalue, rowdata, options) {
                            if (!!cellvalue) {
                                return "<img src=\"" + cellvalue + "\" style=\"width:25px;height:25px;\" />";
                            } else {
                                return "<i class=\"fa fa-pencil-square-o\" onclick=\"selectattachment(this)\"></i>";
                            }
                        }
                    },
                    {
                        label: '功能', name: 'module_fun_id', width: 155, align: "center", formatter: function (cellvalue, rowdata, options) {
                            if (!!cellvalue) {
                                return "<img src=\"" + cellvalue + "\" style=\"width:25px;height:25px;\" />";
                            } else {
                                return "<i class=\"fa fa-pencil-square-o\" onclick=\"selectfun(this)\"></i>";
                            }
                        }
                    },
                    {
                        label: '排序', name: 'module_sort', width: 108, align: "center", formatter: function (cellvalue, rowdata, options) {
                            return "<input id=\"txt_Keyword\" type=\"text\" class=\"form-control\" style=\"text-align:center;\" placeholder=\"排序\" />";
                        }
                    },
                    {
                        label: '操作', name: 'id', width: 50, align: "center", formatter: function (cellvalue, rowdata, options) {
                            console.log(rowdata)
                            return "<span class=\"label label-danger\" style=\"cursor: pointer;\" onclick=\"removerow('" + gridtable_id + "','" + cellvalue + "');return;\">删除</span>";
                        }
                    }
                ],
                isShowNum: false,
                isAutoHeight: true
            });
        }, existgridtable(id) {
            return $("#gridtable" + id).length > 0;
        }
    };
    // 保存数据
    acceptClick = function (callBack) {
        if (!$('#form').lrValidform()) {
            return false;
        }
        var postData = $('#form').lrGetFormData();
        $.lrSaveForm(top.$.rootUrl + '/DM_APPManage/dm_decoration_template/SaveForm?keyValue=' + keyValue, postData, function (res) {
            // 保存成功后才回调
            if (!!callBack) {
                callBack();
            }
        });
    };
    addrow = function (gridtable_id) {
        var module_type = parseInt($("#" + gridtable_id).parent().attr("module_type"));
        var dbTable = $('#' + gridtable_id).jfGridGet('rowdatas');
        if (module_type == 4 && dbTable.length >= 1) {
            learun.alert.error("自由拼图(一联)只能添加一项!"); return;
        } else if (module_type == 5 && dbTable.length >= 2) {
            learun.alert.error("自由拼图(两联)只能添加两项!"); return;
        } else if (module_type == 6 && dbTable.length >= 3) {
            learun.alert.error("自由拼图(两联)只能添加三项!"); return;
        } else if (module_type == 7 && dbTable.length >= 4) {
            learun.alert.error("自由拼图(两联)只能添加四项!"); return;
        } else if (module_type == 8 && dbTable.length >= 1) {
            learun.alert.error("滚动公告只能添加一项!"); return;
        }


        $('#' + gridtable_id).jfGridSet('addRow', { row: { id: learun.newGuid(), module_item_name: "", module_item_image: "", module_fun_id: "", module_sort: 0 } });
    };
    removerow = function (gridtable_id, id) {
        var dbTable = $('#' + gridtable_id).jfGridGet('rowdatas');
        for (var i = 0; i < dbTable.length; i++) {
            var item = dbTable[i];
            if (item.id == id) {
                dbTable.splice(item, 1); break;
            }
        }
        $('#' + gridtable_id).jfGridSet('refreshdata', { rowdatas: dbTable });
    };
    selectattachment = function (thi) {
        learun.layerForm({
            id: 'form',
            title: '选择附件',
            url: top.$.rootUrl + '/DM_APPManage/dm_attachment/AttachMentManage?keyValue=0',
            width: 900,
            height: 650,
            callBack: function (id) {
                var selectedData = top[id].selectFile();
                if (!selectedData) {
                    learun.alert.error("请选择文件!");
                } else {
                    var parent_con = $(thi).prev();
                    if (!!parent_con && parent_con.length > 0) {
                        debugger;
                        var nodename = parent_con[0].nodeName;
                        if (nodename == "IMG") {
                            parent_con[0].src = selectedData;
                        }
                    } else {
                        $(thi).before("<img src=\"" + selectedData + "\" />");
                    }
                    learun.layerClose("form");
                }
            }
        });
    };
    selectfun = function (thi) {
        learun.layerForm({
            id: 'form',
            title: '功能选择',
            url: top.$.rootUrl + '/DM_APPManage/dm_decoration_fun_manage/SelectFun',
            width: 600,
            height: 650,
            callBack: function (id) {

            }
        });
    };
    removemodule = function (thi) {
        $(thi).parent().remove();
    }
    page.init();
}
