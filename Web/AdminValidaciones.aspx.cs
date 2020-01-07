// --------------------------------
// <copyright file="AdminValidaciones.aspx.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework.UI;
using ShortcutFramework.Item;

public partial class AdminValidaciones : Page
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
        this.master.AddBreadCrumbInvariant(this.Dictionary["Admin_Validations"]);
        this.master.Titulo = this.Dictionary["Admin_Validations"];
        this.master.TitleInvariant = true;
        this.RenderTables();
    }

    private void RenderTables()
    {
        var validaciones = Validaciones.All;
        var lastMonth = validaciones.Where(v => v.FechaInicio > DateTime.Now.AddDays(-31)).ToList();
        var lastWeek = validaciones.Where(v => v.FechaInicio > DateTime.Now.AddDays(-7)).ToList();



        this.LtBodyLastMonthCount.Text = lastMonth.Count.ToString();
        this.LtBodyLastWeekCount.Text = lastWeek.Count.ToString();
        this.LtBodyNeverCount.Text = validaciones.Count.ToString();

        var res = new StringBuilder();
        foreach (var row in validaciones)
        {
            var urg = row.Urgente ? @"<i class=""fas fa-check""></i>" : string.Empty;
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"
<tr>
    <td style=""width:40px;"" title=""{0}"">{7}</td>
    <td style=""width:70px;"">{1}</td>
    <td style=""width:50px;text-align:center;"">{8}</td>
    <td style=""width:90px;text-align:center"">{2:dd/MM/yyy}</td>
    <td style=""width:90px;"">{3}</td>
    <td style=""width:200px;"">{4}</td>
    <td>{5}</td>
    <td style=""width:200px;"">{6}</td>
</tr>",
                row.Status,
                row.Codigo,
                row.FechaInicio,
                row.Dni,
                row.Nombre,
                row.CentroName,
                row.Poliza,
                StatusIcon(row),
                urg);
        }
        this.LtBodyNever.Text = res.ToString();

        res = new StringBuilder();
        if (lastMonth.Count > 0)
        {
            foreach (var row in lastMonth)
            {
                var urg = row.Urgente ? @"<i class=""fas fa-check""></i>" : string.Empty;
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"
<tr>
    <td style=""width:40px;"" title=""{0}"">{7}</td>
    <td style=""width:70px;"">{1}</td>
    <td style=""width:50px;text-align:center;"">{8}</td>
    <td style=""width:90px;text-align:center"">{2:dd/MM/yyy}</td>
    <td style=""width:90px;"">{3}</td>
    <td style=""width:200px;"">{4}</td>
    <td>{5}</td>
    <td style=""width:200px;"">{6}</td>
</tr>",
                    row.Status,
                    row.Codigo,
                    row.FechaInicio,
                    row.Dni,
                    row.Nombre,
                    row.CentroName,
                    row.Poliza,
                StatusIcon(row),
                    urg);
            }
        }
        else
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr><td>No hay validaciones en los últimos 30 dias</td></tr>");
        }
        this.LtBodyLastMonth.Text = res.ToString();

        res = new StringBuilder();
        if (lastWeek.Count > 0)
        {
            foreach (var row in lastWeek)
            {
                var urg = row.Urgente ? @"<i class=""fas fa-check""></i>" : string.Empty;
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"
<tr>
    <td style=""width:40px;"" title=""{0}"">{7}</td>
    <td style=""width:70px;"">{1}</td>
    <td style=""width:50px;text-align:center;"">{8}</td>
    <td style=""width:90px;text-align:center"">{2:dd/MM/yyy}</td>
    <td style=""width:90px;"">{3}</td>
    <td style=""width:200px;"">{4}</td>
    <td>{5}</td>
    <td style=""width:200px;"">{6}</td>
</tr>",
                    row.Status,
                    row.Codigo,
                    row.FechaInicio,
                    row.Dni,
                    row.Nombre,
                    row.CentroName,
                    row.Poliza,
                StatusIcon(row),
                    urg);
            }
        }
        else
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr><td>No hay validaciones en los últimos 7 dias</td></tr>");
        }
        this.LtBodyLastWeek.Text = res.ToString();
    }

    private string StatusIcon(Validaciones row)
    {
        var iconPattern = "<i class=\"{0}\" title=\"{2}\" style=\"color:{1};font-size:18px;\"></i>";
        var color = "#bbb";
        var icon = "fas fa-question";
        var title = ApplicationDictionary.Translate("Item_Validaciones_Status_Pendiente");
        switch (row.Status)
        {
            case 1:
                color = "#3c3";
                icon = "fas fa-check";
                title = ApplicationDictionary.Translate("Item_Validaciones_Status_Aprobada");
                break;
            case 282310003:
                color = "#77f";
                icon = "fas fa-check";
                title = ApplicationDictionary.Translate("Item_Validaciones_Status_Coincidente");
                break;
            case 282310000:
                color = "#f77";
                icon = "fas fa-exclamation-triangle";
                title = ApplicationDictionary.Translate("Item_Validaciones_Status_Denegada");
                break;
        }

        return string.Format(
            CultureInfo.InvariantCulture,
            iconPattern,
            icon,
            color,
            title);
    }
}