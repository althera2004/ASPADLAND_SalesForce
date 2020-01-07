window.onload = function () {
    Resize();
}

window.onresize = function () {
    Resize();
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDivCentro").height(containerHeight - 350);
}