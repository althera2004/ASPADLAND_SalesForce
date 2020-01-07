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
            return Colectivo.JsonList(Colectivo.All);
        }
    }

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    public Guid PolizaId { get; private set; }

    public string MascotaId { get; private set; }

    public string AseguradoId { get; private set; }

    public string PolizaNum { get; private set; }

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
        this.GetData();
        this.GetActos();
        this.RenderActosRealizados();
        //this.RenderPresupuestos();
    }

    private void GetData()
    {
        var query = string.Format(
            CultureInfo.InvariantCulture,
            @"select
	            ISNULL(M.qes_name,'<i>sin nombre</i>'),
	            M.qes_Sexo,
	            M.qes_Tipomascota,
	            ISNULL(M.qes_NMicrochip,''),
	            P.qes_polizaId,
	            ISNULL(P.qes_name,''),
	            P.qes_AseguradoId,
                P.qes_AseguradoIdName,
	            P.qes_dni,
	            P.qes_ColectivoId,
	            P.qes_ColectivoIdName,
                P.qes_productoIdName
            FROM qes_mascotas M WITH(NOLOCK)
            INNER JOIN qes_poliza P WITH(NOLOCK)
            ON M.qes_PolizaId = P.qes_polizaId
            WHERE M.qes_mascotasId  = '{0}'",
            this.MascotaId);
        using (var cmd = new SqlCommand(query))
        {
            cmd.CommandType = CommandType.Text;
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                using(var rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {

                        rdr.Read();
                        this.AseguradoId = rdr.GetString(8).ToUpperInvariant();
                        this.PolizaNum = rdr.GetString(5).ToUpperInvariant();
                        this.PolizaId = rdr.GetGuid(4);
                        this.ColectivoId = rdr.GetGuid(9).ToString();
                        this.LtNombreMascota.Text = rdr.GetString(0);
                        this.LtMicrohip.Text = rdr.GetString(3);
                        this.Chip = rdr.GetString(3);
                        this.LtColectivo.Text = "<a title=\"Descargar baremo en formato PDF\" href=\"/Documentos/Tarifas/" + rdr.GetString(10).Replace(" ", string.Empty) + "_" + rdr.GetString(11).Replace(" ", string.Empty) + ".pdf\" target=\"_blank\" style=\"font-size:11px;\"><div id=\"TableBaremo\" style=\"display:inline-block;\">Ver baremo<br />" + rdr.GetString(10) + " / " + rdr.GetString(11) + "</div><div id=\"_TableBaremoIcon\" style=\"display:inline-block;margin-left:4px;margin-top:-4px;vertical-align:super;\"><img src=\"logopolizas/" + this.ColectivoId + ".png\" style=\"max-height:30px;\" /></div></a>";
                        this.LtAsegurado.Text = "<div onclick=\"GoBusquedaUsuarios(colectivoId, aseguradoId, polizaNum);\"><a href=\"#\">" + rdr.GetString(8).ToUpperInvariant() + " - " + rdr.GetString(7) + "<div style=\"font-size:14px;color:#333;margin-top:8px;\">Póliza:&nbsp;" + rdr.GetString(5) + "</a></div></div>";

                        if (!rdr.IsDBNull(1))
                        {
                            var sexo = rdr.GetInt32(1);
                            if (sexo == 100000000) { LtSexo.Text = "Macho"; }
                            if (sexo == 100000001) { LtSexo.Text = "Hembra"; }
                        }

                        if (!rdr.IsDBNull(2))
                        {
                            var tipo = rdr.GetInt32(2);
                            if (tipo == 100000000) { LtTipoMascota.Text = "Perro"; }
                            if (tipo == 100000001) { LtTipoMascota.Text = "Gato"; }
                        }
                    }
                }
            }
        }
    }

    private void GetActos()
    {
        var query = string.Format(
            CultureInfo.InvariantCulture,
            @"select DISTINCT
	            E.qes_especialidadId,
	            E.qes_especialidadIdName AS EspecialidadName,
	            E.ProductId,
	            PPL.ProductIdName,
	            PPL.Amount_Base,
	            PPL.Percentage,
	            PPL.PriceLevelId,
	            PPL.PriceLevelIdName
            from product E WITH(NOLOCK)
            INNER JOIN ProductPriceLevel PPL WITH(NOLOCK)
            ON	 PPL.ProductId = E.ProductId
            INNER JOIN qes_poliza POL
            ON	POL.qes_TarifaId = PPL.PriceLevelId
            AND POL.qes_polizaId = '{1}'
            INNER JOIN qes_centro_servicios PC WITH(NOLOCK)
	            INNER JOIN qes_centro_especialidad EC WITH(NOLOCK)
	            ON	EC.qes_EspecialidadId = PC.qes_EspecialidadId
	            AND	EC.qes_CentroId = PC.qes_CentroId
            ON PC.qes_especialidadId = E.qes_especialidadId
            AND PC.statecode = 0
            where
	            PC.qes_CentroId ='{0}'
            AND E.statecode = 0
	
            ORDER BY E.qes_especialidadIdName,PPL.ProductIdName",
            this.user.Id,
            this.PolizaId);
        var res = new StringBuilder("[");
        var resCombo = new StringBuilder();

        using(var cmd = new SqlCommand(query))
        {
            cmd.CommandType = CommandType.Text;
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                using(var rdr = cmd.ExecuteReader())
                {
                    var especialidadIdActual = Guid.Empty;
                    bool first = true;
                    while (rdr.Read())
                    {
                        var especialidadId = rdr.GetGuid(0);
                        if (especialidadId != especialidadIdActual)
                        {
                            if (especialidadIdActual != Guid.Empty)
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
                                rdr.GetString(1));
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

                        if (!rdr.IsDBNull(4))
                        {
                            amount = string.Format("{0:####0.00}", rdr.GetDecimal(4)).Replace(',', '.');
                        }

                        if (!rdr.IsDBNull(5))
                        {
                            percentage = string.Format("{0:####0.00}", rdr.GetDecimal(5)).Replace(',', '.');
                        }

                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"{{""Id"":""{0}"",""Name"":""{1}"",""Amount"":{2},""Discount"":{3},""T"":""{4}""}}",
                            rdr.GetGuid(2),
                            SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(3)),
                            amount,
                            percentage,
                            SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(7)));

                        resCombo.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"<option value=""{0}"">{1} - {2}</option>",
                            rdr.GetGuid(2),
                            rdr.GetString(1),
                            rdr.GetString(3));
                    }

                    if (especialidadIdActual != Guid.Empty)
                    {
                        res.Append("]}");
                    }
                }
            }
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
        using (var cmd = new SqlCommand(query))
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
        }

        json.Append("]");
        this.ActosRealizados = json.ToString();
        this.LtActosRealizados.Text = res.ToString();
        this.LtActosRealizadosCount.Text = count.ToString();
    }
}