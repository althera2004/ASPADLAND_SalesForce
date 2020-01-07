$(document).ready(function () {
    $("#ErrorMessage").hide();
    $("#BtnLogin").click(Login);
    $("#BtnReset").click(ResetPassword);
    $("#TxtUserName").focus();
});

$(document).keypress(function (e) {
    if (e.which === 13) {
        Login();
    }
});

function ResetPassword() {
    $("#LoginForm").attr("action", "ResetPassword.aspx");
    $("#LoginForm").submit();
}

function Login() {
    $("#ErrorMessage").hide();
    var ok = true;
    var errorMessage = "";
    if ($("#TxtUserName").val() === "") {
        ok = false;
        $("#TxtUserName").css("background-color", "#f00");
        errorMessage = "El nombre de usuario es obligatorio.";
    }
    else {
        $("#TxtUserName").css("background-color", "transparent");
    }

    if ($("#TxtPassword").val() === "") {
        ok = false;
        $("#TxtPassword").css("background-color", "#f00");
        if (errorMessage !== "") {
            errorMessage += "<br />";
        }
        errorMessage += "La contraseña es obligatoria.";
    }
    else {
        $("#TxtPassword").css("background-color", "transparent");
    }

    if (ok) {
        var data = {
            "email": $("#TxtUserName").val(),
            "password": $("#TxtPassword").val(),
            "ip": ip
        };

        $("#TxtPassword").attr("disabled", "disabled");
        $("#TxtUserName").attr("disabled", "disabled");
        $("#BtnLogin").attr("disabled", "disabled");

        $.ajax({
            "type": "POST",
            "url": "/Async/DataService.asmx/GetLogin",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                var result = msg.d;
                if (msg.d.ReturnValue.Id === -1)
                {
                    $("#ErrorSpan").show();
                    return false;
                }

                if (msg.d.Success === true) {
                    if (msg.d.MustResetPassword === true) {
                        $("#UserId").val(result.Id);
                        $("#CompanyId").val(result.CompanyId);
                        $("#Password").val($("#TxtPassword").val());
                        document.getElementById("LoginForm").action = "ResetPassword.aspx";
                        $("#LoginForm").submit();
                        return false;
                    }

                    if (msg.d.ReturnValue.Result === 1001) {
                        document.location = "AdminDefault.aspx";
                        return false;
                    }

                    if(msg.d.ReturnValue.Result === 1) {
                        document.location = "DashBoard.aspx?action=" + (Math.random() * (1000 - 100)) + "-" + msg.d.ReturnValue.Id;
                        return false;
                    }

                    if (msg.d.ReturnValue.Result === 2) {
                        document.getElementById("ErrorSpan").style.display = "block";
                        $("#TxtPassword").removeAttr("disabled");
                        $("#TxtUserName").removeAttr("disabled");
                        $("#BtnLogin").removeAttr("disabled");
                        return false;
                    }
                    else if (result.Id === -1) {
                        document.getElementById("ErrorSpan").style.display = "block";
                    }
                }

                return false;
            },
            "error": function (msg) {
                document.getElementById("ErrorSpan").style.display = "block";
            }
        });
    }
    else {
        $("#ErrorMessage").html(errorMessage);
        $("#ErrorMessage").show();
    }
}

function LoginResultToRext(value) {
    for (var x = 0; x < LoginResult.length; x++) {
        if (LoginResult[x].value === value) {
            return LoginResult[x].text;
        }
    }

    return "undefined";
}