using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework.Activity;

public partial class _Default : Page
{
    private string languageBrowser;
    private string ip;
    private string companyCode;

    public string FrameworkVersion
    {
        get
        {
            return ConfigurationManager.AppSettings["version"];
        }
    }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public string CompanyCode
    {
        get { return this.companyCode; }
    }

    public string LanguageBrowser
    {
        get { return this.languageBrowser; }
    }

    public string IP
    {
        get { return this.ip; }
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Navigation"] = null;
        if (this.Request.UserLanguages != null)
        {
            this.languageBrowser = this.Request.UserLanguages[0];
        }

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
        var url = ConfigurationManager.AppSettings["SalesForceUrl"].ToString();
        SforceService binding = new SforceService(url);
        try
        {
            var user = ConfigurationManager.AppSettings["SalesForceUser"].ToString();
            var token = ConfigurationManager.AppSettings["SalesForceToken"].ToString();
            var loginResult = binding.login(user, token);
            Session["SForceSessionId"] = loginResult.sessionId;
            Session["SForceWsUrl"] = loginResult.serverUrl;
            binding.Url = loginResult.serverUrl;
            binding.SessionHeaderValue = new SessionHeader
            {
                sessionId = loginResult.sessionId
            };

            Session["SForceConnection"] = binding;
        }
        catch (Exception ex)
        {
            ExceptionManager.Trace(ex, "Salesforce login");
        }

        this.ip = this.GetUserIP();
        Session["ColectivosASPAD"] = Colectivo.AllASPAD;
        Session["ColectivosASPADJson"] = Colectivo.JsonList(Session["ColectivosASPAD"] as ReadOnlyCollection<Colectivo>);
        Session["ProductosASPAD"] = Producto.AllASPAD;
        Session["PreciosASPAD"] = AspadLandFramework.Item.Acto.AllASPAD;
    }

    private string GetUserIP()
    {
        string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipList))
        {
            return ipList.Split(',')[0];
        }

        return Request.ServerVariables["REMOTE_ADDR"];
    }
}