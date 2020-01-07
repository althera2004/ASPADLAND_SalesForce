<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="Customer_DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" runat="server">
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

        TR:first-child{border-left:none;}

        #colectivosDiv img {
            margin: 20px;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-sm-12">
                                <div class="alert alert-success" style="display: block;background-color:#489e2a;color:#fff;" id="DivPrimaryUser">
                                    <strong><i class="fas fa-info-circle fa-2x"></i></strong>
                                    <h4 style="display:inline;"><%=this.Dictionary["DashBoard_Message"] %></h4>
                                </div>
                            </div>
                            <div class="col-sm-12" style="margin-bottom:20px;" id="colectivosDiv"><asp:Literal runat="server" ID="LtColectivos"></asp:Literal></div>
                            <!--<h4>Documentos</h4>
                            <div class="col-sm-6" style="margin-bottom:20px;">
                                <h5>Documentos privados</h5>
                            </div>
                            <div class="col-sm-6" style="margin-bottom:20px;">
                                <h5>Documentos del centro</h5>

                                <div class="table-responsive" id="scrollTableDiv">
                                    <table class="table table-bordered table-striped" style="margin: 0">
                                        <thead class="thin-border-bottom">
                                            <tr id="ListDataHeader">
                                                <th id="th0" class="search" style="width: 45px;">Tipo</th>
                                                <th id="th1" class="search" onclick="Sort(this,'ListDataTable');">Nombre</th>
                                            </tr>
                                        </thead>
                                    </table>
                                    <div id="ListDataDivCentro" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                        <table class="table table-bordered table-striped" style="border-top: none;">
                                            <tr>
                                                <td style="width: 45px;" align="center"><img src="/img/pdficon.png" /></td>
                                                <td><a target="_blank" href="/Documentos/Guía rápida ASPADland.pdf">Guía rápida ASPADland</a></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45px;" align="center"><img src="/img/pdficon.png" /></td>
                                                <td><a target="_blank" href="/Documentos/Manual ASPADland.pdf">Manual ASPADland</a></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45px;" align="center"><img src="/img/pdficon.png" /></td>
                                                <td><a target="_blank" href="/Documentos/MANUAL DE APOYO A CLINICAS ASPAD.pdf">MANUAL DE APOYO A CLINICAS ASPAD</a></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45px;" align="center"><img src="/img/pdficon.png" /></td>
                                                <td><a target="_blank" href="/Documentos/MODIFICACION DATOS FISCALES ASPAD.pdf">MODIFICACION DATOS FISCALES ASPAD</a></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45px;" align="center"><img src="/img/pdficon.png" /></td>
                                                <td><a target="_blank" href="/Documentos/MSGC Anexo IV Política de Calidad_v2.pdf">MSGC Anexo IV Política de Calidad_v2</a></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45px;" align="center"><img src="/img/pdficon.png" /></td>
                                                <td><a target="_blank" href="/Documentos/Protocolo 2014.pdf">Protocolo 2014</a></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45px;" align="center"><img src="/img/pdficon.png" /></td>
                                                <td><a target="_blank" href="/Documentos/TARIFAS 2018 Canarias igic incluido.pdf">TARIFAS 2018 Canarias igic incluido</a></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45px;" align="center"><img src="/img/pdficon.png" /></td>
                                                <td><a target="_blank" href="/Documentos/TARIFAS 2018 iva incluido.pdf">TARIFAS 2018 iva incluido</a></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 45px;" align="center"><img src="/img/pdficon.png" /></td>
                                                <td><a target="_blank" href="/Documentos/TARIFAS ASISA MASCOTAS.pdf">TARIFAS ASISA MASCOTAS</a></td>
                                            </tr>
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
                            </div>-->
                            <div class="col-sm-12">
                                <div class="alert alert-warning" style="display:<%=this.WarningProfileDisplay %>;color:#fff;background-color:#f77";" id="DivWarningProfile">
                                    <strong><i class="fas fa-info-circle fa-2x"></i></strong>
                                    <h4 style="display:inline;"><%=this.Dictionary["Common_Warning_Profile"] %></h4>
                                </div>
                            </div>
                            
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">  
        <script type="text/javascript">
            function Resize() {
                return false;
                var listTable = document.getElementById("ListDataDivCentro");
                var containerHeight = $(window).height();
                listTable.style.height = (containerHeight - 600) + "px";
            }

            window.onload = function () {
                GetColectivos();
                Resize();
            }
            window.onresize = function () { Resize(); }

            function Go(sender) {
                var query = ac + "&colectivoId=" + sender.id;
                query = $.base64.encode(query);
                document.location = "/BusquedaUsuarios.aspx?" + query;
            }

            function RenderColectivoTag(colectivo) {
                //"<img src=""/logopolizas/{0}.png"" alt=""{1}"" title=""{1}"" id=""{0}"" onclick=""Go(this);"" style=""margin:20px;cursor:pointer;"" />",
                console.log("RenderColectivoTag");
                var img = document.createElement("IMG");
                img.src = "/logopolizas/" + colectivo.Id + ".png";
                img.alt = colectivo.Description;
                img.title = colectivo.Description;
                img.id = colectivo.Id;
                img.onclick = function () { Go(this); };
                document.getElementById("colectivosDiv").appendChild(img);
            }

            function GetColectivos() {
                return false;
                var data = {
                    "userId": ApplicationUser.Id
                };
                $.ajax({
                    "type": "POST",
                    "url": "/Async/DataService.asmx/GetColectivos",
                    "data": {},
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "success": function (msg) {
                        var list = [];
                        console.log(list);
                        eval("list=" + msg.d.ReturnValue + ";");
                        for (var x = 0; x < list.length; x++) {
                            RenderColectivoTag(list[x]);
                        }
                    },
                    "error": function (msg, text) {
                        console.log(msg);
                    }
                });
            }

            $("[data-rel=tooltip]").tooltip();

        </script>
</asp:Content>