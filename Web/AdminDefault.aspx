﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminDefault.aspx.cs" Inherits="AdminDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <link href="/nv.d3/nv.d3.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    <div class="row">
        <div class="col-sm-6">
            <div class="alert alert-info">
                <i class="ace-icon fas fa-search"></i>
                <span id="changeMessage">Búsquedas realizadas <strong><asp:Literal runat="server" ID="LtBusquedasTotal"></asp:Literal></strong></span>
            </div>
			<div id="Pie1" style="height:300px;"><svg></svg></div>
            <div class="scrollTable" id="scrollTable" style="width:100%;border-top:2px solid #ccc;">
                <table style="width:100%;" id="DataListTable">
                    <tbody id="DataList"><asp:Literal runat="server" ID="LtBusquedaData"></asp:Literal></tbody>
                </table>
            </div>
        </div>        

        <div class="col-sm-6" style="margin-left:-12px;">

            <div class="col-sm-12 infobox-container">
                <div class="alert alert-info" style="text-align:left;">
                    <i class="ace-icon fas fa-money"></i>
                    <span>Presupuestos ejecutados de ASPADLand <strong><asp:Literal runat="server" ID="LtTotalPresupuestos"></asp:Literal></strong></span>
                </div>
                <!-- Graph -->
                <%=this.PresupuestosGraph %>
                <!-- End Graph -->
            </div>
            
            <div class="col-sm-12">&nbsp;</div>
            <div class="col-sm-12 infobox-container">
                <div class="alert alert-info" style="text-align:left;">
                    <i class="ace-icon fas fa-user"></i>
                    <span>Uso de ASPADLand</span>
                </div>
                <!-- Graph -->
                <%=this.AccesoGraph %>
                <!-- End Graph -->
            </div>
        </div>
    </div>

    <div style="height:30px;">&nbsp;</div>

        <!--<div class="col-sm-12 infobox-container">
										<div class="infobox infobox-green">
											<div class="infobox-icon">
												<i class="ace-icon fa fa-comments"></i>
											</div>

											<div class="infobox-data">
												<span class="infobox-data-number">32</span>
												<div class="infobox-content">comments + 2 reviews</div>
											</div>

											<div class="stat stat-success">8%</div>
										</div>

										<div class="infobox infobox-blue">
											<div class="infobox-icon">
												<i class="ace-icon fa fa-twitter"></i>
											</div>

											<div class="infobox-data">
												<span class="infobox-data-number">11</span>
												<div class="infobox-content">new followers</div>
											</div>

											<div class="badge badge-success">
												+32%
												<i class="ace-icon fa fa-arrow-up"></i>
											</div>
										</div>

										<div class="infobox infobox-pink">
											<div class="infobox-icon">
												<i class="ace-icon fa fa-shopping-cart"></i>
											</div>

											<div class="infobox-data">
												<span class="infobox-data-number">8</span>
												<div class="infobox-content">new orders</div>
											</div>
											<div class="stat stat-important">4%</div>
										</div>

										<div class="infobox infobox-red">
											<div class="infobox-icon">
												<i class="ace-icon fa fa-flask"></i>
											</div>

											<div class="infobox-data">
												<span class="infobox-data-number">7</span>
												<div class="infobox-content">experiments</div>
											</div>
										</div>

										<div class="infobox infobox-orange2">
											<div class="infobox-chart">
												<span class="sparkline" data-values="196,128,202,177,154,94,100,170,224"><canvas width="44" height="26" style="display: inline-block; width: 44px; height: 26px; vertical-align: top;"></canvas></span>
											</div>

											<div class="infobox-data">
												<span class="infobox-data-number">6,251</span>
												<div class="infobox-content">pageviews</div>
											</div>

											<div class="badge badge-success">
												7.2%
												<i class="ace-icon fa fa-arrow-up"></i>
											</div>
										</div>

										<div class="infobox infobox-blue2">
											<div class="infobox-progress">
												<div class="easy-pie-chart percentage" data-percent="42" data-size="46" style="height: 46px; width: 46px; line-height: 45px;">
													<span class="percent">42</span>%
												<canvas height="46" width="46"></canvas></div>
											</div>

											<div class="infobox-data">
												<span class="infobox-text">traffic used</span>

												<div class="infobox-content">
													<span class="bigger-110">~</span>
													58GB remaining
												</div>
											</div>
										</div>

										<div class="space-6"></div>

										<div class="infobox infobox-green infobox-small infobox-dark">
											<div class="infobox-progress">
												<div class="easy-pie-chart percentage" data-percent="61" data-size="39" style="height: 39px; width: 39px; line-height: 38px;">
													<span class="percent">61</span>%
												<canvas height="39" width="39"></canvas></div>
											</div>

											<div class="infobox-data">
												<div class="infobox-content">Task</div>
												<div class="infobox-content">Completion</div>
											</div>
										</div>

										<div class="infobox infobox-blue infobox-small infobox-dark">
											<div class="infobox-chart">
												<span class="sparkline" data-values="3,4,2,3,4,4,2,2"><canvas width="39" height="15" style="display: inline-block; width: 39px; height: 15px; vertical-align: top;"></canvas></span>
											</div>

											<div class="infobox-data">
												<div class="infobox-content">Earnings</div>
												<div class="infobox-content">$32,000</div>
											</div>
										</div>

										<div class="infobox infobox-grey infobox-small infobox-dark">
											<div class="infobox-icon">
												<i class="ace-icon fa fa-download"></i>
											</div>

											<div class="infobox-data">
												<div class="infobox-content">Downloads</div>
												<div class="infobox-content">1,205</div>
											</div>
										</div>
									</div>
    </div>-->
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript">
        $(".easy-pie-chart.percentage").each(function () {
            var $box = $(this).closest(".infobox");
            var barColor = $(this).data("color") || (!$box.hasClass("infobox-dark") ? $box.css("color") : "rgba(255,255,255,0.95)");
            var trackColor = barColor == "rgba(255,255,255,0.95)" ? "rgba(255,255,255,0.25)" : "#E2E2E2";
            var size = parseInt($(this).data("size")) || 50;
            $(this).easyPieChart({
                "barColor": barColor,
                "trackColor": trackColor,
                "scaleColor": false,
                "lineCap": "butt",
                "lineWidth": parseInt(size / 10),
                "animate": 1000,
                "size": size
            });
        });

        $(".sparkline").each(function () {
            var $box = $(this).closest(".infobox");
            var barColor = !$box.hasClass("infobox-dark") ? $box.css("color") : "#FFF";
            $(this).sparkline("html",
                {
                    "tagValuesAttribute": "data-values",
                    "type": "bar",
                    "barColor": barColor,
                    "chartRangeMin": $(this).data("min") || 0
                });
        });        var chartPie1, chartPie1Data;
        var dataPie1 = [<%=this.BusquedasScript%>];
        nv.addGraph(function () {
            chartPie1 = nv.models.pieChart()
                .x(function (d) { return d.label })
                .y(function (d) { return d.value })

                .legendPosition("right")
                .labelThreshold(.05)
                .height(300)
                .showLabels(true)
                .labelType("percent")
                .donut(true).donutRatio(0.1);

            chartPie1Data = d3.select('#Pie1 svg').datum(dataPie1);
            chartPie1Data.transition().duration(500).call(chartPie1);
            nv.utils.windowResize(chartPie1.update);
            return chartPie1;
        });
    </script>
</asp:Content>