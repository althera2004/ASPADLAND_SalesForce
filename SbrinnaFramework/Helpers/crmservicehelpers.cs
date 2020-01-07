// =====================================================================
//  This file is part of the Microsoft Dynamics CRM SDK code samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
// =====================================================================
namespace SbrinnaCoreFramework.Sdk.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Security;
    using System.ServiceModel.Description;
    using Microsoft.Crm.Services.Utility;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Discovery;

    /// <summary>
    /// Type of authentication
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>Active directory</summary>
        AD,

        /// <summary>Passwort credential</summary>
        Passport,

        /// <summary>I dont know this</summary>
        SPLA
    }

    /// <summary>
    /// Provides server connection information.
    /// </summary>
    public class ServerConnection
    {
        /// <summary>
        /// Stores CRM server configuration information.
        /// </summary>
        public class Configuration
        {
            public string ServerAddress;
            public string OrganizationName;
            public Uri DiscoveryUri;
            public Uri OrganizationUri;
            public Uri HomeRealmUri = null;
            public ClientCredentials DeviceCredentials = null;
            public ClientCredentials Credentials = null;
            public AuthenticationType EndpointType;
        }

        public List<Configuration> configurations = null;

        private Configuration config = new Configuration();
        
        /// <summary>
        /// Obtains the server connection information including the target organization's
        /// Uri and user login credentials from the user.
        /// </summary>
        /// <param name="servername">Server name</param>
        /// <param name="userdomain">User domain</param>
        /// <param name="password">Password login</param>
        /// <param name="organizationname">Organization name</param>
        /// <returns>Server configuration</returns>
        public virtual Configuration GetServerConfiguration(string servername, string userdomain, string password, string organizationname)
        {
            InitiateSSLTrust();
            bool ssl = servername.StartsWith("https");

            // Get the server address. If no value is entered, default to Microsoft Dynamics
            // CRM Online in the North American data center.
            this.config.ServerAddress = servername.Replace("https://", string.Empty).Replace("http://", string.Empty);
            if (string.IsNullOrWhiteSpace(this.config.ServerAddress))
            {
                this.config.ServerAddress = "crm.dynamics.com";
            }

            // One of the Microsoft Dynamics CRM Online data centers.
            if (this.config.ServerAddress.EndsWith(".dynamics.com"))
            {
                this.config.DiscoveryUri = new Uri(string.Format("https:/dev.{0}/XRMServices/2011/Discovery.svc", this.config.ServerAddress));

                // Set or get the device credentials. Required for Windows Live ID authentication. 
                this.config.DeviceCredentials = this.GetDeviceCredentials();
            }
            else if (ssl)
            {
                // Does the server use Secure Socket Layer (https)?
                this.config.DiscoveryUri = new Uri(string.Format("https://{0}/XRMServices/2011/Discovery.svc", this.config.ServerAddress));
            }
            else
            {
                this.config.DiscoveryUri = new Uri(string.Format("http://{0}/XRMServices/2011/Discovery.svc", this.config.ServerAddress));
            }

            // Get the user's logon credentials.
            this.config.Credentials = this.GetUserLogonCredentials(userdomain, password);

            // Get the target organization.
            this.config.OrganizationUri = this.GetOrganizationAddress(this.config.DiscoveryUri, organizationname);

            // Store the completed configuration.
            if (this.configurations == null)
            {
                this.configurations = new List<Configuration>();
            }

            this.configurations.Add(this.config);
            return this.config;
        }

        /// <summary>
        /// Método para parchear el acceso en el entorno test
        /// </summary>
        public static void InitiateSSLTrust()
        {
            try
            {
                // Change SSL checks so that all checks pass
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>Discovers the organizations that the calling user belongs to.</summary>
        /// <param name="service">A Discovery service proxy instance.</param>
        /// <returns>Array containing detailed information on each organization that 
        /// the user belongs to.</returns>
        public static OrganizationDetailCollection DiscoverOrganizations(IDiscoveryService service)
        {
            var orgRequest = new RetrieveOrganizationsRequest();
            var orgResponse = (RetrieveOrganizationsResponse)service.Execute(orgRequest);
            return orgResponse.Details;
        }

        /// <summary>
        /// Finds a specific organization detail in the array of organization details
        /// returned from the Discovery service.
        /// </summary>
        /// <param name="friendlyName">The friendly name of the organization to find.</param>
        /// <param name="orgDetails">Array of organization detail object returned from the discovery service.</param>
        /// <returns>Organization details or null if the organization was not found.</returns>
        /// <seealso cref="DiscoveryOrganizations"/>
        public OrganizationDetail FindOrganization(string friendlyName, OrganizationDetail[] orgDetails)
        {
            OrganizationDetail orgDetail = null;
            foreach (OrganizationDetail detail in orgDetails)
            {
                if (string.Compare(detail.FriendlyName, friendlyName) == 0)
                {
                    orgDetail = detail;
                    break;
                }
            }

            return orgDetail;
        }

        /// <summary>Reads a server configuration file.</summary>
        /// <param name="pathname">The file system path to the server configuration file.</param>
        /// <remarks>Server configurations are appended to the public configurations list.</remarks>
        public void ReadConfigurations(string pathname)
        {
            throw new NotImplementedException();
        }

        /// <summary>Writes all server configurations to a file.</summary>
        /// <remarks>If the file exists, it is overwritten.</remarks>
        /// <param name="pathname">The file name and system path of the output configuration file.</param>
        public void SaveConfigurations(string pathname)
        {
            if (this.configurations == null)
            {
                throw new Exception("No server connection configurations were found.");
            }

            foreach (Configuration config in this.configurations)
            {
                this.SaveConfiguration(pathname, config, true);
            }
        }

        /// <summary>Writes a server configuration to a file.</summary>
        /// <param name="pathname">The file name and system path of the output configuration file.</param>
        /// <param name="config">A server connection configuration.</param>
        /// <param name="append">If true, the configuration is appended to the file, otherwise a new file
        /// is created.</param>
        public void SaveConfiguration(string pathname, Configuration config, bool append)
        {
            throw new NotImplementedException();
        }

        /// <summary>Obtains the authentication type of the CRM server.</summary>
        /// <param name="uri">Uri of the CRM Discovery service.</param>
        /// <returns>Authentication type.</returns>
        public AuthenticationProviderType GetServerType(Uri uri)
        {
            return ServiceConfigurationFactory.CreateConfiguration<IDiscoveryService>(uri).AuthenticationType;
        }

        #region Protected methods
        /// <summary>
        /// Obtains the Web address (Uri) of the target organization.
        /// </summary>
        /// <param name="discoveryServiceUri">The Uri of the CRM Discovery service.</param>
        /// <param name="organizationName">Organization name</param>
        /// <returns>Uri of the organization service or an empty string.</returns>
        protected virtual Uri GetOrganizationAddress(Uri discoveryServiceUri, string organizationName)
        {
            using (DiscoveryServiceProxy serviceProxy = new DiscoveryServiceProxy(discoveryServiceUri, null, this.config.Credentials, this.config.DeviceCredentials))
            {
                // Autentica las credenciales.
                serviceProxy.Authenticate();

                // Obtain organization information from the Discovery service. 
                if (serviceProxy != null)
                {
                    // Obtain information about the organizations that the system user belongs to.
                    var orgs = DiscoverOrganizations(serviceProxy);

                    if (orgs.Count > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(organizationName))
                        {
                            for (int n = 0; n < orgs.Count; n++)
                            {
                                if (orgs[n].UniqueName == organizationName)
                                {
                                    return new System.Uri(orgs[n].Endpoints[EndpointType.OrganizationService]);
                                }
                            }

                            throw new Exception("An invalid server/organization name was specified.");
                        }
                        else
                        {
                            throw new Exception("An invalid server/organization name was specified.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nYou do not belong to any organizations on the specified server.");
                        return new Uri(string.Empty);
                    }
                }
                else
                {
                    throw new Exception("An invalid server name was specified.");
                }
            }
        }

        /// <summary>
        /// Obtains the user's logon credentials for the target server.
        /// </summary>
        /// <returns>Logon credentials of the user.</returns>
        protected virtual ClientCredentials GetUserLogonCredentials(string userdomain, string password)
        {
            var credentials = new ClientCredentials();
            string userName;
            string domain;

            // An on-premises Microsoft Dynamics CRM server deployment. 
            if (this.GetServerType(this.config.DiscoveryUri) == AuthenticationProviderType.ActiveDirectory)
            {
                string[] domainAndUserName;
                domainAndUserName = userdomain.Split('\\');

                if (domainAndUserName.Length == 1 && string.IsNullOrWhiteSpace(domainAndUserName[0]))
                {
                    return null;
                }

                domain = domainAndUserName[0];
                userName = domainAndUserName[1];
                credentials.Windows.ClientCredential = new NetworkCredential(userName, password, domain);
            }
            else if (this.GetServerType(this.config.DiscoveryUri) == AuthenticationProviderType.LiveId)
            {
                // An Microsoft Dynamics CRM Online server deployment. 
                userName = userdomain;
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return null;
                }

                credentials.UserName.UserName = userName;
                credentials.UserName.Password = password;
            }
            else if (this.GetServerType(this.config.DiscoveryUri) == AuthenticationProviderType.Federation)
            {
                // An internet facing (IFD) Microsoft Dynamics CRM server deployment.  
                userName = userdomain;
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return null;
                }

                credentials.UserName.UserName = userName;
                credentials.UserName.Password = password;
            }
            else
            {
                return null;
            }

            return credentials;
        }

        protected virtual ClientCredentials GetDeviceCredentials()
        {
            return DeviceIdManager.LoadOrRegisterDevice();
        }
        #endregion Private methods
    }

    #region Internal Classes

    /// <summary>The SolutionComponentType defines the type of solution component.</summary>
    public static class SolutionComponentType
    {
        public const int Attachment = 35;
        public const int Attribute = 2;
        public const int AttributeLookupValue = 5;
        public const int AttributeMap = 47;
        public const int AttributePicklistValue = 4;
        public const int ConnectionRole = 63;
        public const int ContractTemplate = 37;
        public const int DisplayString = 22;
        public const int DisplayStringMap = 23;
        public const int DuplicateRule = 44;
        public const int DuplicateRuleCondition = 45;
        public const int EmailTemplate = 36;
        public const int Entity = 1;
        public const int EntityMap = 46;
        public const int EntityRelationship = 10;
        public const int EntityRelationshipRelationships = 12;
        public const int EntityRelationshipRole = 11;
        public const int FieldPermission = 71;
        public const int FieldSecurityProfile = 70;
        public const int Form = 24;
        public const int KBArticleTemplate = 38;
        public const int LocalizedLabel = 7;
        public const int MailMergeTemplate = 39;
        public const int ManagedProperty = 13;
        public const int OptionSet = 9;
        public const int Organization = 25;
        public const int PluginAssembly = 91;
        public const int PluginType = 90;
        public const int Relationship = 3;
        public const int RelationshipExtraCondition = 8;
        public const int Report = 31;
        public const int ReportCategory = 33;
        public const int ReportEntity = 32;
        public const int ReportVisibility = 34;
        public const int RibbonCommand = 48;
        public const int RibbonContextGroup = 49;
        public const int RibbonCustomization = 50;
        public const int RibbonDiff = 55;
        public const int RibbonRule = 52;
        public const int RibbonTabToCommandMap = 53;
        public const int Role = 20;
        public const int RolePrivilege = 21;
        public const int SavedQuery = 26;
        public const int SavedQueryVisualization = 59;
        public const int SDKMessageProcessingStep = 92;
        public const int SDKMessageProcessingStepImage = 93;
        public const int SDKMessageProcessingStepSecureConfig = 94;
        public const int ServiceEndpoint = 95;
        public const int SiteMap = 62;
        public const int SystemForm = 60;
        public const int ViewAttribute = 6;
        public const int WebResource = 61;
        public const int Workflow = 29;
    }
    #endregion
}