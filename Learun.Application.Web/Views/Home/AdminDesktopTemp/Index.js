$(function () {
    "use strict";
    var page = {
        init: function () {
            page.bind();
        },
        bind: function () {
            top.learun.httpAsyncGet(top.$.rootUrl + '/DM_APPManage/DM_User/GetStaticData', function (res) {
                console.log(res);
                if (res.code == 200) {
                    page.initBaseCount(res.data.StaticData1[0]);
                    page.initOrder(res.data.StaticData2);
                    page.initTask(res.data.StaticData3);
                    page.initLineChart(res.data.StaticData4);
                }
            });
        },
        initBaseCount: function (moduledata) {
            $("#UserCount").html(moduledata.usercount);
            $("#OrderCount").html(moduledata.ordercount);
            $("#TaskCount").html(moduledata.taskcount);
            $("#TotalPayPrice").html(moduledata.totalpayprice);
            $("#TotalCommission").html(moduledata.totalcommission);
        },
        initOrder: function (moduledata) {
            var html = "", plaform = "";
            for (var i = 0; i < moduledata.length; i++) {
                var item = moduledata[i];
                switch (item.type_big) {

                    case 1:
                        plaform = "淘宝";
                        break;
                    case 3:
                        plaform = "京东";
                        break;
                    case 4:
                        plaform = "拼多多";
                        break;
                }
                html += "\<div class=\"lr-msg-line\">" +
                    "<a href=\"#\" style=\"text-decoration:none;\">[" + plaform + "]&nbsp;&nbsp;&nbsp;" + item.title.substr(0, 40) + "..." + "</a>" +
                    "<label>" + item.order_createtime + "</label> " +
                    "</div>";
            }
            $("#OrderData").html(html);
        },
        initTask: function (moduledata) {
            var html = "", plaform = "";
            for (var i = 0; i < moduledata.length; i++) {
                var item = moduledata[i];
                switch (item.plaform) {

                    case 0:
                        plaform = "PC";
                        break;
                    case 1:
                        plaform = "APP";
                        break;
                }
                html += "\<div class=\"lr-msg-line\">" +
                    "<a href=\"#\" style=\"text-decoration:none;\">[" + plaform + "]&nbsp;&nbsp;&nbsp;" + item.task_title.substr(0, 40) + "..." + "</a>" +
                    "<label>" + item.createtime + "</label> " +
                    "</div>";
            }
            $("#TaskData").html(html);
        },
        initLineChart: function (moduledata) {

            var data_month = [], month_pay = [], month_effect = [];
            for (var i = moduledata.length - 1; i >= 0; i--) {
                var item = moduledata[i];
                data_month.push(item.order_create_month);
                month_pay.push(item.month_pay);
                month_effect.push(item.month_effect);
            }

            // 基于准备好的dom，初始化echarts实例
            var lineChart = echarts.init(document.getElementById('linecontainer'));
            // 指定图表的配置项和数据
            var lineoption = {
                tooltip: {
                    trigger: 'axis'
                },
                legend: {
                    bottom: 'bottom',
                    data: ['交易金额', '结算佣金']
                },
                grid: {
                    bottom: '8%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    boundaryGap: false,
                    data: data_month
                },
                yAxis: {
                    type: 'value'
                },
                series: [
                    {
                        name: '交易金额',
                        type: 'line',
                        stack: '元',
                        itemStyle: {
                            normal: {
                                color: "#fc0d1b",
                                lineStyle: {
                                    color: "#fc0d1b"
                                }
                            }
                        },
                        data: month_pay
                    },
                    {
                        name: '结算佣金',
                        type: 'line',
                        stack: '元',
                        itemStyle: {
                            normal: {
                                color: '#344858',
                                lineStyle: {
                                    color: '#344858'
                                }
                            }

                        },
                        data: month_effect
                    }
                ]
            };
            // 使用刚指定的配置项和数据显示图表。
            lineChart.setOption(lineoption);
        }
    }
    // 基于准备好的dom，初始化echarts实例
    /*var pieChart = echarts.init(document.getElementById('piecontainer'));
    // 指定图表的配置项和数据
    var pieoption = {
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            bottom: 'bottom',
            data: ['枢纽楼', 'IDC中心', '端局', '模块局', '营业厅','办公大楼','C网基站']
        },
        series: [
            {
                name: '用电占比',
                type: 'pie',
                radius: '75%',
                center: ['50%', '50%'],
                label : {
                    normal : {
                        formatter: '{b}:{c}: ({d}%)',
                        textStyle : {
                            fontWeight : 'normal',
                            fontSize : 12,
                            color:'#333'
                        }
                    }
                },
                data: [
                    { value: 10, name: '枢纽楼' },
                    { value: 10, name: 'IDC中心' },
                    { value: 10, name: '端局' },
                    { value: 10, name: '模块局' },
                    { value: 10, name: '营业厅' },
                    { value: 10, name: '办公大楼' },
                    { value: 40, name: 'C网基站' }
                ],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
        ,
        color:['#df4d4b','#304552','#52bbc8','rgb(224,134,105)','#8dd5b4','#5eb57d','#d78d2f']
    };
    // 使用刚指定的配置项和数据显示图表。
    pieChart.setOption(pieoption);*/


    window.onresize = function (e) {
        pieChart.resize(e);
        lineChart.resize(e);
    }

    $(".lr-desktop-panel").mCustomScrollbar({ // 优化滚动条
        theme: "minimal-dark"
    });

    page.init();
});