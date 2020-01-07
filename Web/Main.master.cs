// --------------------------------
// <copyright file="Main.master.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using SbrinnaCoreFramework.UI;
using AspadLandFramework;
using AspadLandFramework.UserInterface;
using System.Data.SqlClient;
using System.Data;

/// <summary>Implements the master page of application</summary>
public partial class Main : MasterPage
{
    /// <summary>List of navigation history urls</summary>
    private List<string> navigation;

    public string Version
    {
        get
        {
            return ConfigurationManager.AppSettings["Version"];
        }
    }

    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public int PendentTasks { get; set; }

    public string IssusVersion
    {
        get
        {
            return ConfigurationManager.AppSettings["issusversion"];
        }
    }

    public string BK
    {
        get
        {
            return Session["BK"] as string;
        }
    }

    /// <summary>Gets the navigation history</summary>    
    public string NavigationHistory
    {
        get
        {
            var res = new StringBuilder(Environment.NewLine).Append("<!-- Havigation history -->").Append(Environment.NewLine);
            /*foreach (string link in this.navigation)
            {
                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "    {0}{1}<br />", link, Environment.NewLine));
            }*/

            res.Append("<!-- -->").Append(Environment.NewLine);
            return res.ToString();
        }
    }

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Indicates if the title of page is translatable</summary>
    private bool titleInvariant;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    private Collection<BreadcrumbItem> breadCrumb;

    public string ItemCode { get; set; }

    /// <summary>Gets the JSON representation of appliction logged user</summary>
    public string ApplicationUserJson
    {
        get
        {
            return this.user.Json;
        }
    }

    /// <summary>Gets or sets a JSON list of searcheable items</summary>
    public string SearcheableItems { get; set; }

    /// <summary>Gets the url of referrer page</summary>
    public string Referrer
    {
        get
        {
            if(Session["Navigation"] == null)
            {
                this.navigation = new List<string>();
            }
            else
            {
                this.navigation = Session["Navigation"] as List<string>;
            }

            string actual = this.Request.RawUrl;
            string res = string.Empty;

            if (actual.ToUpperInvariant().IndexOf("DASHBOARD.ASPX") != -1)
            {
                var newNavigation = new List<string>
                {
                    "Dashboard.aspx"
                };
                Session["Navigation"] = newNavigation;
                return "DashBoard.aspx";
            }

            string last1 = string.Empty;
            string last2 = string.Empty;
            string last3 = string.Empty;

            if (this.navigation.Count > 2)
            {
                last1 = this.navigation[this.navigation.Count - 1];
                last2 = this.navigation[this.navigation.Count - 2];
                last3 = this.navigation[this.navigation.Count - 3];
            }
            else if (this.navigation.Count > 1)
            {
                last1 = this.navigation[this.navigation.Count - 1];
                last2 = this.navigation[this.navigation.Count - 2];
            }
            else if (this.navigation.Count == 1)
            {
                last1 = this.navigation[0];
            }

            if (actual == last1)
            {
                this.navigation.RemoveAt(this.navigation.Count - 1);
                res = last2;
            }
            else if (actual == last2)
            {
                this.navigation.RemoveAt(this.navigation.Count - 1);
                this.navigation.RemoveAt(this.navigation.Count - 1);
                res = last3;
            }
            else if (last1.IndexOf(".aspx?id=-1") != -1)
            {
                this.navigation.RemoveAt(this.navigation.Count - 1);
                res = last2;
            }
            else
            {
                res = last1;
            }

            if (actual != res)
            {
                this.navigation.Add(actual);
            }

            Session["Navigation"] = this.navigation;


            return res;
        }
    }

    /// <summary>Gets or sets a value indicating whether if is an administration pagesummary>
    public bool AdminPage { get; set; }

    /// <summary>Gets de breadcrumb elements</summary>
    public Collection<BreadcrumbItem> BreadCrumb
    {
        get
        {
            return this.breadCrumb;
        }
    }

    /// <summary>Gets a value indicating whether if the text of title is translatable</summary>
    public bool TitleInvariant
    {
        get
        {
            return this.titleInvariant;
        }

        set
        {
            this.titleInvariant = value;
        }
    }

    public ApplicationUser ApplicationUser
    {
        get
        {
            return this.user;
        }
    }

    /// <summary>Gets the HTML code for breadcrumb object</summary>
    public string RenderBreadCrumb
    {
        get
        {
            var res = new StringBuilder("<li><i class=\"fas fa-home\"></i><a href=\"DashBoard.aspx\">");
            res.Append(this.Dictionary["Common_Home"]);
            res.Append("</a></li>");

            foreach (var item in this.breadCrumb)
            {
                string label = item.Label;
                if (item.Leaf)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"<li class=""active"">{0}</li>",
                        label);
                }
                else
                {
                    string link = "#";
                    if (!string.IsNullOrEmpty(item.Link))
                    {
                        link = item.Link;
                    }

                    res.Append("<li><a href=\"").Append(link).Append("\" title=\"").Append(label).Append("\">").Append(this.dictionary[item.Label]).Append("</a></li>");
                }
            }

