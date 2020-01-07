// --------------------------------
// <copyright file="DashBoard.aspx.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;

public partial class Customer_DashBoard : Page
{
    /// <summary>Master of page</summary>
    private Main master;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public string WarningProfileDisplay { get; private set; }

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
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master.AddBreadCrumb("Inicio");
        this.master.Titulo = "Inicio";
        this.RenderColectivos();

        this.WarningProfileDisplay = "none";
        if (string.IsNullOrEmpty(this.user.Telefono1)) { this.WarningProfileDisplay = "block"; }
        if (string.IsNullOrEmpty(this.user.Email1)) { this.WarningProfileDisplay = "block"; }
        if (string.IsNullOrEmpty(this.user.FacturacionEmail)) { this.WarningProfileDisplay = "block"; }
        if (string.IsNullOrEmpty(this.user.Poblacion)) { this.WarningProfileDisplay = "block"; }
        if (string.IsNullOrEmpty(this.user.CP)) { this.WarningProfileDisplay = "block"; }
        if (string.IsNullOrEmpty(this.user.Provincia)) { this.WarningProfileDisplay = "block"; }
    }

    private void RenderColectivos()
    {
        var res = new StringBuilder();
        var colectivos = Session["Colectivos"] as ReadOnlyCollection<Colectivo>;
        var colectivosASPAD = Session["ColectivosASPAD"] as ReadOnlyCollection<Colectivo>;
        if(colectivosASPAD != null)
        {
            foreach(var colectivo in colectivosASPAD.OrderBy(c=>c.Description))
            {
                if (colectivos.Any(c => c.Id == colectivo.Id))
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"<img src=""/logopolizas/{1}.png"" alt=""{1}"" title=""{1}"" id=""{0}"" onclick=""Go(this);"" style=""margin:20px;cursor:pointer;"" />",
                        colectivo.Id,
                        colectivo.Description);
                }
                else
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"<img src=""/logopolizas/{1}_bn.png"" alt=""{1}"" id=""{0}"" style=""margin:20px;cursor:pointer;"" class=""tooltip-error"" title=""{2}"" data-rel=""tooltip"" data-placement=""bottom"" />",
                        colectivo.Id,
                        colectivo.Description,
                        this.Dictionary["DashBoard_NOASPAD"]);
                }
            }
        }

        this.LtColectivos.Text = res.ToString();
    }
}