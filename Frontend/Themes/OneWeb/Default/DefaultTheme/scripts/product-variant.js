(function ($) {
//zoom activate
function zoomActivatior(){ 
  $('.productimage .image-placeholder').trigger('zoom.destroy'); 	
  $('.productimage .image-placeholder').zoom({ on:'click' });	
}
zoomActivatior();
  // fetching data from json & declaring into variables
  var string = document.getElementById("product-variant-json").innerHTML;  //product variation data
  var imageJSON = document.getElementById("product-image-gallery-json").innerHTML;  //product image data
  var imageData = JSON.parse(imageJSON);
  var y = imageData.ProductVariantList;
  var stringData = JSON.parse(string);

  // adding is-carousel class for slick
  if (imageData.IsCarousel === true) {
    $(".productimage .inner-wrap .image-container").addClass("is-carousel");
  } 

  //logic on changing filter
  var variantValue = "";
  $(document).on("change", "#primaryvariationvalue", function (e) {
    // flushing previous data on change event
    clearPrevData();    
    $("#secondaryvariationvalue").empty();

    var selectedSizeValue = $("#primaryvariationvalue").find(":selected").val();
    var selectedFlavourValue = $("#secondaryvariationvalue").find(":selected").val();
    var selectedColorValue = $("#tertiaryvariationvalue").find(":selected").val();
    variantValue = "";
    zoomActivatior();
    stringData.ProductVariantList.filter(function (val) {
      if (val.Primary == $("#primaryvariationvalue").find(":selected").val()) {
        $("#secondaryvariationvalue").append(
          `<option value=${val.Secondary}>${val.Secondary}<option>`
        );
      }

      if (val.Primary == $("#primaryvariationvalue").find(":selected").val() && val.Secondary == $("#secondaryvariationvalue").find(":selected").val()) {
        $("#tertiaryvariationvalue").append(`<option>${val.Tertiary}</option>`);
      }

      if (
        (val.Primary == $("#primaryvariationvalue").find(":selected").val() ||
          $("#primaryvariationvalue").find(":selected").val() == undefined) &&
        (val.Secondary ==
          $("#secondaryvariationvalue").find(":selected").val() ||
          $("#secondaryvariationvalue").find(":selected").val() == undefined) &&
        (val.Tertiary == $("#tertiaryvariationvalue").find(":selected").val() ||
          $("#tertiaryvariationvalue").find(":selected").val() == undefined)
      ) {
        variantValue = val.VariantSKU;        
        $(".where-to-buy .ps-widget").attr("ps-sku", variantValue);
        $(".where-to-buy .ps-widget").attr("data-ps-sku", variantValue);
        $(".product-variant .variant-description").append(`<div class="text-wrap">${val.VariantDescription}</div>`);
        
        y.map((data) => {          
          if (data.VariantSKU === variantValue) {            
            data &&
              data.ProductMediaList.map((val) => {               
               
                if (val.MediaType === "image") {                 

                  $(".productimage .inner-wrap .image-container").append(
                    `<div class="field-image"><img src="${val.MediaURL}" /></div>`
                  );                  
                }
                if (val.MediaType === "youtube") {
                  console.log(val.MediaURL);
                  $(".productimage .inner-wrap .image-container").append(
                    `<div class="video-checkbox"><iframe src="https://www.youtube.com/embed/${val.YoutubeId}" ></iframe></div>`
                  );
                }
                if (val.MediaType === "video") {
                  $(".productimage .inner-wrap .image-container").append(
                    `<div class="internal video"><video><source src="${val.MediaURL}" /></video></div>`
                  );
                }
              });
          }
        });
      }      
    });

    //placeholder first image
    placeHolderImage();
    //carousel
    if (imageData.IsCarousel === true) {
      productImageCarausal();
    }
    
  });

  //logic for secondary filter change
  $(document).on("change", "#secondaryvariationvalue", function (e) {
    clearPrevData();
    zoomActivatior();    
    stringData.ProductVariantList.filter(function (val) {
      if (val.Primary == $("#primaryvariationvalue").find(":selected").val() && val.Secondary == $("#secondaryvariationvalue").find(":selected").val()) {
        $("#tertiaryvariationvalue").append(`<option>${val.Tertiary}</option>`);
      }

      if (
        (val.Primary == $("#primaryvariationvalue").find(":selected").val() ||
          $("#primaryvariationvalue").find(":selected").val() == undefined) &&
        (val.Secondary ==
          $("#secondaryvariationvalue").find(":selected").val() ||
          $("#secondaryvariationvalue").find(":selected").val() == undefined) &&
        (val.Tertiary == $("#tertiaryvariationvalue").find(":selected").val() ||
          $("#tertiaryvariationvalue").find(":selected").val() == undefined)
      ) {
        variantValue = val.VariantSKU;
        $(".where-to-buy .ps-widget").attr("ps-sku", variantValue);
        $(".where-to-buy .ps-widget").attr("data-ps-sku", variantValue);
        $(".product-variant .variant-description").append(`<div class="text-wrap">${val.VariantDescription}</div>`);
        y.map((data) => {
          console.log("hello " + data);
          if (data.VariantSKU === variantValue) {            
            data &&
              data.ProductMediaList.map((val) => {                
                if (val.MediaType === "image") {                 

                  $(".productimage .inner-wrap .image-container").append(
                    `<div class="field-image"><img src="${val.MediaURL}" /></div>`
                  );                  
                }
                if (val.MediaType === "youtube") {
                  console.log(val.MediaURL);
                  $(".productimage .inner-wrap .image-container").append(
                    `<div class="video-checkbox"><iframe src="https://www.youtube.com/embed/${val.YoutubeId}" ></iframe></div>`
                  );
                }
                if (val.MediaType === "video") {
                  $(".productimage .inner-wrap .image-container").append(
                    `<div class="internal video"><video><source src="${val.MediaURL}" /></video></div>`
                  );
                }
              });
          }
        });
      }
    });
    //placeholder first image
    placeHolderImage();
    //carousel
    if (imageData.IsCarousel === true) {
      productImageCarausal();
    }
  });

  //initial sku value assign on load
  $("document").ready(function () {
    stringData.ProductVariantList.filter(function (val) {
      if (
        (val.Primary == $("#primaryvariationvalue").find(":selected").val() ||
          $("#primaryvariationvalue").find(":selected").val() == undefined) &&
        (val.Secondary ==
          $("#secondaryvariationvalue").find(":selected").val() ||
          $("#secondaryvariationvalue").find(":selected").val() == undefined) &&
        (val.Tertiary == $("#tertiaryvariationvalue").find(":selected").val() ||
          $("#tertiaryvariationvalue").find(":selected").val() == undefined)
      ) {
        variantValue = val.VariantSKU;

        $(".where-to-buy .ps-widget").attr("ps-sku", variantValue);
        $(".where-to-buy .ps-widget").attr("data-ps-sku", variantValue);
        $(".product-variant .variant-description").append(`<div class="text-wrap">${val.VariantDescription}</div>`);
      }
    });
  });
    
  $(document).on("click", ".image-container .field-image, .image-container .internal, .image-container .video-checkbox ", function () {
    $(".image-placeholder").empty();
    var crtDiv = $(this).html();    
    $(".image-placeholder").append(crtDiv);
    zoomActivatior();
  });

  productImageCarausal();

  // auto video 
  $(document).on("click", ".image-container div", function () {
    if($(this).find('iframe').length > 0){ 
      var curSRC = $('.image-placeholder').find('iframe').attr('src'); 
      $('.image-placeholder').find('iframe').attr('src', curSRC+"?rel=0&autoplay=1&mute=1&controls=0&playlist=p8lggRNAhYI&loop=1" );
    }
  });
  // function for flushing previous data
  function clearPrevData() {
    $('.productimage .is-carousel').slick('unslick');
    $(".productimage .inner-wrap .image-container").empty();
    $(".product-variant .variant-description").empty();
    $(".productimage .image-placeholder").empty()
    $("#tertiaryvariationvalue").empty();
  }

  // image placeholder first image
 function placeHolderImage(){
  var crtDiv = $(".image-container div:first-child").html();
  console.log(crtDiv);
  $(".image-placeholder").append(crtDiv);
  zoomActivatior();
 }

// function for slick
  function productImageCarausal() {
    $(".productimage .is-carousel").slick({
      dots: false,
      infinite: false,
      speed: 300,
      slidesToShow: 3,
      slidesToScroll: 3,
      responsive: [
        {
          breakpoint: 1024,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 3,
            infinite: true,
            dots: true,
          },
        },
        {
          breakpoint: 600,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 2,
          },
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
          },
        },
        
      ],
    });

    
  } 
})(jQuery);
 //condition secondaryvariationvalue hide and show 
    /* if($('#secondaryvariationvalue option').length > 0){
      $('#secondaryvariationvalue').show();
      $('#secondaryvariationvalue').prev('h4').show();
    } else {
      $('#secondaryvariationvalue').hide();
      $('#secondaryvariationvalue').prev('h4').hide();
    } */