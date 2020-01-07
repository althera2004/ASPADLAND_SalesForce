// --------------------------------
// <copyright file="AdminConfiguracion.aspx.cs" company="OpenFramework">
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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class AdminConfiguration : Page
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
        this.master.AddBreadCrumbInvariant(ApplicationDictionary.Translate("Admin_Configuration"));
        this.master.Titulo = ApplicationDictionary.Translate("Admin_Configuration");
        this.master.TitleInvariant = true;
        this.RenderActos();
    }

    private void RenderActos()
    {
        var res = new StringBuilder();
        using(var cmd = new SqlCommand("ASPADLand_ActosRepetibles_Get"))
        {
            using(var cnn= new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while(rdr.Read())
                        {
                            res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"<option value""{0}""{3}>{1} / {2}</option>",
                                rdr.GetGuid(0),
                                rdr.GetString(1),
                                rdr.GetString(2),
                                rdr.GetInt32(3) == 1 ? " selected=\"selected\"" : string.Empty);
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

        this.LtActos.Text = res.ToString();
    }
}