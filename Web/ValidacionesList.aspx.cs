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

public partial class ValidacionesList : Page
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

    public string ValidacionesJson { get; private set; }

    public string ColectivoId { get; private set; }

    public string NIF { get; private set; }
    public string Poliza { get; private set; }

    public string Colectivos
    {
        get
        {
            return "[]"; // Colectivo.JsonList(Colectivo.All);
        }
    }

    public string Colas { get; private set; }

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
            this.Response.Redirect("NoSession.aspx", Constant.EndResponse);
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
        this.user = Session["User"] as ApplicationUser;
        Session["User"] = this.user;
        this.master = this.Master as Main;
        this.company = Session["Company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.master.AddBreadCrumb(this.Dictionary["Item_Validaciones"]);
        this.master.Titulo = this.Dictionary["Item_Validaciones_Plural"];
        this.master.SearcheableItems = Constant.EmptyJsonList;

        var codedQuery = new CodedQuery();
        codedQuery.SetQuery(this.Request.QueryString);
        this.ColectivoId = codedQuery.GetByKey<string>("colectivoId");
        this.NIF = codedQuery.GetByKey<string>("dni");
        this.Poliza = codedQuery.GetByKey<string>("poliza");
        this.RenderData();
    }

    public void FillCmbColectivo()
    {
        var res = new StringBuilder();
        /*foreach(var colectivo in Colectivo.All)
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}"" {2}>{1}</option>",
                colectivo.Id,
                colectivo.Description,
                new Guid(ColectivoId) == colectivo.Id ? " selected=\"selected\"" : string.Empty);
        }*/

        this.cmbColectivoData.Text = res.ToString();
    }

    public void RenderData()
    {
        var res = new StringBuilder("[");
        /*var query = string.Format(
            CultureInfo.InvariantCulture,
            @"SELECT
	            CV.qes_colavalidacionId,
	            ISNULL(CV.qes_Poliza,''),
	            CV.qes_name,
	            CV.qes_dni,
	            CV.qes_ColectivoId,
	            CV.qes_ColectivoIdName,
	            CV.qes_codigo,
	            ISNULL(CV.qes_descripcion,''),
	            ISNULL(CV.qes_telephone1,''),
	            CV.qes_Urgente,
	            CV.qes_fechainicio,
	            CV.qes_fechafin,
                CV.statuscode,
                CV.qes_AseguradoId,
                CV.qes_AseguradoIdName
            FROM qes_colavalidacion CV WITH(NOLOCK)
            WHERE 
	            CV.qes_centroid = '{0}'
            --AND CV.statecode = 0
            --AND CV.qes_Poliza IS NOT NULL
            AND CV.qes_fechainicio > GETDATE()-{1}",
            this.user.Id,
            ConfigurationManager.AppSettings["DiasValidacion"].ToString());

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
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                res.Append(",");
                            }

                            string fechaInicio = string.Empty;
                            if (!rdr.IsDBNull(10))
                            {
                                fechaInicio = string.Format(CultureInfo.InstalledUICulture, "{0:dd/MM/yyyy}", rdr.GetDateTime(10));
                            }

                            string fechaFinal = string.Empty;
                            if (!rdr.IsDBNull(11))
                            {
                                fechaFinal = string.Format(CultureInfo.InstalledUICulture, "{0:dd/MM/yyyy}", rdr.GetDateTime(11));
                            }

                            var aseguradoId = Guid.Empty;
                            var aseguradoName = string.Empty;

                            if (!rdr.IsDBNull(13))
                            {
                                aseguradoId = rdr.GetGuid(13);
                            }

                            if (!rdr.IsDBNull(14))
                            {
                                aseguradoName = SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(14));
                            }

                            res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"{{""Id"":""{0}"",
                                ""Poliza"":""{1}"",
                                ""Asegurado"":""{2}"",
                                ""DNI"":""{3}"",
                                ""ColectivoId"":""{4}"",
                                ""ColectivoName"":""{5}"",
                                ""Codigo"":""{6}"",
                                ""Descripcion"":""{7}"",
                                ""Telefono"":""{8}"",
                                ""Urgente"":{9},
                                ""Inicio"":""{10:dd/MM/yyyy}"",
                                ""Fin"":""{11}"",
                                ""Status"":""{12}"",
                                ""AseguradoId"":""{13}"",
                                ""AseguradoName"":""{14}""}}",
                                rdr.GetGuid(0),
                                rdr.GetString(1).Trim(),
                                SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(2)),
                                rdr.GetString(3).Trim(),
                                rdr.GetGuid(4),
                                rdr.GetString(5).Trim(),
                                rdr.GetString(6).Trim(),
                                SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(7)).Replace('\n',' '),
                                rdr.GetString(8).Trim(),
                                rdr.GetBoolean(9) ? "true" : "false",
                                fechaInicio,
                                fechaFinal,
                                rdr.GetInt32(12),
                                aseguradoId,
                                aseguradoName);
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
        var query = string.Format(
             CultureInfo.InvariantCulture,
             @"select
                N_Cola__c,
                Asegurado_Cola__r.NIF__pc,
                Asegurado_Cola__r.Producto_ASPAD__r.Nombre_Compa_ia__c,
                Urg_Cola__c,
                Description,
                LastModifiedDate,
                Asegurado_Cola__r.Name,
                Asegurado_Cola__r.Phone,
                P_liza_Cola01__r.name,
                Status
                from case
                where RecordType.DeveloperName = 'Colas_de_validaci_n'", this.user.UserName);
                //and Usuario_ASPADLand__c = '{0}'", this.user.UserName);
        var binding = Session["SForceConnection"] as SforceService;
        var bindingResult = binding.query(query);

        if (bindingResult != null)
        {
            foreach (var record in bindingResult.records)
            {
                var cola = record as Case;
                res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"{{""Id"":""{0}"",
                                ""Poliza"":""{1}"",
                                ""Asegurado"":""{2}"",
                                ""DNI"":""{3}"",
                                ""ColectivoId"":""{4}"",
                                ""ColectivoName"":""{5}"",
                                ""Codigo"":""{6}"",
                                ""Descripcion"":""{7}"",
                                ""Telefono"":""{8}"",
                                ""Urgente"":{9},
                                ""Inicio"":""{10:dd/MM/yyyy}"",
                                ""Fin"":""{11}"",
                                ""Status"":""{12}"",
                                ""AseguradoId"":""{13}"",
                                ""AseguradoName"":""{14}""}}",
                                0,
                                cola.P_liza_Cola01__r.Name,
                                SbrinnaCoreFramework.Tools.JsonCompliant(cola.Asegurado_Cola__r.Name) + "???",
                                cola.Asegurado_Cola__r.NIF__pc.Trim(),
                                string.Empty,
                                cola.Asegurado_Cola__r.Producto_ASPAD__r.Nombre_Compa_ia__c,
                                cola.N_Cola__c.Trim(),
                                SbrinnaCoreFramework.Tools.JsonCompliant(cola.Description).Replace('\n', ' '),
                                cola.Asegurado_Cola__r.Phone,
                                cola.Urg_Cola__c.Value ? "true" : "false",
                                cola.LastModifiedDate,
                                string.Empty,
                                cola.Status,
                                0,
                                SbrinnaCoreFramework.Tools.JsonCompliant(cola.Asegurado_Cola__r.Name));
            }
        }

        res.Append("]");
        this.Colas = res.ToString();
    }
}