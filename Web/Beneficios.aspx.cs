using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspadLandFramework;
using AspadLandFramework.Item;

public partial class Beneficios : Page
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

    public string Colectivos
    {
        get
        {
            return Session["ColectivosASPADJson"] as string;
        }
    }

    public string ActualActos { get; private set; }

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set;}

    public string UserJson
    {
        get
        {
            return this.user.Json;
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

        this.master.AddBreadCrumb(ApplicationDictionary.Translate("Item_Beneficios"));
        this.master.Titulo = ApplicationDictionary.Translate("Item_Beneficios");
        this.master.SearcheableItems = "[]";
    }
}