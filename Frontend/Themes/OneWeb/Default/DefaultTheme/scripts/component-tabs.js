// document ready
(function($) {
    if (($(".component.tabs").length)) {
        $(".component.tabs").each(function() {
            $(this).find(".tabs-container .tab").each(function(i) {
                var $tabContent = $(this).parents(".tabs-inner").eq(0).find('.tabs-heading li').eq(i).find(".row").html();
                $(this).prepend("<div class='row tab-heading-mob'>" + $tabContent + "</div>");
            });
        });
    }
    // console.log("Desktop " + $(window).width());
    if ($(window).width() > 997) {
        tabs_desk_hndlr();
    } else {
        accordion_tabs_mob_hndlr();
    }


    //start mobile tab  
    var winScrollTop = $(window).scrollTop();
    var scrollTop;

    if ($('.tabs-container').length > 0) {
        if ($(window).width() < 992) {
            $('.tabs-container .tab-heading-mob').click(function() {
                var position = $(this).position();
                //  e.preventDefault();
                $('html,body').animate({
                    scrollTop: position.top - 15
                }, 700);

            })
        }
    }
    //end  mobile tab


})(jQuery);

// to be run when window resize by user
jQuery(window).resize(function() {
    // console.log("Mobile " + jQuery(window).width());
    if (jQuery(window).width() > 997) {
        tabs_desk_hndlr();
    } else {
        accordion_tabs_mob_hndlr();
    }
});

// tabs accordian handling for mobile and tabs screen
function accordion_tabs_mob_hndlr() {
    jQuery('.tabs-container .tab .tab-heading-mob').on('click', function() {
        jQuery(this).closest('.tabs-container').find(".tab").removeClass('active');
        jQuery(this).closest('.tab').addClass("active").slideDown();

        // remove active class from all headig li
        jQuery('.tabs-heading li').removeClass('active');

        jQuery(this).closest('.tabs-container').find(".tab").each(function(count) {
            if (jQuery(this).is(".active")) {
                jQuery(this).find('.row:last').slideDown();

                // below code use to select tabs button based on accordian active in mobile
                jQuery('.tabs-heading li:eq(' + count + ')').addClass('active');
            } else {
                jQuery(this).find('.row:last').slideUp();
            }
        });
    });
}

// tabs handling for large screen
function tabs_desk_hndlr() {
    jQuery('.tabs-container').find('.tab').each(function() {
        // jQuery(this).find('.tab-heading-mob').next('.row').removeAttr('style');
        //  jQuery(this).find('.row:last').slideDown();
    });
}