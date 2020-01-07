window.onload = function () {
    if (colectivoId === "") {
        document.location = "DashBoard.aspx";
    }

    if (colectivoId === "566f49a9-98ef-e211-91bc-0050568a7d4d") {
        $("#TxtPolizaLabel").hide();
        $("#TxtPoliza").hide();
    }

    FillComboTipo();
    $("#BtnSearch").on("click", Search);
    Resize();

    for (var x = 0; x < Colectivos.length; x++) {
        if (Colectivos[x].Id === colectivoId) {
            $("H1").html(Dictionary.Item_BusquedaUsuarios + "&nbsp;<strong>" + Colectivos[x].Description + "</strong>");
            $("H1").parent().removeClass("col-sm-8");
            $("h1").parent().addClass("col-sm-12");
        }
    }

    $("#TxtDNI").on("blur", VerificarNIF);
    $("#TxtPoliza").on("blur", VerificarPoliza);

    $("#TxtDNI").keyup(function (e) {
        if (e.keyCode === 13) {
            $("#BtnSearch").click();
        }
        else {
            TextLockLayout();
        }
    });

    $("#TxtPoliza").keyup(function (e) {
        if (e.keyCode === 13) {
            $("#BtnSearch").click();
        }
        else {
            TextLockLayout();
        }
    });

    document.getElementById("TxtDNI").focus();

    if (polizaId !== "" && aseguradoId !== "") {
        $("#TxtDNI").val(aseguradoId);
        $("#TxtPoliza").val(polizaId);
        $("#BtnSearch").click();
    }
}

function TextLockLayout() {
    if ($("#TxtDNI").val() !== "") {
        $("#TxtPoliza").attr("readonly", "readonly");
    }
    else {
        $("#TxtPoliza").removeAttr("readonly");
    }

    if ($("#TxtPoliza").val() !== "") {
        $("#TxtDNI").attr("readonly", "readonly");
    }
    else {
        $("#TxtDNI").removeAttr("readonly");
    }
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

        // AME
        if (colectivoId === "65a4fa8b-00fb-e511-a004-0050568a7d4d")
        {
            if (poliza.length !== 6 || poliza.substr(0, 1) !== "7") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por “7” y tiene 6 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }

        // CASER
        if (colectivoId === "e3622fc4-7575-e411-996c-0050568a7d4d") {
            if (poliza.length !== 8 || poliza.substr(0, 2) !== "01") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por “01” y tiene 8 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }

        // SEGURCAIXA
        if (colectivoId === "ca622705-9bef-e211-91bc-0050568a7d4d") {
            if (poliza.length !== 8 || poliza.substr(0, 2) !== "64" || poliza.substr(0, 2) !== "79") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por “64” ó “79” y  tiene 8 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }

        // DIVINA PASTORA
        if (colectivoId === "19e2520f-3283-e711-a794-0050568a7d4d") {
            if (poliza.length !== 7 || poliza.substr(0, 1) !== "7") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por “7” y tiene 7 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }

        // ILERA
        if (colectivoId === "9f6350a1-a9bc-e811-a322-0050568a7d4d") {
            if (poliza.length !== 9 || poliza.substr(0, 1) !== "3") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por “3” y tiene 9 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }

        // MEDIPREMIUM
        if (colectivoId === "3ea28326-5229-e511-9484-0050568a7d4d") {
            if (poliza.length !== 11 || poliza.substr(0, 2) !== "20") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por “20” y tiene 11 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }

        // NATIONALE
        if (colectivoId === "7ec70518-0137-e711-bb80-0050568a7d4d") {
            if (poliza.length !== 8 || poliza.substr(0, 2) !== "06") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por “06” y tiene 8 dígitos");
                $("#ErrorPolizaMessage").show();
            }
        }

        // NECTAR
        if (colectivoId === "8b18eb2e-bf3b-e711-bb80-0050568a7d4d") {
            if (poliza.length !== 11 || poliza.substr(0, 1) !== "M") {
                $("#ErrorPolizaMessage").html("La póliza debe empezar por “M” y tiene 11 dígitos ");
                $("#ErrorPolizaMessage").show();
            }
        }
    }
}

window.onresize = function () {
    Resize();
}

function Resize() {
    var containerHeight = $(window).height();
    $("#ListDataDivCentro").height(containerHeight - 460);
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
    $("#CmbColectivo").chosen({ "placeholder": Dictionary.Item_BusquedaUsuarios_CmbColectivoPlaceHolder });
}

