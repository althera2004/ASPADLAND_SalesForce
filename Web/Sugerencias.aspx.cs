using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sugerencias : Page
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
            return Colectivo.JsonList(Colectivo.All);
        }
    }

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

        this.master.AddBreadCrumb(ApplicationDictionary.Translate("Item_Sugerencias"));
        this.master.Titulo = ApplicationDictionary.Translate("Item_Sugerencias");
        this.master.SearcheableItems = "[]";

        this.RenderSugerencias();
    }

    private void RenderSugerencias()
    {
        var res = new StringBuilder(string.Format(
            CultureInfo.InvariantCulture,
            @"<tr><td colspan=""2"" style=""font-size:14px;""><i>{0}</i></td></tr>",
            ApplicationDictionary.Translate("Item_Sugerencias_TableNoData")));

        /*var query = string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT  S.Id, S.CentroId, S.Text, S.date
                    FROM AspadLandSugerencias S WITH (NOLOCK)
                    WHERE S.CentroId = '{0}' ORDER BY S.Date DESC",
                this.user.Id);

        using (var cmd = new SqlCommand("ASPADLand_Comunicaciones_ByCentro"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@CentroId", this.user.Id));

                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            res = new StringBuilder();
                            while (rdr.Read())
                            {
                                var tipo = "Envío a ASPAD";
                                switch (rdr.GetInt32(1))
                                {
                                    case 1: tipo = "Cambios en el cuadro médico"; break;
                                    case 2: tipo = "Cambio de horario"; break;
                                    case 3: tipo = "Envío de sugerencia"; break;
                                    case 4: tipo = "Cambios de datos del centro"; break;
                                }

                                res.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    @"
                                    <tr id=""{3}"">
                                        <td id=""{3}_btn"" style=""width:30px;text-align:center;"" onclick=""Toggle(this);"">+</td>
                                        <td>{2}</td><td style=""width:120px;text-align:center"">{1:dd/MM/yyy}</td></tr>
                                    <tr id=""{3}_data"" class=""trData"" style=""display:none;""><td colspan=""3"">{0}</td>",
                                    rdr.GetString(3),
                                    rdr.GetDateTime(2),
                                    tipo,
                                    rdr.GetInt64(0));
                            }
                        }
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

        res.AppendFormat(
            CultureInfo.InvariantCulture,
            @"
            <tr id=""{3}"">
                <td id=""{3}_btn"" style=""width:30px;text-align:center;"" onclick=""Toggle(this);"">+</td>
                <td>{2}</td><td style=""width:120px;text-align:center"">{1:dd/MM/yyy}</td></tr>
            <tr id=""{3}_data"" class=""trData"" style=""display:none;""><td colspan=""3"">{0}</td>",
            "Falta consulta en documentación Konozca",
            DateTime.Now,
            "Develop",
           0);

        this.LtSugerenciasList.Text = res.ToString();
    }
}