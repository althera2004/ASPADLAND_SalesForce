<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="BusquedaUsuarios.aspx.cs" Inherits="BusquedaUsuarios" %>

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

        .ui-button-icon-primary {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var busqueda = null;
        var colectivoId = "<%=this.ColectivoId %>";
        var Colectivos = <%=this.Colectivos%>;
        var aseguradoId = "<%=this.AseguradoId %>";
        var polizaId = "<%=this.PolizaId %>";
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <div class="row" style="padding-bottom:8px;">
                                    <div class="col-xs-12">
                                        <label class="col-xs-1">DNI/NIF:</label>
                                        <div class="col-xs-2">
                                            <input type="text" id="TxtDNI" class="col-sm-12" placeholder="DNI/NIF" maxlength="9" />
                                        </div>
                                        <div class="col-xs-3"></div>
                                        <label class="col-xs-1" id="TxtPolizaLabel">N<sup>o</sup>&nbsp;p&oacute;liza:</label>
                                        <div class="col-xs-3">
                                            <input type="text" id="TxtPoliza" class="col-sm-12" placeholder="Nº póliza" />
                                        </div>
                                        <div class="col-xs-2"></div>
                                    </div>
                                </div>
                                <div class="row" style="padding-bottom:8px;display:none;">
                                    <div class="col-xs-12">
                                        <label class="col-xs-1">Colectivo:</label>
                                        <div class="col-xs-5">
                                            <select id="CmbColectivo" class="col-sm-12" placeholder="Seleccionar familias de producto"></select>
                                        </div>
                                        <label class="col-xs-1">Nombre:</label>
                                        <div class="col-xs-5">
                                            <input type="text" id="_TxtNombre" class="col-sm-12" placeholder="Nombre y apellidos sin comas" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="padding-bottom:8px;">
                                    <div class="col-xs-10">
                                        <span id="ErrorMessage" style="display:none;color:#d33;padding-left:30px;font-size:18px;"></span>
                                        <ul>
                                            <li id="ErrorNIFMessage" style="display:none;color:#d33;padding-left:30px;font-size:18px;"></li>
                                            <li id="ErrorPolizaMessage" style="display:none;color:#d33;padding-left:30px;font-size:18px;"></li>
                                        </ul>
                                    </div>
                                    <div class="col-xs-2">
                                        <button class="btn btn-success" type="button" id="BtnSearch"><i class="fas fa-search bigger-110"></i>&nbsp;<%=this.Dictionary["Common_Search"] %></button>
                                    </div>
                                </div>
                            </div>
							<div class="col-sm-12" style="margin-bottom:20px;">
                                <h4><%=this.Dictionary["Item_BusquedaUsuarios_Table_Title"] %></h4>
                                <div class="table-responsive" id="scrollTableDiv">
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataHeader">
                                                <th id="th0" class="search" onclick="Sort(this,'ListDataTable');"><%=this.Dictionary["Item_BusquedaUsuarios_Table_Nombre"] %></th>
                                                <th id="th1" class="search" style="width: 90px;"><%=this.Dictionary["Item_BusquedaUsuarios_Table_DNI"] %></th>
                                                <th id="th2" class="search" style="width: 150px;"><%=this.Dictionary["Item_BusquedaUsuarios_Table_Producto"] %></th>
                                                <th id="th3" class="search" style="width: 120px;"><%=this.Dictionary["Item_BusquedaUsuarios_Table_Poliza"] %></th>
                                                <!--<th id="th4" class="search" style="width: 80px;"><%=this.Dictionary["Item_BusquedaUsuarios_Table_Estado"] %></th>-->
                                                <th id="th5" class="search" style="width: 130px;"><%=this.Dictionary["Item_BusquedaUsuarios_Table_Chip"] %></th>
                                                <th id="th6" class="search" style="width: 60px;"><%=this.Dictionary["Item_BusquedaUsuarios_Table_Tipo"] %></th>
                                                <th id="th7" class="search" style="width: 120px;"><%=this.Dictionary["Item_BusquedaUsuarios_Table_Mascota"] %></th>
                                                <th id="th8" class="search" style="width: 106px;">&nbsp;</th>
                                            </tr>
                                        </thead>
                                    </table>
                                    <div id="ListDataDivCentro" style="overflow: scroll; overflow-x: hidden; padding: 0;min-height: 100px;">
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

                            <div id="popupMascotaUpdate" class="hide" style="width: 500px;">
                                <form class="form-horizontal" role="form" id="formMascotaUpdate">
                                    <input class="form-control" id="TxtMascotaGuid" type="text" style="display:none;" />
                                    <div class="form-group">
                                        <label id="TxtNombreLabel" class="col-sm-3 control-label no-padding-right" for="TxtNombre">Nombre</label>
                                        <div class="col-sm-9">
                                            <input class="form-control" id="TxtNombre" type="text" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="TxtChipLabel" class="col-sm-3 control-label no-padding-right" for="TxtChip">Chip</label>
                                        <div class="col-sm-9">
                                            <input class="form-control" id="TxtChip" type="text" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="TxtTipoLabel" class="col-sm-3 control-label no-padding-right" for="TxtRegistroComments">Tipo</label>
                                        <div class="col-sm-9" style="margin-top:4px;">
                                            <input type="radio" id="RTipo1" name="RTipo" />Perro
                                            &nbsp;
                                            <input type="radio" id="RTipo2" name="RTipo" />Gato
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="TxtSexoLabel" class="col-sm-3 control-label no-padding-right" for="TxtRegistroComments">Sexo</label>
                                        <div class="col-sm-9" style="margin-top:4px;">
                                            <input type="radio" id="RSexo2" name="RSexo" />Macho
                                            &nbsp;
                                            <input type="radio" id="RSexo1" name="RSexo" />Hembra
                                        </div>
                                    </div>
                                </form>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript" src="/js/BusquedaUsuarios.js?<%=this.AntiCache %>"></script>
</asp:Content>