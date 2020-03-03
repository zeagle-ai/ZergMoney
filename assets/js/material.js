(function ($) {
  'use strict';

  mdc.autoInit();

  // Ripple for buttons
  var buttons = document.querySelectorAll('.mdc-button');
  for (var i = 0, button; button = buttons[i]; i++) {
    mdc.ripple.MDCRipple.attachTo(button);
  }

  // Focus for textfields
  var textFields = document.querySelectorAll('.mdc-text-field');
  for (var i = 0, textField; textField = textFields[i]; i++) {
    mdc.textField.MDCTextField.attachTo(textField);
  }


  const menuEls = Array.from(document.querySelectorAll('.mdc-menu'));
  menuEls.forEach((menuEl, index) => {
    
    const menu = new mdc.menu.MDCMenu(menuEl);
    const buttonEl = menuEl.parentElement.querySelector('.mdc-menu-button');
    buttonEl.addEventListener('click', () => {
      menu.open = !menu.open;
    })
    menu.setAnchorCorner(mdc.menu.Corner.BOTTOM_LEFT);
    menu.setAnchorElement(buttonEl)
  });


  // Ripple for sliders
  var sliders = document.querySelectorAll('.mdc-slider');
  for (var i = 0, slider; slider = sliders[i]; i++) {
    mdc.ripple.MDCRipple.attachTo(slider);
    var aaa = new mdc.slider.MDCSlider(slider);
  }

  // Tabs
  var tabBars = document.querySelectorAll('.mdc-tab-bar');
  for (var i = 0, tabBar; tabBar = tabBars[i]; i++) {
    var currentTabBar = new mdc.tabBar.MDCTabBar(tabBar);
    currentTabBar.listen('MDCTabBar:activated', function(event) {
      var $this = $(this);
      var contentEls = $this.siblings('.content');
      contentEls.map((index, contentEl) => {
        contentEl.classList.remove('content--active');
      })
      contentEls[event.detail.index].classList.add('content--active');
    });
  }

  /* Progress bar */
  var determinates = document.querySelectorAll('.mdc-linear-progress');
  for (var i = 0, determinate; determinate = determinates[i]; i++) {
    var linearProgress = mdc.linearProgress.MDCLinearProgress.attachTo(determinate);
    linearProgress.progress = 0.5;
    if (determinate.dataset.buffer) {
      linearProgress.buffer = 0.75;
    }
  }

})(jQuery);