using AspadLandFramework;
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

public partial class ProductoDental : Page
{
    /// <summary>Master of page</summary>
    private Main master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    /// <summary>User logged in</summary>
    private ApplicationUser user;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null)
        {
            this.Response.Redirect("NoSession.aspx", true);
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
        this.user = (ApplicationUser)Session["User"];
        Session["User"] = this.user;
        this.master = this.Master as Main;
        this.company = Session["Company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.master.AddBreadCrumb("Producto dental");
        this.master.Titulo = "Producto dental";

        this.RenderAsegurados();
    }

    /// <summary>Render html list of asegurados</summary>
    private void RenderAsegurados()
    {
        var res = new StringBuilder();

        var query = string.Format(
            CultureInfo.InvariantCulture,
            "select qes_name, ISNULL(aisa_nif,'') from qes_personadecontacto where qes_Cargo = 'ADE' and qes_clienteId = '{0}' AND statecode = 0 AND statuscode = 1 ",
            this.user.Id);

        int count = 0;
        using (var cmd = new SqlCommand(query))
        {
            cmd.CommandType = CommandType.Text;
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                try
                {
                    cmd.Connection.Open();
                    using(var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                count++;
                                res.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    @"<tr><td>{0}</td><td style=""width:120px;"">{1}</td></tr>",
                                    rdr.GetString(0),
                                    rdr.GetString(1));
                            }
                        }
                        else
                        {
                            res.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    @"<tr><td colspan=""2"" style=""font-size:14px;""><i>{0}</i></td></tr>",
                                    ApplicationDictionary.Translate("Item_ProductoDental_NoAsegurados"));
                        }
                    }
                }
                finally
                {
                    if(cmd.Connection.State != ConnectionState.Closed)
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