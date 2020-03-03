(function ($) {
  showSuccessToast = function(){
    'use strict';
    resetToastPosition();
    $.toast({
      heading: 'Success',
      text: 'And these were just the basic demos! Scroll down to check further details on how to customize the output.',
      showHideTransition: 'slide',
      icon: 'success',
      bgColor: '#00c853',
      loaderBg: '#d50000',
      position: 'top-right'
    })
  };
  showInfoToast = function(){
    'use strict';
    resetToastPosition();
    $.toast({
      heading: 'Info',
      text: 'And these were just the basic demos! Scroll down to check further details on how to customize the output.',
      showHideTransition: 'slide',
      icon: 'info',
      bgColor: '#00b8d4',
      loaderBg: '#00c853',
      position: 'top-right'
    })
  };
  showWarningToast = function(){
    'use strict';
    resetToastPosition();
    $.toast({
      heading: 'Warning',
      text: 'And these were just the basic demos! Scroll down to check further details on how to customize the output.',
      showHideTransition: 'slide',
      icon: 'warning',
      bgColor: '#ff6d00',
      loaderBg: '#00b8d4',
      position: 'top-right'
    })
  };
  showDangerToast = function(){
    'use strict';
    resetToastPosition();
    $.toast({
      heading: 'Danger',
      text: 'And these were just the basic demos! Scroll down to check further details on how to customize the output.',
      showHideTransition: 'slide',
      icon: 'error',
      bgColor: '#d50000',
      loaderBg: '#ff6d00',
      position: 'top-right'
    })
  };
  showToastPosition = function(position) {
    'use strict';
    resetToastPosition();
    $.toast({
      heading: 'Positioning',
      text: 'Specify the custom position object or use one of the predefined ones',
      position: String(position),
      icon: 'info',
      stack: false,
      bgColor: '#3f51b5',
      loaderBg: '#ff4081'
    })
  }
  showToastInCustomPosition = function() {
    'use strict';
    resetToastPosition();
    $.toast({
      heading: 'Custom positioning',
      text: 'Specify the custom position object or use one of the predefined ones',
      icon: 'info',
      position: {
        left: 120,
        top: 120
      },
      stack: false,
      bgColor: '#3f51b5',
      loaderBg: '#ff4081'
    })
  }
  resetToastPosition = function() {
    $('.jq-toast-wrap').removeClass('bottom-left bottom-right top-left top-right mid-center'); // to remove previous position class
    $(".jq-toast-wrap").css({"top": "", "left": "", "bottom":"", "right": ""}); //to remove previous position style
  }
})(jQuery);
