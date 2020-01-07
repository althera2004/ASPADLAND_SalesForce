// <copyright file="Configuration.cs" company="OpenFramework">
// (c) 2013 Sbrinna
// </copyright>
// <author>Juan Castilla</author>
namespace SbrinnaCoreFramework.Sdk.Helpers
{
    using System;
    using System.Configuration;
    using System.Xml;
    using Microsoft.Win32;

    /// <summary>Indica los tipos de ficheros de configuración</summary>
    public enum TipoConfiguracion
    {
        /// <summary>Fichero principal de configuración</summary>
        Principal,

        /// <summary>Fichero de sentencias</summary>
        Sentencias
    }

    /// <summary>Indica a que origen de datos vamos a atacar con las clases de acceso a datos.</summary>
    public enum TipoOrigenDatos
    {
        /// <summary>Base de datos de Microsoft CRM</summary>
        MicrosoftCrm,

        /// <summary>Base de datos de Microsoft CRM</summary>
        EntryCrm,

        /// <summary>Ficheros con datos de Navision</summary>
        Navision
    }

    /// <summary>Clase para obtener y escribir datos en el fichero de configuración "CodigoCrm.xml"</summary>
    public static class Configuracion
    {
        /// <summary>Accedemos al registro para obtener el directorio de instalación de CRM</summary>
        /// <returns>Ruta del directorio de instalación de crm</returns>
        public static string ObtenerDirectorioInstalacionCrm()
        {
            var regMscrm = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\MSCRM");
            if (regMscrm == null)
            {
                // no estamos en un entorno con CRM
                return null;
            }

            var dirInstCrm = regMscrm.GetValue("CRM_Server_InstallDir");
            if (dirInstCrm == null)
            {
                throw new ApplicationException("No se encuentra el valor 'CRM_Server_InstallDir' la subclave del registro de Windows 'SOFTWARE\\Microsoft\\MSCRM' en 'HKEY_LOCAL_MACHINE'. La aplicación debe ejecutarse en la máquina de CRM.");
            }

            return (string)dirInstCrm;
        }

        /*/// <summary>Obtain a value setting</summary>
        /// <param name="crmServer">Name of CRM server</param>
        /// <param name="key">key of setting</param>
        /// <returns>Value of setting</returns>
        public static string GetSetting(string crmServer, string key)
        {
            var configDocument = new XmlDocument();
            XmlNode configNodo = null;
            string configValue = string.Empty;
            configDocument.Load(string.Format(@"C:\WebBlancaConfig\Configuracion_Extension_{0}.xml", crmServer));
            if (configDocument.HasChildNodes)
            {
                configNodo = configDocument.SelectSingleNode(string.Format("configuration/applicationSettings/setting[@name='{0}']/value", key));
                if (configNodo != null)
                {
                    configValue = configNodo.InnerText;
                }
            }

            return configValue;
        }*/

        /// <summary>Obtiene un valor de configuración</summary>
        /// <param name="key">Clave de configuración</param>
        /// <returns>Valor de configuración</returns>
        public static string GetSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            if(ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key];
            }

            return string.Empty;
        }
    }
}
