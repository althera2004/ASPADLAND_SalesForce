<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="AseguradosList.aspx.cs" Inherits="AseguradosList" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">                            
							<div class="col-sm-12" style="margin-bottom:20px;">
                                <h4><%=this.Dictionary["Item_Validaciones_List_Title"] %></h4>
                                <div class="table-responsive" id="scrollTableDiv">
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataHeader">
                                                <th id="th0" class="search" onclick="Sort(this,'ListDataTable');"><%=this.Dictionary["Item_Asegurados_List_Header_Asegurado"] %></th>
                                                <th id="th1" class="search" style="width: 200px;"><%=this.Dictionary["Item_Asegurados_List_Header_Poliza"] %></th>
                                                <th id="th2" class="search" style="width: 250px;"><%=this.Dictionary["Item_Asegurados_List_Header_Colectivo"] %></th>
                                                <th id="th4" class="search" style="width: 277px;"><%=this.Dictionary["Item_Asegurados_List_Header_Producto"] %></th>
                                                
                                            </tr>
                                        </thead>
                                    </table>
                                    <div id="ListDataDivCentro" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                        <table class="table table-bordered table-striped" style="border-top: none;" id="TableResults">
                                            <tbody><asp:Literal runat="server" ID="LtAsegurados"></asp:Literal></tbody>
                                        </table>
                                    </div>
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataFooter">
                                                <th style="color: #aaa;"><i><%=this.Dictionary["Common_Total"] %>:&nbsp;<span id="TotalRecords"><asp:Literal runat="server" ID="LtAseguradosCount"></asp:Literal></span></i></th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript" src="/js/AseguradosList.js?<%=this.AntiCache %>"></script>
</asp:Content>

