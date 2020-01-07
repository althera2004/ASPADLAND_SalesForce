window.onload = function () {
    if (colectivoId !== "") {
        $("#validationForm").show();
    }
    FillComboTipo();
    $("#BtnSearch").on("click", Search);
    Resize();

    for (var x = 0; x < Colectivos.length; x++) {
        if (Colectivos[x].Id === colectivoId) {
            $("H1").html("Validación de usuario de <strong>" + Colectivos[x].Description + "</strong>");
        }
    }

    RenderTable();
    $("#BtnAddValidacion").on("click", AddValidacion);

    $("#TxtDNI").on("blur", VerificarNIF);
    $("#TxtPoliza").on("blur", VerificarPoliza);
}

window.onresize = function () {
    Resize();
}

function Resize() {
    var containerHeight = $(window).height();
    var resto = colectivoId === "" ? 350 : 470;
    $("#ListDataDivCentro").height(containerHeight - resto);
}

function FillComboTipo() {
    var optionDefault = document.createElement("OPTION");
    optionDefault.value = "";
    optionDefault.appendChild(document.createTextNode("Seleccionar..."));
    document.getElementById("CmbColectivo").appendChild(optionDefault);
    for (var f = 0; f < Colectivos.length; f++) {
        var option = document.createElement("OPTION");
        option.value = Colectivos[f].Id;
        option.appendChild(document.createTextNode(Colectivos[f].Description));

        if (Colectivos[f].Id === colectivoId) {
            option.selected = true;
        }

        document.getElementById("CmbColectivo").appendChild(option);
    }

    $("#CmbColectivo").trigger("chosen:updated");
    $("#CmbColectivo").chosen({ placeholder: 'Seleccionar colectivo' });
}

function Search() {
    $("#ErrorMessage").html("");
    var nif = $("#TxtDNI").val();
    var poliza = $("#TxtPoliza").val();
    var nombre = $("#TxtNombre").val();
    var colectivo = $("#CmbColectivo").val();

    if (nif === "" && poliza === "" && nombre === "") {
        $("#ErrorMessage").html("<i class=\"fas fa-warning-sign\"></i>&nbsp;Es necesario algún criterio de búsqueda");
        return false;
    }

    $("#BtnSearch").html("<i class=\"fas fa-cog fa-spain\"></i>&nbsp;Buscando...");
    $("#BtnSearch").attr("disabled", "disabled");

    var data =
        {
            "nif": nif,
            "poliza": poliza,
            "nombre": nombre,
            "colectivo": colectivo
        };

    console.log("data", data);

    $.ajax({
        "type": "POST",
        "url": "/BusquedaUsuarios.aspx/BuscarAsegurado",
        "data": JSON.stringify(data),
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "async": true,
        "success": function (msg) {
            $("#BtnSearch").html("<i class=\"fas fa-search bigger-110\"></i>&nbsp;" + Dictionary.Common_Search);
            $("#BtnSearch").removeAttr("disabled");
            var list = [];
            eval("list=" + msg.d + ";");
            console.log(list);
            $("#TotalRecords").html(list.length);
            $("#TableResults").html("");

            if (list.length === 0) {
                $("#TableResults").html("<tr><td>" + Dictionary.Common_ListNoData + "</tr></td>");
            }
            else {
                for (var x = 0; x < list.length; x++) {
                    RenderRow(list[x]);
                }
            }
        },
        "error": function (msg, text) {
            console.log(msg);
            $("#BtnSearch").html("<i class=\"fas fa-search bigger-110\"></i>&nbsp;" + Dictionary.Common_Search);
            $("#BtnSearch").removeAttr("disabled");
            $("#TotalRecords").html(0);
            $("#TableResults").html("<tr><td>" + msg.responseJSON.Message + "</tr></td>");
        }
    });
}

function RenderTable() {
    var total = 0;
    for (var x = 0; x < Colas.length; x++) {
        RenderRow(Colas[x]);
        total++;
    }

    $("#TotalRecords").html(total);
}

