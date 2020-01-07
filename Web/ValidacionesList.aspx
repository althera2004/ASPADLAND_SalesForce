<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ValidacionesList.aspx.cs" Inherits="ValidacionesList" %>

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
        var colectivoId = "<%=this.ColectivoId %>";
        var Colectivos = <%=this.Colectivos%>;
        var Colas = <%=this.Colas %>;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12" id="validationForm" style="display:none;">
                                <div class="row" style="padding-bottom:8px;">
                                    <div class="col-xs-12">
                                        <label class="col-xs-1" id="TxtDNIId"><%=this.Dictionary["Item_Validaciones_Label_DNI"] %><span style="color:#f00;">*</span>:</label>
                                        <div class="col-xs-3">
                                            <input type="text" id="TxtDNI" class="col-sm-12" value="<%=this.NIF %>" />
                                            <label id="ErrorNIFMessage" class="ErrorMessage" style="display:none;"><%=this.Dictionary["Common_Error_NIFInvalid"] %></label>
                                        </div>
                                        <label class="col-xs-1" id="CmbColectivoId"><%=this.Dictionary["Item_Validaciones_Label_Colectivo"] %><span style="color:#f00;">*</span>:</label>
                                        <div class="col-xs-3">
                                            <select id="CmbColectivo" class="col-sm-12" disabled="disabled">
                                                <asp:Literal runat="server" ID="cmbColectivoData"></asp:Literal>
                                            </select>
                                        </div>
                                        <label class="col-xs-1"><%=this.Dictionary["Item_Validaciones_Label_Poliza"] %>:</label>
                                        <div class="col-xs-3">
                                            <input type="text" id="TxtPoliza" class="col-sm-12" value="<%=this.Poliza %>" />
                                            <label id="ErrorPolizaMessage" class="ErrorMessage" style="display:none;"></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="padding-bottom:8px;">
                                    <div class="col-xs-12">
                                        <label class="col-xs-1" id="TxtNombreId"><%=this.Dictionary["Item_Validaciones_Label_Nombre"] %><span style="color:#f00;">*</span>:</label>
                                        <div class="col-xs-3">
                                            <input type="text" id="TxtNombre" class="col-sm-12" />
                                        </div>
                                        <label class="col-xs-1" id="TxtApellido1Id"><%=this.Dictionary["Item_Validaciones_Label_Apellido1"] %><span style="color:#f00;">*</span>:</label>
                                        <div class="col-xs-3">
                                            <input type="text" id="TxtApellido1" class="col-sm-12" />
                                        </div>
                                        <label class="col-xs-1" id="TxtApellido2Id"><%=this.Dictionary["Item_Validaciones_Label_Apellido2"] %><span style="color:#f00;">*</span>:</label>
                                        <div class="col-xs-3">
                                            <input type="text" id="TxtApellido2" class="col-sm-12" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="padding-bottom:8px;">
                                    <div class="col-xs-8">
                                        <span id="ErrorMessage" style="display:none;color:#d33;padding-left:30px;font-size:18px;">Revise los campos obligatorios</span>
                                    </div>
                                    <div class="col-xs-2">
                                        <input type="checkbox" id="ChkUrgente" />&nbsp;<%=this.Dictionary["Item_Validaciones_Label_Urgente"] %>
                                    </div>
                                    <div class="col-xs-2">
                                        <button class="btn btn-success" type="button" id="BtnAddValidacion"><i class="fas fa-exclamation-triangle bigger-110"></i>&nbsp;<%=this.Dictionary["Item_Validaciones_Btn_Send"] %></button>
                                    </div>
                                </div>
                            </div>
							<div class="col-sm-12" style="margin-bottom:20px;">
                                <h4><%=this.Dictionary["Item_Validaciones_List_Title"] %></h4>
                                <div class="table-responsive" id="scrollTableDiv">
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataHeader">
                                                <th id="th0" class="search" style="width: 30px;">&nbsp;</th>
                                                <th id="th1" class="search" style="width: 75px;" onclick="Sort(this,'ListDataTable');">N<sup>o</sup>cola</th>
                                                <th id="th2" class="search" style="width: 80px;"><%=this.Dictionary["Item_Validaciones_List_Header_DNI"] %></th>
                                                <th id="th3" class="search" style="width: 140px;"><%=this.Dictionary["Item_Validaciones_List_Header_Colectivo"] %></th>
                                                <th id="th4" class="search" style="width: 45px;"><%=this.Dictionary["Item_Validaciones_List_Header_Urgente"] %></th>
                                                <th id="th5" class="search"><%=this.Dictionary["Item_Validaciones_List_Header_Observaciones"] %></th>
                                                <th id="th6" class="search" style="width: 160px;"><%=this.Dictionary["Item_Validaciones_List_Header_Dates"] %></th>
                                                <th id="th8" class="search" style="width: 150px;"><%=this.Dictionary["Item_Validaciones_List_Header_Asegurado"] %></th>
                                                <th id="th9" class="search" style="width: 100px;"><%=this.Dictionary["Item_Validaciones_List_Header_Telefono"] %></th>
                                                <th id="th10" class="search" style="width: 150px;"><%=this.Dictionary["Item_Validaciones_List_Header_Poliza"] %></th>
                                            </tr>
                                        </thead>
                                    </table>
                                    <div id="ListDataDivCentro" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                        <table class="table table-bordered table-striped" style="border-top: none;" id="TableResults"></table>
                                    </div>
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataFooter">
                                                <th style="color: #aaa;"><i><%=this.Dictionary["Common_Total"] %>:&nbsp;<span id="TotalRecords">0</span></i></th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript" src="/js/ValidacionesList.js?<%=this.AntiCache %>"></script>
</asp:Content>

