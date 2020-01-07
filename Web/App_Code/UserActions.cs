using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework.Activity;
using SbrinnaCoreFramework.DataAccess;
using ShortcutFramework.Item;

/// <summary>Descripción breve de UserActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class UserActions : WebService
{
    private const int smtpPort = 25;

    public UserActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult SendChanges(
        Guid centroId,
        string actualUserId,
        string actualUserName,
        string userName,
        string telefono1,
        string telefono2,
        string direccion,
        string poblacion,
        string cp,
        string provincia,
        string email,
        string emailAlternativo,
        bool urg24,
        bool urgTel,
        string emailFacturacion)
    {
        var res = ActionResult.NoAction;
        try
        {
            using (var mail = new MailMessage())
            {
                using (var SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"]))
                {
                    var actualUser = (ApplicationUser)Session["User"];
                    mail.Subject = "ASPADLand - Cambio datos clínica " +  actualUserId + " - " + actualUser.Nombre;
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["MailSender"]);
                    mail.To.Add(ConfigurationManager.AppSettings["MailDestination"]);
                    mail.Bcc.Add("jcastilla@sbrinna.com");

                    var datos = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<h4>Se ha solicitado a través de ASPADLand un cambio de datos del centro <strong>{11}</strong>.</h4>
                        <table border=""1"">
                        <thead><th></th><th>Dato actual</th><th>Cambio solicitado</th></tr></thead>
                        <tbody>
                        <tr><td>Nombre del centro:</td><td>{12}</td><td>{0}</td></tr>
                        <tr><td>Dirección:</td><td>{13}</td><td>{1}</td></tr>
                        <tr><td>Código postal:</td><td>{14}</td><td>{2}</td></tr>
                        <tr><td>Población:</td><td>{15}</td><td>{3}</td></tr>
                        <tr><td>Provincia:</td><td>{16}</td><td>{4}</td></tr>
                        <tr><td>Email principal:</td><td>{17}</td><td>{5}</td></tr>
                        <tr><td>Email alternativo:</td><td>{18}</td><td>{6}</td></tr>
                        <tr><td>Teléfono contacto:</td><td>{19}</td><td>{7}</td></tr>
                        <tr><td>Teléfono urgencias:</td><td>{20}</td><td>{8}</td></tr>
                        <tr><td>Urgencias presenciales:</td><td>{21}</td><td>{9}</td></tr>
                        <tr><td>Urgencias telefónicas:</td><td>{22}</td><td>{10}</td></tr>
                        <tr><td>Email facturación:</td><td>{24}</td><td>{23}</td></tr>
                        </tbody>
                        </table>",
                        actualUserName.Equals(actualUser.Nombre) ? string.Empty : actualUserName,
                        direccion.Equals(actualUser.Direccion) ? string.Empty : direccion,
                        cp.Equals(actualUser.CP) ? string.Empty : cp,
                        poblacion.Equals(actualUser.Poblacion) ? string.Empty : poblacion,
                        provincia.Equals(actualUser.Provincia) ? string.Empty : provincia,
                        email.Equals(actualUser.Email1) ? string.Empty : email,
                        emailAlternativo.Equals(actualUser.Email2) ? string.Empty : emailAlternativo,
                        telefono1.Equals(actualUser.Telefono1) ? string.Empty : telefono1,
                        telefono2.Equals(actualUser.Telefono2) ? string.Empty : telefono2,
                        urg24 == actualUser.UrgenciasPresenciales ? string.Empty : (urg24 ? "SI" : "NO"),
                        urgTel == actualUser.UrgenciasTelefono ? string.Empty : (urgTel ? "SI" : "NO"),
                        actualUserName,
                        actualUser.Nombre,
                        actualUser.Direccion,
                        actualUser.CP,
                        actualUser.Poblacion,
                        actualUser.Provincia,
                        actualUser.Email1,
                        actualUser.Email2,
                        actualUser.Telefono1,
                        actualUser.Telefono2,
                        actualUser.UrgenciasPresenciales ? "SI" : "NO",
                        actualUser.UrgenciasTelefono ? "SI" : "NO",
                        emailFacturacion.Equals(actualUser.FacturacionEmail) ? string.Empty : emailFacturacion,
                        actualUser.FacturacionEmail);

                    mail.IsBodyHtml = true;
                    mail.Body = datos;

                    SmtpServer.Port = smtpPort;
                    SmtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailSenderUser"], ConfigurationManager.AppSettings["MailSenderPassword"]);
                    SmtpServer.Send(mail);
                    res.SetSuccess();

                    var mailTrace = new MailTrace
                    {
                        Body = datos,
                        CentroId = centroId,
                        SendDate = DateTime.Now,
                        Sender = ConfigurationManager.AppSettings["MailSender"] as string,
                        To = email,
                        Subject = mail.Subject
                    };
                    mailTrace.Insert();

                    ComunicationInsert(centroId, 4, datos);
                }
            }
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult SendSugerencia(
        Guid centroId,
        string userId,
        string actualUserId,
        string actualUserName,
        string sugerencia)
    {
        var res = ActionResult.NoAction;
        try
        {
            using (var mail = new MailMessage())
            {
                using (var SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"]))
                {
                    mail.Subject = "ASPADLand - Sugerencia por parte de " + actualUserId + " - " + actualUserName;
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["MailSender"]);
                    mail.To.Add(ConfigurationManager.AppSettings["MailDestination"]);
                    mail.Bcc.Add("jcastilla@sbrinna.com");

                    var datos = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<h4>Se ha recibido una sugerencia con el siguiente texto:</h4>
                        <p>{0}</p>",
                        sugerencia);

                    mail.IsBodyHtml = true;
                    mail.Body = datos;

                    SmtpServer.Port = smtpPort;
                    SmtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailSenderUser"], ConfigurationManager.AppSettings["MailSenderPassword"]);
                    SmtpServer.Send(mail);

                     /* CREATE PROCEDURE ASPADLADN_Sugerencia_Insert
                      *   @Text text,
                      *   @CentroId uniqueidentifier */
                    using(var cmd = new SqlCommand("ASPADLADN_Sugerencia_Insert"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CentroId", centroId));
                        cmd.Parameters.Add(new SqlParameter("@Text", SqlDbType.Text));
                        cmd.Parameters["@Text"].Direction = ParameterDirection.Input;
                        cmd.Parameters["@Text"].Value = sugerencia;
                        using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                        {
                            cmd.Connection = cnn;
                            try
                            {
                                cmd.Connection.Open();
                                cmd.ExecuteNonQuery();
                            }
                            finally
                            {
                                if(cmd.Connection.State != ConnectionState.Closed)
                                {
                                    cmd.Connection.Close();
                                }
                            }
                        }
                    }

                    res.SetSuccess();

                    var mailTrace = new MailTrace
                    {
                        Body = datos,
                        CentroId = centroId,
                        SendDate = DateTime.Now,
                        Sender = ConfigurationManager.AppSettings["MailSender"] as string,
                        To = ConfigurationManager.AppSettings["MailDestination"] as string,
                        Subject = mail.Subject
                    };
                    mailTrace.Insert();

                    ComunicationInsert(centroId, 3, datos);
                }
            }
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult SendChangesHorario(
        Guid centroId,
        string actualUserId,
        string actualUserName,
        string L1,
        string L2,
        string M1,
        string M2,
        string X1,
        string X2,
        string J1,
        string J2,
        string V1,
        string V2,
        string S1,
        string S2,
        string D1,
        string D2)
    {
        var res = ActionResult.NoAction;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        try
        {
            using (var mail = new MailMessage())
            {
                using (var SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"]))
                {
                    mail.Subject = string.Format(
                        CultureInfo.InvariantCulture,
                        dictionary["Item_Profile_Horario_MailSubject"],
                        actualUserId,
                        actualUserName);
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["MailSender"]);
                    mail.To.Add(ConfigurationManager.AppSettings["MailDestination"]);
                    mail.Bcc.Add("jcastilla@sbrinna.com");

                    var title = string.Format(
                        CultureInfo.InvariantCulture,
                        dictionary["Item_Profile_Horario_MailTitle"],
                        actualUserName);

                    var datos = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<h4>{15}</h4>
                        <table>
                        <tr><td>Dia</td><td>Mañana</td><td>Tarde</td></tr>
                        <tr><td>Lunes</td><td>{1}</td><td>{2}</td></tr>
                        <tr><td>Martes</td><td>{3}</td><td>{4}</td></tr>
                        <tr><td>Miércoles</td><td>{5}</td><td>{6}</td></tr>
                        <tr><td>Jueves</td><td>{7}</td><td>{8}</td></tr>
                        <tr><td>Viernes</td><td>{9}</td><td>{10}</td></tr>
                        <tr><td>Sábado</td><td>{11}</td><td>{12}</td></tr>
                        <tr><td>Domingo</td><td>{13}</td><td>{14}</td></tr>
                        </table>",
                        actualUserName,
                        L1, L2, M1, M2, X1, X2, J1, J2, V1, V2, S1, S2, D1, D2,title);

                    mail.IsBodyHtml = true;
                    mail.Body = datos;

                    SmtpServer.Port = smtpPort;
                    SmtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailSenderUser"], ConfigurationManager.AppSettings["MailSenderPassword"]);
                    SmtpServer.Send(mail);
                    res.SetSuccess();

                    var mailTrace = new MailTrace
                    {
                        Body = datos,
                        CentroId = centroId,
                        SendDate = DateTime.Now,
                        Sender = ConfigurationManager.AppSettings["MailSender"] as string,
                        To = ConfigurationManager.AppSettings["MailDestination"] as string,
                        Subject = mail.Subject
                    };
                    mailTrace.Insert();

                    ComunicationInsert(centroId, 2, datos);
                }
            }
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult SendCuadroMedico(
        Guid centroId,
        string actualUserId,
        string actualUserName,
        string activos,
        string inactivos)
    {
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var res = ActionResult.NoAction;
        var actos = this.Session["Actos"] as ReadOnlyCollection<Acto>;
        var activosId = activos.Split('|').Where(i => !string.IsNullOrEmpty(i)).ToList();
        var inactivosId = inactivos.Split('|').Where(i => !string.IsNullOrEmpty(i)).ToList();

        var activosIdsUnique = new List<string>();
        var inactivosIdsUnique = new List<string>();
        foreach (var id in activosId)
        {
            if (!activosIdsUnique.Contains(id))
            {
                activosIdsUnique.Add(id);
            }
        }

        foreach (var id in inactivosId)
        {
            if (!inactivosIdsUnique.Contains(id))
            {
                inactivosIdsUnique.Add(id);
            }
        }

        var activosText = new StringBuilder(dictionary["Item_Profile_Actos_NoNewMessage"]);
        var inactivosText = new StringBuilder(dictionary["Item_Profile_Actos_NoRefusedMessage"]);

        bool sinCambios = true;

        if (activosIdsUnique.Count > 0)
        {
            activosText = new StringBuilder(string.Empty);
            foreach (var actoId in activosIdsUnique)
            {
                foreach (var acto in actos)
                {
                    if (acto.Id.ToString().ToLowerInvariant() == actoId)
                    {
                        activosText.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"<tr><td>{0}</td><td>{1}</td></tr>",
                            acto.EspecialidadName, acto.Description);
                        sinCambios = false;
                        break;
                    }
                }
            }
        }

        if (inactivosIdsUnique.Count > 0)
        {
            inactivosText = new StringBuilder(string.Empty);
            foreach (var actoId in inactivosIdsUnique)
            {
                foreach (var acto in actos)
                {
                    if (acto.Id.ToString().ToLowerInvariant() == actoId)
                    {
                        inactivosText.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"<tr><td>{0}</td><td>{1}</td></tr>",
                            acto.EspecialidadName, acto.Description);
                        sinCambios = false;
                        break;
                    }
                }
            }
        }

        if (sinCambios)
        {
            res.SetFail("No ha habido cambios en el cuadro medico");
            return res;
        }

        try
        {
            using (var mail = new MailMessage())
            {
                using (var SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"]))
                {
                    mail.Subject = string.Format(
                        CultureInfo.InvariantCulture,
                        dictionary["Item_Profile_Actos_MailSubject"],
                        actualUserId,
                        actualUserName);
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["MailSender"]);
                    mail.To.Add(ConfigurationManager.AppSettings["MailDestination"]);
                    mail.Bcc.Add("jcastilla@sbrinna.com");

                    string title = string.Format(
                        CultureInfo.InvariantCulture,
                        dictionary["Item_Profile_Actos_MailTitle"],
                        actualUserName);

                    var datos = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<h4>{3}</h4>
                        <table>
                            <tr><td colspan=""2""><h4>Nuevos actos</h4></td></tr>
                            {1}
                            <tr><td colspan=""2""><h4>Actos retirados</h4></td></tr>
                            {2}
                        </table>",
                        actualUserName,
                        activosText,
                        inactivosText,
                        title);

                    mail.IsBodyHtml = true;
                    mail.Body = datos;

                    SmtpServer.Port = smtpPort;
                    SmtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailSenderUser"], ConfigurationManager.AppSettings["MailSenderPassword"]);
                    SmtpServer.Send(mail);
                    res.SetSuccess();

                    var mailTrace = new MailTrace
                    {
                        Body = datos,
                        CentroId = centroId,
                        SendDate = DateTime.Now,
                        Sender = ConfigurationManager.AppSettings["MailSender"] as string,
                        To = ConfigurationManager.AppSettings["MailDestination"] as string,
                        Subject = mail.Subject
                    };
                    mailTrace.Insert();

                    ComunicationInsert(centroId, 1, datos);
                }
            }
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }

    private void ComunicationInsert(Guid centroId, int type, string message)
    {
        try
        {
            /* CREATE PROCEDURE ASPADLand_Comunicaciones_Insert
             *   @CentroId uniqueidentifier,
             *   @Tipo int,
             *   @Message text */
            using (var cmd = new SqlCommand("ASPADLand_Comunicaciones_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CentroId", centroId));
                    cmd.Parameters.Add(DataParameter.Input("@Tipo", type));
                    cmd.Parameters.Add(DataParameter.Input("@Message", message));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }
        finally
        {

        }
    }
}