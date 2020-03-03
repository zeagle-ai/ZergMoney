(function($) {
	'use strict';
	$(function() {
		$('#snackbar-default').on('click', function() {
			var MDCSnackbar = mdc.snackbar.MDCSnackbar;
			var snackbar = new MDCSnackbar(document.getElementById('mdc-default-snackbar'));
			var dataObj = {
				message: 'fdsfdsf',
				actionText: 'Undo',
				actionHandler: function () {
					console.log('my cool function');
				}
			};			
			snackbar.open(dataObj);
		})
		$('#snackbar-rtl').on('click', function() {
			var MDCSnackbar = mdc.snackbar.MDCSnackbar;
			var snackbar = new MDCSnackbar(document.getElementById('mdc-rtl-snackbar'));
			var dataObj = {
				message: 'fdsfdsf',
				actionText: 'Undo',
				actionHandler: function () {
					console.log('my cool function');
				}
			};			
			snackbar.open(dataObj);
		})
		$('#snackbar-aligned-start').on('click', function() {
			var MDCSnackbar = mdc.snackbar.MDCSnackbar;
			var snackbar = new MDCSnackbar(document.getElementById('mdc-align-start-snackbar'));
			var dataObj = {
				message: 'fdsfdsf',
				actionText: 'Undo',
				actionHandler: function () {
					console.log('my cool function');
				}
			};			
			snackbar.open(dataObj);
		})
		$('#snackbar-aligned-start-rtl').on('click', function() {
			var MDCSnackbar = mdc.snackbar.MDCSnackbar;
			var snackbar = new MDCSnackbar(document.getElementById('mdc-rtl-align-start-snackbar'));
			var dataObj = {
				message: 'fdsfdsf',
				actionText: 'Undo',
				actionHandler: function () {
					console.log('my cool function');
				}
			};			
			snackbar.open(dataObj);
		})
	});
})(jQuery);