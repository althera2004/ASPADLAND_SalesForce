<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminConfiguration.aspx.cs" Inherits="AdminConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="/assets/css/jquery-ui-1.10.3.full.min.css" />
    <link rel="stylesheet" href="/assets/css/bootstrap-duallistbox.min.css" />
	<link rel="stylesheet" href="/assets/css/bootstrap-multiselect.min.css" />
	<link rel="stylesheet" href="/assets/css/select2.css" />
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
                                                    <a data-toggle="tab" href="#config"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Tab_Configruration") %></a>
                                                </li>
                                                <li class="" id="TabHorario">
                                                    <a data-toggle="tab" href="#repeat"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Tab_Repeat") %></a>
                                                </li>
                                            </ul>
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="config" class="tab-pane active">
                                                    <form class="form-horizontal" role="form">
                                            <h4><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Impuestos") %></h4>
                                            <div class="form-group">
                                                <label class="col-sm-1"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Impuestos_IVA") %></label>
                                                <div class="col-sm-2">
                                                    <input type="text" id="TxtIVA" style="text-align:right;" maxlength="2" size="2" class="form-control" readonly="readonly" spellcheck="false" value="<%=System.Configuration.ConfigurationManager.AppSettings["IVA"] %>" />
                                                </div>
                                                <label class="col-sm-1"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Impuestos_IGIC") %></label>
                                                <div class="col-sm-2">
                                                    <input type="text" id="TxtICIG" style="text-align:right;" maxlength="2" size="2" class="form-control" readonly="readonly" spellcheck="false" value="<%=System.Configuration.ConfigurationManager.AppSettings["IGIC"] %>" />
                                                </div>
                                                <label class="col-sm-1"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Impuestos_Ceuta") %></label>
                                                <div class="col-sm-2">
                                                    <input type="text" id="TxtCeuta" style="text-align:right;" maxlength="2" size="2" class="form-control" readonly="readonly" spellcheck="false" value="<%=System.Configuration.ConfigurationManager.AppSettings["Ceuta"] %>" />
                                                </div>
                                                <label class="col-sm-1"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Impuestos_Melilla") %></label>
                                                <div class="col-sm-2">
                                                    <input type="text" id="TxtMelilla" style="text-align:right;" maxlength="2" size="2" class="form-control" readonly="readonly" spellcheck="false" value="<%=System.Configuration.ConfigurationManager.AppSettings["Melilla"] %>" />
                                                </div>
                                            </div>
                                            <h4><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Periodo") %></h4>
                                            <div class="form-group">
                                                
                                                <div class="col-sm-4">
                                                    <label class="col-sm-6"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Periodo_Validaciones") %></label>
                                                    <div class="col-sm-6">
                                                        <input type="text" id="TxtValidaciones" style="text-align: right;" maxlength="2" size="2" class="form-control" readonly="readonly" spellcheck="false" value="<%=System.Configuration.ConfigurationManager.AppSettings["DiasValidacion"] %>" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <label class="col-sm-6"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Periodo_Presupuestos") %></label>
                                                    <div class="col-sm-6">
                                                        <input type="text" id="TxtPresupuestos" style="text-align:right;" maxlength="2" size="2" class="form-control" readonly="readonly" spellcheck="false" value="15" />
                                                    </div>
                                                </div>
                                                
                                            </div>
                                        </form>
                                                </div>
                                                <div id="repeat" class="tab-pane">
                                                    <div class="alert alert-info" style="display: block;" id="DivPrimaryUser">
                                                        <strong><i class="fas fa-info-circle fa-2x"></i></strong>
                                                        <h3 style="display:inline;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Tab_Repeat") %></h3>
                                                        <p style="margin-left:40px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Repeat_Message") %></p>
                                                    </div>
                                                    <div class="form-group" id="DivRepeat" style="display:none;">
                                                        <div class="col-sm-6"><h5><strong><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Repeat_Available") %></strong></h5></div>
                                                        <div class="col-sm-6"><h5><strong><%=AspadLandFramework.ApplicationDictionary.Translate("Admin_Config_Repeat_Selected") %></strong></h5></div>
                                                        <div class="col-sm-12">
											                <select multiple="multiple" size="10" name="duallistbox_membership[]" id="membership" style="height:300px;">
												                <asp:Literal runat="server" ID="LtActos"></asp:Literal>
											                </select>
											                <div class="hr hr-16 hr-dotted"></div>
										                </div>
                                                    </div>
                                                </div>
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
        <script type="text/javascript" src="/js/AdminConfiguration.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/assets/js/jquery.bootstrap-duallistbox.min.js"></script>
        <script type="text/javascript">
            var demo1 = $('select[name="duallistbox_membership[]"]').bootstrapDualListbox(
                {
                    "infoText": "all",//Dictionary.Common_DualListBox_ShowingAll,
                    "infoTextFiltered": "filtered"//Dictionary.Common_DualListBox_Filtered
                });
            var container1 = demo1.bootstrapDualListbox("getContainer");
            container1.find(".btn").addClass("btn-white btn-info btn-bold");
            $("#DivRepeat").show();
        </script>
</asp:Content>