using AspadLandFramework.Item;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminDefault : Page
{
    /// <summary> Master of page</summary>
    private AdminMaster master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public string AccesoGraph { get; private set; }
    public string PresupuestosGraph { get; private set; }

    public string BusquedasGraph { get; private set; }
    public string BusquedasScript { get; private set; }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.company = Session["company"] as Company;

        Session["Dictionary"] = this.Dictionary;
        Session["company"] = this.company;

        this.master = this.Master as AdminMaster;
        this.master.AddBreadCrumbInvariant(this.Dictionary["Admin_Dashboard"]);
        this.master.Titulo = "Dashboard";
        this.master.TitleInvariant = true;

        this.Accesos();
        this.Busquedas();
        this.Presupuesto();
    }

    private void Accesos()
    {
        using (var cmd = new SqlCommand("ASPADLand_Admin_Accesos"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    var a = 0;
                    var t = 0;
                    var m = 0;
                    var w = 0;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (rdr.GetString(1) == "A") { a = rdr.GetInt32(0); }
                            if (rdr.GetString(1) == "T") { t = rdr.GetInt32(0); }
                            if (rdr.GetString(1) == "M") { m = rdr.GetInt32(0); }
                            if (rdr.GetString(1) == "W") { w = rdr.GetInt32(0); }
                        }
                    }

                    this.AccesoGraph = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<div class=""infobox infobox-green"">

                        <div class=""infobox-icon""><i class=""ace-icon fa fa-comments""></i></div>
                            <div class=""infobox-data"">
                                <span class=""infobox-data-number"">{0}</span>
                                <div class=""infobox-content"">Total de centros</div>
                            </div>
						</div>

                        <div class=""infobox infobox-blue2"">
                            <div class=""infobox-progress"">
							    <div class=""easy-pie-chart percentage"" data-percent=""{4}"" data-size=""46"" style=""height: 46px; width: 46px; line-height: 46px;"">
								    <span class=""percent"">{4}</span>%
									<canvas height = ""46"" width=""46""></canvas>
								</div>
                            </div>
                            <div class=""infobox-data"">
								<span class=""infobox-text"">{1}</span>
								<div class=""infobox-content""><span class=""bigger-110"">Han usado SPADLand</span></div>
                            </div>
						</div>

                        <div class=""infobox infobox-blue2"">
                            <div class=""infobox-progress"">
							    <div class=""easy-pie-chart percentage"" data-percent=""{5}"" data-size=""46"" style=""height: 46px; width: 46px; line-height: 45px;"">
								    <span class=""percent"">{5}</span>%
									<canvas height = ""46"" width=""46""></canvas>
								</div>
                            </div>
                            <div class=""infobox-data"">
								<span class=""infobox-text"">{2}</span>
								<div class=""infobox-content""><span class=""bigger-110"">El último mes</span></div>
                            </div>
                            <div class=""stat stat-{9}"">{7}</div>
						</div>
                                                                                      
                        <div class=""infobox infobox-blue2"">
                            <div class=""infobox-progress"">
							    <div class=""easy-pie-chart percentage"" data-percent=""{6}"" data-size=""46"" style=""height: 46px; width: 46px; line-height: 45px;"">
								    <span class=""percent"">{6}</span>%
									<canvas height = ""46"" width=""46""></canvas>
								</div>
                            </div>
                            <div class=""infobox-data"">
								    <span class=""infobox-text"">{3}</span>
									<div class=""infobox-content""><span class=""bigger-110"">La última semana</span></div>
                            </div>
                            <div class=""stat stat-{10}"">{8}</div>
						</div>",
                        a,
                        t,
                        m,
                        w,
                        t * 100 / a,
                        m * 100 / a,
                        w * 100 / a,
                        m - t,
                        w - m,
                        m < t ? "important" : "success",
                        w < m ? "important" : "success");
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }
    }

    private void Presupuesto()
    {
        using (var cmd = new SqlCommand("ASPADLand_Admin_PresupuestosEjecutados"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    var a = 0;
                    var t = 0;
                    var m = 0;
                    var w = 0;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (rdr.GetString(1) == "A") { a = rdr.GetInt32(0); }
                            if (rdr.GetString(1) == "T") { t = rdr.GetInt32(0); }
                            if (rdr.GetString(1) == "M") { m = rdr.GetInt32(0); }
                            if (rdr.GetString(1) == "W") { w = rdr.GetInt32(0); }
                        }
                    }

                    this.PresupuestosGraph = string.Format(
                        CultureInfo.InvariantCulture,
                        @"

                        <div class=""infobox infobox-blue2"">
                            <div class=""infobox-progress"">
							    <div class=""easy-pie-chart percentage"" data-percent=""{5}"" data-size=""46"" style=""height: 46px; width: 46px; line-height: 45px;"">
								    <span class=""percent"">{5}</span>%
									<canvas height = ""46"" width=""46""></canvas>
								</div>
                            </div>
                            <div class=""infobox-data"">
								<span class=""infobox-text"">{2}</span>
								<div class=""infobox-content""><span class=""bigger-110"">El último mes</span></div>
                            </div>
						</div>
                                                                                      
                        <div class=""infobox infobox-blue2"">
                            <div class=""infobox-progress"">
							    <div class=""easy-pie-chart percentage"" data-percent=""{6}"" data-size=""46"" style=""height: 46px; width: 46px; line-height: 45px;"">
								    <span class=""percent"">{6}</span>%
									<canvas height = ""46"" width=""46""></canvas>
								</div>
                            </div>
                            <div class=""infobox-data"">
								    <span class=""infobox-text"">{3}</span>
									<div class=""infobox-content""><span class=""bigger-110"">La última semana</span></div>
                            </div>
						</div>",
                        a,
                        t,
                        m,
                        w,
                        t * 100 / a,
                        m * 100 / t,
                        w * 100 / t,
                        m - t,
                        w - m,
                        m < t ? "important" : "success",
                        w < m ? "important" : "success");

                    this.LtTotalPresupuestos.Text = t.ToString();
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }
    }

    private void Busquedas()
    {
        int total = 0;
        var data = new List<busqueda>();
        using (var cmd = new SqlCommand(@"SELECT B.[Date], B.[ColectivoId], A.Name
        FROM[ASPAD_MSCRM].[dbo].[AspadLand_Traces] AS B WITH(NOLOCK)
        INNER JOIN account A WITH(NOLOCK)
        ON A.AccountId = B.ColectivoId WHERE Type = 9 AND A.AccountId <> '5D1AFCF7-77B1-E211-BD57-0050568A7D4D'"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Connection = cnn;
                    cmd.Connection.Open();
                    using(var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            total++;
                            var colectivoId = rdr.GetGuid(1);
                            var name = rdr.GetString(2);
                            var date = rdr.GetDateTime(0);

                            var m = date > DateTime.Now.AddDays(-30) ? 1 : 0;
                            var w = date > DateTime.Now.AddDays(-7) ? 1 : 0;

                            if(!data.Any(d=>d.id == colectivoId))
                            {
                                data.Add(new busqueda
                                {
                                    id = colectivoId,
                                    name = name,
                                    date = date,
                                    t = 1,
                                    m = m,
                                    w = w
                                });
                            }
                            else
                            {
                                var temp = new List<busqueda>();
                                foreach (var d1 in data)
                                {
                                    if (d1.id == colectivoId)
                                    {
                                        var d2 = new busqueda
                                        {
                                            t = d1.t + 1,
                                            m = d1.m + m,
                                            w = d1.w + w,
                                            id = colectivoId,
                                            name = name
                                        };

                                        temp.Add(d2);
                                    }
                                    else
                                    {
                                        temp.Add(d1);
                                    }
                                }

                                data = temp;
                            }
                        }
                    }
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        data = data.OrderBy(d => d.name).ToList();
        var table = new StringBuilder();
        var res = new StringBuilder();
        bool first = true;
        foreach(var d in data)
        {
            table.AppendFormat(CultureInfo.InstalledUICulture, @"<tr><td>{0}</td><td align=""right"">{1}</td></tr>", d.name, d.t);

            if (first)
            {
                first = false;
            }
            else
            {
                res.Append(",");
            }

            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"{{""label"": ""{0}"", ""value"": {1} }}",
                d.name,
                d.t);
        }

        this.LtBusquedaData.Text = table.ToString();
        this.LtBusquedasTotal.Text = total.ToString();
        this.BusquedasScript = res.ToString();

    }

    private struct busqueda
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public int t { get; set; }
        public int m { get; set; }
        public int w { get; set; }
    }
}