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
                //style =\"background-image:url(" + img.ThumbnailImage + ");background-repeat: round;\"
                $("#uploaderFiles").append("<li class=\"weui-uploader__file\"><img src=\"" + img.ThumbnailImage+"\" alt=\"\"></li>");
            }
            var viewer = new Viewer(document.getElementById('uploaderFiles'));
        }
    }
    page.init();
}