using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework;

public partial class AseguradosList : Page
{
    /// <summary>Master of page</summary>
    private Main master;

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

    public string ValidacionesJson { get; private set; }

    public string ColectivoId { get; private set; }

    public string NIF { get; private set; }
    public string Poliza { get; private set; }

    public string Colectivos
    {
        get
        {
            return Colectivo.JsonList(Colectivo.All);
        }
    }

    public string Colas { get; private set; }

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null)
        {
            this.Response.Redirect("NoSession.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.Go();
        }
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.user = Session["User"] as ApplicationUser;
        Session["User"] = this.user;
        this.master = this.Master as Main;
        this.company = Session["Company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.master.AddBreadCrumb(this.Dictionary["Item_Asegurados"]);
        this.master.Titulo = this.Dictionary["Item_Asegurados_Title"];
        this.master.SearcheableItems = Constant.EmptyJsonList;

        var codedQuery = new CodedQuery();
        codedQuery.SetQuery(this.Request.QueryString);
        this.ColectivoId = codedQuery.GetByKey<string>("colectivoId");
        this.NIF = codedQuery.GetByKey<string>("dni");
        this.Poliza = codedQuery.GetByKey<string>("poliza");
        this.RenderData();
    }

    public void RenderData()
    {
        var res = new StringBuilder();
        var query = string.Format(
            CultureInfo.InvariantCulture,
            @"select DISTINCT
	            AR.qes_AseguradoIdName,
	            AR.qes_PolizaIdName,
	            POL.qes_ColectivoIdName,
	            POL.qes_ProductoIdName
            FROM qes_actorealizado AR WITH(NOLOCK)
            INNER JOIN qes_poliza POL
            ON	POL.qes_polizaId = AR.qes_PolizaId
            AND	POL.statecode = 0
            AND POL.statuscode = 1
            AND AR.statecode = 0
            AND AR.statuscode = 1

            WHERE
	            AR.qes_CentroId = '{0}'

            ORDER BY AR.qes_AseguradoIdName, AR.qes_PolizaIdName",
            this.user.Id,
            ConfigurationManager.AppSettings["DiasValidacion"].ToString());

        var count = 0;
        using (var cmd = new SqlCommand(query))
        {
            cmd.CommandType = CommandType.Text;
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"<tr><td>{0}</td><td style=""width:200px;"">{1}</td><td style=""width:250px;"">{2}</td><td style=""width:261px;"">{3}</td></tr>",
                                rdr.GetString(0),
                                rdr.GetString(1),
                                rdr.GetString(2),
                                rdr.GetString(3));
                            count++;
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

        this.LtAsegurados.Text = res.ToString();
        this.LtAseguradosCount.Text = count.ToString();
    }
}