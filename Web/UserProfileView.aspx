<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="UserProfileView.aspx.cs" Inherits="UserProfileView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        .sidebar1{width:190px;float:left;position:relative;border:1px solid #ccc;border-width:0 1px 0 0;background-color:#f2f2f2}
        .sidebar1:before{content:"";display:block;width:190px;position:fixed;z-index:-1;background-color:#f2f2f2;border:1px solid #ccc;border-width:0 1px 0 0}
        .avatar{float:left;padding:4px;background-color:#fff;border:1px solid #ccc;}
        .avatarSelected{float:left;padding:4px;background-color:#0f0;border:1px solid #0f0;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var User = <%=this.UserJson %>;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <ul class="nav nav-tabs padding-18">
                                                <li class="active">
                                                    <a data-toggle="tab" href="#home"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_Tab_Principal") %></a>
                                                </li>
                                                <li class="" id="TabHorario">
                                                    <a data-toggle="tab" href="#horario"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_Tab_Horario") %></a>
                                                </li>
                                                <li class="" id="TabDepartments">
                                                    <a data-toggle="tab" href="#password"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_Tab_Password") %></a>
                                                </li>
                                            </ul>
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="home" class="tab-pane active"> 
                                                    <div class="alert alert-info" style="display: block;" id="DivPrimaryUser">
                                                        <strong><i class="fas fa-info-circle fa-2x"></i></strong>
                                                        <h3 style="display:inline;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ChangeDataTitle") %></h3>
                                                        <p style="margin-left:40px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ChangeDataMessage") %></p>
                                                    </div>
                                                                                                
                                                    <form class="form-horizontal" role="form">
                                                        <div class="form-group">
                                                            <label id="TxtNombreLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_NameLabel") %></label>
                                                            <div class="col-sm-6">
                                                                <input type="text" id="TxtNombre" value="<%=this.ApplicationUser.Nombre %>" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtNombreErrorRequired" style="display:none;"><%=AspadLandFramework.ApplicationDictionary.Translate("Common_Required") %></span>
                                                            </div>
                                                            <label id="TxtTelefonoLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_TelefonoLabel") %></label>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtTelefono" value="<%=this.ApplicationUser.Telefono1 %>" class="aol-xs-12 col-sm-12" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtEmail1Label" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_Email1Label") %></label>
                                                            <div class="col-sm-4">
                                                                <input type="text" id="TxtEmail1" value="<%=this.ApplicationUser.Email1 %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <label id="TxtEmail2Label" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_Email2Label") %></label>
                                                            <div class="col-sm-4">
                                                                <input type="text" id="TxtEmail2" value="<%=this.ApplicationUser.Email2 %>" class="col-xs-12 col-sm-12" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtDireccionLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_AddressLabel") %></label>
                                                            <div class="col-sm-10">
                                                                <input type="text" id="TxtDireccion" value="<%=this.ApplicationUser.Direccion %>" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtDireccionErrorRequired" style="display:none;"><%=AspadLandFramework.ApplicationDictionary.Translate("Common_Required") %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtPoblacionLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_CityLabel") %></label>
                                                            <div class="col-sm-4">
                                                                <input type="text" id="TxtPoblacion" value="<%=this.ApplicationUser.Poblacion %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <label id="TxtCPLabel" class="col-sm-1 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_CPLabel") %></label>
                                                            <div class="col-sm-1">
                                                                <input type="text" id="TxtCP" value="<%=this.ApplicationUser.CP %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <label id="TxtProvinciaLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ProvinceLabel") %></label>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtProvincia" value="<%=this.ApplicationUser.Provincia %>" class="aol-xs-12 col-sm-12" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtUrgenciasPresencialLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_UrgenciasPresencial") %></label>
                                                            <div class="col-sm-2">
                                                                <input type="checkbox" id="TxtUrgenciasPresencial" <%=this.ApplicationUser.UrgenciasPresenciales ? " checked=\"checked\"" : string.Empty %> />
                                                            </div>
                                                            <label id="TxtUrgenciasTelefonoLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_UrgenciasTelefono") %></label>
                                                            <div class="col-sm-2">
                                                                <input type="checkbox" id="TxtUrgenciasTelefono" <%=this.ApplicationUser.UrgenciasTelefono ? " checked=\"checked\"" : string.Empty %> />
                                                            </div>
                                                            <label id="TxtTelefono2Label" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_Telefono2Label") %></label>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtTelefono2" value="<%=this.ApplicationUser.Telefono2 %>" class="aol-xs-12 col-sm-12" />
                                                            </div>
                                                        </div>  
                                                        <div class="form-group">
                                                            <label id="TxtFacturacionNameLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_Email") %></label>
                                                            <div class="col-sm-4">
                                                                <input type="text" id="TxtFacturacionName" value="<%=this.ApplicationUser.FacturacionEmail %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <label class="col-sm-6"><i style="color:#777;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_Profile_Title_Facturacion") %></i></label>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-12 control-label no-padding-right">
                                                                <button class="btn btn-success" type="button" id="BtnSendChanges" onclick="SendChanges();">
                                                                    <i class="far fa-envelope bigger-110"></i>
                                                                    <%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ChangeDataButton") %>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </form>     
                                                </div>
                                                <div id="horario" class="tab-pane">
                                                    <div class="alert alert-info" style="display: block;" id="DivPrimaryUser1">
                                                        <strong><i class="fas fa-info-circle fa-2x"></i></strong>
                                                        <h3 style="display:inline;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ChangeDataTitle") %></h3>
                                                        <p style="margin-left:40px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ChangeDataMessage") %></p>
                                                    </div>
                                                    <form class="form-horizontal" role="form">

                                                        <div class="form-group">
                                                            <label class="col-sm-5 control-label">&nbsp;</label>
                                                            <label class="col-sm-2 control-label" style="text-align:center;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioManana") %></label>
                                                            <label class="col-sm-2 control-label" style="text-align:center;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioTarde") %></label>
                                                        </div>
                                                        
                                                        <div class="form-group">
                                                            <label id="TxtLunesLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioLunesLabel") %></label>
                                                            <label id="" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioCompleto") %></label>
                                                            <div class="col-sm-1">
                                                                <input type="checkbox" id="TxtLunesCompleto" onclick="ChangeDiaCompleto(this);" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtLunesManana" value="<%=this.ApplicationUser.HorarioLunes.Split('-')[0].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtLunesTarde" value="<%=this.ApplicationUser.HorarioLunes.Split('-')[1].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="form-group">
                                                            <label id="TxtMartesLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioMartesLabel") %></label>
                                                            <label id="" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioCompleto") %></label>
                                                            <div class="col-sm-1">
                                                                <input type="checkbox" id="TxtMartesCompleto" onclick="ChangeDiaCompleto(this);" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtMartesManana" value="<%=this.ApplicationUser.HorarioMartes.Split('-')[0].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtMartesTarde" value="<%=this.ApplicationUser.HorarioMartes.Split('-')[1].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="form-group">
                                                            <label id="TxtMiercolesLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioMiercolesLabel") %></label>
                                                            <label id="" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioCompleto") %></label>
                                                            <div class="col-sm-1">
                                                                <input type="checkbox" id="TxtMiercolesCompleto" onclick="ChangeDiaCompleto(this);" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtMiercolesManana" value="<%=this.ApplicationUser.HorarioMiercoles.Split('-')[0].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtMiercolesTarde" value="<%=this.ApplicationUser.HorarioMiercoles.Split('-')[1].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="form-group">
                                                            <label id="TxtJuevesLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioJuevesLabel") %></label>
                                                            <label id="" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioCompleto") %></label>
                                                            <div class="col-sm-1">
                                                                <input type="checkbox" id="TxtJuevesCompleto" onclick="ChangeDiaCompleto(this);" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtJuevesManana" value="<%=this.ApplicationUser.HorarioJueves.Split('-')[0].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtJuevesTarde" value="<%=this.ApplicationUser.HorarioJueves.Split('-')[1].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="form-group">
                                                            <label id="TxtViernesLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioViernesLabel") %></label>
                                                            <label id="" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioCompleto") %></label>
                                                            <div class="col-sm-1">
                                                                <input type="checkbox" id="TxtViernesCompleto" onclick="ChangeDiaCompleto(this);" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtViernesManana" value="<%=this.ApplicationUser.HorarioViernes.Split('-')[0].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtViernesTarde" value="<%=this.ApplicationUser.HorarioViernes.Split('-')[1].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="form-group">
                                                            <label id="TxtSabadoLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioSabadoLabel") %></label>
                                                            <label id="" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioCompleto") %></label>
                                                            <div class="col-sm-1">
                                                                <input type="checkbox" id="TxtSabadoCompleto" onclick="ChangeDiaCompleto(this);" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtSabadoManana" value="<%=this.ApplicationUser.HorarioSabado.Split('-')[0].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtSabadoTarde" value="<%=this.ApplicationUser.HorarioSabado.Split('-')[1].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="form-group">
                                                            <label id="TxtDomingoLabel" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioDomingoLabel") %></label>
                                                            <label id="" class="col-sm-2 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioCompleto") %></label>
                                                            <div class="col-sm-1">
                                                                <input type="checkbox" id="TxtDomingoCompleto" onclick="ChangeDiaCompleto(this);" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtDomingoManana" value="<%=this.ApplicationUser.HorarioDomingo.Split('-')[0].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <input type="text" id="TxtDomingoTarde" value="<%=this.ApplicationUser.HorarioDomingo.Split('-')[1].Trim() %>" class="col-xs-12 col-sm-12"" />
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <div class="col-sm-10 control-label no-padding-right">
                                                                <button class="btn btn-success" type="button" id="BtnHorario" onclick="SendHorario();">
                                                                    <i class="far fa-envelope bigger-110"></i>
                                                                    <%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_HorarioButton") %>
                                                                </button>
                                                            </div>
                                                        </div>

                                                    </form>
                                                </div>
                                                <div id="password" class="tab-pane">	                                            
                                                    <form class="form-horizontal" role="form">
                                                        <div class="alert alert-danger" style="display: block;" id="DivPrimaryUser2">
                                                            <strong><i class="fas fa-exclamation-triangle fa-2x"></i></strong>
                                                            <h3 style="display:inline;"><%=AspadLandFramework.ApplicationDictionary.Translate("Common_Warning") %></h3>
                                                            <p style="margin-left:40px;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ChangePasswordLogout") %></p>
                                                        </div>

                                                        <div class="form-group">
                                                            <label id="TxtPassActualLabel" class="col-sm-3 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ActualPassword")%></label>
                                                            <div class="col-sm-4">
                                                                <input type="password" placeholder="<%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ActualPassword") %>" id="TxtPassActual" value="" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtPassActualErrorRequired" style="display:none;"><%=AspadLandFramework.ApplicationDictionary.Translate("Common_Required") %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtPassNew1Label" class="col-sm-3 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_NewPassword") %></label>
                                                            <div class="col-sm-4">
                                                                <input type="password" placeholder="<%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_NewPassword") %>" id="TxtPassNew1" value="" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtPassNew1ErrorRequired" style="display:none;"><%=AspadLandFramework.ApplicationDictionary.Translate("Common_Required") %></span>
                                                                <span class="ErrorMessage" id="TxtPassNew1ErrorMatch" style="display:none;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_User_ErrorMessage_PaswordsNoMatch") %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtPassNew2Label" class="col-sm-3 control-label no-padding-right"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ConfirmPassword") %></label>
                                                            <div class="col-sm-4">
                                                                <input type="password" placeholder="<%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ConfirmPassword") %>" id="TxtPassNew2" value="" class="col-xs-12 col-sm-12"/>
                                                                <span class="ErrorMessage" id="TxtPassNew2ErrorRequired" style="display:none;"><%=AspadLandFramework.ApplicationDictionary.Translate("Common_Required") %></span>
                                                                <span class="ErrorMessage" id="TxtPassNew2ErrorMatch" style="display:none;"><%=AspadLandFramework.ApplicationDictionary.Translate("Item_User_ErrorMessage_PaswordsNoMatch") %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-12 control-label no-padding-right">
                                                                <button class="btn btn-success" type="button" id="BtnPasswordOk" onclick="ChangePassword();">
                                                                    <i class="far fa-envelope bigger-110"></i>
                                                                    <%=AspadLandFramework.ApplicationDictionary.Translate("Item_UserProfile_ChangePasswordButton") %>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </form> 
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
        <script type="text/javascript" src="/js/UserProfileView.js?ac=<%=this.AntiCache %>"></script>
</asp:Content>