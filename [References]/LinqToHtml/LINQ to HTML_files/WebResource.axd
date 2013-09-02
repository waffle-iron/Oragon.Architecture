function GoogleEventTigger(id, gap, url) {
	var e = document.getElementById(id);

	var method = function() {
		if (typeof _gaq == "undefined") {
			_gaq = [];
		}

		if (gap) {
			_gaq.push(gap);
		}

		if (url) {
			_gaq.push(["_trackPageview", url]);
		}
	};

	if (e.addEventListener) {
		e.addEventListener("click", method, false);
	}
	else if (e.attachEvent) {
		e.attachEvent("onclick", method);
	}
}
