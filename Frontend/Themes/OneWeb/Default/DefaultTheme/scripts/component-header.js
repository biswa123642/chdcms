//varient 3
(function($) {
    //$('.header-shell.varient-3 .items .item').find('.sub-menu-list').removeClass('open');
    $(
        ".header-shell.varient-2 .item .has-sublist > a, .header-shell.varient-2 .item .has-sublist .scLooseFrameZone > a"
    ).on("click", function(e) {
        e.preventDefault();
        e.stopPropagation();
        var $sibling = $(this).parents(".item").eq(0).find(".sub-menu-list");
        var $sibling2 = $(this).parents(".has-sublist");
        $(this)
            .parents(".item")
            .siblings()
            .find(".sub-menu-list")
            .removeClass("open");

        if ($sibling.hasClass("open")) {
            $sibling.removeClass("open");
            $sibling2.removeClass("open");
        } else {
            $sibling.addClass("open");
            $sibling2.addClass("open");
        }
    });
    //global-menu-varaint-3
    $(document).on("mouseover", ".header-shell.varient-3 .item", function() {
        $(this).find(".sub-menu-list").show();
    });
    $(document).on("mouseleave", ".header-shell.varient-3 .item", function() {
        $(this).find(".sub-menu-list").hide();
    });
    $(".header-shell.varient-3 .search-box-button").on("click", function(e) {
        e.preventDefault();
        var $searchbox = $(this)
            .closest(".header-shell")
            .find(".search-box-input")
            .eq(1);
        if ($searchbox.is(":visible")) {
            $searchbox.hide();
        } else {
            $searchbox.show();
            $searchbox.focus();
        }
    });

    $(".header-shell.varient-2 .mobile-search-toggle").on("click", function(e) {
        e.preventDefault();
        e.stopPropagation();
        if ($(".header-shell.varient-2 .header-global-menu").is(":visible")) {
            $(".header-global-menu ").hide();
            $(".hamburger .hamburger-inner").removeClass("is-active");
        }
        let headHight = $(".header-shell").innerHeight();
        var $searchbox = $(this).closest(".header-shell").find(".search-box");
        if ($searchbox.is(":visible")) {
            $searchbox.hide();
        } else {
            //let setSearchBoxContWidth = $('.header-shell .component-content .container').innerWidth();
            $searchbox.show();
            $searchbox.css({ "margin-top": headHight });
        }
    });

    $(window).resize(function() {
        let menuWrapper = $(".menu-wrapper")
        var newheadHight = $(".header-shell").innerHeight(),
            $searchBoxCont = $(".header-shell.varient-2").find(".search-box");
        if ($(window).width() > 997) {
            $searchBoxCont.css({ display: "block", "margin-top": "auto" }); //desktop
            menuWrapper.css("display", "block");

        } else {
            $searchBoxCont.css({ "margin-top": newheadHight }); //mob
            menuWrapper.css("display", "none");

        }

    });


    // for hiding sub menu list dopdown on outside canvas click / touched
    $("body, html")
        .off("click")
        .on("click", function(e) {
            if (!$(e.target || e.srcElement).is(
                    ".header-shell.varient-3 .item .has-sublist > a, .header-shell.varient-3 .item .has-sublist .scLooseFrameZone > a"
                )) {
                if ($(".sub-menu-list, .has-sublist ").hasClass("open")) {
                    $(".sub-menu-list, .has-sublist").removeClass("open");
                }
            }
        });

    //drop-down-menu

    $('html, body').click(function() {
        if ($(".header-shell.varient-2 .search-box").is(":visible")) {
            $(".header-shell.varient-2 .search-box").hide();
        }

        if ($(".header-shell.varient-2 .header-global-menu").is(":visible")) {
            if ($(".header-shell.varient-2 .sub-menu-list").hasClass("open")) {
                $(".header-shell.varient-2 .sub-menu-list, .header-shell.varient-2 .has-sublist").removeClass("open");
                return false;
            } else {
                $(".header-shell.varient-2 .header-global-menu ").hide();
                $(".header-shell.varient-2 .hamburger-box").find('.hamburger-inner').removeClass('is-active');
            }
        }
    });


    $('.header-shell.varient-2 .header-global-menu .items,.header-shell.varient-2 .search-box').click(function(e) {
        e.stopPropagation();
    });

    // hamburger
    $(".header-shell.varient-3 .hamburger").on("click", function() {
        $(this).closest(".container").find(".header-global-menu").slideToggle();
    });
    // hamburger
    $(".header-shell.varient-2 .hamburger").on("click", function(event) {
        event.stopPropagation();
        if ($(".header-shell.varient-2 .search-box").is(":visible")) {
            $(".header-shell.varient-2 .search-box").hide();
        }
        $(this)
            .parents(".component-content")
            .find(".hamburger-inner")
            .toggleClass("is-active");
        $(this)
            .parents(".component-content")
            .find(".header-global-menu")
            .slideToggle();
    });



    // variant 1

    //prevents dropdown from closing when clicked inside

    $(document).on("click", ".has-sublist", function(e) {
        e.stopPropagation();
    });

    // Force dropdowns to open on hover

    $(".header-shell.varient-1 .has-sublist").hover(
        function() {
            $(this).addClass("open");
        },
        function() {
            $(this).removeClass("open");
        }
    );
    // hide search onfocus out
    $(".header-shell.varient-1 .search-box-button").on("click", function(e) {
        e.preventDefault();
        var $searchbox = $(".header-shell.varient-1 .search-box-input").eq(1);
        if ($searchbox.is(":visible")) {
            $searchbox.hide();
        } else {
            $searchbox.show();
            setTimeout(function() {
                $searchbox.focus();
            }, 0);
        }
    });

    /* document.addEventListener("mouseup", function(e) {
          let container = document.getElementById("container");
          if (!container.contains(e.target)) {
              container.style.display = "none";
          }
      }); */

    // hamburger

    // hamburger menu for var-1

    $(".header-shell.varient-1 .hamburger").on("click", function() {
        $(this)
            .parents(".component-content")
            .find(".hamburger")
            .toggleClass("collapsed");
        $(this).parents(".component-content").find(".menu-wrapper").slideToggle();
    });
    $(window).on("click", function(e) {
        var current = e.target;
        var match = $(".plain-html ,.menu-wrapper")
            .find("*")
            .get()
            .find(function(x) {
                return x == current;
            });
        if ($(window).width() < 991) {
            if (match == undefined) {
                if ($(window).width() < 992) {
                    $(".hamburger").removeClass("collapsed");
                    $(".menu-wrapper").slideUp();
                }
            }

            if (!$(".menu-wrapper").is(e.target) &&
                $(".menu-wrapper").has(e.target).length === 0
            ) {
                $(".sub-menu-container").slideUp().siblings('.arrow').removeClass('active');
            }
        }
    });
    // varient-1 arrow toggle functionality
    $(".header-shell.varient-1 .items .arrow").on("click", function() {
        var _this = $(this);
        if (_this.siblings(".sub-menu-container").hasClass('activated') == true) {
            _this.siblings(".sub-menu-container").removeClass('activated').slideUp();
            _this.removeClass('active');

        } else {
            _this.siblings(".sub-menu-container").addClass('activated').slideDown();
            _this.addClass('active');
        }

        $(".sub-menu-container.activated").each(function() {
            if ($(this)[0] != _this.siblings(".sub-menu-container")[0]) {

                $(this).slideUp().removeClass('activated').siblings('.arrow').removeClass('active');
            }
        })

    });


    //submenu class for arrow

    /* $(".header-shell.varient-1  .arrow").on("click", function () {
       if ($(this).hasClass("active")) {
         $(this).removeClass("active");
       } else {
         $(this).addClass("active");
       }
     });*/

    //header match height for fixed position variant-1


    //focus out

    $(".header-shell.varient-1 .search-box input").focusout(function() {
        var _this = $(this);
        setTimeout(function() {
            _this.closest("input").hide();
        }, 150);
        //  $(this).closest("input").hide();
    });
    //header match height for fixed position -- variant-2
    $(document).ready(function() {
        var h = $(".header-shell.varient-2,.header-shell.varient-3").height();
        $("body").css({ "padding-top": h + "px" });

    });
    $(document).ready(function() {
        var h = $(".header-shell.varient-1").height();
        if ($(window).width() > 997) {
            $("body").css({ "padding-top": h - 50 + "px" }); //desktop
        } else {
            $("body").css({ "padding-top": h + "px" }); //mob
        }
    });
    // clone top nav
    var topNav = $('.header-shell.varient-2 .top-nav').html();
    $(".header-shell.varient-2 .header-global-menu").append('<div class="top-nav-clone col-12">' + topNav + '</div>');
})(jQuery);