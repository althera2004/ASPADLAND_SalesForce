window.onload = function () {
    if ($("#TxtLunesTarde").val() === "") { $("#TxtLunesCompleto").click(); }
    if ($("#TxtMartesTarde").val() === "") { $("#TxtMartesCompleto").click(); }
    if ($("#TxtMiercolesTarde").val() === "") { $("#TxtMiercolesCompleto").click(); }
    if ($("#TxtJuevesTarde").val() === "") { $("#TxtJuevesCompleto").click(); }
    if ($("#TxtViernesTarde").val() === "") { $("#TxtViernesCompleto").click(); }
    if ($("#TxtSabadoTarde").val() === "") { $("#TxtSabadoCompleto").click(); }
    if ($("#TxtDomingoTarde").val() === "") { $("#TxtDomingoCompleto").click(); }
}

function ChangeDiaCompleto(sender) {
    var id = sender.id;
    if (id === "TxtLunesCompleto") {
        if (sender.checked) {
            $("#TxtLunesTarde").hide();
        }
        else {

            $("#TxtLunesTarde").show();
        }
    }
    if (id === "TxtMartesCompleto") {
        if (sender.checked) {
            $("#TxtMartesTarde").hide();
        }
        else {

            $("#TxtMartesTarde").show();
        }
    }
    if (id === "TxtMiercolesCompleto") {
        if (sender.checked) {
            $("#TxtMiercolesTarde").hide();
        }
        else {

            $("#TxtMiercolesTarde").show();
        }
    }
    if (id === "TxtJuevesCompleto") {
        if (sender.checked) {
            $("#TxtJuevesTarde").hide();
        }
        else {

            $("#TxtJuevesTarde").show();
        }
    }
    if (id === "TxtViernesCompleto") {
        if (sender.checked) {
            $("#TxtViernesTarde").hide();
        }
        else {

            $("#TxtViernesTarde").show();
        }
    }
    if (id === "TxtSabadoCompleto") {
        if (sender.checked) {
            $("#TxtSabadoTarde").hide();
        }
        else {

            $("#TxtSabadoTarde").show();
        }
    }
    if (id === "TxtDomingoCompleto") {
        if (sender.checked) {
            $("#TxtDomingoTarde").hide();
        }
        else {

            $("#TxtDomingoTarde").show();
        }
    }
}

function ChangePassword() {
    var ok = true;                
    if (!RequiredFieldText("TxtPassActual")) { ok = false; }
    if (!RequiredFieldText("TxtPassNew1")) { ok = false; }
    if (!RequiredFieldText("TxtPassNew2")) { ok = false; }
    if (!MatchRequiredBothFieldText("TxtPassNew1", "TxtPassNew2")) { ok = false; }

    if(ok===false) {
        window.scrollTo(0, 0); 
        return false;
    }

    var data = {
        "oldPassword": $("#TxtPassActual").val(),
        "newPassword": $("#TxtPassNew1").val(),
        "userId": ApplicationUser.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/DataService.asmx/ChangePassword",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success === true) {
                console.log(response.d);
                if (response.d.ReturnValue * 1 === -1) {
                    alertUI("La contraseña actual no es correcta");
                }
                if (response.d.ReturnValue * 1 === 1) {
                    document.location = "/LogOut.aspx";
                }
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function SendChanges() {
    var data = {
        "centroId": User.Id,
        "actualUserId": User.Codigo,
        "actualUserName": $("#TxtNombre").val(),
        "userName": $("#TxtNombre").val(),
        "telefono1": $("#TxtTelefono").val(),
        "telefono2": $("#TxtTelefono2").val(),
        "direccion": $("#TxtDireccion").val(),
        "poblacion": $("#TxtPoblacion").val(),
        "cp": $("#TxtCP").val(),
        "provincia": $("#TxtProvincia").val(),
        "email": $("#TxtEmail1").val(),
        "emailAlternativo": $("#TxtEmail2").val(),
        "urg24": document.getElementById("TxtUrgenciasPresencial").checked,
        "urgTel": document.getElementById("TxtUrgenciasTelefono").checked,
        "emailFacturacion": $("#TxtFacturacionName").val()
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/UserActions.asmx/SendChanges",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success === true) {
                alertUI("Se han enviado los datos para la confirmación de los cambios solicitados");
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}

function SendHorario() {
    var data = {
        "centroId": User.Id,
        "actualUserId": User.Codigo,
        "actualUserName": CentroName,
        "L1": $("#TxtLunesManana").val(),
        "L2": $("#TxtLunesTarde").val(),
        "M1": $("#TxtMartesManana").val(),
        "M2": $("#TxtMartesTarde").val(),
        "X1": $("#TxtMiercolesManana").val(),
        "X2": $("#TxtMiercolesTarde").val(),
        "J1": $("#TxtJuevesManana").val(),
        "J2": $("#TxtJuevesTarde").val(),
        "V1": $("#TxtViernesManana").val(),
        "V2": $("#TxtViernesTarde").val(),
        "S1": $("#TxtSabadoManana").val(),
        "S2": $("#TxtSabadoTarde").val(),
        "D1": $("#TxtDomingoManana").val(),
        "D2": $("#TxtDomingoTarde").val()
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/UserActions.asmx/SendChangesHorario",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            if (response.d.Success === true) {
                alertInfoUI("Se han enviado los datos para la confirmación de los cambios solicitados");
            }
            if (response.d.Success !== true) {
                alertUI(response.d.MessageError);
            }
        },
        "error": function (jqXHR, textStatus, errorThrown) {
            LoadingHide();
            alertUI(jqXHR.responseText);
        }
    });
}