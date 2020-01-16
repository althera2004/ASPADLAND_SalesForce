using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DocumentsList : Page
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
            return Colectivo.JsonList(Colectivo.AllASPAD);
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

    public string DocumentosPrivados { get; private set; }
    public string DocumentosCentro { get; private set; }

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

        this.master.AddBreadCrumb(ApplicationDictionary.Translate("Item_Documentos"));
        this.master.Titulo = ApplicationDictionary.Translate("Item_Documentos");
        this.master.SearcheableItems = "[]";
        this.RenderDocumentsPrivados();
        this.RenderDocumentsCentro();
    }

    private void RenderDocumentsPrivados()
    {
        var res = new StringBuilder();
        string path = this.Request.PhysicalApplicationPath;
        if (!path.EndsWith("\\"))
        {
            path += "\\";
        }

        path += "Documentos\\" + (string.IsNullOrEmpty(this.user.Code) ? "0" : this.user.Code) + "\\";
		if(!Directory.Exists(path)){
			Directory.CreateDirectory(path);
		}
		
        string pattern = string.Format(CultureInfo.InvariantCulture, @"{0}_*.pdf", this.user.Code);
        var files = Directory.GetFiles(path, pattern);
		/*res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr>
                    <td style=""width:45px;text-align:center;""><img src=""/img/pdficon.png"" /></td>
                    <td><a target=""_blank"" href=""/DocsPrivados/{0}"">{0}</a></td></tr>",
                path);*/
        foreach(string file in files)
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr>
                    <td style=""width:50px;text-align:center;""><img src=""/img/pdficon.png"" /></td>
                    <td><a target=""_blank"" href=""/DocsPrivados/{0}"">{0}</a></td></tr>",
                Path.GetFileName(file));
        }

        this.DocumentosPrivados = res.ToString();
    }

    private void RenderDocumentsCentro()
    {
        var res = new StringBuilder();
        string path = this.Request.PhysicalApplicationPath;
        if (!path.EndsWith("\\"))
        {
            path += "\\";
        }

        path += "Documentos\\";
        var files = Directory.GetFiles(path, "*.pdf");
        foreach (string file in files)
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr>
                    <td style=""width:50px;text-align:center;""><img src=""/img/pdficon.png"" /></td>
                    <td><a target=""_blank"" href=""/Documentos/{0}"">{0}</a></td></tr>",
                Path.GetFileName(file));
        }

        this.DocumentosCentro = res.ToString();
    }
}