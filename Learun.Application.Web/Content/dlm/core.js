// JavaScript Document
$(function () {

    //鍏ㄥ眬鍙橀噺
    var wnd = $(window), bd = $('body'), peanutDiary = $('.peanut-diary_body'), head = $('header'), foot = $('footer'), mask = $('.mask');

    //鍒ゆ柇瀹㈡埛绔槸鍚︿负PC
    function isPC() {
        var userAgentInfo = navigator.userAgent;
        var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
        var flag = true;
        for (var v = 0; v < Agents.length; v++) {
            if (userAgentInfo.indexOf(Agents[v]) > 0) { flag = false; break; }
        }
        return flag;
    };
    if (isPC()) {
        bd.addClass('pc');
    } else {
        bd.addClass('mo');
    }

    //闈為椤�
    if (!$('main').hasClass('mt1')) {
        bd.addClass('no-home');
    }


    //璧嬪€�
    //var ssBar= $('.search_bar');

    window.onresize = function () {
        assigns();
    }
    function assigns() {
		/*
		if(wnd.width()>1024){
			ssBar.attr('href','javascript:;')
		}else{
			ssBar.attr('href','https://guangfan.com/')	
		}
		*/
    } assigns();

    //appear
	/*
	$('.aniTop,.aniLeft,.aniRight').appear()
	$(document.body).on('appear',".aniTop,.aniLeft,.aniRight", function() {
		$(this).addClass('ani-active');
	});
	setTimeout(function () {
		//$(".aniTop").addClass('ani-active');
	},2000);
	*/
    new WOW().init();


    //鍥剧墖鍔犺浇
	/*
	loadImg();
	$(window).resize(function(){
		loadImg();
	})
	function loadImg(){
		
	}
	$("img").delayLoading({
		defaultImg: "",
		errorImg: "",
		imgSrcAttr: "originalSrc",
		beforehand: 0,
		event: "scroll",
		duration: "normal", 
		container: window
	});
	*/


    //scroll
    //var head_area1=$('.head_area1');
    $(window).on('scroll reaize', function (event) {
        var scrollTop = wnd.scrollTop();
        if (scrollTop >= wnd.height()) {
            bd.addClass('gd');
        } else {
            bd.removeClass('gd');
        }
    })

    //nav
    var nav = $('nav'), navLi = nav.find('li'), navLen = navLi.length, navArr = new Array();

    if ($('nav li.current').length == 0) {
        nav.children().prepend('<li class="current" style="width:0; overflow:hidden; margin-right:0;"><a href="#"></a></li>')
    }

    var yjLine = $(".yj_line"), initWidth = $(".current").width(), initPos = $(".current").position().left;

    yjLinePos()
    navLi.mouseenter(function (e) {
        var curWidth = $(this).width(), curPos = $(this).position().left;
        yjLine.stop().animate({
            left: curPos,
            width: curWidth
        });
    });
    nav.mouseleave(function (e) {
        yjLinePos()
    });
    function yjLinePos() {
        yjLine.stop().animate({
            left: initPos,
            width: initWidth
        });
    };


    navLi.find('.ejbody').css('display', 'none');

    var nav = $('nav'), navLi = nav.find('li'), navBar = $('.nav_bar');
    navLi.hover(function () {
        $(this).find('.ejbody').stop().slideDown();
        $(this).siblings().find('.ejbody').stop().slideUp(0);
    }, function () {
        $(this).find('.ejbody').stop().slideUp(0);
    })

    navBar.click(function () {
        bd.toggleClass('kq_nav');
    })

    peanutDiary.after('<!-- monav --><div class="monav ui_siz ui_trans05 mcs ui_hide"><div class="nav_main">' + $('nav').html() + '</div></div>');

    var monavYj = $('.monav .yjtit');
    monavYj.each(function () {
        var ejL = $(this).siblings().children().length;
        if (ejL > 0) $(this).attr('href', 'javascript:;');
    })
    monavYj.click(function () {
        $(this).parent('li').toggleClass('on').siblings('li').removeClass('on');
        $(this).siblings('.ejbody').stop().slideToggle();
        $(this).parent('li').siblings('li').find('.ejbody').stop().slideUp();
    })


    //鑿滃崟-鍐呭-swiper鍙屽叧鑱�
    var gl_mBar = $('.swiper_gl_m .swiper-slide');
    var swiper_gl_m = new Swiper('.swiper_gl_m', {
        slidesPerView: 'auto',
    });
    var loop = false;//鍙紑鍚惊鐜�
    var i;
    function on() {
        if (loop == true) i = i - 1;
        if (i >= gl_mBar.length) i = 0;
        gl_mBar.eq(i).addClass('on').siblings().removeClass('on');
    }
    var swiper_gl_c = new Swiper('.swiper_gl_c', {
        slidesPerView: 1,
        //effect : 'fade',
        speed: 1000,
        //loop:loop,
        autoHeight: true,
        simulateTouch: false,
        navigation: {
            prevEl: '.gl_c_prev',
            nextEl: '.gl_c_next',
        },
        on: {
            init: function () {
                i = this.activeIndex;
                on();
            },
            slideChangeTransitionStart: function () {
                i = this.activeIndex;
                on();
            },
        },
    });

    gl_mBar.click(function () {
        var k = $(this).index();
        if (loop == true) k = k + 1;
        $(this).addClass('on').siblings().removeClass('on');
        swiper_gl_c.slideTo(k, 1000, false);
    })


    //search
	/*
	ssBar.click(function(){
		//mc.show();
		bd.addClass('kq_ss');
	})
	ssClose.click(function(){
		bd.removeClass('kq_ss');
		//mc.hide();
	})
	*/


    //妯℃嫙select
	/*
	var mn_cur=$('.mn_cur'),mn_lists=$('.mn_lists');
	mn_cur.click(function(){
		mn_lists.stop().fadeToggle();	
	})
	mn_lists.find('li').click(function(){
		$(this).addClass('selected').siblings().removeClass('selected');
		mn_cur.text($(this).text());
		mn_lists.stop().fadeOut();	
	})
	*/


    //video
    var v = $('.v-item'), vPOP = $('#video_pop,#play_area'), vTit = $('.video_tit');
    var dataV = '';
    var Prompt = '<div class="Prompt">鏆傛棤瑙嗛锛屾暚璇锋湡寰咃紒</div>';
    var reg = /<[^>]+>/g;

    var itemV = $('.v-item.on');

    function playV() {
        var dataV = itemV.attr('data-v');
        var dataWH = itemV.attr('data-wh');
        var dataST = itemV.attr('data-st');
        if (dataST === undefined) dataST = '';
        var wndW = head.find('.wrp1').width(), wndH = vPOP.children().height();

        var whArr = dataWH.split(","); stArr = dataST.split(",");
        var W = whArr[0], H = whArr[1], Vstyle = stArr[0], Vtit = stArr[1];

        if (W <= wndW && H <= wndH) newW = W, newH = H;
        if (W <= wndW && H > wndH) newW = wndH / (H / W), newH = wndH;
        if (W > wndW && H <= wndH) newW = wndW, newH = wndW * (H / W);


        if (Vstyle != 'style1') bd.addClass('v-open');


        if (reg.test(dataV) == true) {
            vPOP.children().append('<div class="v_area">' + dataV + '<a class="close-v"></a></div><div class="v_tit ui_mid3">' + Vtit + '</div>');
        } else {
            var file = dataV,
                filename = file.replace(/.*(\/|\\)/, ""),
                fileExt = (/[.]/.exec(filename)) ? /[^.]+$/.exec(filename.toLowerCase()) : '';
            if (fileExt == 'mp4') {
                vPOP.children().append('<div class="v_area"><video src="' + dataV + '" controls></video><a class="close-v"></a></div><div class="v_tit ui_mid3">' + Vtit + '</div>');
            } else {
                vPOP.children().append(Prompt);
            }
        }
        vPOP.find('iframe,video').css({
            width: newW,
            height: newH,
        })

        if (Vstyle == 'style1') $("html,body").animate({ scrollTop: 0 }, 800);
    }
    if (v.hasClass('on')) playV();

    v.click(function () {
        removeV();

        itemV = $(this);

        playV();

    });
    $(document).on('click', '.close-v', function () {
        removeV();
    });

    function removeV() {
        bd.removeClass('v-open');
        vPOP.children().html('');
    }


    //琛ㄥ崟鍏冪礌澶勭悊
    var dateInput = $('input[type="date"]');
    dateInput.on('input propertychange', function () {
        $(this).siblings('label').text($(this).val());
    })


    //headroom
    //var myElement = document.querySelector("body");
    //var headroom  = new Headroom(myElement);
    //headroom.init();


    //婊氬姩鏉＄編鍖�
    var mcs = $('.mcs');
    $(window).load(function () {
        if (isPC()) {
            mcs.mCustomScrollbar();
        } else {
            mcs.css('overflow-y', 'auto');
        }
    })


    //鍔ㄧ敾
    //new WOW().init();

    //returnTop		
    $(".top").on('click', function (event) {
        $("html,body").animate({ scrollTop: 0 }, 800);
    });

})