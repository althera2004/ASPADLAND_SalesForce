<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminTrazas.aspx.cs" Inherits="AdminTrazas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <div class="tabbable">
                                    <ul class="nav nav-tabs padding-18">
                                        <li class="active" id="TabBasic">
                                            <a data-toggle="tab" href="#pendientes"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_PresupuestosPendientes") %></a>
                                        </li>
                                        <li class="" id="TabPendientes">
                                            <a data-toggle="tab" href="#sinpresupuesto"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_BusquedaSinPresupuesto") %></a>
                                        </li>
                                        <li class="" id="TabActosRealizados">
                                            <a data-toggle="tab" href="#descartados"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_PresupuestosDescartados") %></a>
                                        </li>
                                    </ul>
                                    <div class="tab-content no-border padding-24" style="height:500px;">
                                        <div id="pendientes" class="tab-pane active"> 
                                            <div class="table-responsive" id="scrollTableDiv">
                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                    <thead class="thin-border-bottom">
                                                        <tr id="ListDataHeader">
                                                            <!--<th id="th0" class="search" style="width:170px;""><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Event") %></th>-->
                                                            <th id="th0" class="search" style="width:250px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Centro") %></th>
                                                            <th id="th1" class="search" style="width:200px;">Asegurado</th>
                                                            <th id="th2" class="search" style="width:200px;">Poliza</th>
                                                            <th id="th3" class="search" style="width:130px;">Chip</th>
                                                            <th id="th4" class="search" style="width:80px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Date") %></th>
                                                            <th id="th5"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Data") %></th>                                                                       
                                                        </tr>
                                                    </thead>
                                                </table>
                                                <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;font-size:12px;">
                                                    <table class="table table-bordered table-striped" style="border-top: none;">
                                                        <tbody id="TableResults"></tbody>
                                                    </table>
                                                </div>
                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                    <thead class="thin-border-bottom">
                                                        <tr id="ListDataFooter">
                                                            <th style="color: #aaa;"><i><%=this.Dictionary["Common_Total"] %>:&nbsp;<span id="TotalRecords"></span></i></th>
                                                        </tr>
                                                    </thead>
                                                </table>
                                            </div>
                                            <div class="col-sm-12" style="text-align:right;">
                                                <button class="btn btn-success" type="button" onclick="ExcelPendientes();">Descargar Excel</button>
                                            </div>
                                        </div>
                                        <div id="sinpresupuesto" class="tab-pane"> 
                                            <div class="table-responsive" id="scrollTableDivSinPresupuesto">
                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                    <thead class="thin-border-bottom">
                                                        <tr id="ListDataHeaderSinPresupuesto">
                                                            <th id="th0" class="search" style="width:450px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Centro") %></th>
                                                            <th id="th1" class="search" style="width:80px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Date") %></th>
                                                            <th id="th2" class="search" style="width:150px;">Colectivo</th>
                                                            <th id="th2"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Data") %></th>                                                                       
                                                        </tr>
                                                    </thead>
                                                </table>
                                                <div id="ListDataDivSinPresupuesto" style="overflow: scroll; overflow-x: hidden; padding: 0;font-size:12px;">
                                                    <table class="table table-bordered table-striped" style="border-top: none;">
                                                        <tbody id="TableResultsSinPresupuesto"><asp:Literal ID="LtSinPresupuesto" runat="server"></asp:Literal></tbody>
                                                    </table>
                                                </div>
                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                    <thead class="thin-border-bottom">
                                                        <tr id="ListDataFooterSinPresupuesto">
                                                            <th style="color: #aaa;"><i><%=this.Dictionary["Common_Total"] %>:&nbsp;<asp:Literal runat="server" id="TotalRecordsSinPresupuesto"></asp:Literal></i></th>
                                                        </tr>
                                                    </thead>
                                                </table>
                                            </div>
                                            <div class="col-sm-12" style="text-align:right;">
                                                <button class="btn btn-success" type="button" onclick="ExcelSinPresupuesto();">Descargar Excel</button>
                                            </div>
                                        </div>                                        
                                        <div id="descartados" class="tab-pane"> 
                                            <div class="table-responsive" id="scrollTableDivDescartados">
                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                    <thead class="thin-border-bottom">
                                                        <tr id="ListDataHeaderDescartados">
                                                            <th id="th0" class="search" style="width:450px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Centro") %></th>
                                                            <th id="th1" class="search" style="width:90px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Date") %></th>
                                                            <th id="th2" class="search" style="width:150px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Presupuesto") %></th>
                                                            <th id="th3" class="search" style="width:150px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Colectivo") %></th>
                                                            <th id="th4"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_PolizaDNI") %></th>                                                                       
                                                        </tr>
                                                    </thead>
                                                </table>
                                                <div id="ListDataDivDescartados" style="overflow: scroll; overflow-x: hidden; padding: 0;font-size:12px;">
                                                    <table class="table table-bordered table-striped" style="border-top: none;">
                                                        <tbody id="TableResultsDescartados"><asp:Literal runat="server" ID="LtDescartados"></asp:Literal></tbody>
                                                    </table>
                                                </div>
                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                    <thead class="thin-border-bottom">
                                                        <tr id="ListDataFooterDescartados">
                                                            <th style="color: #aaa;"><i><%=this.Dictionary["Common_Total"] %>:&nbsp;<asp:Literal runat="server" id="TotalRecordsDescartados"></asp:Literal></i></th>
                                                        </tr>
                                                    </thead>
                                                </table>
                                            </div>
                                            <div class="col-sm-12" style="text-align:right;">
                                                <button class="btn btn-success" type="button" onclick="ExcelDescartados();">Descargar Excel</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/assets/js/chosen.jquery.min.js"></script>
        <script type="text/javascript" src="/assets/js/fuelux/fuelux.spinner.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/bootstrap-timepicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/moment.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/daterangepicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/bootstrap-colorpicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.knob.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.autosize.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.inputlimiter.1.3.1.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.maskedinput.min.js"></script>
        <script type="text/javascript" src="/assets/js/bootstrap-tag.min.js"></script>
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/Admin/Trazas.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript">
            var trazas = <%=this.Trazas %>;
        </script>
</asp:Content>