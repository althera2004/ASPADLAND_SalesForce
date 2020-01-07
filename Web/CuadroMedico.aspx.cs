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

public partial class CuadroMedico : Page
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

    public string ChangeMessage
    {
        get
        {
            return ApplicationDictionary.Translate("Item_CuadroMedico_ChangeDataMessage").Replace("#", user.Telefono1);
        }
    }

    public string ColectivoId { get; private set; }

    public string Colectivos
    {
        get
        {
            return Colectivo.JsonList(Colectivo.All);
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

        this.master.AddBreadCrumb(ApplicationDictionary.Translate("Item_CuadroMedico"));
        this.master.Titulo = ApplicationDictionary.Translate("Item_CuadroMedico");
        this.master.SearcheableItems = "[]";
        this.RenderCuadroMedico();
    }

    private void RenderCuadroMedico()
    {
        var res = new StringBuilder();
        var json = new StringBuilder("[");
        var actos = this.Session["Actos"] as ReadOnlyCollection<Acto>;

        var list = actos.OrderBy(a => a.EspecialidadName).ThenBy(a=>a.Description).ToList();
        int cont = 1;
        var especialidadName = string.Empty;
        bool first = true;

        foreach(var acto in list)
        {
            if(especialidadName != acto.EspecialidadName)
            {
                if (!string.IsNullOrEmpty(especialidadName))
                {
                    res.Append("</div></div></div>");
                }
                res.Append(RenderHeader(acto.EspecialidadName, "e" + cont.ToString()));
                especialidadName = acto.EspecialidadName;
                cont ++;
            }

            res.Append(RenderActo(acto));

            if (acto.Ofertado)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    json.Append(",");
                }

                json.AppendFormat(CultureInfo.InvariantCulture, @"""{0}""", acto.Id);
            }
        }

        res.Append("</div></div></div></div>");
        json.Append("]");
        this.ActualActos = json.ToString();
        this.LtCuadroMedico.Text = res.ToString();
    }

    private string RenderHeader(string especialidad, string especialidadId)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            @"<div class=""panel panel-default""><div class=""panel-heading"">
                  <h4 class=""panel-title"">
                      <a class=""accordion-toggle collapsed"" data-toggle=""collapse"" data-parent=""#accordion"" href=""#{0}"">
                          <i class=""ace-icon fa fa-angle-down bigger-110"" data-icon-hide=""ace-icon fa fa-angle-down"" data-icon-show=""ace-icon fa fa-angle-right""></i>
                          &nbsp;{1}
                      </a></h4></div><div class=""panel-collapse collapse"" id=""{0}"">
                                            <div class=""panel-body"">",
            especialidadId,
            especialidad);
    }

    private string RenderActo(Acto acto)
    {
        var actoChecked = acto.Ofertado ? " checked=\"checked\"" : string.Empty;
        return string.Format(
            CultureInfo.InvariantCulture,
            @"<div class=""col col-xs-4"" style=""margin-bottom:8px;""><input class=""ckacto"" type=""checkbox"" {0} id=""{1}"">&nbsp;{2}</div>",
            actoChecked,
            acto.Id,
            acto.Description);
    }
}