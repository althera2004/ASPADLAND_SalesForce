<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ProductoDental.aspx.cs" Inherits="ProductoDental" %>

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
            font-size: 14px;
        }

        .ui-button-icon-primary {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    <div class="row">
        <label class="col-xs-12"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_ProductoDental_Intro1") %></label>
        <label class="col-xs-12"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_ProductoDental_Intro2") %></label>
        <label class="col-xs-12"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_ProductoDental_Intro3") %></label>
        <label class="col-xs-12"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_ProductoDental_Intro4") %></label>
    </div>
    <div class="tabbable" style="margin-top:12px;">
        <ul class="nav nav-tabs padding-18">
            <li class="active" id="TabBasic">
                <a data-toggle="tab" href="#tab1"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_ProductoDental_Tab_Basic") %></a>
            </li>
            <li class="" id="TabPendientes">
                <a data-toggle="tab" href="#tab2"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_ProductoDental_Tab_Servicio") %></a>
            </li>
        </ul>
        <div class="tab-content no-border padding-24" style="height:500px;">
            <div id="tab1" class="tab-pane active"> 
                <div class="col-xs-12">
                    <iframe style="width:100%;border:none;height:800px;" src="https://customer.adegroup.eu/index.php/geoV2?customerUrl=de65aa559582d5f62c118410cdcf2ac92000c83b" allow="geolocation"></iframe>
                </div>
                <div style="height:60px;">&nbsp;</div>
            </div>
            <div id="tab2" class="tab-pane"> 
                <div class="col-xs-12">
                    <div class="table-responsive" id="scrollTableDiv">
                        <table class="table table-bordered table-striped" style="margin: 0">
                            <thead class="thin-border-bottom">
                                <tr id="ListDataHeader">
                                    <th id="th0" class="search" onclick="Sort(this,'ListDataTable');"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_ProductoDental_Table_Nombre") %></th>
                                    <th id="th1" class="search" style="width: 137px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_ProductoDental_Table_NIF") %></th>
                                </tr>
                            </thead>
                        </table>
                        <div id="ListDataDivCentro" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                            <table class="table table-bordered table-striped" style="border-top: none;" id="TableResults">
                                <tbody><asp:Literal runat="server" id="LtAsegurados"></asp:Literal></tbody>
                            </table>
                        </div>
                        <table class="table table-bordered table-striped" style="margin: 0">
                            <thead class="thin-border-bottom">
                                <tr id="ListDataFooter">
                                    <th style="color: #aaa;"><i><%=AspadLandFramework.ApplicationDictionary.Translate("Common_Total") %>:&nbsp;<span id="TotalRecords"><asp:Literal runat="server" id="LtAseguradosCount"></asp:Literal></span></i></th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript" src="/js/ProductoDental.js?<%=this.AntiCache %>"></script>
</asp:Content>