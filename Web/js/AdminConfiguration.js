window.onload = function () {
	console.log("AdminConfiguration.js","loaded");
    Resize();
}

window.onresize = function () {
    Resize();
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDivCentro").height(containerHeight - 350);
}