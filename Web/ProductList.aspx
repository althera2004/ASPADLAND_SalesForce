<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ProductList.aspx.cs" Inherits="Customer_ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv,#scrollTableDiv2{
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
		
        .iconfile {
            border: 1px solid #777;
            background-color: #fdfdfd;
            -webkit-box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            -moz-box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            padding-left:0!important;
            padding-top:4px !important;
            padding-bottom:4px !important;
			margin:8px;
            margin-bottom:12px !important;
			min-height:150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var Products = <%=this.ProductsJson %>;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <div class="col-xs-12">
                                <div class="row" style="padding-bottom:8px;" id="SelectRow">
                                    <div class="col-xs-12">
                                        <label class="col-xs-1">Tipo:</label>
                                        <div class="col-xs-8">
                                            <select id="CmbTipo" class="col-sm-12" onchange="CmbTipoChange()" multiple="multiple" placeholder="Seleccionar familias de producto"></select>
                                        </div>
                                    </div>
                                </div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <!--<ul class="nav nav-tabs padding-18">
                                                <li class="active">
                                                    <a data-toggle="tab" href="#active" id="SelectorTabActive"><%=this.Dictionary["Common_Active_Plural"] %></a>
                                                </li>
                                                <li class="">
                                                    <a data-toggle="tab" href="#inactive" id="SelectorTabInactive"><%=this.Dictionary["Common_Inactive_Plural"] %></a>
                                                </li>
                                            </ul>-->
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="active" class="tab-pane active"> 
                                                    <div class="row">
                                                        <div class="table-responsive" id="scrollTableDiv" style="display:none";>
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataHeader">
                                                                        <th id="th0" class="search" style="width: 150px;" onclick="Sort(this,'ListDataTable');"><%=this.Dictionary["Common_Code"] %></th>
                                                                        <th id="th1" class="search">Modelo</th>
                                                                        <th id="th2" class="search" style="width: 150px;">Familia</th>
                                                                        <th id="th3" class="search" style="width: 90px;">Talla</th>
                                                                        <th style="width: 106px;">&nbsp;</th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                                    <tbody id="ListDataTable"></tbody>
                                                                </table>
                                                            </div>
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataFooter">
                                                                        <th style="color: #aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="TotalRecords"></span></th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </div>
														<div id="Gallery" style="overflow:auto;background-color:#fafafa;border:1px solid #f7f7f7;">
														</div>
                                                    </div><!-- /span -->
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <div id="EmployeeDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Employee_PopupDelete_Message"] %>&nbsp;<strong><span id="EmployeeName"></span></strong>?</p>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/Customer/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/chosen.jquery.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/fuelux/fuelux.spinner.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/date-time/bootstrap-timepicker.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/date-time/moment.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/date-time/daterangepicker.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/bootstrap-colorpicker.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/jquery.knob.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/jquery.autosize.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/jquery.inputlimiter.1.3.1.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/jquery.maskedinput.min.js"></script>
        <script type="text/javascript" src="/Customer/assets/js/bootstrap-tag.min.js"></script>
        <script type="text/javascript" src="/Customer/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/Customer/js/ProductList.js?<%=this.AntiCache %>"></script>
</asp:Content>