$(document).ready(function () {
    $("#ErrorMessage").hide();
    $("#BtnReset").click(ResetPassword);
    $("#TxtUserName").focus();
});

$(document).keypress(function (e) {
    if (e.which == 13) {
        Login();
    }
});

function ResetPassword() {
    $("#ErrorSpan").hide();
    var ok = true;
    var errorMessage = "";
    var email = $("#TxtUserName").val();

    if (email === "") {
        ok = false;
        $("#ErrorSpan").html("Hay que especificar el email");
    }
    else if (validateEmail(email) === false) {
        ok = false;
        $("#ErrorSpan").html("El email indicado no es válido");
    }

    if (ok === false) {
        $("#ErrorSpan").show();
        return false;
    }

    if (ok) {
        var webMethod = "/Async/LoginActions.asmx/RecuperarPassword";
        var data = { "email": email };

        $.ajax({
            "type": "POST",
            "url": webMethod,
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                var result = msg.d;
                $("#Password").val($("#TxtPassword1").val());
                $("#LoginForm").submit();
                return false;
            },
            error: function (msg) {
                $("#ErrorMessage").show();
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
        if (LoginResult[x].value == value) {
            return LoginResult[x].text;
        }
    }

    return 'undefined';
}