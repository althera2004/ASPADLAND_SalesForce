<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Presupuestos.aspx.cs" Inherits="Presupuestos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv, #scrollTableDivActosRealizados{
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

        .ui-datepicker-prev {
            font-family: "Font Awesome 5 Free";
            font-style:normal;
            font-variant-caps:normal;
            font-variant-east-asian:normal;
            font-variant-ligatures:normal;
            font-variant-numeric:normal;
            font-weight:900;
            height:25.1875px;
            left:2px;
            line-height:26px;
            max-width:32px;
            min-width:32px;
            position:absolute;
            text-decoration-color:rgba(0, 0, 0, 0);
            text-decoration-line:none;
            text-decoration-style:solid ;
            text-rendering:auto;
            -webkit-font-smoothing:antialiased;
        }

        .ui-datepicker-next {
            font-family: "Font Awesome 5 Free";
            font-style:normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
    <script type="text/javascript">
        var precios = <%=this.Precios %>;
        var polizaId = "<%=this.PolizaId %>";
        var aseguradoId = "<%=this.AseguradoId %>";
        var polizaNum = "<%=this.PolizaNum %>";
        var mascotaId = "<%=this.MascotaId %>";
        var colectivoId = "<%=this.ColectivoId %>";
        var actosRealizados = <%=this.ActosRealizados %>;
        var chip = "<%=this.Chip %>";
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col col-sm-12 col-xs-12">
                                <div class="row" style="padding-bottom:8px;display:none;" id="DataRow1">
                                    <div class="col-sm-12">
                                        <label class="col-sm-1 col-xs-3"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Label_Asegurado") %>:</label>
                                        <label class="col-sm-7 col-xs-9" id="DataAseguradoName"><strong><%=this.PolizaNum %></strong></label>
                                        <label class="col-sm-1 col-xs-3"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Label_Colectivo") %>:</label>
                                        <label class="col-sm-3 col-xs-9" id="DataColectivo"><strong><%=this.ColectivoId %></strong></label>
                                    </div>
                                </div>
                                <div class="row" style="padding-bottom:8px;">
                                    <div class="col-sm-12 col-xs-12">
                                        <label class="col-sm-1"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Label_Mascota") %>:</label>
                                        <label class="col-sm-2"><strong><%=this.Mascota.Name %></strong></label>
                                        <label class="col-sm-1"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Label_Chip") %>:</label>
                                        <label class="col-sm-2"><strong><%=this.Mascota.Chip %></strong></label>
                                        <label class="col-sm-1"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Label_TipoMascota") %>:</label>
                                        <label class="col-sm-1"><strong><%=this.Mascota.Tipo %></strong></label>
                                        <label class="col-sm-1"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Label_SexoMascota") %>:</label>
                                        <label class="col-sm-3"><strong><%=this.Mascota.Sexo %></strong></label>
                                    </div>
                                </div>
                                <div class="row" style="display:none;" id="rowWarningChip">
                                    <div class="alert alert-danger">
                                        <i class="ace-icon fas fa-warning"></i>
                                        <span id="changeMessage">Por tu seguridad  recuerda informar el microchip de la mascota. Gracias</span>
                                    </div>
                                </div>
                                <div class="tabbable">
                                    <ul class="nav nav-tabs padding-18">
                                        <li class="active" id="TabBasic">
                                            <a data-toggle="tab" href="#new"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Tab_New") %></a>
                                        </li>
                                        <li class="" id="TabPendientes">
                                            <a data-toggle="tab" href="#history"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Tab_History") %></a>
                                        </li>
                                        <li class="" id="TabActosRealizados">
                                            <a data-toggle="tab" href="#realizados"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Tab_ActosRealizados") %></a>
                                        </li>
                                    </ul>
                                    <div class="tab-content no-border padding-24" style="height:500px;">
                                        <div id="new" class="tab-pane active"> 
                                            <div class="row" style="padding-bottom:8px;">
                                                <div class="col-xs-12 col-sm-12">
                                                    <label class="col-sm-2 col-xs-12"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_ActoLabel") %>:</label>
                                                    <div class="col-sm-8 col-xs-12">
                                                        <select id="CmbEspecialidad" class="col-sm-12 col-xs-12" onchange="CmbEspecialidadChanged();">
                                                            <option value=""><%=AspadLandFramework.ApplicationDictionary.Translate("Common_SelectOne") %></option>
                                                            <asp:Literal runat="server" ID="CmbEspecialidadItems"></asp:Literal>
                                                        </select>
                                                    </div>
                                                    <div class="col-sm-2" style="display:none;">
                                                        <button class="btn btn-success" style="height:30px;padding-top:0;" type="button" id="BtnAddActo"><i class="fas fa-plus bigger-110"></i>&nbsp;<%=AspadLandFramework.ApplicationDictionary.Translate("Common_Add") %></button>
                                                    </div>
                                                </div>
                                                
							                    <div class="col-sm-12 cols-xs-12" style="margin-bottom:20px;">
                                                    <h4><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Title_Presupuesto") %></h4>
                                                    <div class="table-responsive" id="scrollTableDiv">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataHeader">
                                                                    <th id="th0" class="search" style="width: 300px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Header_Especialidad") %></th>
                                                                    <th id="th1" class="search"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Header_Act") %></th>
                                                                    <th id="th2" class="search" style="width: 90px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Header_Amount") %></th>
                                                                    <th id="th3" class="search" style="width: 90px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Header_Discount") %></th>
                                                                    <th style="width: 106px;">&nbsp;</th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListActosPresupuestoDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;min-height: 100px">
                                                            <table class="table table-bordered table-striped" style="border-top: none;" id="ListActosPresupuesto"></table>
                                                        </div>
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListActosPresupuestoFooter">
                                                                    <th style="color: #aaa;"><i><%=AspadLandFramework.ApplicationDictionary.Translate("Common_Total") %>:&nbsp;<span id="TotalRecords">0</span></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="col-sm-12 col-xs-12">
                                                    <label class="col-sm-2 col-xs-12"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presuspuesto_Notes") %>:</label>
                                                    <div class="col-sm-10 col-xs-12">
                                                        <textarea id="TxtObservaciones" rows="3" style="width:98%"></textarea>
                                                    </div>
                                                </div>                                                
                                                <div class="col-sm-12 col-xs-12" style="text-align:right;margin-top:8px;">
                                                    <button class="btn btn-info" type="button" id="BtnPrint"><i class="fas fa-print bigger-110"></i>&nbsp;<%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Btn_Print") %></button>
                                                    <button class="btn btn-success" type="button" id="BtnPrintAndSave"><i class="fas fa-save bigger-110"></i>&nbsp;<%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Btn_Mark") %></button>
                                                    <button class="btn" type="button" id="BtnCancel" onclick="document.location='BusquedaUsuarios.aspx';"><i class="fas fa-undo bigger-110"></i>&nbsp;<%=AspadLandFramework.ApplicationDictionary.Translate("Common_Exit") %></button>
                                                </div>
                                            </div>
                                        </div>
                                        <div id="history" class="tab-pane"> 
                                            <div id="accordion" class="accordion-style1 panel-group" style="height:500px;overflow:auto;overflow-x:hidden;">
                                                <asp:Literal runat="server" ID="LtPendientes"></asp:Literal>
                                            </div>
                                        </div>
                                        <div id="realizados" class="tab-pane">
                                            <div class="col-sm-12" style="margin-bottom:20px;">
                                                    <h4><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_Title_ActosRealizados") %></h4>
                                                    <div class="table-responsive" id="scrollTableDivActosRealizados">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataHeaderActosRealizados">
                                                                    <th id="th0" class="search" style="width: 300px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_List_ActosRealizados_Header_Especialidad") %></th>
                                                                    <th id="th1" class="search"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_List_ActosRealizados_Header_Acto") %></th>
                                                                    <th id="th2" class="search" style="width: 90px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_List_ActosRealizados_Header_Amount") %></th>
                                                                    <th id="th3" class="search" style="width: 90px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_List_ActosRealizados_Header_Discount")%></th>
                                                                    <th id="th4" class="search" style="width: 106px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_List_ActosRealizados_Header_Date") %></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListActosPresupuestoDivActosRealizados" style="overflow: scroll; overflow-x: hidden; padding: 0;min-height: 100px">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody><asp:Literal runat="server" ID="LtActosRealizados"></asp:Literal></tbody>
                                                            </table>
                                                        </div>
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListActosPresupuestoFooterActosRealizados">
                                                                    <th style="color: #aaa;"><i><%=AspadLandFramework.ApplicationDictionary.Translate("Common_Total") %>:&nbsp;<asp:Literal runat="server" ID="LtActosRealizadosCount"></asp:Literal></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
    
                            <div id="popupActoDescuento" class="hide" style="width:600px;">
                                <form class="form-horizontal" role="form" id="formPopupActoDescuento">
                                    <p><%= AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_ActoDescuento_Message") %></p>
                                    <div class="form-group">
                                        <label id="TxtActoDescuentoLabel" class="col-sm-3 control-label no-padding-right" for="TxtNombre"><%= this.Dictionary["Item_Presupuesto_ActoLabel"] %>:</label>
                                        <div class="col-sm-9">
                                            <input class="form-control" id="TxtActoDescuento" type="text" readonly="readonly" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="TxtAmountDescuentoLabel" class="col-sm-3 control-label no-padding-right" for="TxtNombre"><%=this.Dictionary["Item_Presuspuesto_Amount"] %>:</label>
                                        <div class="col-sm-9">
                                            <input class="form-control" id="TxtAmountDescuento" type="text" />
                                        </div>
                                    </div>
                                </form>
                            </div>
    
                            <div id="popupNoASPAD" class="hide" style="width:600px;">
                                <form class="form-horizontal" role="form" id="formPopupNoASPAD">
                                    <p><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Presupuesto_NOASPAD_Message") %></p>
                                    <div class="form-group">
                                        <label id="TxtActoLabel" class="col-sm-3 control-label no-padding-right" for="TxtNombre"><%= this.Dictionary["Item_Presupuesto_ActoLabel"] %>:</label>
                                        <div class="col-sm-9">
                                            <input class="form-control" id="TxtActo" type="text" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="TxtAmountLabel" class="col-sm-3 control-label no-padding-right" for="TxtNombre"><%=this.Dictionary["Item_Presuspuesto_Amount"] %>:</label>
                                        <div class="col-sm-9">
                                            <input class="form-control" id="TxtAmount" type="text" />
                                        </div>
                                    </div>
                                </form>
                            </div>

                            <div id="popupActoObservaciones" class="hide" style="width: 500px;">
                                <form class="form-horizontal" role="form">
                                    <p>
                                        <%=this.Dictionary["Item_Presuspuesto_Notes"] %>:
                                        <input type="text" id="ActoSelected" style="display:none;" />
                                    </p>
                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <textarea id="TxtActoObservaciones" rows ="3" style="width:80%"></textarea>
                                        </div>
                                    </div>
                                </form>
                            </div>

                            <div id="popupFechaRealizado" class="hide" style="width: 500px;">
                                <form class="form-horizontal" role="form">
                                    <p>
                                        Fecha realizaci&oacute;n:
                                        <input type="text" id="FechaTxt" readonly="readonly" />
                                        <input type="text" class="date-picker" id="Fecha" style="display:none;" />
                                    </p>
                                </form>
                            </div>

                            <div id="popupDescartar" class="hide" style="width: 500px;">
                                <form class="form-horizontal" role="form">
                                    <p>
                                        <%= this.Dictionary["Item_Presupuesto_DiscardPopupMessage"] %> <strong><span id="PresupuestoDescartadoCodigo"></span></strong>?
                                    </p>
                                </form>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript" src="/js/Presupuestos.js?<%=this.AntiCache %>"></script>
</asp:Content>