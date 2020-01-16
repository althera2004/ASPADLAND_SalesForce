using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.DataAccess;

public partial class Presupuestos : Page
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

    public string Precios { get; private set; }

    public string ActosRealizados { get; private set; }

    public string Colectivos
    {
        get
        {
            return Session["ColectivosASPADJson"] as string;
        }
    }

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    public string PolizaId { get; private set; }

    public string MascotaId { get; private set; }

    public string TarifaId { get; private set; }

    public string AseguradoId { get; private set; }

    public string PolizaNum { get; private set; }

    public Mascota Mascota { get; private set; }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public string Chip { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Chip = string.Empty;
        if (this.Session["User"] == null)
        {
            this.Response.Redirect("NoSession.aspx", true);
        }
        else
        {
            this.Go();
        }

        Context.ApplicationInstance.CompleteRequest();
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.user = (ApplicationUser)Session["User"];
        this.master = this.Master as Main;
        this.company = Session["Company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;

        // Renew session
        Session["User"] = this.user;
        Session["Dictionary"] = this.Dictionary;

        this.master.AddBreadCrumb(ApplicationDictionary.Translate("Item_Presupuestos"));
        this.master.Titulo = ApplicationDictionary.Translate("Item_Presupuesto");
        this.master.SearcheableItems = "[]";

        var codedQuery = new CodedQuery();
        codedQuery.SetQuery(this.Request.QueryString);
        this.MascotaId = codedQuery.GetByKey<string>("mascotaId");
        this.Mascota = Mascota.ById(this.MascotaId);
        this.TarifaId = codedQuery.GetByKey<string>("tarifaId");
        this.ColectivoId = codedQuery.GetByKey<string>("colectivo");
        this.PolizaId = codedQuery.GetByKey<string>("polizaId");
        this.GetActos();
        this.RenderActosRealizados();

        var poliza = Poliza.ById(this.PolizaId);
        if (!string.IsNullOrEmpty(poliza.AseguradoNombre))
        {
            this.PolizaNum = poliza.AseguradoNombre + " - " + poliza.Numero;
        }
        else
        {
            this.PolizaNum = poliza.Numero;
        }
    }

    private void GetActos()
    {
        var res = new StringBuilder("[");
        var resCombo = new StringBuilder();
        var precios = Acto.ByTarifa(this.TarifaId).OrderBy(a => a.EspecialidadName);
        var especialidadIdActual = string.Empty;
        var first = true;
        foreach (var acto in precios)
        {
            var especialidadId = acto.EspecialidadName;
            if (especialidadId != especialidadIdActual)
            {
                if (especialidadIdActual != string.Empty)
                {
                    res.Append("]},");
                    res.Append(Environment.NewLine);
                }

                especialidadIdActual = especialidadId;
                first = true;
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":""{0}"",""Name"":""{1}"",""Actos"":[",
                    especialidadId,
                    especialidadId);
            }

            if (first)
            {
                first = false;
            }
            else
            {
                res.Append(",");
            }

            var amount = "null";
            var percentage = "null";

            if (acto.Precio != null)
            {
                amount = string.Format("{0:####0.00}", acto.Precio).Replace(',', '.');
            }

            if (acto.Porcentage != null)
            {
                percentage = string.Format("{0:####0.00}", acto.Porcentage).Replace(',', '.');
            }

            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"{{""Id"":""{0}"",""Name"":""{1}"",""Amount"":{2},""Discount"":{3},""E"":""{5}"",""T"":""{4}""}}",
                acto.Id,
                SbrinnaCoreFramework.Tools.JsonCompliant(acto.Description),
                amount,
                percentage,
                SbrinnaCoreFramework.Tools.JsonCompliant(TarifaId),
                SbrinnaCoreFramework.Tools.JsonCompliant(especialidadId));

            resCombo.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}"">{1} - {2}</option>",
                acto.Id,
                acto.EspecialidadName,
                acto.Description);
        }

        if (especialidadIdActual != string.Empty)
        {
            res.Append("]}");
        }

        res.Append("]");
        this.Precios = res.ToString();
        this.CmbEspecialidadItems.Text = resCombo.ToString();
    }

    private void RenderPresupuestos()
    {
        var res = new StringBuilder();
        using(var cmd = new SqlCommand("ASPADLAND_GetPresupuestoPendienteByMascotaId"))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(DataParameter.Input("@MascotaId", this.MascotaId));
            cmd.Parameters.Add(DataParameter.Input("@CentroId", this.user.Id));
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                try
                {
                    var actual = Guid.Empty;
                    cmd.Connection.Open();
                    using(var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (actual != Guid.Empty)
                            {
                                res.Append("</div></div>");
                            }

                            var presupuestoId = rdr.GetGuid(0);
                            if (presupuestoId != actual)
                            {
                                res.Append(RenderHeader(rdr.GetString(1), presupuestoId));
                                actual = presupuestoId;
                            }

                            var actoId = rdr.GetGuid(2);
                            string actoName = rdr.GetString(3);
                            string especialidad = rdr.GetString(4);
                            res.Append(RenderActo(actoId, actoName, especialidad, 1, 2, rdr.GetString(9)));
                        }

                        if (actual != Guid.Empty)
                        {
                            res.Append("</div></div>");
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

        this.LtPendientes.Text = res.ToString();
    }

    private string RenderHeader(string presupuestoCodigo, Guid presupuestoId)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            @"<div class=""panel panel-default""><div class=""panel-heading"">
                  <h4 class=""panel-title"">
                      <a class=""accordion-toggle collapsed"" data-toggle=""collapse"" data-parent=""#accordion"" href=""#{0}"">
                          <i class=""ace-icon fa fa-angle-down bigger-110"" data-icon-hide=""ace-icon fa fa-angle-down"" data-icon-show=""ace-icon fa fa-angle-right""></i>
                          &nbsp;{1}
                      </a>
                 </h4>
             </div>
             <div class=""panel-collapse collapse"" id=""{0}"">
                 <div class=""panel-body"">",
            presupuestoId,
            presupuestoCodigo);
    }

    private string RenderActo(Guid actoId, string especialidad, string acto, decimal amount, decimal discount, string observaciones)
    {
        string observacionesDiv = string.Empty;
        if (!string.IsNullOrEmpty(observacionesDiv))
        {
            observacionesDiv = string.Format(
                CultureInfo.InvariantCulture,
                @"
                 <div class=""col col-xs-12""><i>{0}</i></div>",
                observaciones);
        }

        return string.Format(
            CultureInfo.InvariantCulture,
            @"<div class=""col col-xs-12"" style=""margin-bottom:8px;"">
                <div class=""col col-xs-4"">{1}</div>
                <div class=""col col-xs-4"">{2}</div>
                <div class=""col col-xs-2"">{3}</div>{4}
            </div>",
            actoId,
            especialidad,
            acto,
            amount,
            observacionesDiv);
    }

    public void RenderActosRealizados()
    {
        var json = new StringBuilder("[");
        var res = new StringBuilder();
        var query = string.Format(
            CultureInfo.InvariantCulture,
            @"select
                    AR.qes_EspecialidadIdName,
                    AR.qes_ActoIdName,
                    ISNULL(AR.qes_importe_Base,0),
                    ISNULL(AR.qes_porcentaje,0),
	                AR.qes_fechaproceso,
                    ISNULL(AR.qes_descripcion,'')
                FROM qes_actorealizado AR WITH(NOLOCK)
                INNER JOIN qes_poliza POL
                ON	POL.qes_polizaId = AR.qes_PolizaId
                AND	POL.statecode = 0
                AND POL.statuscode = 1
                AND AR.statecode = 0
                AND AR.statuscode = 1

                WHERE
                    AR.qes_MascotaId = '{1}'
	            AND AR.qes_CentroId = '{0}'

                ORDER BY AR.qes_fechaproceso DESC",
            this.user.Id,
            this.MascotaId);

        var count = 0;
        /*using (var cmd = new SqlCommand(query))
        {
            cmd.CommandType = CommandType.Text;
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        bool first = true;
                        while (rdr.Read())
                        {
                            res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"<tr><td style=""width:300px;"">{0}</td><td>{1}{5}</td><td style=""width:90px;text-align:right;"">{2}&nbsp;</td><td style=""width:90px;text-align:right;"">{3}&nbsp;</td><td style=""width:90px;text-align:center"">{4:dd/MM/yyyy}</td></tr>",
                                rdr.GetString(0),
                                rdr.GetString(1),
                                rdr.IsDBNull(2) ? string.Empty : string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", rdr.GetDecimal(2)),
                                rdr.IsDBNull(3) ? string.Empty : string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", rdr.GetDecimal(3)),
                                rdr.GetDateTime(4).AddDays(0.5),
                                string.IsNullOrEmpty(rdr.GetString(5)) ? string.Empty : ("<br /><i>" + rdr.GetString(5) + "</i>"));
                            count++;

                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                json.Append(",");
                            }

                            json.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"{{""esp"":""{0}"",""acto"":""{1}"",""imp"":{2},""dto"":{3},""f"":""{4:dd/MM/yyyy}""}}",
                                rdr.GetString(0),
                                rdr.GetString(1),
                                rdr.IsDBNull(2) ? string.Empty : string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", rdr.GetDecimal(2)),
                                rdr.IsDBNull(3) ? string.Empty : string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", rdr.GetDecimal(3)),
                                rdr.GetDateTime(4).AddDays(0.5),
                                string.IsNullOrEmpty(rdr.GetString(5)) ? string.Empty : ("<br /><i>" + rdr.GetString(5) + "</i>"));
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

        json.Append("]");
        this.ActosRealizados = json.ToString();
        this.LtActosRealizados.Text = res.ToString();
        this.LtActosRealizadosCount.Text = count.ToString();
    }
}