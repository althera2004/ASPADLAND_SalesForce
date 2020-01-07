// --------------------------------
// <copyright file="LoginActions.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GISOWeb
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Net.Mail;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using AspadLandFramework;
    using AspadLandFramework.LogOn;
    using SbrinnaCoreFramework;
    using SbrinnaCoreFramework.Activity;
    using SbrinnaCoreFramework.DataAccess;
    using ShortcutFramework.Item;

    /// <summary>Summary description for LoginActions</summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class LoginActions : WebService
    {
        public LoginActions()
        {
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult GetLogin(string email, string password, string ip)
        {
            var res = ActionResult.NoAction;
            res = ApplicationLogOn.ApplicationAccess(email, password, ip);
            if (res.Success)
            {
                var logon = res.ReturnValue as LogOnObject;
                HttpContext.Current.Session["dictionary"] = ApplicationDictionary.Load("es");
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ChangePassword(string email)
        {
            var res = ActionResult.NoAction;

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult RecuperarPassword(string email)
        {
            var res = ActionResult.NoAction;
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            try
            {
                var userId = Guid.Empty;
                var userName = string.Empty;
                var password = string.Empty;
                using(var cmd = new SqlCommand("ASPADLAN_GetUserByMail"))
                {
                    using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Email", email, 100));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    userId = rdr.GetGuid(0);
                                    userName = rdr.GetString(1);
                                }
                            }

                            if(userId != Guid.Empty)
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                                cmd.Parameters.Add(DataParameter.OutputString("@Password", 10));
                                cmd.CommandText = "ASPADLAND_ResetPassword";
                                cmd.ExecuteNonQuery();
                                password = cmd.Parameters["@Password"].Value.ToString();
                                res.SetSuccess();
                            }
                        }
                        catch(Exception ex)
                        {
                            res.SetFail(ex);
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

                #region Send Mail
                if (res.Success)
                {
                    string user = ConfigurationManager.AppSettings["MailSenderUser"];
                    string sender = ConfigurationManager.AppSettings["MailSender"];
                    string pass = ConfigurationManager.AppSettings["MailSenderPassword"];
                    string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];

                    var senderMail = new MailAddress(sender, "ASPADLand");
                    var to = new MailAddress(email);

                    var client = new SmtpClient()
                    {
                        Host = smtpServer,
                        Credentials = new System.Net.NetworkCredential(user, pass),
                        Port = Constant.SmtpPort,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    
                    string body = string.Format(
                        CultureInfo.InvariantCulture,
                        "Hola {0}.<br />Se ha reiniciado la contraseña para el sitio http://aspadland.aspad.es/,.<br />La nueva contraseña es {1}",
                        userName,
                        password);                    

                    var mail = new MailMessage(senderMail, to)
                    {
                        IsBodyHtml = true,
                        Subject = "ASPADLand - Reinicio de contraseña",
                        Body = body
                    };

                    mail.Bcc.Add(new MailAddress("jcastilla@sbrinna.com"));

                    client.Send(mail);

                    var mailTrace = new MailTrace
                    {
                        Body = body,
                        CentroId = userId,
                        SendDate = DateTime.Now,
                        Sender = sender,
                        To = email,
                        Subject = mail.Subject
                    };
                    mailTrace.Insert();
                }
                #endregion

            }
            catch (SqlException ex)
            {
                res.SetFail(dictionary["MailTemplate_ResetPassword_Error"]);
            }
            finally
            {

            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ChangePassword(int userId, string oldPassword, string newPassword, int companyId)
        {
            /*ActionResult res = ApplicationUser.ChangePassword(userId, oldPassword, newPassword, companyId);
            if (res.MessageError == "NOPASS")
            {
                Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                if (dictionary != null)
                {
                    res.MessageError = dictionary["Common_Error_IncorrectPassword"];
                }
                else
                {
                    res.MessageError = "Incorrect password";
                }
            }*/

            return ActionResult.NoAction;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ResetPassword(string userId, int companyId)
        {
            return ApplicationUser.ResetPassword(userId, companyId);
        }
    }
}