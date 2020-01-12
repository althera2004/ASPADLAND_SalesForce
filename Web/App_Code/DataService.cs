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
            res.SetSuccess(Colectivo.JsonList(Colectivo.All));
            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string BuscarAsegurado(string nif, string poliza, string nombre, string colectivo)
        {
            var centro = ApplicationUser.Empty;
            if (this.Session["User"] != null)
            {
                centro = this.Session["User"] as ApplicationUser;
            }

            //weke
            ///* CREATE PROCEDURE AspadLand_Trace_Insert
            //    *   @CentroId uniqueidentifier,
            //    *   @Type int,
            //    *   @Busqueda nvarchar(50),
            //    *   @ColectivoId uniqueidentifier,
            //    *   @PresupuestoId uniqueidentifier */
            //using (var cmdT = new SqlCommand("AspadLand_Trace_Insert"))
            //{
            //    using (var cnnT = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            //    {
            //        cmdT.Connection = cnnT;
            //        cmdT.CommandType = CommandType.StoredProcedure;
            //        cmdT.Parameters.Add(DataParameter.Input("@CentroId", centro.Id));
            //        cmdT.Parameters.Add(DataParameter.Input("@Type", 9));
            //        cmdT.Parameters.Add(DataParameter.Input("@Busqueda", poliza + nif));
            //        cmdT.Parameters.Add(DataParameter.Input("@ColectivoId", colectivo));
            //        cmdT.Parameters.Add(DataParameter.InputNull("@PresupuestoId"));
            //        try
            //        {
            //            cmdT.Connection.Open();
            //            cmdT.ExecuteNonQuery();
            //        }
            //        finally
            //        {
            //            if (cmdT.Connection.State != System.Data.ConnectionState.Closed)
            //            {
            //                cmdT.Connection.Close();
            //            }
            //        }
            //    }
            //}

            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            var res = new StringBuilder("[");

            var query = @"
                    SELECT 
                    Name,
                    (SELECT 
	                    Asegurado__r.Name,
	                    Asegurado__r.NIF__pc,
	                    Asegurado__r.Producto_ASPAD__r.name 
                    FROM Agrupaci_nAsegPoliza__r),
                    (SELECT Mascota__r.Name,
	                    Mascota__r.N_de_microchip__c,
	                    Mascota__r.Sexo__c,
	                    Mascota__r.Tipo_de_mascota__c 
                    FROM Relaci_n_Mascotas_P_lizas__r)
                    FROM P_liza__c 
                    WHERE Id IN (
		                    SELECT P_liza__c FROM Agrupaci_nAsegPoliza__c
		                    WHERE 
		                    Asegurado__r.NIF__pc = '94178107F')
                    AND	Producto_ASPAD__r.Nombre_Compa_ia__c = 'CASER'";
            var datos = AspadLandFramework.Tools.SalesForcceQuery(query);
            var first = true;
            foreach (var result in datos.records)
            {
                var record = result as P_liza__c;
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                var asegurado = record.Agrupaci_nAsegPoliza__r.records.First() as AspadLandFramework.Account;
                var mascota = record.Relaci_n_Mascotas_P_lizas__r.records.First() as Mascota__c;

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
                                ""MascotaId"":""{9}""}}",
                                SbrinnaCoreFramework.Tools.JsonCompliant(asegurado.Name),
                                asegurado.NIF__c.ToUpperInvariant().Trim(),
                                asegurado.Producto_ASPAD__c.Trim(),
                                record.Name.Trim(),
                                mascota.N_de_microchip__c,
                                mascota.Name,
                                mascota.Sexo__c,
                                1,
                                mascota.Tipo_de_mascota__c,
                                mascota.Id);
            }
            /*var query = @"
            SELECT     
	            P.qes_AseguradoIdName AS FullName,
	            P.qes_dni, 
	            P.qes_name, 
	            M.statecode, 
                ISNULL(M.qes_name,'') AS mascota,
                M.qes_Tipomascota,
                M.qes_Sexo, 
                M.qes_mascotasId, 
	            ISNULL(M.qes_NMicrochip,'') AS MicroChip,
	            P.qes_ProductoIdName, 
	            P.qes_ColectivoId
            FROM dbo.qes_poliza P WITH (NOLOCK)
            INNER JOIN dbo.qes_mascotas M WITH(NOLOCK)
            ON	M.qes_PolizaId = P.qes_polizaId
            WHERE  -- weke añadir pertenencia a al centro
	            P.statecode = 0
            AND M.statecode = 0";

            if (!string.IsNullOrEmpty(nif))
            {
                query += " AND P.qes_dni = '" + nif + "'";
            }

            if (!string.IsNullOrEmpty(poliza))
            {
                query += " AND P.qes_name = '" + poliza + "'";
            }

            if (!string.IsNullOrEmpty(colectivo))
            {
                query += " AND P.qes_colectivoId = '" + colectivo + "'";
            }

            query += " ORDER BY P.qes_AseguradoIdName ASC, P.qes_ProductoIdName ASC";

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

                                string chip = string.Empty;
                                if (!rdr.IsDBNull(8))
                                {
                                    chip = rdr.GetString(8).Trim();
                                }

                                string mascota = string.Empty;
                                if (!rdr.IsDBNull(4))
                                {
                                    mascota = rdr.GetString(4).Trim().Replace("\"", string.Empty);
                                }

                                string estado = rdr.GetInt32(3) == 0 ? dictionary["Item_Poliza_Status_Active"] : dictionary["Item_Poliza_Status_Inactive"];

                                string animal = string.Empty;

                                if (!rdr.IsDBNull(5))
                                {
                                    if (rdr[5].ToString() == "100000000")
                                    {
                                        animal = dictionary["Item_Mascota_Type_DogMale"];
                                    }
                                    else
                                    {
                                        animal = dictionary["Item_Mascota_Type_CatMale"];
                                    }
                                }

                                if (!rdr.IsDBNull(6))
                                {
                                    if (rdr[6].ToString() == "100000001")
                                    {
                                        if (animal == dictionary["Item_Mascota_Type_DogMale"])
                                        {
                                            animal = dictionary["Item_Mascota_Type_DogFemale"];
                                        }
                                        else
                                        {
                                            if (animal == dictionary["Item_Mascota_Type_CatMale"])
                                            {
                                                animal = dictionary["Item_Mascota_Type_CatFemale"];
                                            }
                                        }
                                    }
                                }

                                res.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    @"{{
                                ""Asegurado"":""{0}"",
                                ""DNI"":""{1}"",
                                ""Producto"":""{2}"",
                                ""Poliza"":""{3}"",
                                ""Chip"":""{4}"",
                                ""Nombre"":""{8}"",
                                ""Mascota"":""{5}"",
                                ""Estado"":""{6}"",
                                ""Animal"":""{7}"",
                                ""MascotaId"":""{9}""}}",
                                SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(0).Trim()),
                                rdr.GetString(1).Trim().ToUpperInvariant(),
                                rdr.GetString(9).Trim(),
                                rdr.GetString(2).Trim(),
                                chip.Trim(),
                                mascota.Trim(),
                                estado,
                                animal,
                                SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(4).Trim()),
                                rdr.GetGuid(7));
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
            }*/

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
            int columnActoName =3;
            int columnEspecialidadName = 4;
            int columnAmount=5;
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
                        using(var rdr = cmd.ExecuteReader())
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
                        if(cmd.Connection.State != System.Data.ConnectionState.Closed)
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
        public ActionResult SavePresupuesto(Guid centroId, Guid presupuestoOriginalId, Guid polizaId, Guid mascotaId, string data, string presupuestoObservaciones, int status, string code)
        {
            var res = ActionResult.NoAction;
            var presupuestoId = Guid.NewGuid();
            int count = 0;

            if (string.IsNullOrEmpty(code))
            {
                /* CREATE PROCEDURE ASPADLAND_GetPresupuestoCode
                 *   @CentroId uniqueidentifier,
                 *   @PresupuestoId uniqueidentifier */
                using (var cmd2 = new SqlCommand("ASPADLAND_GetPresupuestoCode"))
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
                    "DELETE FROM [dbo].[AspadLandPresuspuesto] WHERE Code = '{0}' AND CentroId = '{1}'",
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
            /* CREATE PROCEDURE ASPADLAND_PresupuestoInsert2
             *   @CentroId uniqueidentifier,
             *   @PolizaId uniqueidentifier,
             *   @MascotaId uniqueidentifier,
             *   @Actoid uniqueidentifier,
             *   @Amount decimal(18,3),
             *   @Discount decimal(18,3),
             *   @Status int,
             *   @Fecha Datetime,
             *   @FechaRealizacion Datetime,
             *   @PresupuestoId uniqueidentifier,
             *   @Code nvarchar(50),
             *   @Count int */

            using (var cmd = new SqlCommand("ASPADLAND_presupuestoInsert2"))
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
                            cmd.Parameters.Add(DataParameter.Input("@CentroId", centroId));
                            cmd.Parameters.Add(DataParameter.Input("@PolizaId", polizaId));
                            cmd.Parameters.Add(DataParameter.Input("@MascotaId", mascotaId));
                            cmd.Parameters.Add(DataParameter.Input("@Fecha", DateTime.Now));
                            cmd.Parameters.Add(DataParameter.Input("@Status", status));
                            cmd.Parameters.Add(DataParameter.Input("@Code", code));
                            cmd.Parameters.Add(DataParameter.Input("@Count", count));
                            cmd.Parameters.Add(DataParameter.Input("@PresupuestoObservaciones", presupuestoObservaciones, 500));

                            string actoIdText = part.Split(':')[1].Split(',')[0].Replace("\"", string.Empty);// part.Split(':')[0].Split(':')[1].Replace("\"", "");
                            string amountText = part.Split(':')[3].Split(',')[0].Replace("\"", string.Empty);// part.Split(':')[2].Split(':')[1].Replace("\"", "");
                            string discountText = part.Split(':')[4].Split(',')[0].Replace("\"", string.Empty);// part.Split(':')[3].Split(':')[1].Replace("\"", "");
                            string actoName = part.Split(':')[2].Replace("\"", string.Empty).Replace(",Amount", string.Empty);// part.Split(':')[1].Split(':')[1].Replace("\"", "");
                            string actoObservaciones = part.Split(':')[6].Split(',')[0].Replace("\"", string.Empty).Replace("}]", string.Empty);// part.Split(':')[5].Split(':')[1].Split('}')[0].Replace("\"", "");

                            cmd.Parameters.Add(DataParameter.Input("@ActoName", actoName, 100));
                            cmd.Parameters.Add(DataParameter.Input("@ActoId", new Guid(actoIdText)));
                            cmd.Parameters.Add(DataParameter.Input("@ActoObservaciones", actoObservaciones, 500));

                            if (amountText == "null")
                            {
                                cmd.Parameters.Add(DataParameter.InputNull("@Amount"));
                            }
                            else
                            {
                                cmd.Parameters.Add(DataParameter.Input("@Amount", Convert.ToDecimal(amountText.Replace(".",","))));
                            }

                            if (discountText == "null")
                            {
                                cmd.Parameters.Add(DataParameter.InputNull("@Discount"));
                            }
                            else
                            {
                                cmd.Parameters.Add(DataParameter.Input("@Discount", Convert.ToDecimal(discountText.Replace(".",","))));
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
                    catch(Exception ex)
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

        public ActionResult MarcarActoRealizado(Guid presupuestoId, Guid actoId, Guid actoRealizadoId, DateTime fecha)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ASPADLAND_PresupuestoRealizado
             *   @PresupuestoId uniqueidentifier,
             *   @ActoId uniqueidentifier,
             *   @ActoRealizadoId uniqueidentifier */
            using (var cmd = new SqlCommand("ASPADLAND_PresupuestoRealizado"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@PresupuestoId", presupuestoId));
                cmd.Parameters.Add(DataParameter.Input("@ActoId", actoId));
                cmd.Parameters.Add(DataParameter.Input("@ActoRealizadoId", actoRealizadoId));
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
            using(var cmd = new SqlCommand("ASPADLAND_GetPresupuestoPendienteByPresupuestoIdToCRM"))
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
                        using(var rdr = cmd.ExecuteReader())
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
                                    res = MarcarActoRealizado(presupuestoId, rdr.GetGuid(0), new Guid(res.ReturnValue.ToString()), fecha);
                                }
                                else
                                {
                                    res.SetFail(res.MessageError);
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if(cmd.Connection.State != System.Data.ConnectionState.Closed)
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