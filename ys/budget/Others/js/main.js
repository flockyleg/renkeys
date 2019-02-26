(function($){
	"use strict";

	$(window).load(function() {
		var $container = $('#fh5co-projects-feed'),
		containerWidth = $container.outerWidth();

		$container.masonry({
			itemSelector : '.fh5co-project',
			columnWidth: function( containerWidth ) {
				//if( containerWidth <= 330 ) {    /*陈斌修改为下面的170*/
				//	return 310;
				//} else {
				//	return 330;
			    //}
			    if( containerWidth <= 170 ) {
			    	return 150;
			    } else {
			    	return 170;
			    }
			},

			isAnimated: !Modernizr.csstransitions
		});
	});

})(window.jQuery);