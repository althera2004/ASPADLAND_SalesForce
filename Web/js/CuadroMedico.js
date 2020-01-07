/*var DepartmentSelected;
function DepartmentDeleteAction() {
    var data = {
        "departmentId": DepartmentSelected,
        "companyId": Company.Id,
        "userId": user.Id
    };
    $("#DepartmentDeleteDialog").dialog("close");
    LoadingShow(Dictionary.Common_Message_Saving);
    $.ajax({
        "type": "POST",
        "url": "/Async/DepartmentActions.asmx/DepartmentDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            document.location = document.location + "";
        },
        "error": function (msg) {
            LoadingHide();
            alertUI(msg.responseText);
        }
    });
}

function DepartmentDelete(id, name) {
    $('#DepartmentName').html(name);
    DepartmentSelected = id;
    var dialog = $("#DepartmentDeleteDialog").removeClass('hide').dialog({
        resizable: false,
        modal: true,
        title: '<h4 class="smaller">' + Dictionary.Item_Department_Popup_Delete_Title+'</h4>',
        title_html: true,
        buttons:
        [
            {
                html: "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Yes,
                "class": "btn btn-danger btn-xs",
                click: function () {
                    DepartmentDeleteAction();
                }
            },
            {
                html: "<i class='icon-remove bigger-110'></i>&nbsp;" + Dictionary.Common_No,
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function DepartmentUpdate(id, name) {
    document.location = 'DepartmentView.aspx?id=' + id;
    return false;
}**/

function Resize() {
    var containerHeight = $(window).height();
    $("#accordion").height(containerHeight - 360);
}

window.onload = function () { Resize(); }
window.onresize = function () { Resize(); }