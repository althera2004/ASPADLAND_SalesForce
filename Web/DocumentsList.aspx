<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="DocumentsList.aspx.cs" Inherits="DocumentsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv,#scrollTableDivPrivados{
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
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-sm-6" style="margin-bottom:20px;">
                                <h5><%=this.Dictionary["Item_Documentos_Title_Custom"] %></h5>
                                <div class="table-responsive" id="scrollTableDivPrivados">
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataHeaderPrivados">
                                                <th id="th0" class="search" style="width:50px;"><%=this.Dictionary["Item_Documentos_Header_Type"] %></th>
                                                <th id="th1" class="search" onclick="Sort(this,'ListDataTable');"><%=this.Dictionary["Item_Documentos_Header_Name"] %></th>
                                            </tr>
                                        </thead>
                                    </table>
                                    <div id="ListDataDivPrivados" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                        <table class="table table-bordered table-striped" style="border-top: none;">
                                            <%=this.DocumentosPrivados %>
                                        </table>
                                    </div>
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataFooterPrivados">
                                                <th style="color: #aaa;"><i>Total:&nbsp;<span id="TotalRecordsPrivados">9</span></th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
                            <div class="col-sm-6" style="margin-bottom:20px;">
                                <h5><%=this.Dictionary["Item_Documentos_Title_General"] %></h5>

                                <div class="table-responsive" id="scrollTableDiv">
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataHeader">
                                                <th id="th0" class="search" style="width:50px;"><%=this.Dictionary["Item_Documentos_Header_Type"] %></th>
                                                <th id="th1" class="search" onclick="Sort(this,'ListDataTable');"><%=this.Dictionary["Item_Documentos_Header_Name"] %></th>
                                            </tr>
                                        </thead>
                                    </table>
                                    <div id="ListDataDivCentro" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                        <table class="table table-bordered table-striped" style="border-top: none;">
                                            <%=this.DocumentosCentro %>
                                        </table>
                                    </div>
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataFooter">
                                                <th style="color: #aaa;"><i>Total:&nbsp;<span id="TotalRecords">9</span></th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">    
    <script type="text/javascript">
        function Resize() {
            var containerHeight = $(window).height();
            console.log(containerHeight);
            $("#ListDataDivCentro").height(containerHeight - 330);
            $("#ListDataDivPrivados").height(containerHeight - 330);

            $("#TotalRecords").html($("#ListDataDivCentro tr").length);
            $("#TotalRecordsPrivados").html($("#ListDataDivPrivados tr").length);
        }

        window.onload = function () { Resize(); }
        window.onresize = function () { Resize(); }
   </script>
</asp:Content>