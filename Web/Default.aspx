﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>ASPAND Land</title>
    <link rel="icon" href="/favicon.ico?v=2" type="image/x-icon" />
    <!--[if !IE]> -->
    <script type="text/javascript" src="assets/js/jquery-2.0.3.min.js"></script>
    <!-- <![endif]-->

    <!--[if IE]>
        <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <![endif]-->

    <meta name="description" content="User login page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    
    <link type="text/css" rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css" />
    <link type="text/css" rel="stylesheet" href="assets/css/bootstrap.min.css" />
    <link type="text/css" rel="stylesheet" href="/assets/css/font-awesome.min.css" />

    <!--[if IE 7]>
          <link rel="stylesheet" href="assets/css/font-awesome-ie7.min.css" />
        <![endif]-->

    <!-- page specific plugin styles -->

    <!-- fonts -->

    <link rel="stylesheet" href="/assets/css/ace-fonts.css" />

    <!-- ace styles -->

    <link rel="stylesheet" href="/assets/css/ace.min.css" />
    <link rel="stylesheet" href="/assets/css/ace-rtl.min.css" />
    <style type="text/css">
        @media only screen and (min-width:481px) {
            .only-480 {
                display: none !important;
            }
        }

        .widget-main {
            -webkit-box-shadow: 4px 4px 24px 4px rgba(0,0,0,0.55);
            -moz-box-shadow: 4px 4px 24px 4px rgba(0,0,0,0.55);
            box-shadow: 4px 4px 24px 4px rgba(0,0,0,0.55);
        }
		
		a:hover { text-decoration:none; font-weight:bold;}
		
		.btn { border-radius:5px;}
		
		.widget-main, .widget-box {	border-radius: 10px;}								   
    </style>

    <!--[if lte IE 8]>
          <link rel="stylesheet" href="assets/css/ace-ie.min.css" />
        <![endif]-->

    <!-- inline styles related to this page -->

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->

    <!--[if lt IE 9]>
        <script src="assets/js/html5shiv.js"></script>
        <script src="assets/js/respond.min.js"></script>
        <![endif]-->

    <script type="text/javascript" src="/js/Login.js"></script>
    <script type="text/javascript">
        var language = navigator.language || navigator.userLanguage;
        var ip = "<%=this.IP %>";

        window.onload = function () { }
    </script>
</head>

<body class="login-layout" style="background-color: #f7f5e4;">
    <div class="main-container">
        <div class="main-content" style="padding:0;">
            <div class="row" style="background-color:#b33e0e;height:100px;text-align:center;"><img src="/img/logo.png" title="ASPAD" style="margin-top:20px;" /></div>
            <div class="row" style="margin-top:50px;">
                <div class="col-sm-10 col-sm-offset-1">
                    <div class="login-container">
                        <div class="position-relative">
                            <div id="login-box" class="login-box visible widget-box no-border" style="z-index: 99999; background-color: transparent; box-shadow: 4px 4px 4px rgba(0, 0, 0, 0.25)">
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <h4 class="header black lighter bigger" id="PageTitle">Acceso a la aplicaci&oacute;n</h4>

                                        <div class="space-6"></div>
                                        <form id="LoginForm" action="InitSession.aspx" method="post">
                                            <div style="display: none;">
                                                <input type="text" name="UserId" id="UserId" />
                                                <input type="text" name="CompanyId" id="CompanyId" />
                                                <input type="text" name="Password" id="Password" />
                                            </div>
                                            <fieldset>
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <input type="text" class="form-control" placeholder="Usuario / email" id="TxtUserName" value="" />
                                                    </span>
                                                </label>
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <input type="password" class="form-control" placeholder="Password" id="TxtPassword" value="" />
                                                    </span>
                                                </label>
                                                <div class="space"></div>
                                                <div class="clearfix">
													<button type="button" class="width-35 pull-right btn btn-sm btn-success" id="BtnLogin">Acceder</button>
                                                </div>
                                                
												<div class="space-4"></div>
                                                <h4>
                                                    <span id="ErrorSpan" style="color: #f00; display: none;text-align:center;">
                                                        Acceso no v&aacute;lido.<br /><br />
                                                        <button type="button" class="pull-right btn btn-sm btn-success" id="BtnReset">Recordar contraseña</button>
                                                    </span>
                                                </h4>
                                            </fieldset>
                                        </form>
										<asp:Literal runat="server" ID="LtCompnayName"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        if ("ontouchend" in document) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
    </script>
</body>
</html>