            return res.ToString();
        }
    }

    public string UserId
    {
        get
        {
            return this.user.Id.ToString();
        }
    }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    public string Titulo { get; set; }

    public UIButton ButtonNewItem { get; set; }

    public string ButtonNewItemHtml
    {
        get
        {
            if (this.ButtonNewItem != null)
            {
                return this.ButtonNewItem.Render;
            }

            return string.Empty;
        }
    }

    public void AddBreadCrumb(string label)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem() { Link = "#", Label = label, Leaf = true };
        this.breadCrumb.Add(newBreadCrumb);
    }

    public void AddBreadCrumbInvariant(string label)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem { Link = "#", Label = label, Leaf = true, Invariant = true };
        this.breadCrumb.Add(newBreadCrumb);
    }

    public void AddBreadCrumb(string label, bool leaf)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem { Link = "#", Label = label, Leaf = leaf };
        this.breadCrumb.Add(newBreadCrumb);
    }

    public void AddBreadCrumb(string label, string link, bool leaf)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem { Link = link, Label = label, Leaf = leaf };
        this.breadCrumb.Add(newBreadCrumb);
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.LtBuild.Text = ConfigurationManager.AppSettings["Version"];
        if (this.Session["User"] == null)
        {
            this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }

        this.Session["LastTime"] = DateTime.Now;
        this.navigation = Session["Navigation"] as List<string>;

        this.user = Session["User"] as ApplicationUser;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

        // Renew session
        //// ---------------------------------
        this.Session["User"] = this.user;
        this.Session["Dictionary"] = this.dictionary;
        //// ---------------------------------

        this.Alerts();
    }

    /// <summary>Render alerts items on top menu</summary>
    private void Alerts()
    {
        var res = new StringBuilder("");

        var query = string.Format(
            CultureInfo.InvariantCulture,
            @"SELECT
	            CV.qes_colavalidacionId,
	            ISNULL(CV.qes_Poliza,''),
	            CV.qes_name,
	            CV.qes_dni,
	            CV.qes_ColectivoId,
	            CV.qes_ColectivoIdName,
	            CV.qes_codigo,
	            ISNULL(CV.qes_descripcion,''),
	            ISNULL(CV.qes_telephone1,''),
	            CV.qes_Urgente,
	            CV.qes_fechainicio,
	            CV.qes_fechafin,
                CV.statuscode,
                CV.qes_AseguradoId,
                CV.qes_AseguradoIdName
            FROM qes_colavalidacion CV WITH(NOLOCK)
            WHERE 
	            CV.qes_centroid = '{0}'
            --AND CV.statecode = 0
            --AND CV.qes_Poliza IS NOT NULL
            AND CV.qes_fechainicio > GETDATE()-1",
            this.user.Id);

        /*using (var cmd = new SqlCommand(query))
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
                        var cont = 0;
                        while (rdr.Read())
                        {
                            cont++;
                            if (cont < 7)
                            {
                                string fechaInicio = string.Empty;
                                if (!rdr.IsDBNull(10))
                                {
                                    fechaInicio = string.Format(
                                        CultureInfo.InstalledUICulture,
                                        "{0:dd/MM/yyyy}",
                                        rdr.GetDateTime(10));
                                }

                                string fechaFinal = string.Empty;
                                if (!rdr.IsDBNull(11))
                                {
                                    fechaFinal = string.Format(
                                        CultureInfo.InstalledUICulture,
                                        "{0:dd/MM/yyyy}",
                                        rdr.GetDateTime(11));
                                }

                                var aseguradoId = Guid.Empty;
                                var aseguradoName = string.Empty;

                                if (!rdr.IsDBNull(13))
                                {
                                    aseguradoId = rdr.GetGuid(13);
                                }

                                if (!rdr.IsDBNull(14))
                                {
                                    aseguradoName = SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(14));
                                }

                                var statusText = "Pendiente";
                                var background = "#fff";
                                var onclick = string.Empty;
                                switch (rdr.GetInt32(12))
                                {
                                    case 1: statusText = "Aprobada"; background = "#b9fcb5"; break;
                                    case 282310003: statusText = "Coincidente"; background = "#b5c8fc"; break;
                                    case 282310000: statusText = "Denegada"; background = "#fcb5b5"; break;
                                }

                                if(rdr.GetInt32(12)==1 || rdr.GetInt32(12)== 282310003)
                                {
                                    onclick = " onclick=\"GoBusquedaUsuarios('" + rdr.GetGuid(4) + "', '" + rdr.GetString(3) + "', '" + rdr.GetString(1) + "');\"";
                                }

                                res.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    @"<li style=""margin:0:background:{7}""{8}>
                                    <a href=""#"">
                                        <strong>{0}: {6}</strong><br />
                                        <div class=""clearfix"">
                                            <span class=""pull-left"">
                                                <table border=""0""><tbody><tr><td><span style=""width:150px;white-space:normal;""><strong>{2}</strong></span><br />D.N.I./N.I.F.: <strong>{3}</strong><br />Poliza: <strong>{1}</strong><br />Colectivo: <strong>{5}</strong></td></tr></tbody></table></span>
                                        </div>
                                    </a>
                                    </li>",
                                    statusText,
                                    rdr.GetString(1).Trim(),
                                    SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(2)),
                                    rdr.GetString(3).Trim(),
                                    rdr.GetGuid(4),
                                    rdr.GetString(5).Trim(),
                                    rdr.GetString(6).Trim(),
                                    background,
                                    onclick);
                            }
                        }

                        this.LtAlertasCount.Text = cont.ToString();
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
        }*/

        this.LtAlertas.Text = res.ToString();
    }
}