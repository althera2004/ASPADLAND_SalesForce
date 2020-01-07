using System;
using System.Collections.Generic;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework;

public partial class BusquedaUsuarios : Page
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

    public string ColectivoId { get; private set; }

    public string PolizaId { get; private set; }

    public string AseguradoId { get; private set; }

    public string Colectivos
    {
        get
        {
            return Colectivo.JsonList(Colectivo.All);
        }
    }

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
        this.user = (ApplicationUser)Session["User"];
        Session["User"] = this.user;
        this.master = this.Master as Main;
        this.company = Session["Company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.master.AddBreadCrumb(this.Dictionary["Item_BusquedaUsuarios"]);
        this.master.Titulo = this.Dictionary["Item_BusquedaUsuarios"];
        this.master.SearcheableItems = "[]";

        var codedQuery = new CodedQuery();
        codedQuery.SetQuery(this.Request.QueryString);
        this.ColectivoId = codedQuery.GetByKey<string>("colectivoId");
        this.PolizaId = codedQuery.GetByKey<string>("polizaId");
        this.AseguradoId = codedQuery.GetByKey<string>("aseguradoId");

        if (string.IsNullOrEmpty(this.ColectivoId))
        {
            this.Response.Redirect("DashBoard.aspx", Constant.EndResponse);
        }
    }
}