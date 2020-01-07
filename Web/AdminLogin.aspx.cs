// --------------------------------
// <copyright file="AdminLogin.aspx.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
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
using SbrinnaCoreFramework.UI;

public partial class AdminLogin : Page
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
        this.master.AddBreadCrumbInvariant(this.Dictionary["Admin_Login"]);
        this.master.Titulo = this.Dictionary["Admin_Login"];
        this.master.TitleInvariant = true;
        this.RenderTables();
    }

    private void RenderTables()
    {
        var lastMonth = new List<TableRow>();
        var lastWeek = new List<TableRow>();
        var never = new List<TableRow>();

        using(var cmd = new SqlCommand("ASPADLand_Admin_LastConnection"))
        {
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    using(var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var newRow = new TableRow
                            {
                                CentroId = rdr.GetGuid(0),
                                Centro = rdr.GetString(1),
                                Codigo = rdr.GetInt32(3)
                            };

                            if (!rdr.IsDBNull(2))
                            {
                                newRow.Date = rdr.GetDateTime(2);
                            }

                            if (newRow.Date.HasValue)
                            {
                                if(newRow.Date > DateTime.Now.AddDays(-7))
                                {
                                    lastMonth.Add(newRow);
                                    lastWeek.Add(newRow);
                                }
                                else if (newRow.Date > DateTime.Now.AddDays(-30))
                                {
                                    lastMonth.Add(newRow);
                                }
                            }
                            else
                            {
                                never.Add(newRow);
                            }
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

        this.LtBodyLastMonthCount.Text = lastMonth.Count.ToString();
        this.LtBodyLastWeekCount.Text = lastWeek.Count.ToString();
        this.LtBodyNeverCount.Text = never.Count.ToString();

        var res = new StringBuilder();
        foreach(var row in never)
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr><td style=""width:50px;"">{0}</td><td>{1}</td><td style=""width:110px;text-align:center"">{2:dd/MM/yyy}</td></tr>",
                row.Codigo,
                row.Centro,
                row.Date);
        }
        this.LtBodyNever.Text = res.ToString();

        res = new StringBuilder();
        foreach (var row in lastMonth)
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr><td style=""width:50px;"">{0}</td><td>{1}</td><td style=""width:110px;text-align:center"">{2:dd/MM/yyy}</td></tr>",
                row.Codigo,
                row.Centro,
                row.Date);
        }
        this.LtBodyLastMonth.Text = res.ToString();

        res = new StringBuilder();
        foreach (var row in lastWeek)
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr><td style=""width:50px;"">{0}</td><td>{1}</td><td style=""width:110px;text-align:center"">{2:dd/MM/yyy}</td></tr>",
                row.Codigo,
                row.Centro,
                row.Date);
        }
        this.LtBodyLastWeek.Text = res.ToString();
    }

    private struct TableRow
    {
        public Guid CentroId { get; set; }
        public string Centro { get; set; }
        public DateTime? Date { get; set; }
        public int Codigo { get; set; }
    }
}