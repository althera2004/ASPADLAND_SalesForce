// --------------------------------
// <copyright file="ApplicationLogOn.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace AspadLandFramework.LogOn
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using SbrinnaCoreFramework.Activity;
    using SbrinnaCoreFramework.DataAccess;
    using AspadLandFramework.Item;

    /// <summary>Implements ApplicationLogOn class</summary>
    public static class ApplicationLogOn
    {
        /// <summary>Enum for log on result</summary>
        public enum LogOnResult
        {
            /// <summary>None - 0</summary>
            None = 0,

            /// <summary>Ok - 1</summary>
            Ok = 1,

            /// <summary>NoUser - 2</summary>
            NoUser = 2,

            /// <summary>LockUser - 3</summary>
            LockUser = 3,

            /// <summary>Fail - 4</summary>
            Fail = 4,

            /// <summary>Admin - 1001</summary>
            Admin = 1001,

            /// <summary>Administrative - 1002</summary>
            Administrative = 1002
        }

        /// <summary>Trace a log on failed</summary>
        /// <param name="userId">Identifier of user that attemps to log on</param>
        public static void LogOnFailed(Guid userId)
        {
            /*using (SqlCommand cmd = new SqlCommand("LogonFailed"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cmd.Connection.Open();
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }*/
        }

        /// <summary>Log on application</summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="clientAddress">IP address from log on action</param>
        /// <returns>Result of action</returns>
        public static ActionResult ApplicationAccess(string email, string password, string clientAddress)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return ActionResult.NoAction;
            }

            var res = ActionResult.NoAction;
            var result = new LogOnObject
            {
                Id = string.Empty,
                UserName = string.Empty,
                Result = LogOnResult.NoUser,
                MustResetPassword = false
            };

            var binding = HttpContext.Current.Session["SForceConnection"] as SforceService;
            var query = string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT Id, name,nIF__c,usuario_ASPADLand__c,Password_ASPADLand__c FROM Account WHERE usuario_ASPADLand__c = '{0}'",
                email);

            var bindingResult = binding.query(query);
            var login = false;
            if (bindingResult != null)
            {
                foreach (var r in bindingResult.records)
                {
                    Account ac = r as Account;
                    if (ac.Password_ASPADLand__c.Equals(password))
                    {
                        var actosCentro = Acto.ByCentro(ac.Usuario_ASPADLand__c);
                        var actos = Acto.All;

                        foreach (var acto in actos)
                        {
                            foreach (var actoCentro in actosCentro)
                            {
                                if (actosCentro.Any(a => a.Id.Equals(acto.Id, StringComparison.OrdinalIgnoreCase)))
                                {
                                    acto.Ofertado = true;
                                    break;
                                }
                            }
                        }

                        login = true;
                        result.Result = LogOnResult.Ok;
                        result.Id = ac.Id;
                        result.UserName = ac.Name;
                        HttpContext.Current.Session["User"] = ApplicationUser.GetById(ac.Id);
                        HttpContext.Current.Session["Actos"] = actosCentro;
                        HttpContext.Current.Session["Colectivos"] = Colectivo.All;
                        HttpContext.Current.Session["ColectivosASPAD"] = Colectivo.AllASPAD;
                        break;
                    }
                }
            }

            if (!login)
            {
                result.Result = LogOnResult.Fail;
            }
            
            /*using(var cmdAdmin = new SqlCommand("ASPADLand_Admin_Login"))
            {
                using (var cnnAdmin = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmdAdmin.Connection = cnnAdmin;
                    cmdAdmin.CommandType = CommandType.StoredProcedure;
                    cmdAdmin.Parameters.Add(DataParameter.Input("@UserName", email, 50));
                    cmdAdmin.Parameters.Add(DataParameter.Input("@Password", password, 50));
                    try
                    {
                        cmdAdmin.Connection.Open();
                        using (var rdrAdmin = cmdAdmin.ExecuteReader())
                        {
                            if (rdrAdmin.HasRows)
                            {
                                result.Result = LogOnResult.Admin;
                            }
                        }
                    }
                    finally
                    {
                        if(cmdAdmin.Connection.State != ConnectionState.Closed)
                        {
                            cmdAdmin.Connection.Close();
                        }
                    }
                }
            }

            if (result.Result != LogOnResult.NoUser)
            {
                res.SetSuccess(result);
                return res;
            }

            var query = string.Format(
                CultureInfo.InvariantCulture,
                "SELECT accountid, qes_user, qes_contrasenya, ISNULL(qes_resetpassword,0) FROM account where qes_user = '{0}' AND qes_contrasenya = '{1}'",
                email,
                password);
            using (var cmd = new SqlCommand(query))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            bool multiCompany = false;
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    result.Id = rdr.GetGuid(0);
                                    result.UserName = email;
                                    result.MustResetPassword = rdr.GetBoolean(3);
                                    result.Result = LogOnResult.Ok;

                                    if (result.Result == LogOnResult.Fail)
                                    {
                                        LogOnFailed(result.Id);
                                    }
                                    else
                                    {
                                        HttpContext.Current.Session["User"] = ApplicationUser.GetById(rdr.GetGuid(0));
                                        HttpContext.Current.Session["Actos"] = Acto.ByCentro(rdr.GetGuid(0));
                                        var productos = Producto.ByCentro(rdr.GetGuid(0));
                                        HttpContext.Current.Session["Productos"] = productos;
                                        var colectivos = Colectivo.All.ToList();
                                        colectivos = colectivos.Where(c => productos.Any(p => p.Id == c.ProductoId)).ToList();
                                        HttpContext.Current.Session["Colectivos"] = colectivos;
                                    }

                                    result.MultipleCompany = multiCompany;
                                    multiCompany = true;
                                }
                            }
                            else
                            {
                                result.Result = LogOnResult.NoUser;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        result.Result = LogOnResult.Fail;
                        result.Id = Guid.Empty;
                        result.UserName = ex.Message;
                    }
                    catch (FormatException ex)
                    {
                        result.Result = LogOnResult.Fail;
                        result.Id = Guid.Empty;
                        result.UserName = ex.Message;
                    }
                    catch (NullReferenceException ex)
                    {
                        result.Result = LogOnResult.Fail;
                        result.Id = Guid.Empty;
                        result.UserName = ex.Message;
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

            /* CREATE PROCEDURE AspadLand_Trace_Insert
             *   @CentroId uniqueidentifier,
             *   @Type int,
             *   @Busqueda nvarchar(50),
             *   @ColectivoId uniqueidentifier,
             *   @PresupuestoId uniqueidentifier *
            using (var cmdT = new SqlCommand("AspadLand_Trace_Insert"))
            {
                using (var cnnT = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmdT.Connection = cnnT;
                    cmdT.CommandType = CommandType.StoredProcedure;
                    cmdT.Parameters.Add(DataParameter.Input("@CentroId", result.Id));
                    cmdT.Parameters.Add(DataParameter.Input("@Type", result.Id == Guid.Empty ? 8 : 7));
                    cmdT.Parameters.Add(DataParameter.Input("@Busqueda", email));
                    cmdT.Parameters.Add(DataParameter.InputNull("@ColectivoId"));
                    cmdT.Parameters.Add(DataParameter.InputNull("@PresupuestoId"));
                    try
                    {
                        cmdT.Connection.Open();
                        cmdT.ExecuteNonQuery();
                    }
                    finally
                    {
                        if(cmdT.Connection.State != ConnectionState.Closed)
                        {
                            cmdT.Connection.Close();
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(clientAddress))
            {
                clientAddress = "no-ip";
            }*/

            res.SetSuccess(result);
            return res;
        }

        /// <summary>Insert into data base a trace of log on action</summary>
        /// <param name="userName">User name</param>
        /// <param name="ip">IP address from log on action</param>
        /// <param name="result">Result of action</param>
        /// <param name="userId">Identifier of users that attemps to log on</param>
        /// <param name="companyCode">Code of compnay to log in</param>
        /// <param name="companyId">Identifier of company to log in</param>
        private static void InsertLog(string userName, string ip, int result, Guid userId, string companyCode, int companyId)
        {
            /* CREATE PROCEDURE Log_Login
             * @UserName nvarchar(50),
             * @Date datetime,
             * @Ip nvarchar(50),
             * @Result int,
             * @UserId int,
             * @CompanyCode nvarchar(10),
             * @CompanyId int
             */
            using (SqlCommand cmd = new SqlCommand("Log_Login"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(DataParameter.Input("@UserName", userName));
                cmd.Parameters.Add(DataParameter.Input("@Ip", ip));
                cmd.Parameters.Add(DataParameter.Input("@Result", result));
                cmd.Parameters.Add(DataParameter.Input("@CompanyCode", companyCode));

                if (companyId != 0)
                {
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                }
                else
                {
                    companyId = Company.GetByCode(companyCode);
                    if (companyId == 0)
                    {
                        cmd.Parameters.Add(DataParameter.InputNull("@CompanyId"));
                    }
                    else
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    }
                }

                if (userId != Guid.Empty)
                {
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                }
                else
                {
                    cmd.Parameters.Add(DataParameter.InputNull("@UserId"));
                }

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
}