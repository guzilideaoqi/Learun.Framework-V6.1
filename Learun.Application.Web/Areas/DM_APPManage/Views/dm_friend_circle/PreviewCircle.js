var bootstrap = function ($, learun) {
    "use strict";
    var selectedRow = learun.frameTab.currentIframe().selectedRow;
    var page = {
        init: function () {
            top.$(".layui-layer-btn0").hide();
            $("#t_content").html(selectedRow.t_content);
            var ImageList = JSON.parse(selectedRow.t_images.replace(/'/g, "\""));
            for (var i = 0; i < ImageList.length; i++) {
                var img = ImageList[i];
                $("#uploaderFiles").append("<li class=\"weui-uploader__file\" style=\"background-image:url(" + img.ThumbnailImage + ")\"></li>");
            }
            
        }
    }
    page.init();
}