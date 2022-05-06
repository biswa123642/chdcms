(function($) {
   
    $('.component.linked-thumbnail ul').each(function(i, e) {
        var settings = {
            infinite: true,
            speed: 300,
            dots: false,
            slidesToShow: 10,
            slidesToScroll: 1,
            adaptiveHeight: false,
            responsive: [{
                    breakpoint: 999,
                   //settings: 'unslick'
                   settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1,
                    dots: false
                }
                },
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1,
                        dots: false
                    }
                }
            ]
        };
        $(e).slick(settings);

        
        //Resize function...
       /* var w = 0;
        $(window).resize(_.debounce(function() {
            if (!($(".on-page-editor").length) && w != $(window).width()) {
                if ($(window).width() <= 1023 && !$(e).hasClass('slick-initialized')) {
                    $(e).slick(settings);
                }
                w = $(window).width();
            }
        }, 400));*/
    });

})(jQuery);