function RenderRow(data) {
    var tr = document.createElement("TR");
    var statusText = Dictionary.Item_Validaciones_Status_Pendiente;
    var background = "#bbb";
    var icon = "fas fa-question";
    if (data.Status === "1") {
        statusText = Dictionary.Item_Validaciones_Status_Aprobada;
        background = "#3c3"
        icon = "fas fa-check";
    }
    else if (data.Status === "282310003") {
        statusText = Dictionary.Item_Validaciones_Status_Coincidente;
        background = "#77f";
        icon = "fas fa-check";
    }
    else if (data.Status === "282310000") {
        statusText = Dictionary.Item_Validaciones_Status_Denegada;
        background = "#f77";
        icon = "fas fa-exclamation-triangle";
    }

    if (data.Status === "1" || data.Status === "282310003") {
        tr.onclick = function () { GoBusquedaUsuarios(data.ColectivoId, data.DNI, data.Poliza); };
        tr.style.cursor = "pointer";
    }

    var tdCodigo = document.createElement("TD");
    tdCodigo.appendChild(document.createTextNode(data.Codigo));

    var tdDNI = document.createElement("TD");
    tdDNI.align = "center";
    tdDNI.appendChild(document.createTextNode(data.DNI));

    var tdColectivo = document.createElement("TD");
    tdColectivo.appendChild(document.createTextNode(" " + data.ColectivoName));

    var tdUrgente = document.createElement("TD");
    tdUrgente.align = "center";
    if (data.Urgente === true) {
        var iconUrgente = document.createElement("I");
        iconUrgente.className = "fas fa-check";
        tdUrgente.appendChild(iconUrgente);
    }
    else {
        tdUrgente.appendChild(document.createTextNode(" "));
    }

    var tdEstado = document.createElement("TD");
    var iconstado = document.createElement("I");
    iconstado.className = icon;
    iconstado.style.color = background;
    tdEstado.title = statusText;
    iconstado.style.fontSize = "18px";
    tdEstado.appendChild(iconstado);
    tdEstado.align = "center";

    var tdObervaciones = document.createElement("TD");
    tdObervaciones.appendChild(document.createTextNode(data.Descripcion));

    var fecha = data.Inicio;
    if (data.Fin !== "") {
        fecha += " / " + data.Fin;
    }

    var tdInicio = document.createElement("TD");
    tdInicio.align = "center";
    tdInicio.appendChild(document.createTextNode(fecha));

    var tdAsegurado = document.createElement("TD");
    tdAsegurado.appendChild(document.createTextNode(data.AseguradoName !== "" ? data.AseguradoName : data.Asegurado));

    var tdTelefono = document.createElement("TD");
    tdTelefono.appendChild(document.createTextNode(data.Telefono));

    var tdPoliza = document.createElement("TD");
    tdPoliza.appendChild(document.createTextNode(data.Poliza));

    var tdActions = document.createElement("TD");
    tdActions.appendChild(document.createTextNode(" "));

    tdCodigo.style.width = "75px";
    tdDNI.style.width = "90px";
    tdColectivo.style.width = "140px";
    tdUrgente.style.width = "75px";
    tdEstado.style.width = "30px";
    tdInicio.style.width = "160px";
    tdAsegurado.style.width = "150px";
    tdTelefono.style.width = "100px";
    tdPoliza.style.width = "134px";    

    tr.appendChild(tdEstado);
    tr.appendChild(tdCodigo);
    tr.appendChild(tdDNI);
    tr.appendChild(tdColectivo);
    tr.appendChild(tdUrgente);
    tr.appendChild(tdObervaciones);
    tr.appendChild(tdInicio);
    tr.appendChild(tdAsegurado);
    tr.appendChild(tdTelefono);
    tr.appendChild(tdPoliza);

    document.getElementById("TableResults").appendChild(tr);
}

function AddValidacion() {
    $("#ErrorMessage").hide();
    $("#TxtDNIId").css("color", "#000");
    $("#TxtNombreId").css("color", "#000");
    $("#CmbColevtivoId").css("color", "#000");
    $("#TxtApellido1").css("color", "#000");
    $("#TxtApellido2").css("color", "#000");
    var ok = true;
    if ($("#TxtDNI").val() === "") { ok = false; }
    if ($("#TxtNombre").val() === "") { ok = false; }
    if ($("#TxtApellido1").val() === "") { ok = false; }
    if ($("#TxtApellido2").val() === "") { ok = false; }

    if (ok === false) {
        $("#ErrorMessage").show();
        return false;
    }

    var data = {
        "nombre": $("#TxtNombre").val(),
        "apellido1": $("#TxtApellido1").val(),
        "apellido2": $("#TxtApellido2").val(),
        "dni": $("#TxtDNI").val(),
        "poliza": $("#TxtPoliza").val(),
        "centroId": ApplicationUser.Id,
        "colectivoId": $("#CmbColectivo").val(),
        "urgente": document.getElementById("ChkUrgente").checked
    };
    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/AddValidacion",
        "data": JSON.stringify(data),
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "async": true,
        "success": function (msg) {
            document.location = document.location + "";
        },
        "error": function (msg, text) {
            console.log(msg);
        }
    });
}

function VerificarNIF() {
    $("#ErrorNIFMessage").html("");
    $("#ErrorNIFMessage").hide();
    if ($("#TxtDNI").val() !== "") {
        var nif = $("#TxtDNI").val();
        if (valida_nif_cif_nie(nif) < 1) {
            $("#ErrorNIFMessage").html(Dictionary.Common_Error_NIFInvalid);
            $("#ErrorNIFMessage").show();
        }
    }
}

function VerificarPoliza() {
    $("#ErrorPolizaMessage").html("");
    $("#ErrorPolizaMessage").hide();

    var poliza = $("#TxtPoliza").val();
    if (poliza !== "") {
        if (colectivoId === "65a4fa8b-00fb-e511-a004-0050568a7d4d") {
            if (poliza.length !== 6 || poliza.substr(0, 1) !== "7") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por 7 y tiene 6 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }

        if (colectivoId === "e3622fc4-7575-e411-996c-0050568a7d4d") {
            if (poliza.length !== 8 || poliza.substr(0, 2) !== "01") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por 01 y tiene 8 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }

        if (colectivoId === "39cc70ff-082a-e511-9484-0050568a7d4d") {
            if (poliza.length !== 9) {
                $("#ErrorPolizaMessage").html("La póliza debe tener 9 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }
    }
}