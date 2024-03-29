﻿var startTime = 0;
var end = new Date();
var diff = 0;
var lastTime = new Date();
var timerID = 0;
var sessionControl;
var start = Date.now();
var timePassed = null;

function chrono() {
    end = new Date();
    lastTime = end;
    diff = end - start;
    diff = new Date(diff);
    var cr_msec = diff.getMilliseconds();
    var cr_sec = diff.getSeconds();
    var cr_min = diff.getMinutes();
    timePassed = diff.getMinutes() * 1;
    var cr_hr = diff.getHours() - 1;

    if (cr_min < 10) { cr_min = "0" + cr_min; }
    if (cr_sec < 10) { cr_sec = "0" + cr_sec; }
    if (cr_msec < 10) { cr_msec = "00" + cr_msec; }
    else if (cr_msec < 100) { cr_msec = "0" + cr_msec; }

    if (cr_hr === 0 && cr_min === 0) {
        document.getElementById("chronotime").innerHTML = cr_sec + " segundos";
    }
    else if (cr_hr === 0) {
        document.getElementById("chronotime").innerHTML = cr_min + " minutos " + cr_sec + " segundos";
    }
    else {
        document.getElementById("chronotime").innerHTML = cr_hr + "horas " + cr_min + " minutos";
    }

    if (timePassed > 30) {
        console.log("TimeOut");
        timeoutAlert();
    }

    timerID = setTimeout("chrono()", 300);
}

function SessionRestart() {
    // clearTimeout(sessionControl);
    start = Date.now();
    // sessionControl = setTimeout(function () { document.location = '/LogOut.aspx'; }, timeout * 1000);
}

function SessionTimeout() {
    // sessionControl = setTimeout(function () { document.location = '/LogOut.aspx'; }, timeout * 1000);
}

chrono();
SessionRestart();