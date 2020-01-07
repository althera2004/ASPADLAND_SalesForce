<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminLogin.aspx.cs" Inherits="AdminLogin" %>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <ul class="nav nav-tabs padding-18">
                                                <li class="active">
                                                    <a data-toggle="tab" href="#lastweek"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Login_Tab_LastWeek") %></a>
                                                </li>
                                                <li class="" id="TabHorario">
                                                    <a data-toggle="tab" href="#lastmonth"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Login_Tab_LastMonth") %></a>
                                                </li>
                                                <li class="" id="TabDepartments">
                                                    <a data-toggle="tab" href="#never"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Login_Tab_Never") %></a>
                                                </li>
                                            </ul>
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="lastweek" class="tab-pane active">
                                                    <div class="col-sm-12" style="margin-bottom:20px;">
                                                        <h4><%=this.Dictionary["Item_BusquedaUsuarios_Table_Title"] %></h4>
                                                        <div class="table-responsive" id="scrollTableDivLastWeek">
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataHeaderLastWeek">
                                                                        <th id="th0" class="search" onclick="Sort(this,'ListDataHeaderLastWeek');"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Login_Header_CentroName") %></th>
                                                                        <th id="th1" class="search" style="width: 127px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Login_Header_LastConnection") %></th>                                                                        
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                            <div id="ListDataDivLastWeek" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                                <table class="table table-bordered table-striped" style="border-top: none;" id="TableResultsLastWeek">
                                                                    <asp:Literal runat="server" ID="LtBodyLastWeek"></asp:Literal>
                                                                </table>
                                                            </div>
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataFooterLastWeek">
                                                                        <th style="color: #aaa;"><i><%=this.Dictionary["Common_Total"] %>:&nbsp;<span id="TotalRecordsLastWeek"><asp:Literal runat="server" ID="LtBodyLastWeekCount"></asp:Literal></span></i></th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="lastmonth" class="tab-pane">
                                                    <div class="col-sm-12" style="margin-bottom:20px;">
                                                        <h4><%=this.Dictionary["Item_BusquedaUsuarios_Table_Title"] %></h4>
                                                        <div class="table-responsive" id="scrollTableDivLastMonth">
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataHeaderLastMonth">
                                                                        <th id="th0" class="search" onclick="Sort(this,'ListDataHeaderLastMonth');"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Login_Header_CentroName") %></th>
                                                                        <th id="th1" class="search" style="width: 127px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Login_Header_LastConnection") %></th>                                                                        
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                            <div id="ListDataDivLastMonth" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                                <table class="table table-bordered table-striped" style="border-top: none;" id="TableResultsLastMonth">
                                                                    <asp:Literal runat="server" ID="LtBodyLastMonth"></asp:Literal>
                                                                </table>
                                                            </div>
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataFooterLastMonth">
                                                                        <th style="color: #aaa;"><i><%=this.Dictionary["Common_Total"] %>:&nbsp;<span id="TotalRecordsLastMonth"><asp:Literal runat="server" ID="LtBodyLastMonthCount"></asp:Literal></span></i></th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="never" class="tab-pane">
                                                    <div class="col-sm-12" style="margin-bottom:20px;">
                                                        <h4><%=this.Dictionary["Item_BusquedaUsuarios_Table_Title"] %></h4>
                                                        <div class="table-responsive" id="scrollTableDivNever">
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataHeaderNever">
                                                                        <th id="th0" class="search" onclick="Sort(this,'ListDataHeaderNever');"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Login_Header_CentroName") %></th>
                                                                        <th id="th1" class="search" style="width: 127px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Login_Header_LastConnection") %></th>                                                                        
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                            <div id="ListDataDivNever" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                                <table class="table table-bordered table-striped" style="border-top: none;" id="TableResultsNever">
                                                                    <asp:Literal runat="server" ID="LtBodyNever"></asp:Literal>
                                                                </table>
                                                            </div>
                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                <thead class="thin-border-bottom">
                                                                    <tr id="ListDataFooterNever">
                                                                        <th style="color: #aaa;"><i><%=this.Dictionary["Common_Total"] %>:&nbsp;<span id="TotalRecordsNever"><asp:Literal runat="server" ID="LtBodyNeverCount"></asp:Literal></span></i></th>
                                                                    </tr>
                                                                </thead>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript">
        window.onload = function () { Resize(); }
        window.onresize = function () { Resize(); }

        function Resize() {
            var containerHeight = $(window).height();
            $("#ListDataDivLastWeek").height(containerHeight - 420);
            $("#ListDataDivLastMonth").height(containerHeight - 420);
            $("#ListDataDivNever").height(containerHeight - 420);
        }
    </script>
</asp:Content>