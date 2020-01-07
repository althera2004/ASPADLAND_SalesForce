<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CuadroMedico.aspx.cs" Inherits="CuadroMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv{
            background-color:#fafaff;
            border:1px solid #e0e0e0;
            border-top:none;
            display:block;
        }

        .truncate {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            padding:0;
            margin:0;
        }

        TR:first-child {
            border-left: none;
        }

        td {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var User = <%=this.UserJson %>;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    <div class="col col-xs-12"> 
        <div class="alert alert-info" style="display: block;" id="DivPrimaryUser">
            <strong><i class="fas fa-info-circle fa-2x"></i></strong>
            <h3 style="display:inline;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_CuadroMedico_ChangeDataTitle") %></h3>
            <p style="margin-left:40px;"><%=this.ChangeMessage %></p>
        </div>
        <div id="accordion" class="accordion-style1 panel-group" style="height:500px;overflow:auto;overflow-x:hidden;">
            <asp:Literal runat="server" ID="LtCuadroMedico"></asp:Literal>
        </div>
    </div>
    <div class="col col-xs-12">        
        <div class="col col-xs-8">  &nbsp;</div>
        <div class="col-xs-4">
            <button class="btn btn-success" type="button" id="BtnSearch" onclick="SendCuadroMedico();"><i class="fas fa-search bigger-110"></i>&nbsp;<%=AspadLandFramework.ApplicationDictionary.Translate("Item_CuadroMedico_SendButton") %></button>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript" src="/js/CuadroMedico.js?<%=this.AntiCache %>"></script>
    <script type="text/javascript">
        var actos = <%=this.ActualActos%>;

        function SendCuadroMedico() {
            var activos = "|";
            var inactivos = "|";

            $(".ckacto").each(function (index, item) {
                console.log(item.id, item.checked);

                if (item.checked === true && jQuery.inArray(item.id, actos) === -1) {
                    activos += item.id + "|";
                }

                if (item.checked === false && jQuery.inArray(item.id, actos) !== -1) {
                    inactivos += item.id + "|";
                }
            });

            console.log(activos);
            console.log(inactivos);
            
            var data = {
                "centroId": User.Id,
                "actualUserId": User.Codigo,
                "actualUserName": CentroName,
                "activos": activos,
                "inactivos": inactivos
            };

            $.ajax({
                "type": "POST",
                "url": "/Async/UserActions.asmx/SendCuadroMedico",
                "contentType": "application/json; charset=utf-8",
                "dataType": "json",
                "data": JSON.stringify(data, null, 2),
                "success": function (response) {
                    if (response.d.Success === true) {
                        alertUI(Dictionary.Item_Profile_Actos_MailSentMessage);
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
    </script>
</asp:Content>