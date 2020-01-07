<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ProductView.aspx.cs" Inherits="Customer_ProductView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
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

    .k-overlayLoader {
        margin: 0px;
        padding: 0px;
        position: fixed;
        right: 0px;
        top: 0px;
        width: 100%;
        height: 100%;
        background-color: rgb(102, 102, 102);
        z-index: 30001;
        opacity: 0.8;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                        <div class="col-xs-12">
                            <div class="center-1">
                                <!--product breadcrumb-->
                                <div class="page product-details-page">
                                    <div class="page-body">
                                        <form action="#" id="product-details-form" method="post">
                                            <div itemscope itemtype="http://schema.org/Product" data-productid="<%=this.ProductoId %>">
                                                <div class="product-essential">
                                                    <!--product pictures-->
                                                    <div class="gallery col-sm-6">
                                                        <div class="picture">
                                                            <img id="zoom_01" alt="Imagen de <%=this.ProductoName %>" src="<%=this.Frontal %>" title="imagen de <%=this.ProductoName %>" itemprop="image" />
                                                        </div>
                                                        
                                                        <div class="col-full">
                                                            <div class="wrap-col">
                                                                <div class="picture-thumbs">
                                                                    <div class="car_wrap">
                                                                        <ul id="mycarousel" class="jcarousel-skin-tango">
                                                                            <%=this.Images %>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div id="accordion">
                                                    <h3>Descripción completa</h3>
                                                    <div>
                                                        <p>
                                                            <div id="tabs-3">
                                                                <p style="white-space: pre;"><%=this.Caracteristicas %></p>
                                                            </div>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                    </div>
                                    <div class="product-collateral">
                                    </div>
                                </div>
                                </form>        
                            </div>
                        </div>
                    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
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
        <script type="text/javascript" src="/Themes/NyuSunGlasses/Content/carouseljs/jquery.jcarousel.js"></script>
        <script type="text/javascript" src="/Themes/NyuSunGlasses/Content/zoomsl-3.0.js"></script>
        <script type="text/javascript" src="/Customer/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/Customer/js/ProductView.js?<%=this.AntiCache %>"></script>
        <script src="/Themes/NyuSunGlasses/Content/Scripts/tabs.js"></script>
        <link href="/Themes/NyuSunGlasses/Content/css/tabs.css" rel="stylesheet" />
        <script type="text/javascript">
                        var windwidth = screen.width;
                        if (windwidth > 767) {
                            $("#zoom_01").imagezoomsl({

                                descarea: ".big-caption",
                                zoomstart: 1.68,
                                cursorshadeborder: "1px solid black",
                                magnifiereffectanimate: "fadeIn",
                                showstatus: false,
                                innerzoom: true
                            });
                        }

                        $(document).ready(function () {
                            jQuery("#mycarousel").jcarousel();
                            $(".thumb-link").click(function () {
                                $("#zoom_01").attr("src", $(this).find("img").attr("data-src"));
                            });

                            //console.log("Carrousel",$('#mycarousel>li').length);
                            //si hay menos de 4 fotos eliminamos las flechitas de navegación
                            if ($('#mycarousel>li').length < 4) {
                                $('.jcarousel-prev, .jcarousel-next').hide();
                            }

                            var str = "";
                            if (str != "") {
                                $(".k-overlay").show();
                            }
                            else if (str == "") {
                                $(".k-overlay").hide();
                            }
                        });

                        $(function () {
                            if (window.innerWidth <= 800 && window.innerHeight <= 600) {
                                $("#tabs").hide();
                                $("#accordion").show();
                                $("#accordion").accordion({
                                    collapsible: true,
                                    heightStyle: "content"
                                });

                            } else {
                                $("#tabs").show();
                                $("#accordion").hide();
                                $("#tabs").tabs({
                                    collapsible: true
                                });

                            }
                        });
                    </script>
</asp:Content>