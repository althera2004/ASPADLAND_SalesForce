using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using AspadLandFramework;

public partial class testsoap : Page
{
    public static HttpWebRequest CreateWebRequest(string url)
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Headers.Add("SOAP:Action");
        webRequest.ContentType = "text/xml;charset=\"utf-8\"";
        webRequest.Accept = "text/xml";
        webRequest.Method = "POST";
        return webRequest;
    }

    /// <summary>
    /// Execute a Soap WebService call
    /// </summary>
    public void Execute(string url)
    {
        HttpWebRequest request = CreateWebRequest(url);
        XmlDocument soapEnvelopeXml = new XmlDocument();
        soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
<soap:Body>
    <HelloWorld3 xmlns=""http://tempuri.org/"">
        <parameter1>test</parameter1>
        <parameter2>23</parameter2>
        <parameter3>test</parameter3>
    </HelloWorld3>
</soap:Body>
</soap:Envelope>");

        using (Stream stream = request.GetRequestStream())
        {
            soapEnvelopeXml.Save(stream);
        }

        using (WebResponse response = request.GetResponse())
        {
            using (StreamReader rd = new StreamReader(response.GetResponseStream()))
            {
                string soapResult = rd.ReadToEnd();
                Console.WriteLine(soapResult);
            }
        }
    }


    public void LoginSoap(string url)
    {
        HttpWebRequest request = CreateWebRequest(url);
        XmlDocument soapEnvelopeXml = new XmlDocument();
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12 ;

        soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:enterprise.soap.sforce.com"">
   <soapenv:Header>
      <urn:SessionHeader>
         <urn:sessionId>00D2p000000QP4e!AQ0AQH.vRYSsneZO8z8WTqSufxGQM1JTagTlVdDW9VoUm2c1vM7fQ0yh4lXCVG73w3rOOUWa0_IypeuvlLPTVJ.SODaKJcPl</urn:sessionId>
      </urn:SessionHeader>
   </soapenv:Header>
   <soapenv:Body>
      <urn:query>
<urn:queryString>
SELECT Name,
(SELECT Asegurado__r.Name, Asegurado__r.NIF__pc,
Asegurado__r.Producto_ASPAD__r.name FROM Agrupaci_nAsegPoliza__r),
(SELECT Mascota__r.Name, Mascota__r.N_de_microchip__c, Mascota__r.Sexo__c,
Mascota__r.Tipo_de_mascota__c FROM Relaci_n_Mascotas_P_lizas__r)
FROM P_liza__c WHERE Id IN (SELECT P_liza__c FROM Agrupaci_nAsegPoliza__c
WHERE Asegurado__r.NIF__pc = '94178107F') and
Producto_ASPAD__r.Nombre_Compa_ia__c = 'CASER'
</urn:queryString>
</urn:query>
   </soapenv:Body>
</soapenv:Envelope>");

        /*using (Stream stream = request.GetRequestStream())
        {
            soapEnvelopeXml.Save(stream);
        }*/

        using (WebResponse response = request.GetResponse())
        {
            using (StreamReader rd = new StreamReader(response.GetResponseStream()))
            {
                string soapResult = rd.ReadToEnd();
                this.ltdebug.Text = soapResult;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // LoginSoap("https://login.salesforce.com/services/Soap/u/47.0/0DF2p000000UBIL");


        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;

        SforceService binding = new SforceService();

        LoginResult loginResult;
        try
        {
            var user = ConfigurationManager.AppSettings["SalesForceUser"].ToString();
            var token = ConfigurationManager.AppSettings["SalesForceToken"].ToString();
            loginResult = binding.login(user, token);
            Session["SForceSessionId"] = loginResult.sessionId;
            Session["SForceWsUrl"] = loginResult.serverUrl;
            binding.Url = loginResult.serverUrl;
            binding.SessionHeaderValue = new SessionHeader
            {
                sessionId = loginResult.sessionId
            };

            Session["SForceConnection"] = binding;
        }
        catch(Exception ex)
        {
            this.ltdebug.Text = ex.Message;
        }

        var res = binding.query(@"SELECT Id, name,nIF__c,usuario_ASPADLand__c,Password_ASPADLand__c FROM Account ");

        foreach(var r in res.records)
        {
            Account ac = r as Account;
            if (!string.IsNullOrEmpty(ac.Password_ASPADLand__c))
            {
                this.ltdebug.Text += ac.Id + " == " + ac.Name + " :: " + ac.NIF__c + " :: " + ac.Usuario_ASPADLand__c + " --> " + ac.Password_ASPADLand__c + "<br />";
            }
        }
    }
}