<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminBusquedas.aspx.cs" Inherits="AdminBusquedas" %>

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
                                        <div id="output" style="margin: 30px;"><h4>Generando report...</h4></div>
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

            $.getJSON("/Data/Search.aspx", function (mps) {
                $("#output").pivotUI(mps, {
                    renderers: renderers,
                    cols: ["Colectivo"], rows: ["Centro"],
                    rendererName: "Table",
                    rowOrder: "value_z_to_a", colOrder: "value_z_to_a"
                });
            });
    });</script>
</asp:Content>