function Search() {
	$("#ErrorMessage").html("");
    var nif = $("#TxtDNI").val();
    var poliza = $("#TxtPoliza").val();
    var colectivo = $("#CmbColectivo").val();

    if (nif === "" && poliza === "") {
        $("#ErrorMessage").html("<i class=\"fas fa-exclamation-triangle\"></i>&nbsp;" + Dictionary.ItemBusquedaUsuarioNoCriteria);
        return false;
    }

    $("#BtnSearch").html("<i class=\"fa fa-cog fa-spain\"></i>&nbsp;" + Dictionary.Common_Searching);
    $("#BtnSearch").attr("disabled", "disabled");

    var data =
        {
            "nif": nif,
            "poliza": poliza,
            "nombre": "",
            "colectivo": colectivo
        };

    console.log("data", data);

    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/BuscarAsegurado",
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
            busqueda = list;
            $("#TotalRecords").html(list.length);
            $("#TableResults").html("");

            if (list.length === 0) {
                $("#TableResults").html("<tr><td style=\"font-size:20px;color:#a00;padding:20px;background-color:#f4f4b4\" align=\"center\"><i class=\"fas fa-exclamation-triangle\"></i>&nbsp;No se han encontrado resultados <span style=\"text-decoration:underline;cursor:pointer;\" onclick=\"GoValidation();\">solicitar validación</span></tr></td>");
            }
            else {
                for (var x = 0; x < list.length; x++) {
                    RenderRow(list[x]);
                }
            }
        },
        "error": function (msg, text) {
            console.log(msg);
            $("#BtnSearch").html("<i class=\"fas fa--search bigger-110\"></i>&nbsp;" + Dictionary.Common_Search);
            $("#BtnSearch").removeAttr("disabled");
            $("#TotalRecords").html(0);
            $("#TableResults").html("<tr><td>" + msg.responseJSON.Message + "</tr></td>");
        }
    });
}

function RenderRow(data) {
    var tr = document.createElement("TR");
    tr.id = data.MascotaId;

    var tdAsegurado = document.createElement("TD");
    tdAsegurado.appendChild(document.createTextNode(data.Asegurado));
    tdAsegurado.onclick = function () { GoPresupuesto(this.parentNode.id); }

    var tdDNI = document.createElement("TD");
    tdDNI.appendChild(document.createTextNode(data.DNI));
    tdDNI.onclick = function () { GoPresupuesto(this.parentNode.id); }

    var tdProducto = document.createElement("TD");
    tdProducto.appendChild(document.createTextNode(data.Producto));
    tdProducto.onclick = function () { GoPresupuesto(this.parentNode.id); }

    var tdPoliza = document.createElement("TD");
    tdPoliza.appendChild(document.createTextNode(data.Poliza));
    tdPoliza.onclick = function () { GoPresupuesto(this.parentNode.id); }

    var tdEstado = document.createElement("TD");
    tdEstado.align = "center";
    tdEstado.appendChild(document.createTextNode(data.Estado));
    tdEstado.onclick = function () { GoPresupuesto(this.parentNode.id); }

    var tdNombre = document.createElement("TD");
    tdNombre.appendChild(document.createTextNode(data.Nombre));
    tdNombre.onclick = function () { GoPresupuesto(this.parentNode.id); }

    var tdChip = document.createElement("TD");
    tdChip.appendChild(document.createTextNode(data.Chip));
    tdChip.onclick = function () { GoPresupuesto(this.parentNode.id); }

    var tdAnimal = document.createElement("TD");
    tdAnimal.align = "center";
    tdAnimal.appendChild(document.createTextNode(data.Animal));
    tdAnimal.onclick = function () { GoPresupuesto(this.parentNode.id); }

    var tdMascota = document.createElement("TD");
    tdMascota.appendChild(document.createTextNode(data.Mascota));
    tdMascota.onclick = function () { GoPresupuesto(this.parentNode.id); }

    var tdActions = document.createElement("TD");
    var btnUpdate = document.createElement("SPAN");
    btnUpdate.id = data.MascotaId;
    btnUpdate.title = "Actualizar " + data.Nombre;
    btnUpdate.className = "btn btn-xs btn-info2";
    btnUpdate.onclick = function () { MascotaUpdate(this.id); }
    var iconUpdate = document.createElement("I");
    iconUpdate.className = "fas fa-pencil-alt bigger-120";
    iconUpdate.style.color = "#489e2a";
    btnUpdate.appendChild(iconUpdate);
    tdActions.appendChild(btnUpdate);

    tdActions.appendChild(document.createTextNode(" "));

    var btnAdd = document.createElement("SPAN");
    btnAdd.id = data.MascotaId;
    btnAdd.title = "Crear presupuesto";
    btnAdd.className = "btn btn-xs btn-info2";
    btnAdd.onclick = function () { GoPresupuesto(this.id); }
    var iconAdd = document.createElement("I");
    iconAdd.className = "fas fa-calculator bigger-120";
    iconAdd.style.color = "#489e2a";
    btnAdd.appendChild(iconAdd);
    tdActions.appendChild(btnAdd);

    tdDNI.style.width = "90px";
    tdDNI.align = "center";
    tdProducto.style.width = "150px";
    tdPoliza.style.width = "120px";
    tdPoliza.align = "center";
    tdEstado.style.width = "80px";
    tdChip.style.width = "130px";
    tdAnimal.style.width = "60px";
    tdMascota.style.width = "120px";
    tdActions.style.width = "90px";

    tr.appendChild(tdAsegurado);
    tr.appendChild(tdDNI);
    tr.appendChild(tdProducto);
    tr.appendChild(tdPoliza);
    tr.appendChild(tdChip);
    tr.appendChild(tdAnimal);
    tr.appendChild(tdMascota);
    tr.appendChild(tdActions);
    document.getElementById("TableResults").appendChild(tr);
}

