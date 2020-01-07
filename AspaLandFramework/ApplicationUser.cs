// --------------------------------
// <copyright file="ApplicationUser.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace AspadLandFramework
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Net.Mail;
    using System.Web;
    using AspadLandFramework.Item;
    using SbrinnaCoreFramework.Activity;
    using SbrinnaCoreFramework.DataAccess;
    using SbrinnaCoreFramework;

    /// <summary>Implements ApplicationUser class.</summary>
    public class ApplicationUser
    {
        /// <summary>Value to indicate that an invalid password is send to change</summary>
        private const string IncorrectPassword = "NOPASS";

        /// <summary>Application user identifier</summary>
        public string Id { get; set; }

        /// <summary>Gets or sets user code</summary>
        public string Code { get; set; }

        /// <summary>Gets or sets a value indicating whether is online help is showed</summary>
        public bool ShowHelp { get; set; }

        /// <summary>Gets or sets User name</summary>
        public string UserName { get; set; }
        public string NIF { get; set; }
        public string Nombre { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Direccion { get; set; }
        public string CP { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }

        public string FacturacionFirstName { get; set; }
        public string FacturacionLastName { get; set; }
        public string FacturacionNIF { get; set; }
        public string FacturacionEmail { get; set; }

        public bool UrgenciasPresenciales { get; set; }
        public bool UrgenciasTelefono { get; set; }

        /// <summary>Gets or sets monday schelude</summary>
        public string HorarioLunes { get; set; }

        /// <summary>Gets or sets tuesday schelude</summary>
        public string HorarioMartes { get; set; }

        /// <summary>Gets or sets wednesday schelude</summary>
        public string HorarioMiercoles { get; set; }

        /// <summary>Gets or sets thursday schelude</summary>
        public string HorarioJueves { get; set; }

        /// <summary>Gets or sets friday schelude</summary>
        public string HorarioViernes { get; set; }

        /// <summary>Gets or sets saturday schelude</summary>
        public string HorarioSabado { get; set; }

        /// <summary>Gets or sets sunday schelude</summary>
        public string HorarioDomingo { get; set; }

        /// <summary>Initializes a new instance of the ApplicationUser class</summary>
        public ApplicationUser()
        {
            this.Id = string.Empty;
            this.UserName = string.Empty;
        }

        public int CompanyId { get; set; }

        /// <summary>Gets a empty user</summary>
        public static ApplicationUser Empty
        {
            get
            {
                return new ApplicationUser()
                {
                    Id = string.Empty,
                    UserName = string.Empty
                };
            }
        }

        public static ActionResult ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE ASPADLAND_ChangePassword
             *   @UserId uniqueidentifier,
             *   @OldPassword nvarchar(50),
             *   @NewPassword nvarchar(50),
             *   @ActionResult int */
            using(var cmd = new SqlCommand("ASPADLAND_ChangePassword"))
            {
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@OldPassword", oldPassword, Constant.DefaultDataBaseLength));
                    cmd.Parameters.Add(DataParameter.Input("@NewPassword", newPassword, Constant.DefaultDataBaseLength));
                    cmd.Parameters.Add(DataParameter.OutputInt("@ActionResult"));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(Convert.ToInt32(cmd.Parameters["@ActionResult"].Value));
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

            return res;
        }

        /// <summary>Gets the HTML code for the link to Employee view page</summary>
        public string Link
        {
            get
            {
                var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                string toolTip = dictionary["Common_Edit"];
                return string.Format(CultureInfo.InvariantCulture, "<a href=\"UserView.aspx?id={0}\" title=\"{2} {1}\">{1}</a>", this.Id, this.UserName, toolTip);
            }
        }

        /// <summary>Gets a json key/value structure of user data</summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture, 
                    @"{{""Id"":{0},""Value"":""{1}""}}",
                    this.Id, 
                    SbrinnaCoreFramework.Tools.JsonCompliant(this.UserName));
            }
        }

        /// <summary>Gets a json structure of user</summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":""{0}"",""Codigo"":""{1}"",""Description"":""{2}""}}",
                    this.Id,
                    this.Code,
                    this.UserName);
            }
        }

        public static ActionResult SetPassword(int applicationUserId, string password)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[ApplicationUser_SetPassword]
             *   @UserId int,
             *   @Password nvarchar(50) */
            using (var cmd = new SqlCommand("ApplicationUser_SetPassword"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@UserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@Password", password, Constant.DefaultDataBaseLength));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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
            return res;
        }

        /// <summary>Gets user the by identifier.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Data of application user</returns>
        public static ApplicationUser GetById(string userId)
        {
            var res = ApplicationUser.Empty;

            var query = string.Format(
                CultureInfo.InvariantCulture,
                @"select id,
                usuario_ASPADLand__c,
                name,
                Direcci_n_comercial__c,
                Email__c,
                Poblaci_n1__c,
                C_digo_Postal__r.Name,
                Provincia__c,
                Realizaci_n_de_urgencias__c,
                Tel_fono_Urgencias__c,
                Email_Factura_1__c,
                HorarioLunes__c,
                Horario_Martes__c,
                Horario_Miercoles__c,
                Horario_Jueves__c,
                Horario_Viernes__c,
                Horario_S_bado__c,
                Horario_Domingo__c
                from account
                where RecordTypeId = '0122p000000cuvJAAQ' and
                Id = '{0}'", userId);
            var binding = HttpContext.Current.Session["SForceConnection"] as SforceService;
            var bindingResult = binding.query(query);

            if (bindingResult != null)
            {
                Account ac = bindingResult.records[0] as Account;
                res.Id = ac.Id;
                res.UserName = ac.Usuario_ASPADLand__c;
                res.NIF = ac.NIF__c;
                res.Direccion = ac.Direcci_n_comercial__c;
                res.Poblacion = ac.Poblaci_n1__c;
                res.Provincia = ac.Provincia__c;
                if (ac.C_digo_Postal__r != null) { res.CP = ac.C_digo_Postal__r.Name; }
                res.Email1 = ac.Email__c;
                res.Email2 = string.Empty;
                res.Telefono1 = string.Empty;
                res.Telefono2 = ac.Tel_fono_Urgencias__c;
                res.Nombre = ac.Name;
                res.HorarioLunes = ac.HorarioLunes__c;
                res.HorarioMartes = ac.Horario_Martes__c;
                res.HorarioMiercoles = ac.Horario_Miercoles__c;
                res.HorarioJueves = ac.Horario_Jueves__c;
                res.HorarioViernes = ac.Horario_Viernes__c;
                res.HorarioSabado = ac.Horario_S_bado__c;
                res.HorarioDomingo = ac.Horario_Domingo__c;
                res.UrgenciasPresenciales = false;
                res.UrgenciasTelefono = ac.Realizaci_n_de_urgencias__c.Equals("Telfónicas");
                res.Code = string.Empty;
                res.FacturacionFirstName = string.Empty;
                res.FacturacionLastName = string.Empty;
                res.FacturacionNIF = string.Empty;
                res.FacturacionEmail = ac.Email_Factura_1__c;
            }

            return res;
        }

        /// <summary>Change user's password</summary>
        /// <param name="userId">User identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult ResetPassword(string userId, int companyId)
        {
            var res = ActionResult.NoAction;
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;

            /* CREATE PROCEDURE ApplicationUser_ChangePassword
             * @UserId int,
             * @CompanyId int,
             * @Result int out */
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_ResetPassword"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Result", SqlDbType.Int);
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@Result"].Value = DBNull.Value;
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50);
                    cmd.Parameters["@UserName"].Value = DBNull.Value;
                    cmd.Parameters["@Password"].Value = DBNull.Value;
                    cmd.Parameters["@Email"].Value = DBNull.Value;
                    cmd.Parameters["@UserName"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@Password"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@Email"].Direction = ParameterDirection.Output;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    #region Send Mail
                    var company = new Company(companyId);
                    string userName = cmd.Parameters["@UserName"].Value as string;
                    string password = cmd.Parameters["@Password"].Value as string;
                    string email = cmd.Parameters["@Email"].Value as string;

                    var selectedUser = ApplicationUser.GetById(userId);

                    string sender = ConfigurationManager.AppSettings["mailaddress"];
                    string pass = ConfigurationManager.AppSettings["mailpass"];

                    var senderMail = new MailAddress(sender, "ISSUS");
                    var to = new MailAddress(email);

                    var client = new SmtpClient()
                    {
                        Host = "smtp.scrambotika.com",
                        Credentials = new System.Net.NetworkCredential(sender, pass),
                        Port = Constant.SmtpPort,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };

                    var mail = new MailMessage(senderMail, to)
                    {
                        IsBodyHtml = true,
                        Subject = dictionary["MailTemplate_ResetPassword_DefaultBody"]
                    };

                    string templatePath = HttpContext.Current.Request.PhysicalApplicationPath;
                    string translatedTemplate = "ResetPassword.tpl";
                    if (!templatePath.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                    {
                        templatePath = string.Format(CultureInfo.InvariantCulture, @"{0}\", templatePath);
                    }

                    if(!File.Exists(templatePath + "Templates\\"+ translatedTemplate))
                    {
                        translatedTemplate = "ResetPassword.tpl";
                    }

                    string body = string.Format(
                        CultureInfo.InvariantCulture,
                        dictionary["MailTemplate_ResetPassword_DefaultBody"],
                        userName,
                        password);

                    if(File.Exists(templatePath + "Templates\\" + translatedTemplate))
                    {
                        using(StreamReader input = new StreamReader(templatePath + "Templates\\" + translatedTemplate))
                        {
                            body = input.ReadToEnd();
                        }

                        body = body.Replace("#USERNAME#", userName).Replace("#PASSWORD#", password).Replace("#EMAIL#", email).Replace("#EMPRESA#", company.Name);
                    }

                    mail.Subject = dictionary["MailTemplate_ResetPassword_Subject"];
                    mail.Body = body;
                    client.Send(mail);
                    #endregion

                    if (cmd.Parameters["@Result"].Value.ToString().Trim() == "1")
                    {
                        res.SetSuccess();
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "ResetPassword({0}, {1})", userId, companyId));
                    res.SetFail(dictionary["MailTemplate_ResetPassword_Error"]);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return res;
        }

        public static ActionResult Delete(long userItemId, int companyId, long userId)
        {
            var res = ActionResult.NoAction;

            /* CREATE PROCEDURE ApplicationUser_Delete
             *   @UserItemId bigint,
             *   @CompanyId int,
             *   @UserId bigint */
            using (var cmd = new SqlCommand("ApplicationUser_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@UserItemId", userItemId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return res;
        }
    }
}