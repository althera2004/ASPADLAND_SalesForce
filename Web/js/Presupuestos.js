var actoDescuento = null;
var actosPresupuestoActual = [];
var presupuestos = [];
var nomatchValue = "";
var lastEspecialidad = null;
var PresupuestoOriginalId = "";
var presupuestoDiscardId = "";
var presupuestoRealizadoId = "";
var presupuestoSelected = null;
var code = "";

function FillComboEspecialidades() {
    return false;
    /*$("#CmbEspecialidad").html("");

    var defaultOption = document.createElement("option");
    defaultOption.value = "";
    defaultOption.appendChild(document.createTextNode("Seleccionar..."));
    document.getElementById("CmbEspecialidad").appendChild(defaultOption);

    for (var x = 0; x < precios.length; x++) {
        var option = document.createElement("option");
        option.value = precios[x].Id;
        option.appendChild(document.createTextNode(precios[x].Name));
        document.getElementById("CmbEspecialidad").appendChild(option);
    }*/
}

function AddObservaciones(sender) {
    var id = sender.parentNode.parentNode.id;
    $("#ActoSelected").val(id);
    var comment = "";
    if ($("#TRCC" + id.split('TR')[1]).length > 0) {
        comment = $("#TRCC" + id.split('TR')[1]).html();
    }
	
	var recordId = id.split('_')[1];

    $("#TxtActoObservaciones").val(comment);
    var dialog = $("#popupActoObservaciones").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Presupuesto_AddObservacionesPopupTitle,
        "width": 500,
        "buttons": [
            {
                "id": "BtnActoAdd",
                "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    AddObservacionesConfirmed(recordId);
                    $(this).dialog("close");
                }
            },
            {
                "id": "BtnActoCancel",
                "html": "<i class=\"fa fa-times bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function AddObservacionesConfirmed(recordId) {
	recordId = recordId * 1;
    var id = $("#ActoSelected").val().split('TR')[1];
	console.log("AddObservacionesConfirmed", id);
    for (var x = 0; x < actosPresupuestoActual.length; x++) {
        //if (actosPresupuestoActual[x].Id === id) {
		if (actosPresupuestoActual[x].timeStamp === recordId) {
            actosPresupuestoActual[x].Observaciones = $("#TxtActoObservaciones").val();
            $("#TRC" + id).remove();
            $("#" + $("#ActoSelected").val()).after("<tr id=\"TRC" + id + "\"><td align=\"right\">Observaciones:</td><td id=\"TRCC" + id + "\" colspan=\"3\">" + $("#TxtActoObservaciones").val() + "</td><td></td></tr>");
        }
    }
}

function DescartarPresupuesto(id, codigo) {
    presupuestoDiscardId = id;
    $("#PresupuestoDescartadoCodigo").html(codigo);
    var dialog = $("#popupDescartar").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Presupuesto_DiscardPopupTitle,
        "width": 500,
        "buttons": [
            {
                "id": "BtnActoAdd",
                "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Discard,
                "class": "btn btn-danger btn-xs",
                "click": function () {
                    DescartarPresupuestoConfirmed();
                    $(this).dialog("close");
                }
            },
            {
                "id": "BtnActoCancel",
                "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function DescartarPresupuestoConfirmed() {
    var data = {
        "presupuestoId": presupuestoDiscardId
    };
    console.log("Descartar", presupuestoDiscardId);
    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/DiscardPresupuesto",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            popupNoASPAD
            console.log(msg);
            if (msg.d.Success === true) {
                alertUI(Dictionary.Item_Presupuesto_DiscardOkMessage);
                $("#Panel-" + presupuestoDiscardId).hide();
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function RealizarPresupuesto(id) {
    presupuestoRealizadoId = id;
    if (presupuestoSelected !== null) {
        presupuestoRealizadoId = presupuestoSelected;
    }
    var data = {
        //"presupuestoId": presupuestoRealizadoId,
        "presupuestoId": id,
        "centroId": ApplicationUser.Id,
        "fecha": GetDate($("#Fecha").val(), "/")
    };
    console.log("Descartar", presupuestoRealizadoId);
    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/RealizarPresupuesto",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            if (msg.d.Success === true) {
                alertInfoUI(Dictionary.Item_Presupuesto_RealizadoOkMessage, RealizarPresupuestoCallBack);
                $("#Panel-" + presupuestoRealizadoId).hide();
            }
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function RecuperarPresupuesto(id) {
    PresupuestoOriginalId = id;
    presupuestoSelected = id;
    $("#TabBasic a").click();
    $("#ListActosPresupuesto").html("");
    $("#TotalRecords").html("0");
    actosPresupuestoActual = [];
    var count = 0;
    for (var x = 0; x < presupuestos.length; x++) {
        var p = presupuestos[x];
        code = p.Description;
        if (p.PresupuestoId === id) {
            if (typeof p.PresupuestoOriginalId !== "undefined" && p.PresupuestoOriginalId !== null && p.PresupuestoOriginalId !== "") {
                PresupuestoOriginalId = p.PresupuestoOriginalId;
            }
            count++;
            actoDescuento = {
                "Id": p.ActoId,
                "Name": p.Acto,
                "Amount": p.Amount,
                "Discount": p.Discount,
                "T":"",
                "Observaciones": p.Observaciones,
				"timeStamp": x
            };
            actosPresupuestoActual.push(actoDescuento);
            var precio = "";
            var descuento = "";

            if (actoDescuento.Amount !== null) {
                precio = actoDescuento.Amount + "&nbsp;&euro;"
            }

            if (actoDescuento.Discount !== null) {
                descuento = actoDescuento.Discount + "&nbsp;%";
            }

            var hasObservaciones = typeof p.Observaciones !== "undefined" && p.Observaciones !== null && p.Observaciones !== "";

			var id2 = actoDescuento.Id+"_"+actoDescuento.timeStamp;
			
            var row = "<tr id=\"TR" + id2 + "\">";
            row += "<td style=\"width:300px;\">" + p.Especialidad + "</td>";
            row += "<td>" + actoDescuento.Name + "</td>";
            row += "<td style=\"width:90px;\" align=\"right\">" + precio + "</td>";
            row += "<td style=\"width:90px;\" align=\"right\">" + descuento + "</td>";
            row += "<td style=\"width:89px;\"";
            if (hasObservaciones === true) { row += " rowspan=\"2\""; }
            row += ">";
            row += "<span id=\"" + id2 + "\" title=\"Eliminar\" class=\"btn btn-xs btn-danger\" onclick=\"RemoveActo(this);\"><i class=\"fas fa-times bigger-120\"></i></span>";
            row += "&nbsp;";
            row += "<span id=\"" + id2+ "\" title=\"Observaciones\" class=\"btn btn-xs btn-info\" onclick=\"AddObservaciones(this);\"><i class=\"far fa-comments bigger-120\"></i></span>";
            row += "</td>";
            row += "</tr>";

            if (hasObservaciones === true){
                row += "<tr id=\"TRC" + id2 + "\"><td align=\"right\">Observaciones:</td><td colspan=\"3\">" + p.Observaciones + "</td></tr>";
            }

            $("#ListActosPresupuesto").append(row);
        }

        $("#TotalRecords").html(count);
        $("#BtnPrintAndSave").removeAttr("disabled");
    }
}

function AddNoASPAD() {
    $("#TxtActo").val($(".chosen-search input").val());
    if ($("#TxtActo").val() === "") { return false; }
    $("#TxtAmount").val("");
    var dialog = $("#popupNoASPAD").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Presupuesto_AddActoPopupTitle,
        "width": 500,
        "buttons": [
            {
                "id": "BtnActoAdd",
                "html": "<i class=\"fas fa-save bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    AddNoASPADConfirmed();
                    $(this).dialog("close");
                }
            },
            {
                "id": "BtnActoCancel",
                "html": "<i class=\"fas fa-times bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function AddNoASPADConfirmed() {
    console.log($("#TxtActo").val(), $("#TxtAmount").val());
    var acto = {
        "Id": guid(),
        "Name": $("#TxtActo").val(),
        "Amount": $("#TxtAmount").val().split(",").join("."),
        "Discount": null,
        "T": Dictionary.Item_Presupuesto_OwnAmount,
        "Observaciones": ""
    };

    actosPresupuestoActual.push(acto);
    RenderActoRow(acto, Dictionary.Common_Undefined_Female, null, null, true);
    var total = $("#TotalRecords").html() * 1;
    $("#TotalRecords").html(total + 1);
}

function AddActoDescuento(acto) {
    $("#TxtActoDescuento").val(acto.Name);
    $("#TxtAmountDescuento").val("");
    actoDescuento = acto;
    var dialog = $("#popupActoDescuento").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Item_Presupuesto_ActoDescuento_PopupTitle,
        "width": 500,
        "buttons": [
            {
                "id": "BtnActoDescuentoAdd",
                "html": "<i class=\"fa fas-check bigger-110\"></i>&nbsp;" + Dictionary.Common_Add,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    actoDescuento.Amount = $("#TxtAmountDescuento").val() * 1;
                    AddDescuentoConfirmed();
                    $(this).dialog("close");
                }
            },
            {
                "id": "BtnActoDescuentoCancel",
                "html": "<i class=\"fas fa-times bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function AddDescuentoConfirmed() {
    for (var x = 0; x < precios.length; x++) {
        var actos = precios[x].Actos;
        for (var y = 0; y < actos.length; y++) {
            if (actoDescuento.Id === actos[y].Id) {
                actoDescuento.Observaciones = "";
                actosPresupuestoActual.push(actoDescuento);
                RenderActoRow(actoDescuento, precios[x].Name, null, null, true);

                var total = $("#TotalRecords").html() * 1;
                $("#TotalRecords").html(total + 1);
                break;
            }
        }
    }

    actoDescuento = null;
}

function CmbEspecialidadChanged() {
    var especialidadId = $("#CmbEspecialidad").val();
    console.log("CmbEspecialidadChanged", especialidadId + " :: " + $(".chosen-search input").val());
    if (especialidadId === "") {
        $("#BtnAddActo").attr("disabled", "disabled");
        AddNoASPAD();
    }
    else {
        $("#BtnAddActo").removeAttr("disabled");
        $("#BtnAddActo").click();
    }
    return false;
}

function CmbActoChanged() {
    console.log("CmbActoChanged");
    var actoId = $("#CmbActo").val();
    if (actoId === "") {
        $("#BtnAddActo").attr("disabled", "disabled");
    }
    else {
        $("#BtnAddActo").removeAttr("disabled");
    }
}

function AddActo(sender) {
	console.log("AddActo", sender);
    var actoId = $("#CmbEspecialidad").val();
	// Permitir duplicar acto
    if (sender.timeStamp - lastEspecialidad < 1000) {
        return false;
    }

    lastEspecialidad = sender.timeStamp;
    for (var x = 0; x < precios.length; x++) {
        var actos = precios[x].Actos;
        for (var y = 0; y < actos.length; y++) {
            if (actos[y].Id === actoId) {
                if (actos[y].Discount !== null) {
                    AddActoDescuento(actos[y]);
                    return false;
                }

                actos[y].Observaciones = "";
				actos[y].timeStamp = lastEspecialidad;
                actosPresupuestoActual.push(actos[y]);

                RenderActoRow(actos[y], precios[x].Name, null, null, true, lastEspecialidad);

                var total = $("#TotalRecords").html() * 1;
                $("#TotalRecords").html(total + 1);
                return;
            }
        }
    }
}

function RenderActoRow(acto, especialidad, precio, descuento, newRow, timeStamp) {
    precio = "";
    descuento = "";

    if (acto.Amount !== null) {
        precio = acto.Amount + "&nbsp;&euro;"
    }

    if (acto.Discount !== null) {
        descuento = acto.Discount + "&nbsp;%";
    }
	
	var id = acto.Id + "_" + timeStamp
	
    var row = "<tr id=\"TR" + id + "\">";
    row += "<td style=\"width:300px;\">" + especialidad + "</td>";
    row += "<td>" + acto.Name + "</td>";
    row += "<td style=\"width:90px;\" align=\"right\">" + precio + "</td>";
    row += "<td style=\"width:90px;\" align=\"right\">" + descuento + "</td>";
    row += "<td style=\"width:90px;\">";
    row += "<span id=\"" + id + "\" title=\"Eliminar\" class=\"btn btn-xs btn-danger\" onclick=\"RemoveActo(this);\"><i class=\"fas fa-times bigger-120\"></i></span>";
    row += "&nbsp;";
    row += "<span id=\"" + id + "\" title=\"Observaciones\" class=\"btn btn-xs btn-info\" onclick=\"AddObservaciones(this);\"><i class=\"far fa-comments bigger-120\"></i></span>";
    row += "</td>";
    row += "</tr>";
	console.log("newRow", newRow);
	if(typeof newRow !== "undefined" && newRow !== null && newRow === true){ 
		$("#ListActosPresupuesto").prepend(row);
	}
	else {		
		$("#ListActosPresupuesto").append(row);
	}
}

function RemoveActo(sender) {
    var id = sender.id;
    $("#TR" + id).remove();
    $("#TRC" + id).remove();

    var found = false;
    var temp = [];
	var actoId = id.split('_')[0];
    for (var x = 0; x < actosPresupuestoActual.length; x++) {
        if (actosPresupuestoActual[x].Id + "_" + actosPresupuestoActual[x].timeStamp === id && found === false) {
            found = true;
        }
        else {
            temp.push(actosPresupuestoActual[x]);
        }
    }

    actosPresupuestoActual = temp;
    var total = $("#TotalRecords").html() * 1;
    $("#TotalRecords").html(total - 1);
}

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

window.onload = function () {
    $("h1").html($("#DataAseguradoName").html());
    $("#HeaderButtons").html($("#DataColectivo").html());
    FillComboEspecialidades();
    $("#BtnAddActo").attr("disabled", "disabled");
    $("#BtnAddActo").on("click", AddActo);
    $("#BtnPrint").on("click", Print);
    $("#BtnPrintAndSave").on("click", SaveAndRealizado);
    Resize();
    GetPresupuestos();
    $("#CmbEspecialidad").chosen().on("chosen:hiding_dropdown", function () {
        nomatchValue = $(".chosen-search input").val();
        CmbEspecialidadChanged();
    }).on("chosen:showing_dropdown", function () {
        $("#CmbEspecialidad").val("");
        });
		
	$("#chosen-container").css("width","99%!important");
    var date = new Date();
    var options = $.extend({}, $.datepicker.regional["es"], { "autoopen": false, "autoclose": true, "todayHighlight": true, "minDate": date.addDays(-15), "maxDate": FormatDate(new Date(), "/") });
    $(".date-picker").datepicker(options);
    $(".date-picker").attr("readonly", "readonly");

    $("#FechaTxt").on("click", function () {
        $("#FechaTxt").hide();
        $("#Fecha").show();
    });

    if (chip === "") {
        $("#rowWarningChip").show();
    }

}

window.onresize = function () {
    Resize();
}

function Resize() {
    var delta = (chip === "") ? 40 : 0;
    var containerHeight = $(window).height();
    $("#ListActosPresupuestoDiv").height(containerHeight - 650 - delta);
    $("#ListActosPresupuestoDivActosRealizados").height(containerHeight - 450 - delta);
}

function Print() {
    if (actosPresupuestoActual.length === 0) {
        warningInfoUI(Dictionary.Item_Presupuesto_NoActosErrorMessage);
        return false;
    }

    var actoAspad = false;
    for (var x = 0; x < actosPresupuestoActual.length; x++) {
        if (actosPresupuestoActual[x].T !== "Acto no baremado") {
            actoAspad = true;
            break;
        }
    }

    if (actoAspad=== false) {
        warningInfoUI(Dictionary.Item_Presupuesto_NoActosASPADErrorMessage);
        return false;
    }

    console.log(data);
	
	if(PresupuestoOriginalId === ""){
		PresupuestoOriginalId = "00000000-0000-0000-0000-000000000000";
	}

    var data = {
        "centroId": ApplicationUser.Id,
        "presupuestoOriginalId": PresupuestoOriginalId,
        "polizaId": polizaId,
        "mascotaId": mascotaId,
        "data": JSON.stringify(actosPresupuestoActual),
        "presupuestoObservaciones": $("#TxtObservaciones").val(),
        "status": 0,
        "code": code
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/SavePresupuesto",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            if (msg.d.Success === true) {
                alertUI(Dictionary.Item_Presupuesto_SavedOkMessage);
            }

            GetPresupuestos();
            window.open("PrintPresupuesto.aspx?presupuestoId=" + msg.d.ReturnValue + "&centroId=" + ApplicationUser.Id);
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function PrintAndSave() {
    if (actosPresupuestoActual.length === 0) {
        warningInfoUI(Dictionary.Item_Presupuesto_NoActosErrorMessage);
        return false;
    }

    var data = {
        "centroId": ApplicationUser.Id,
        "polizaId": polizaId,
        "mascotaId": mascotaId,
        "data": JSON.stringify(actosPresupuestoActual),
        "presupuestoObservaciones": $("#TxtObservaciones").val(),
        "status": 1
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/SavePresupuesto",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            RealizarPresupuesto(msg.d.ReturnValue);
            window.open("PrintPresupuesto.aspx?presupuestoId=" + msg.d.ReturnValue + "&centroId=" + ApplicationUser.Id);            
            

            
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function SaveAndRealizado() {
    if (actosPresupuestoActual.length === 0) {
        warningInfoUI(Dictionary.Item_Presupuesto_NoActosErrorMessage);
        return false;
    }

    // Por defecto aparece la fecha actual
    $("#Fecha").val(FormatDate(new Date(), "/"));
    $("#FechaTxt").val(FormatDate(new Date(), "/"));

    $("#Fecha").hide();
    $("#FechaTxT").show();

    $("#TxtActoObservaciones").val("");
    var dialog = $("#popupFechaRealizado").removeClass("hide").dialog({
        "resizable": false,
        "modal": true,
        "title": Dictionary.Presupuesto_Popup_Date,
        "width": 500,
        "buttons": [
            {
                "id": "BtnRealizarPresupuestoOk",
                "html": "<i class=\"fa fa-check bigger-110\"></i>&nbsp;" + Dictionary.Presupuesto_Btn_MarcarRealizado,
                "class": "btn btn-success btn-xs",
                "click": function () {
                    SaveAndRealizadoConfirmed();
                    $(this).dialog("close");
                }
            },
            {
                "id": "BtnRealizarPresupuestoCancel",
                "html": "<i class=\"fa fa-times bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                "class": "btn btn-xs",
                "click": function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
    $("#BtnRealizarPresupuestoCancel").focus();
}

function SaveAndRealizadoConfirmed() {
	
	if(PresupuestoOriginalId === ""){
		PresupuestoOriginalId = "00000000-0000-0000-0000-000000000000";
	}
	
    var data = {
        "centroId": ApplicationUser.Id,
        "polizaId": polizaId,
        "mascotaId": mascotaId,
        "data": JSON.stringify(actosPresupuestoActual),
        "presupuestoObservaciones": $("#TxtObservaciones").val(),
        "status": 1,
        "code": code,
		"presupuestoOriginalId": PresupuestoOriginalId
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/SavePresupuesto",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            RealizarPresupuesto(msg.d.ReturnValue);
        },
        "error": function (msg) {
            alertUI(msg.responseText);
        }
    });
}

function GetPresupuestos() {
    var data = {
        "centroId": ApplicationUser.Id,
        "mascotaId": mascotaId
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/GetPresupuestosByMascota",
        "data": JSON.stringify(data),
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "async": true,
        "success": function (msg) {
            console.log(msg);
            eval("presupuestos=" + msg.d + ";");
            RenderPresupuestos();
        },
        "error": function (msg, text) { }
    });
}

function RenderPresupuestos() {
    var actualId = "";
    var actualCode = "";
    var res = "";
    for (var x = 0; x < presupuestos.length; x++) {
        var p = presupuestos[x];
        if (p.PresupuestoId !== actualId) {

            if (x > 0) {
                res += "<div class=\"col col-xs-12\" style=\"font-size:12px;text-align:right;\">";
                res += "<button class=\"btn btn-success\" type=\"button\" id=\"BtnRealizar\" onclick=\"RecuperarPresupuesto('" + actualId + "','" + actualCode + "');\"><i class=\"fas fa-save bigger-110\"></i>&nbsp;" + Dictionary.Item_Presupuesto_Get + "</button>&nbsp;";
                res += "<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"window.open('PrintPresupuesto.aspx?presupuestoId=" + actualId + "&centroId=" + ApplicationUser.Id + "');\"><i class=\"fas fa-print bigger-110\"></i>&nbsp;" + Dictionary.Item_Presupuesto_Btn_Print + "</button>&nbsp;&nbsp;&nbsp;";
                res += "<button class=\"btn btn-danger\" type=\"button\" id=\"BtnEliminar\" onclick=\"DescartarPresupuesto('" + actualId + "','" + actualCode + "');\"><i class=\"fas fa-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Discard + "</button></div>";
                res += "</div></div></div>";
            }

            actualId = p.PresupuestoId;
            actualCode = p.Description;

            res += "<div class=\"panel panel-default\" id=\"Panel-" + p.PresupuestoId + "\"><div class=\"panel-heading\">";
            res += "<h4 class=\"panel-title\">";
            res += "<a class=\"accordion-toggle collapsed\" data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#e" + p.PresupuestoId + "\">";
            res += "<i class=\"ace-icon fa fa-angle-down bigger-110\" data-icon-hide=\"ace-icon fa fa-angle-down\" data-icon-show=\"ace-icon fa fa-angle-right\"></i>";
            res += "&nbsp;" + Dictionary.Item_Presupuesto_Title_Presupuesto + ":&nbsp;<strong>" + p.Description + "</strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Fecha:&nbsp;<strong>" + p.F + "</strong>";
            res += "</a></h4></div><div class=\"panel-collapse collapse\" id=\"e" + p.PresupuestoId + "\">";
            res += "<div class=\"row\" style=\"padding:8px;\">";
            res += "<div class=\"col col-xs-3\" style=\"font-size:12px;\"><strong>Especialidad</strong></div>";
            res += "<div class=\"col col-xs-4\" style=\"font-size:12px;\"><strong>Acto</strong></div>";
            res += "<div class=\"col col-xs-2\" style=\"font-size:12px;text-align:right;\"><strong>Importe</strong></div>";
            res += "<div class=\"col col-xs-2\" style=\"font-size:12px;text-align:right;\"><strong>Descuento</strong></div>";
            res += "<div class=\"col col-xs-1\" style=\"font-size:12px;text-align:right;\">&nbsp;</div>";            
            res += "</div>";
        }

        res += "<div class=\"row\" style=\"padding:8px;\">";
        res += "<div class=\"col col-xs-3\" style=\"font-size:12px;\">" + p.Especialidad + "</div>";
        res += "<div class=\"col col-xs-4\" style=\"font-size:12px;\">" + p.Acto + "</div>";
        res += "<div class=\"col col-xs-2\" style=\"font-size:12px;text-align:right;\">" + (p.Amount === null ? "" : p.Amount + "&nbsp;&euro;") + "</div>";
        res += "<div class=\"col col-xs-2\" style=\"font-size:12px;text-align:right;\">" + (p.Discount === null ? "" : p.Discount + "%") + "</div>";
        res += "<div class=\"col col-xs-1\" style=\"font-size:12px;text-align:right;\">&nbsp;</div>";
        res += "</div>";
    }

    if (x > 0) {
        res += "<div class=\"col col-xs-12\" style=\"font-size:12px;text-align:right;\">";
        res += "<button class=\"btn btn-success\" type=\"button\" id=\"BtnRealizar\" onclick=\"RecuperarPresupuesto('" + actualId + "','" + actualCode + "');\"><i class=\"fas fa-save bigger-110\"></i>&nbsp;" + Dictionary.Item_Presupuesto_Get + "</button>&nbsp;";
        res += "<button class=\"btn btn-info\" type=\"button\" id=\"BtnExportList\" onclick=\"window.open('PrintPresupuesto.aspx?presupuestoId=" + actualId + "&centroId=" + ApplicationUser.Id + "');\"><i class=\"fas fa-print bigger-110\"></i>&nbsp;" + Dictionary.Item_Presupuesto_Btn_Print + "</button>&nbsp;&nbsp;&nbsp;";
        res += "<button class=\"btn btn-danger\" type=\"button\" id=\"BtnEliminar\" onclick=\"DescartarPresupuesto('" + actualId + "','" + actualCode + "');\"><i class=\"fas fa-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Discard + "</button></div>";
        res += "</div></div></div>";
    }

    $("#accordion").html(res);
}

function GoDashBoard() {
    document.location = "DashBoard.aspx";
}

function RealizarPresupuestoCallBack() {
    /*$("#BtnPrintAndSave").attr("disabled", "disabled");
    GetPresupuestos();*/
	document.location = document.location + "";
}