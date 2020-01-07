<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminMail.aspx.cs" Inherits="AdminMail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="col-sm-12" style="margin-bottom:20px;">
                                            <div class="table-responsive" id="scrollTableDiv">
                                                <table class="table table-bordered table-striped" style="margin: 0">
                                                    <thead class="thin-border-bottom">
                                                        <tr id="ListDataHeader">
                                                            <th id="th0" class="search" style="width:170px;""><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Event") %></th>
                                                            <th id="th1" class="search" style="width:300px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Centro") %></th>
                                                            <th id="th2" class="search" style="width:150px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Date") %></th>
                                                            <th id="th3"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Traces_Data") %></th>                                                                       
                                                        </tr>
                                                    </thead>
                                                </table>
                                                <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
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