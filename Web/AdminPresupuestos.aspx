<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminPresupuestos.aspx.cs" Inherits="AdminPresupuestos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/c3/0.4.11/c3.min.css" />
        <link rel="stylesheet" type="text/css" href="/css/pivot.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <ul class="nav nav-tabs padding-18">
                                                <li class="active">
                                                    <a data-toggle="tab" href="#resumen"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Presupuesto_Tab_Resumen") %></a>
                                                </li>
                                                <li class="" id="tabRealizados">
                                                    <a data-toggle="tab" href="#realizados"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Presupuesto_Tab_Realizados") %></a>
                                                </li>
                                            </ul>
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="resumen" class="tab-pane active">
                                                    <div id="output" style="margin: 30px;"><h4>Generando report...</h4></div>
                                                </div>
                                                <div id="realizados" class="tab-pane">
                                                    <div class="col-sm-12" style="margin-bottom:20px;">
                                                        <h4><%=this.Dictionary["Admin_Presupuesto_Title_Realizados"] %></h4>
                                                        <div class="table-responsive" id="scrollTable">
                                                            <table class="table table-bordered table-striped" style="margin: 0;font-size:11px!important;">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataHeader">
                                                                        <th id="th0" class="search" style="width:90px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Presupuesto_Header_Fecha") %></th> 
                                                                        <th id="th1"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Presupuesto_Header_Centro") %></th>
                                                                        <th id="th2" class="search" style="width:150px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Presupuesto_Header_Codigo") %></th>
                                                                        <th id="th3" class="search" style="width:220px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Presupuesto_Header_Poliza") %></th>                                                                 
                                                                        <th id="th4" class="search" style="width:217px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Presupuesto_Header_Mascota") %></th>                                                                 
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                                <table class="table table-bordered table-striped" style="border-top: none;" id="TableResults">
                                                                    <asp:Literal runat="server" ID="LtBody"></asp:Literal>
                                                                </table>
                                                            </div>
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataFooterLastWeek">
                                                                        <th style="color: #aaa;"><i><%=this.Dictionary["Common_Total"] %>:&nbsp;<span id="TotalRecords"><asp:Literal runat="server" ID="LtBodyCount"></asp:Literal></span></i></th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                    </div>
                                </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/d3/3.5.5/d3.min.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/c3/0.4.11/c3.min.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js"></script>
        <script type="text/javascript" src="/js/pivot.js"></script>
        <script type="text/javascript" src="/js/c3_renderers.js"></script>
    <script type="text/javascript">
    $(function () {

            var derivers = $.pivotUtilities.derivers;
            var renderers = $.extend($.pivotUtilities.renderers,
                $.pivotUtilities.c3_renderers);

            $.getJSON("/Data/SearchPresupuestos.aspx", function (mps) {
                $("#output").pivotUI(mps, {
                    renderers: renderers,
                    cols: ["Colectivo"], rows: ["Fecha"],
                    rendererName: "Table",
                    rowOrder: "value_z_to_a", colOrder: "value_z_to_a"
                });
            });
        });

        window.onload = function () {
            Resize();
        }

        window.onresize = function () {
            Resize();
        }

        function Resize() {
            var containerHeight = $(window).height();
            $("#ListDataDiv").height(containerHeight - 390);
        }</script>
</asp:Content>