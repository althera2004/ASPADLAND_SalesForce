namespace SbrinnaCoreFramework.DAL
{
    using SbrinnaCoreFramework;
    using SbrinnaCoreFramework.Sdk.Helpers;
    using SbrinnaCoreFramework.Sdk.Xrm;
    using System;
    using System.Text;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using SbrinnaCoreFramework.Activity;
    using System.Globalization;

    /// <summary>Operaciones transaccionales con CRM 2011</summary>
    public static class CRMOperations
    {
        private static ServerConnection cnn;
        private static ServerConnection.Configuration config;
        private static string Url;
        private static string Organizacion;
        private static string Usuario;
        private static string Dominio;
        private static string Contraseña;
        public static QServiceContext crmservice;
        public static OrganizationServiceProxy organizationService = null;
        public static string DebugText { get; set; }
        public static Guid ColectivoFiatc { get; set; }
        public static Guid ProductoVithas { get; set; }
        public static Guid ProductoPMC { get; set; }
        public static Guid ProductoDiagonal { get; set; }
        public static Guid ProductoSanitas { get; set; }
        public static Guid TarifaVithas { get; set; }
        public static Guid TarifaPMC { get; set; }
        public static Guid TarifaDiagonal { get; set; }
        public static Guid TarifaSanitas { get; set; }

        /// <summary>
        /// Obtiene la o las poblaciones de un código postal
        /// </summary>
        /// <param name="codigoPostal">Código postal</param>
        /// <returns>Lista con las poblaciones del código postal</returns>
        public static string GetCodigosPostales(string codigoPostal)
        {
            if (organizationService == null)
            {
                GetCRMConnection();
            }

            string fetchXml = @"<fetch mapping='logical' distinct='true'>
                            <entity name='qes_codigopostal'>
                            <attribute name='qes_name' />
                            <attribute name='qes_provinciaid' />
                            <attribute name='qes_poblacionid' />
                            <attribute name='qes_codigopostalid' />
                            <link-entity name='qes_provincia' from='qes_provinciaid' to='qes_provinciaid' visible='false' link-type='outer' alias='provincia'>
                              <attribute name='qes_comunidadid' />
                            </link-entity>
                            <filter type='and'>
                                <condition attribute='qes_name' operator='eq' value='" + codigoPostal + @"' />
                            </filter>
                            </entity>
                        </fetch>";
            var conversionRequest = new FetchXmlToQueryExpressionRequest
            {
                FetchXml = fetchXml
            };

            var conversionResponse = (FetchXmlToQueryExpressionResponse)organizationService.Execute(conversionRequest);
            var queryExpression = conversionResponse.Query;
            var res = organizationService.RetrieveMultiple(queryExpression);
            var sb = new StringBuilder();
            if (res.Entities.Count > 0)
            {
                string cpostal = Tools.GetStringValue(res.Entities[0], "qes_name");
                var cpostalid = new Guid(Tools.GetStringValue(res.Entities[0], "qes_codigopostalid"));
                var provincia = Tools.GetReferenceId(res.Entities[0], "qes_provinciaid");
                var poblacion = Tools.GetReferenceId(res.Entities[0], "qes_poblacionid");
                var comunidad = Tools.GetAliasedId(res.Entities[0], "provincia.qes_comunidadid");
                sb.Append(cpostal).Append("|").Append(cpostalid).Append("|").Append(provincia).Append("|").Append(poblacion).Append("|").Append(comunidad);
            }
            else
            {
                sb.Append("||||");
            }

            return sb.ToString();
        }

        // Conexion CRM
        #region Conexion CRM

        public static string GetConnected(string crmServer)
        {
            if (organizationService == null)
            {
                //GetCRMConnection(crmServer);
                GetCRMConnection();
            }

            return "ok";
        }

        public static string GetConnectedRequired(string crmServer)
        {
            //GetCRMConnection(crmServer);
            GetCRMConnection();
            return "ok";
        }

        /// <summary>
        /// Obtiene la conexion con crm
        /// </summary>
        /// <param name="servername">Nombre del servidor</param>
        /// <param name="userdomain">Nombre del usuario del dominio</param>
        /// <param name="pwd">Password del usuario</param>
        /// <param name="organizationname">Nombre de la organización CRM</param>
        private static void GetCRMConnection(string servername, string userdomain, string pwd, string organizationname)
        {
            cnn = new SbrinnaCoreFramework.Sdk.Helpers.ServerConnection();
            config = cnn.GetServerConfiguration(servername, userdomain, pwd, organizationname);
            organizationService = new OrganizationServiceProxy(config.OrganizationUri, config.HomeRealmUri, config.Credentials, config.DeviceCredentials);

            // Habilita los tipos XRM
            organizationService.EnableProxyTypes();
            crmservice = new QServiceContext(organizationService);
        }

        /// <summary>Obtiene una conexión a CRM</summary>
        public static void GetCRMConnection()
        {
            Url = Configuracion.GetSetting("CrmServer");
            Organizacion = Configuracion.GetSetting("CrmOrganization");
            Usuario = Configuracion.GetSetting("CrmUserAdminCode");
            Dominio = Configuracion.GetSetting("CrmUserAdminDomain");
            Contraseña = Configuracion.GetSetting("CrmUserAdminPassword");
            GetCRMConnection(Url, string.Format(@"{0}\{1}", Dominio, Usuario), Contraseña, Organizacion);
        }
        #endregion
        public static void ActivarAseguradoPendiente(string serverCrm, Guid aseguradoId)
        {
            if (organizationService == null)
            {
                GetCRMConnection();
            }

            var setStateAsegurado = new SetStateRequest
            {
                EntityMoniker = new EntityReference(Contact.EntityLogicalName, aseguradoId),
                State = new OptionSetValue(0),
                Status = new OptionSetValue(1)
            };

            var myresAsegurado = (SetStateResponse)organizationService.Execute(setStateAsegurado);
        }

        public static ActionResult UpdateMascota(
            Guid id,
            string nombre,
            string chip,
            int sexo,
            int tipo,
            string actualChip)
        {
            var res = ActionResult.NoAction;
            if (CRMOperations.organizationService == null)
            {
                CRMOperations.GetCRMConnection();
            }

            var mascota = new qes_mascotas()
            {
                Id = id,
                qes_name = nombre,
                qes_NMicrochip = chip
            };

            if(actualChip!= chip)
            {
                mascota.Attributes.Add("aisa_fechamicrochipextranet", DateTime.Now.ToUniversalTime());
            }

            if (sexo > 1)
            {
                mascota.qes_Sexo = new Microsoft.Xrm.Sdk.OptionSetValue(sexo);
            }

            if (tipo > 1)
            {
                mascota.qes_Tipomascota = new Microsoft.Xrm.Sdk.OptionSetValue(tipo);
            }

            try
            {
                organizationService.Update(mascota);
                res.SetSuccess();
            }
            catch (Exception ex)
            {
                res.SetFail(ex);
            }

            return res;
        }

        public static ActionResult ActoRealizadoAdd(qes_actorealizado acto)
        {
            var res = ActionResult.NoAction;
            if (organizationService == null)
            {
                GetCRMConnection();
            }
            try
            {
                var actoId = organizationService.Create(acto);
                res.SetSuccess(actoId);
            }
            catch (Exception ex)
            {
                res.SetFail(ex);
            }

            return res;
        }

        public static ActionResult ColaValidacion_Add(
            string nombre, 
            string apellido1,
            string apellido2, 
            string dni,
            string poliza,
            Guid centroId,
            Guid colectivoId,
            bool urgente)
        {
            var res = ActionResult.NoAction;
            if(organizationService == null)
            {
                GetCRMConnection();
            }

            string fullName = string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2}",
                nombre,
                apellido1,
                apellido2).Trim();

            var colaValidacion = new qes_colavalidacion
            {
                qes_name = fullName,
                qes_nombre = nombre,
                qes_Apellido1 = apellido1,
                qes_Apellido2 = apellido2,
                qes_dni = dni,
                qes_Poliza = poliza,
                qes_CentroId = new EntityReference(Account.EntityLogicalName, centroId),
                qes_ColectivoId = new EntityReference(Account.EntityLogicalName, colectivoId),
                qes_Urgente = urgente,
                qes_fechainicio = DateTime.Now.ToLocalTime()
            };

            try
            {
                var colaValidacionId = organizationService.Create(colaValidacion);
                res.SetSuccess();
            }
            catch(Exception ex)
            {
                res.SetFail(ex);
            }

            return res;
        }
    }
}