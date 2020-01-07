using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Text;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;

namespace AuditRecovery.Helper
{
    public class CRMConnectionManager
    {
        private IOrganizationService service;

        public CRMConnectionManager()
        {

        }

        public IOrganizationService GetService(string server, string user, string password, string organization, string domain, bool isHttps)
        {
            if (service == null)
            {
                service = initialize(server, user, password, organization, domain, isHttps);
            }
            return service;
        }

        public IOrganizationService GetService()
        {
            if (service == null)
            {
                CRMConnectionSetting crmSet = new CRMConnectionSetting();
                service = initialize(crmSet.GetServer(), crmSet.GetUser(), crmSet.GetPassword(), crmSet.GetOrganization(),
                    crmSet.GetDomain(), crmSet.GetIsHttps());
            }
            return service;
        }

        private IOrganizationService initialize(string server, string user, string password, string organization, string domain, bool isHttps)
        {
            try
            {
                ClientCredentials clientCredentials = new ClientCredentials();
                clientCredentials.Windows.ClientCredential = new NetworkCredential(user, password, domain);
                //http://localhost:5555/MSCRMServices/2007/CrmServiceWsdl.aspx
                Uri organizationUri = new Uri(
                            string.Format("{0}://{1}/{2}/XRMServices/2011/Organization.svc",
                                isHttps ? "https" : "http",
                                server,
                                organization
                            )
                        );
                Uri HomeRealmUri = null;

                using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(organizationUri, HomeRealmUri, clientCredentials, null))
                {
                    serviceProxy.Timeout = new TimeSpan(0, 20, 0);
                    serviceProxy.EnableProxyTypes();
                    return (IOrganizationService)serviceProxy;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