function GoValidation() {
    var dni = $("#TxtDNI").val();
    var pol = $("#TxtPoliza").val();

    var query = ac + "&colectivoId=" + colectivoId + "&userId=" + ApplicationUser.Id + "&dni=" + dni + "&poliza=" + pol;
    query = $.base64.encode(query);
    document.location = "/ValidacionesList.aspx?" + query;
}

var mascotaIdSelected = null;
var actualMicrochip = null;
function MascotaUpdate(id) {
    mascotaIdSelected = id;
    var mascota = GetMascotaMyId(id);
    if (mascota !== null) {
        $("#TxtMascotaGuid").val(id);
        $("#TxtNombre").val(mascota.Nombre);
        $("#TxtChip").val(mascota.Chip);
        actualMicrochip = mascota.Chip;

        if (mascota.Chip !== "") {
            $("#TxtChip").attr("readonly", "readonly");
        } else {
            $("#TxtChip").removeAttr("readonly");
        }
        document.getElementById("RTipo1").checked = false;
        document.getElementById("RSexo1").checked = false;
        document.getElementById("RTipo2").checked = false;
        document.getElementById("RSexo2").checked = false;
        switch (mascota.Animal) {
            case "Perra":
                document.getElementById("RTipo1").checked = true;
                document.getElementById("RSexo1").checked = true;
                break;
            case "Perro":
                document.getElementById("RTipo1").checked = true;
                document.getElementById("RSexo2").checked = true;
                break;
            case "Gata":
                document.getElementById("RTipo2").checked = true;
                document.getElementById("RSexo1").checked = true;
                break;
            case "Gato":
                document.getElementById("RTipo2").checked = true;
                document.getElementById("RSexo2").checked = true;
                break;
        }

        var dialog = $("#popupMascotaUpdate").removeClass("hide").dialog({
            "resizable": false,
            "modal": true,
            "title": Dictionary.Item_BusquedaUsuarios_MascotaUpdate_Title,
            "width": 500,
            "buttons": [
                {
                    "id": "BtnMascotaSave",
                    "html": "<i class=\"fas fa-save bigger-110\"></i>&nbsp;" + Dictionary.Common_Save,
                    "class": "btn btn-success btn-xs",
                    "click": function () {
                        MascotaUpdateConfirmed();
                    }
                },
                {
                    "id": "BtnMascotaCancel",
                    "html": "<i class=\"fas fa-times bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                    "class": "btn btn-xs",
                    "click": function () {
                        $(this).dialog("close");
                    }
                }
            ]
        });
    }
}

function MascotaUpdateConfirmed() {
    var tipo = 0;
    var sexo = 0;

    if (document.getElementById("RTipo1").checked === true) { tipo = 100000000; }
    if (document.getElementById("RTipo2").checked === true) { tipo = 100000001; }
    if (document.getElementById("RSexo2").checked === true) { sexo = 100000000; }
    if (document.getElementById("RSexo1").checked === true) { sexo = 100000001; }

    var data = {
        "id": $("#TxtMascotaGuid").val(),
        "nombre": $("#TxtNombre").val(),
        "chip": $("#TxtChip").val(),
        "tipo": tipo,
        "sexo": sexo,
        "actualChip": actualMicrochip
    };

    console.log("data", data);

    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/UpdateMascota",
        "data": JSON.stringify(data),
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "async": true,
        "success": function (msg) {
            $("#BtnMascotaCancel").click();
            Search();
        },
        "error": function (msg, text) {
            console.log(msg);
            $("#BtnSearch").html("<i class=\"fas fa--search bigger-110\"></i>&nbsp;" + Dictionary.Common_Search);
            $("#BtnSearch").removeAttr("disabled");
            $("#TotalRecords").html(0);
            $("#TableResults").html("<tr><td>" + msg.responseJSON.Message + "</tr></td>");
        }
    });
}

function GetMascotaMyId(mascotaId) {
    for (var x = 0; x < busqueda.length; x++) {
        if (busqueda[x].MascotaId === mascotaId) {
            return busqueda[x];
        }
    }

    return null;
}

function GoPresupuesto(id) {
    var query = ac + "&mascotaId=" + id;
    query = $.base64.encode(query);
    document.location = "/Presupuestos.aspx?" + query;
}