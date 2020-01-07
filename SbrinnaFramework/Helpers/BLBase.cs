using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SbrinnaCoreFramework.Sdk.Helpers;
using Microsoft.Xrm.Client;
using SbrinnaCoreFramework.Sdk.Xrm;
using System.Data;
using Microsoft.Xrm.Sdk.Client;

namespace SbrinnaCoreFramework.Sdk.Helpers
{
    public abstract class BLBase
    {

        private ServerConnection cnn;
        private ServerConnection.Configuration config;
        private string Url;
        private string Organizacion;
        private string Usuario;
        private string Dominio;
        private string Contraseña;
        public QServiceContext crmservice { get; set; }
        public OrganizationServiceProxy organizationService = null;

        public BLBase()
        {
            GetCRMConnection();
        }

        /// <summary>
        /// Obtiene la conexion con crm
        /// </summary>
        /// <param name="servername"></param>
        /// <param name="userdomain"></param>
        /// <param name="pwd"></param>
        /// <param name="organizationname"></param>
        internal void GetCRMConnection(string servername,
            string userdomain,
            string pwd,
            string organizationname)
        {

            cnn = new ServerConnection();
            config = cnn.GetServerConfiguration(servername, userdomain, pwd, organizationname);
            organizationService = new OrganizationServiceProxy(config.OrganizationUri,
                                config.HomeRealmUri,
                                config.Credentials,
                                config.DeviceCredentials);

            //Habilita los tipos XRM
            organizationService.EnableProxyTypes();
            crmservice = new QServiceContext(organizationService);
        }

        internal void GetCRMConnection()
        {
            this.Url = Configuracion.GetSetting("CrmServer");
            this.Organizacion = Configuracion.GetSetting("CrmOrganization");
            this.Usuario = Configuracion.GetSetting("CrmUserAdminCode");
            this.Dominio = Configuracion.GetSetting("CrmUserAdminDomain");
            this.Contraseña = Configuracion.GetSetting("CrmUserAdminPassword");
            GetCRMConnection(this.Url, (this.Dominio + "\\" + this.Usuario), this.Contraseña, this.Organizacion);
        }
    }
}