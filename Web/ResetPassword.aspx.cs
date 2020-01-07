using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspadLandFramework.Item;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Globalization;

public partial class ResetPassword : Page
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

        this.ip = this.GetUserIP();
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