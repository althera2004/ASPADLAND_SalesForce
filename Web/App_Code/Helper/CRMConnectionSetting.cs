namespace AuditRecovery.Helper
{
    public class CRMConnectionSetting
    {
        const string SERVER_KEY = "Server";
        const string USER_KEY = "User";
        const string PASSWORD_KEY = "Password";
        const string ORGANIZATION_KEY = "Organization";
        const string DOMAIN_KEY = "Domain";
        const string ISHTTPS_KEY = "IsHttps";

        public CRMConnectionSetting()
        {
        }

        public string GetServer()
        {
            return getValue(SERVER_KEY);
        }

        public string GetUser()
        {
            return getValue(USER_KEY);
        }

        public string GetPassword()
        {
            return getValue(PASSWORD_KEY);
        }

        public string GetOrganization()
        {
            return getValue(ORGANIZATION_KEY);
        }

        public string GetDomain()
        {
            return getValue(DOMAIN_KEY);
        }

        public bool GetIsHttps()
        {
            return bool.Parse(getValue(ISHTTPS_KEY));
        }

        private string getValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
