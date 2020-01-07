<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Beneficios.aspx.cs" Inherits="Beneficios" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var User = <%=this.UserJson %>;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    <div class="col-xs-12">
        <p><%= AspadLandFramework.ApplicationDictionary.Translate("Item_Beneficios_Message") %></p>
    </div>
    <div class="col-xs-12">
        <div class="col-xs-6">
            <h2><%= AspadLandFramework.ApplicationDictionary.Translate("Item_Beneficios_ProductoDental_Title") %></h2>
            <p><%= AspadLandFramework.ApplicationDictionary.Translate("Item_Beneficios_ProductoDental_Message") %></p>
            <a href="/ProductoDental.aspx"><%= AspadLandFramework.ApplicationDictionary.Translate("Common_Access") %></a>
        </div>
    </div>
</asp:Content>