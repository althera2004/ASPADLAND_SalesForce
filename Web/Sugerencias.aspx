<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Sugerencias.aspx.cs" Inherits="Sugerencias" %>

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
    <div class="col-sm-12">
        <label class="col-xs-12"><%=this.Dictionary["Item_Sugerencias_Explanation"] %></label>
    </div>
    <div class="tabbable" style="margin-top:12px;">
        <ul class="nav nav-tabs padding-18">
            <li class="active" id="TabBasic">
                <a data-toggle="tab" href="#tabSend"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Sugerencias_Tab_Send") %></a>
            </li>
            <li class="" id="TabPendientes">
                <a data-toggle="tab" href="#tabSent"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Sugerencias_Tab_Sent") %></a>
            </li>
        </ul>
        <div class="tab-content no-border padding-24" style="height:500px;">
            <div id="tabSend" class="tab-pane active"> 
                <div class="col-xs-12">
                    <label class="col-xs-12"><%=this.Dictionary["Item_Sugerencias_Label"] %>:</label>
                    <div class="col-xs-12">
                        <textarea id="TxtSugerencia" rows="8" style="width:98%"></textarea>
                    </div>
                </div>
                <div class="col col-xs-12">&nbsp;</div>
                <div class="col col-xs-12">        
                    <div class="col col-xs-8">&nbsp;</div>
                    <div class="col-xs-4" style="text-align:right;">
                        <button class="btn btn-success" type="button" id="BtnSend" onclick="SendSugerencia();"><i class="fas fa-envelope bigger-110"></i>&nbsp;<%=AspadLandFramework.ApplicationDictionary.Translate("Item_Sugerencias_Btn_Send") %></button>
                    </div>
                </div>
            </div>
            <div id="tabSent" class="tab-pane">
                <div class="col-xs-12">
                    <div class="table-responsive" id="scrollTableDiv">
                        <table class="table table-bordered table-striped" style="margin: 0">
                            <thead class="thin-border-bottom">
                                <tr id="ListDataHeader">
                                    <th id="th0" class="search" onclick="Sort(this,'ListDataTable');"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Sugerencias_TableHeader_Sugerencia") %></th>
                                    <th id="th1" class="search" style="width: 137px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Sugerencias_TableHeader_Date") %></th>
                                </tr>
                            </thead>
                        </table>
                        <div id="ListDataDivCentro" style="overflow: hidden scroll; padding: 0px; height: 81px;">
                            <table class="table table-bordered table-striped" style="border-top: none;" id="TableResults">
                                <tbody><asp:Literal runat="server" ID="LtSugerenciasList"></asp:Literal></tbody>
                            </table>
                        </div>
                        <table class="table table-bordered table-striped" style="margin: 0">
                            <thead class="thin-border-bottom">
                                <tr id="ListDataFooter">
                                    <th style="color: #aaa;"><i><%=AspadLandFramework.ApplicationDictionary.Translate("Common_RegisterCount") %>:&nbsp;<span id="TotalRecords">0</span></i></th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript">
        function SendSugerencia() {            
            var data = {
                "centroId": user.Id,
                "userId": user.Id,
                "actualUserId": User.Codigo,
                "actualUserName": CentroName,
                "sugerencia": $("#TxtSugerencia").val()
            };

            $.ajax({
                "type": "POST",
                "url": "/Async/UserActions.asmx/SendSugerencia",
                "contentType": "application/json; charset=utf-8",
                "dataType": "json",
                "data": JSON.stringify(data, null, 2),
                "success": function (response) {
                    if (response.d.Success === true) {
                        $("#TxtSugerencia").val("");
                        alertInfoUI(Dictionary.Item_Sugerencias_MailSentMessage, GoDashboard);
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

        function GoDashboard() {
            document.location = "/DashBoard.aspx";
        }

        function Toggle(sender) {
            $(".trData").hide();
            var id = sender.id.toString().split('_')[0];

            if (sender.innerHTML === "+") {
                $("#" + id + "_data").show();
                sender.innerHTML = "-";
            } else {
                sender.innerHTML = "+";
            }
        }

        function Resize() {
            var containerHeight = $(window).height();
            $("#ListDataDivCentro").height(containerHeight - 360);
        }

        window.onload = function () { Resize(); }
        window.onresize = function () { Resize(); }
    </script>
</asp:Content>