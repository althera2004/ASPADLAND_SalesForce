namespace GISOWeb
{
    using AspadLandFramework;
    using AspadLandFramework.Item;
    using AspadLandFramework.LogOn;
    using SbrinnaCoreFramework;
    using SbrinnaCoreFramework.Activity;
    using SbrinnaCoreFramework.DAL;
    using SbrinnaCoreFramework.DataAccess;
    using SbrinnaCoreFramework.Sdk.Xrm;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;

    /// <summary>Serive for data management</summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class DataService : WebService
    {
        public DataService()
        {
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ActionResult GetColectivos()
        {
            var res = ActionResult.NoAction;
            res.SetSuccess(Colectivo.JsonList(Colectivo.AllASPAD));
            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string BuscarAsegurado(string nif, string poliza, string colectivo)
        {
            var centro = ApplicationUser.Empty;
            if (this.Session["User"] != null)
            {
                centro = this.Session["User"] as ApplicationUser;
            }

            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            var res = new StringBuilder("[");

            var extraWhere = string.Empty;

            if (!string.IsNullOrEmpty(nif))
            {
                extraWhere = string.Format(
                    CultureInfo.InvariantCulture,
                    @"AND Id IN(
                    SELECT P_liza__c FROM Agrupaci_nAsegPoliza__c
                    WHERE Asegurado__r.NIF__pc = '{0}')", nif);
            }

            if (!string.IsNullOrEmpty(poliza))
            {
                extraWhere = string.Format(CultureInfo.InvariantCulture, "AND P_liza__c.Name = '{0}'", poliza);
            }

            var query = string.Format(
                CultureInfo.InvariantCulture,
                @"
                    SELECT 
                        Id,
                        Name,
                        producto_ASPAD__r.Id,
                        producto_ASPAD__r.Name,
                        Producto_ASPAD__r.Nombre_Compa_ia__c,
                        (SELECT 
	                        Asegurado__r.Id,
	                        Asegurado__r.Name,
	                        Asegurado__r.NIF__pc,
	                        Asegurado__r.Producto_ASPAD__r.name 
                        FROM Agrupaci_nAsegPoliza__r),
                        (SELECT                         
                            Mascota__r.Id,
                            Mascota__r.Name,
	                        Mascota__r.N_de_microchip__c,
	                        Mascota__r.Sexo__c,
	                        Mascota__r.Tipo_de_mascota__c 
                        FROM Relaci_n_Mascotas_P_lizas__r)
                        FROM P_liza__c                      
                        WHERE Producto_ASPAD__r.Nombre_Compa_ia__c = '{0}'
                        {1}",
                colectivo,
                extraWhere);
            var datos = AspadLandFramework.Tools.SalesForcceQuery(query);
            var first = true;

            if (datos.records != null && datos.records.Count() > 0)
            {
                foreach (var result in datos.records)
                {
                    var record = result as P_liza__c;

                    var aseguradoRel = record.Agrupaci_nAsegPoliza__r.records.ToList() as List<sObject>;

                    var aseguradoNombre = string.Empty;
                    var aseguradoNIF = string.Empty;
                    foreach (sObject aseguradoObject in aseguradoRel)
                    {
                        var asegurado = aseguradoObject as AspadLandFramework.Agrupaci_nAsegPoliza__c;
                        if (!string.IsNullOrEmpty(asegurado.Asegurado__r.NIF__pc))
                        {
                            aseguradoNIF = asegurado.Asegurado__r.NIF__pc;
                        }

                        if (!string.IsNullOrEmpty(asegurado.Asegurado__r.Name))
                        {
                            aseguradoNombre = asegurado.Asegurado__r.Name;
                        }
                    }

                    var mascotaRel = record.Relaci_n_Mascotas_P_lizas__r.records.ToList() as List<sObject>;

                    foreach (sObject mascotaObject in mascotaRel)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",");
                        }

                        var mascota = mascotaObject as Relaci_n_Mascotas_P_lizas__c;
                        res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"{{
                                    ""Asegurado"":""{0}"",
                                    ""DNI"":""{1}"",
                                    ""Producto"":""{2}"",
                                    ""Poliza"":""{3}"",
                                    ""Chip"":""{4}"",
                                    ""Nombre"":""{5}"",
                                    ""Mascota"":""{6}"",
                                    ""Estado"":""{7}"",
                                    ""Animal"":""{8}"",
                                    ""MascotaId"":""{9}"",
                                    ""PolizaId"":""{10}"",
                                    ""Colectivo"":""{11}""}}",
                                        SbrinnaCoreFramework.Tools.JsonCompliant(aseguradoNombre),
                                        aseguradoNIF.ToUpperInvariant().Trim(),
                                        record.Producto_ASPAD__r.Name,
                                        record.Name.Trim(),
                                        mascota.Mascota__r.N_de_microchip__c,
                                        mascota.Mascota__r.Name,
                                        mascota.Mascota__r.Sexo__c,
                                        1,
                                        mascota.Mascota__r.Tipo_de_mascota__c,
                                        mascota.Mascota__r.Id,
                                        record.Id,
                                        record.Producto_ASPAD__r.Nombre_Compa_ia__c);
                    }
                }
            }

            res.Append("]");
            return res.ToString();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult GetLogin(string email, string password, string ip)
        {
            var res = ActionResult.NoAction;
            res = ApplicationLogOn.ApplicationAccess(email, password, ip);

            if (res.Success)
            {
                var logon = res.ReturnValue as LogOnObject;
                var dictionary = ApplicationDictionary.Load("es");
                HttpContext.Current.Session["Dictionary"] = dictionary;
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult UpdateMascota(
            Guid id,
            string nombre,
            string chip,
            string actualChip,
            int sexo,
            int tipo)
        {
            var centro = ApplicationUser.Empty;
            if (this.Session["User"] != null)
            {
                centro = this.Session["User"] as ApplicationUser;
            }

            /* CREATE PROCEDURE AspadLand_Trace_Insert
                *   @CentroId uniqueidentifier,
                *   @Type int,
                *   @Busqueda nvarchar(50),
                *   @ColectivoId uniqueidentifier,
                *   @PresupuestoId uniqueidentifier */
            using (var cmdT = new SqlCommand("AspadLand_Trace_Insert"))
            {
                using (var cnnT = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmdT.Connection = cnnT;
                    cmdT.CommandType = CommandType.StoredProcedure;
                    cmdT.Parameters.Add(DataParameter.Input("@CentroId", centro.Id));
                    cmdT.Parameters.Add(DataParameter.Input("@Type", 10));
                    cmdT.Parameters.Add(DataParameter.Input("@Busqueda", nombre + " " + chip, 50));
                    cmdT.Parameters.Add(DataParameter.Input("@ColectivoId", id));
                    cmdT.Parameters.Add(DataParameter.InputNull("@PresupuestoId"));
                    try
                    {
                        cmdT.Connection.Open();
                        cmdT.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (cmdT.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmdT.Connection.Close();
                        }
                    }
                }
            }

            return CRMOperations.UpdateMascota(id, nombre, chip, sexo, tipo, actualChip);

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult AddValidacion(
            string nombre,
            string apellido1,
            string apellido2,
            string dni,
            string poliza,
            Guid centroId,
            Guid colectivoId,
            bool urgente)
        {
            return CRMOperations.ColaValidacion_Add(nombre, apellido1, apellido2, dni, poliza, centroId, colectivoId, urgente);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var res = ApplicationUser.ChangePassword(userId, oldPassword, newPassword);
            if (res.MessageError == "NOPASS")
            {
                res.MessageError = ApplicationDictionary.Translate("Common_Error_IncorrectPassword");
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public string GetPresupuestosByMascota(Guid centroId, Guid mascotaId)
        {
            int columnPresupuestoId = 0;
            int columnCode = 1;
            int columnActoId = 2;
            int columnActoName = 3;
            int columnEspecialidadName = 4;
            int columnAmount = 5;
            int columnDiscount = 6;
            int columnFecha = 7;
            int columnObservaciones = 8;
            int columnPresupuestoOriginalId = 9;

            var res = new StringBuilder("[");
            using (var cmd = new SqlCommand("ASPADLAND_GetPresupuestoPendienteByMascotaId2"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@MascotaId", mascotaId));
                cmd.Parameters.Add(DataParameter.Input("@CentroId", centroId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        bool first = true;
                        using (var rdr = cmd.ExecuteReader())
                        {
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

                                string amount = "null";
                                string discount = "null";

                                if (!rdr.IsDBNull(columnAmount))
                                {
                                    amount = string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", rdr.GetDecimal(columnAmount));
                                }

                                if (!rdr.IsDBNull(columnDiscount))
                                {
                                    discount = string.Format(CultureInfo.InvariantCulture, "{0:#0.00}", rdr.GetDecimal(columnDiscount));
                                }

                                res.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    @"{{
                                        ""PresupuestoId"":""{0}"",
                                        ""PresupuestoOriginalId"":""{10}"",
                                        ""ActoId"":""{1}"",
                                        ""Acto"":""{2}"",
                                        ""Especialidad"":""{3}"",
                                        ""Amount"":{4},
                                        ""Discount"":{5},
                                        ""Status"":{6},
                                        ""F"":""{7:dd/MM/yyyy}"",
                                        ""Description"":""{8}"",
                                        ""Observaciones"":""{9}""}}",
                                    rdr.GetGuid(columnPresupuestoId),
                                    rdr.GetGuid(columnActoId),
                                    rdr.GetString(columnActoName),
                                    rdr.GetString(columnEspecialidadName),
                                    amount,
                                    discount,
                                    0,
                                    rdr.GetDateTime(columnFecha),
                                    rdr.GetString(columnCode),
                                    SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(columnObservaciones)),
                                    rdr.GetString(columnPresupuestoOriginalId));
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            res.Append("]");
            return res.ToString();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult SavePresupuesto(string centroId, Guid presupuestoOriginalId, string polizaId, string mascotaId, string data, string presupuestoObservaciones, int status, string code)
        {
            var res = ActionResult.NoAction;
            var presupuestoId = Guid.NewGuid();
            int count = 0;

            if (string.IsNullOrEmpty(code))
            {
                /* CREATE PROCEDURE ASPADLAND_GetPresupuestoCode
                 *   @CentroId nvarchar(50),
                 *   @PresupuestoId nvarchar(50) */
                using (var cmd2 = new SqlCommand("ASPADLAND_Salesforce_GetPresupuestoCode"))
                {
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add(DataParameter.Input("@CentroId", centroId));
                    cmd2.Parameters.Add(DataParameter.Input("@PresupuestoId", presupuestoId));
                    using (var cnn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd2.Connection = cnn2;
                        try
                        {
                            cmd2.Connection.Open();
                            using (var rdr2 = cmd2.ExecuteReader())
                            {
                                if (rdr2.HasRows)
                                {
                                    rdr2.Read();
                                    code = rdr2.GetString(0);
                                    count = rdr2.GetInt32(1);
                                }
                            }
                        }
                        finally
                        {
                            if (cmd2.Connection.State != System.Data.ConnectionState.Closed)
                            {
                                cmd2.Connection.Close();
                            }
                        }
                    }
                }
            }
            else
            {
                string query = string.Format(
                    CultureInfo.InvariantCulture,
                    "DELETE FROM [dbo].[AspadLandPresuspuesto] WHERE Code = '{0}' AND CentroId2 = '{1}'",
                    code,
                    centroId);
                using (var cmd2 = new SqlCommand(query))
                {
                    cmd2.CommandType = CommandType.Text;
                    using (var cnn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd2.Connection = cnn2;
                        try
                        {
                            cmd2.Connection.Open();
                            cmd2.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (cmd2.Connection.State != System.Data.ConnectionState.Closed)
                            {
                                cmd2.Connection.Close();
                            }
                        }
                    }
                }
            }

            bool ok = true;
            /* CREATE PROCEDURE ASPADLAND_SalesForce_PresupuestoInsert
             *   @CentroId nvarchar(50),
             *   @PolizaId nvarchar(50),
             *   @MascotaId nvarchar(50),
             *   @Actoid nvarchar(50),
             *   @Amount decimal(18,3),
             *   @Discount decimal(18,3),
             *   @Status int,
             *   @Fecha Datetime,
             *   @FechaRealizacion Datetime,
             *   @PresupuestoId uniqueidentifier,
             *   @Code nvarchar(50),
             *   @Count int */

            using (var cmd = new SqlCommand("ASPADLAND_SalesForce_PresupuestoInsert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    var parts = data.Replace("},{", "^").Split('^');
                    try
                    {
                        cmd.Connection.Open();
                        foreach (string part in parts)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(DataParameter.Input("@PresupuestoId", presupuestoId));
                            cmd.Parameters.Add(DataParameter.Input("@PresupuestoOriginalId", presupuestoOriginalId));
                            cmd.Parameters.Add(DataParameter.Input("@CentroId", centroId, 50));
                            cmd.Parameters.Add(DataParameter.Input("@PolizaId", polizaId, 50));
                            cmd.Parameters.Add(DataParameter.Input("@MascotaId", mascotaId, 50));
                            cmd.Parameters.Add(DataParameter.Input("@Fecha", DateTime.Now));
                            cmd.Parameters.Add(DataParameter.Input("@Status", status));
                            cmd.Parameters.Add(DataParameter.Input("@Code", code));
                            cmd.Parameters.Add(DataParameter.Input("@Count", count));
                            cmd.Parameters.Add(DataParameter.Input("@PresupuestoObservaciones", presupuestoObservaciones, 500));

                            string actoIdText = part.Split(':')[1].Split(',')[0].Replace("\"", string.Empty);
                            string amountText = part.Split(':')[3].Split(',')[0].Replace("\"", string.Empty);
                            string discountText = part.Split(':')[4].Split(',')[0].Replace("\"", string.Empty);
                            string actoName = part.Split(':')[2].Replace("\"", string.Empty).Replace(",Amount", string.Empty);
                            string especialidad = part.Split(':')[5].Replace("\"", string.Empty).Replace(",T", string.Empty);
                            string actoObservaciones = part.Split(':')[7].Split(',')[0].Replace("\"", string.Empty).Replace("}]", string.Empty);

                            cmd.Parameters.Add(DataParameter.Input("@ActoName", actoName, 100));
                            cmd.Parameters.Add(DataParameter.Input("@Especialidad", especialidad, 100));
                            cmd.Parameters.Add(DataParameter.Input("@ActoId", actoIdText));
                            cmd.Parameters.Add(DataParameter.Input("@ActoObservaciones", actoObservaciones, 500));

                            if (amountText == "null")
                            {
                                cmd.Parameters.Add(DataParameter.InputNull("@Amount"));
                            }
                            else
                            {
                                cmd.Parameters.Add(DataParameter.Input("@Amount", Convert.ToDecimal(amountText.Replace(".", ","))));
                            }

                            if (discountText == "null")
                            {
                                cmd.Parameters.Add(DataParameter.InputNull("@Discount"));
                            }
                            else
                            {
                                cmd.Parameters.Add(DataParameter.Input("@Discount", Convert.ToDecimal(discountText.Replace(".", ","))));
                            }

                            if (status == 1)
                            {
                                cmd.Parameters.Add(DataParameter.Input("@FechaRealizacion", DateTime.Now));
                            }
                            else
                            {
                                cmd.Parameters.Add(DataParameter.InputNull("@FechaRealizacion"));
                            }

                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }

                    if (ok)
                    {
                        res.SetSuccess(presupuestoId.ToString());
                    }

                }
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult DiscardPresupuesto(Guid presupuestoId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ASPADLAND_PresupuestoDiscard
             *   @PresupuestoId uniqueidentifier */
            using (var cmd = new SqlCommand("ASPADLAND_PresupuestoDiscard"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@PresupuestoId", presupuestoId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }

        public ActionResult MarcarActoRealizado(Guid presupuestoId, string actoId, string actoRealizadoId, DateTime fecha)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ASPADLAND_Salesforce_PresupuestoRealizado
             *   @PresupuestoId uniqueidentifier,
             *   @ActoId nvarchar(50),
             *   @ActoRealizadoId nvarchar(50) */
            using (var cmd = new SqlCommand("ASPADLAND_Salesforce_PresupuestoRealizado"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@PresupuestoId", presupuestoId));
                cmd.Parameters.Add(DataParameter.Input("@ActoId", actoId, 50));
                cmd.Parameters.Add(DataParameter.Input("@ActoRealizadoId", actoRealizadoId, 50));
                cmd.Parameters.Add(DataParameter.Input("@Fecha", fecha));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult RealizarPresupuesto(Guid presupuestoId, Guid centroId, DateTime fecha)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("ASPADLAND_GetPresupuestoPendienteByPresupuestoIdToCRM"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@PresupuestoId", presupuestoId));
                cmd.Parameters.Add(DataParameter.Input("@CentroId", centroId));
                cmd.Parameters.Add(DataParameter.Input("@Fecha", fecha.ToUniversalTime()));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                qes_actorealizado actorealizado = new qes_actorealizado
                                {
                                    qes_CentroId = new Microsoft.Xrm.Sdk.EntityReference("account", centroId),
                                    qes_ActoId = new Microsoft.Xrm.Sdk.EntityReference("product", rdr.GetGuid(0)),
                                    qes_MascotaId = new Microsoft.Xrm.Sdk.EntityReference("qes_mascotas", rdr.GetGuid(1)),
                                    qes_PolizaId = new Microsoft.Xrm.Sdk.EntityReference("qes_poliza", rdr.GetGuid(2)),
                                    qes_AseguradoId = new Microsoft.Xrm.Sdk.EntityReference("contact", rdr.GetGuid(3)),
                                    qes_fechaproceso = fecha.ToUniversalTime(),
                                    qes_descripcion = rdr.GetString(4),
                                    qes_EspecialidadId = new Microsoft.Xrm.Sdk.EntityReference("qes_especialidad", rdr.GetGuid(5)),
                                    qes_TarifaId = new Microsoft.Xrm.Sdk.EntityReference("pricelevel", rdr.GetGuid(8))
                                };

                                if (!rdr.IsDBNull(6))
                                {
                                    actorealizado.qes_Importe = new Microsoft.Xrm.Sdk.Money(rdr.GetDecimal(6));
                                }

                                if (!rdr.IsDBNull(7))
                                {
                                    actorealizado.Attributes.Add("qes_porcentaje", rdr.GetDecimal(7));
                                }

                                res = CRMOperations.ActoRealizadoAdd(actorealizado);

                                if (res.Success)
                                {
                                    res = MarcarActoRealizado(presupuestoId, rdr.GetString(0), res.ReturnValue.ToString(), fecha);
                                }
                                else
                                {
                                    res.SetFail(res.MessageError);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }
    }
}