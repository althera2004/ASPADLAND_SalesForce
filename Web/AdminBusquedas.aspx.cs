// --------------------------------
// <copyright file="Configuracion.aspx.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework.Activity;
using SbrinnaCoreFramework.UI;
using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;

public partial class AdminBusquedas : Page
{
    /// <summary> Master of page</summary>
    private AdminMaster master;

    /// <summary>User logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    private FormFooter formFooter;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    private int userId;

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.Dictionary);
        }
    }

    public ApplicationUser ApplicationUser
    {
        get
        {
            return this.user;
        }
    }

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public string ShowHelpChecked
    {
        get
        {
            return this.user.ShowHelp ? " checked=\"checked\"" : string.Empty;
        }
    }

    public string UserJson
    {
        get
        {
            return this.user.Json;
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.company = Session["company"] as Company;
        this.master = this.Master as AdminMaster;
        this.master.AddBreadCrumbInvariant(this.Dictionary["Admin_Searchs"]);
        this.master.Titulo = this.Dictionary["Admin_Searchs"];
        this.master.TitleInvariant = true;
    }
}