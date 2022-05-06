// document ready  page Scoll button
(function($) {
    // Global vars
    var gridFloatBreakpoint = 992;
    var scrollTop;
    var isDesktop;
    var isScrolledToTop = true;
    $('.scroll-button-wrap').find('a').addClass('score-image-button to-top')

    var backToTop = function() {
        var scrollTrigger = 800;
        //stick to footer
        // function calcFooterOffset() {
        //     return $('body').height() - $('footer').height() - $(window).height();
        // }

        function calcToTopBottomPosition() {
            return responsiveUtils.isDesktop() ? 120 : 0;
        }

        return _.debounce(function() {
            if ($(window).scrollTop() > scrollTrigger) {
                $('.to-top').addClass('show');
            } else {
                $('.to-top').removeClass('show');
            }
            //stick to footer
            // if ($(window).scrollTop() > calcFooterOffset()) {
            //     $('.to-top').css({ bottom: ($(window).scrollTop() - calcFooterOffset()) + 'px' });

            // }
            // else {
            //     $('.to-top').css({ bottom: calcToTopBottomPosition() + 'px' });

            // }
        }, 150);
    };
    $(window).on('scroll', backToTop());

    //Event handlers
    $(document).on('click', '.to-top', function(e) {
        if ($(this).hasClass('show')) {
            e.preventDefault();
            $('html,body').animate({
                scrollTop: 0
            }, 700);
        } else {
            e.stopPropagation();
        }
    });


    var responsiveUtils = {
        isMobile: function() {
            return Modernizr.mq('(max-width: 767px)');
        },
        isTablet: function() {
            return Modernizr.mq('(min-width: 768px) and (max-width: 1024px)');
        },
        isDesktop: function() {
            return Modernizr.mq('(min-width: 1025px)');
        }
    }

})(jQuery);