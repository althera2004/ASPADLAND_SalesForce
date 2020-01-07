window.onload = function () {
    console.log("Trazas.js", "loaded");
    RenderTableTrazas();
    Resize();
}

window.onresize = function () {
    Resize();
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDiv").height(containerHeight - 390);
    $("#ListDataDivSinPresupuesto").height(containerHeight - 390);
    $("#ListDataDivDescartados").height(containerHeight - 390);
}

function RenderTableTrazas() {
    $("#TableResults").html("");
    var cont = 0;
    var tipoActual = 0;
    var datoActual = "";
    for (var x = 0; x < trazas.length; x++) {
        var tr = document.createElement("TR");
        var tdEvent = document.createElement("TD");
        var tdCentro = document.createElement("TD");
        var tdAsegurado = document.createElement("TD");
        var tdPoliza = document.createElement("TD");
        var tdChip = document.createElement("TD");
        var tdFecha = document.createElement("TD");
        var tdDatos = document.createElement("TD");

        var centroName = "";
        if (trazas[x].C !== null) {
            centroName = trazas[x].C.N;
        }

        var evento = trazas[x].T;
        switch (trazas[x].T.toString()) {
            case "1": evento = "Prsto. guardado"; break;
            case "4": evento = "Prsto. descartado"; break;
            case "5": evento = "Prsto. realizado"; break;
            case "6": evento = "Reset password"; break;
            case "7": evento = "login"; break;
            case "8": evento = "Login fallido"; break;
            case "9": evento = "Búsqueda"; break;
            case "10": evento = "Actualizar mascota"; break;
        }

        var datos = trazas[x].B;

        if (trazas[x].P !== null) {
            datos = "Presupuesto: " + trazas[x].P.C;
        }

		
        if (datos !== datoActual) {// && trazas[x].T !== tipoActual) {			
			var divCentro = document.createElement("DIV");
			// style=""width:150px;margin:0;padding:0;white-space:nowrap;overflow:hidden;text-overflow: ellipsis;""
			divCentro.style.margin = "0";
			divCentro.style.width = "230px";
			divCentro.style.whiteSpace = "nowrap";
			divCentro.style.overflow = "hidden";
			divCentro.style.textOverflow = "ellipsis";
			divCentro.appendChild(document.createTextNode(centroName));
            divCentro.title = centroName;

            var divAsegurado = document.createElement("DIV");
            // style=""width:150px;margin:0;padding:0;white-space:nowrap;overflow:hidden;text-overflow: ellipsis;""
            divAsegurado.style.margin = "0";
            divAsegurado.style.width = "180px";
            divAsegurado.style.whiteSpace = "nowrap";
            divAsegurado.style.overflow = "hidden";
            divAsegurado.style.textOverflow = "ellipsis";
            divAsegurado.appendChild(document.createTextNode(trazas[x].A));
            divAsegurado.title = trazas[x].A;

            var divPoliza = document.createElement("DIV");
            // style=""width:150px;margin:0;padding:0;white-space:nowrap;overflow:hidden;text-overflow: ellipsis;""
            divPoliza.style.margin = "0";
            divPoliza.style.width = "180px";
            divPoliza.style.whiteSpace = "nowrap";
            divPoliza.style.overflow = "hidden";
            divPoliza.style.textOverflow = "ellipsis";
            divPoliza.appendChild(document.createTextNode(trazas[x].PL));
            divPoliza.appendChild(document.createElement("br"));
            divPoliza.appendChild(document.createTextNode(trazas[x].co));
            divPoliza.title = trazas[x].PL;

            var divChip = document.createElement("DIV");
            // style=""width:150px;margin:0;padding:0;white-space:nowrap;overflow:hidden;text-overflow: ellipsis;""
            divChip.style.margin = "0";
            divChip.style.width = "110px";
            divChip.style.whiteSpace = "nowrap";
            divChip.style.overflow = "hidden";
            divChip.style.textOverflow = "ellipsis";
            divChip.appendChild(document.createTextNode(trazas[x].ch));
            divChip.title = centroName;

            if (trazas[x].M !== "") { datos += "/ " + trazas[x].M; }
            if (trazas[x].S !== "") {
                if (trazas[x].S === "100000000") { datos += "/ macho"; }
                if (trazas[x].S === "100000001") { datos += "/ hembra"; }
            }
            if (trazas[x].Ty !== "") {
                if (trazas[x].Ty === "100000000") { datos += "/ perro"; }
                if (trazas[x].Ty === "100000001")  { datos += "/ gato"; }
            }

            tdEvent.appendChild(document.createTextNode(evento));
            tdCentro.appendChild(divCentro);
            tdAsegurado.appendChild(divAsegurado);
            tdPoliza.appendChild(divPoliza);
            tdChip.appendChild(divChip);
			tdFecha.appendChild(document.createTextNode(trazas[x].D.substring(0,10)));
			tdDatos.appendChild(document.createTextNode(datos));

            tdEvent.style.width = "170px";
            tdCentro.style.width = "250px";
            tdAsegurado.style.width = "200px";
            tdPoliza.style.width = "200px";
            tdChip.style.width = "130px";
			tdFecha.style.width = "80px";

            //tr.appendChild(tdEvent);
            tr.appendChild(tdCentro);
            tr.appendChild(tdAsegurado);
            tr.appendChild(tdPoliza);
            tr.appendChild(tdChip);
			tr.appendChild(tdFecha);
			tr.appendChild(tdDatos);
			if(trazas[x].T.toString()=== "1")
			{
				document.getElementById("TableResults").appendChild(tr);
			}

            datoActual = datos;
            tipoActual = trazas[x].T;
			cont++;			
		}
    }

    $("#TotalRecords").html(cont);
}

function ExcelDescartados() {
    $.ajax({
        "type": "POST",
        "url": "/AdminTrazas.aspx/DescartadosExcel",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({}, null, 2),
        "success": function (msg) {
            window.open(msg.d.ReturnValue);
        },
        "error": function (msg) {
            alertUI("error:" + msg.responseText);
        }
    });
}

function ExcelSinPresupuesto() {
    $.ajax({
        "type": "POST",
        "url": "/AdminTrazas.aspx/SinPresupuestoExcel",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({}, null, 2),
        "success": function (msg) {
            window.open(msg.d.ReturnValue);
        },
        "error": function (msg) {
            alertUI("error:" + msg.responseText);
        }
    });
}

function ExcelPendientes() {
    $.ajax({
        "type": "POST",
        "url": "/AdminTrazas.aspx/PendientesExcel",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({}, null, 2),
        "success": function (msg) {
            window.open(msg.d.ReturnValue);
        },
        "error": function (msg) {
            alertUI("error:" + msg.responseText);
        }
    });
}