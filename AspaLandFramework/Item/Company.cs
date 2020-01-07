// --------------------------------
// <copyright file="Company.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace AspadLandFramework.Item
{
    using SbrinnaCoreFramework.Activity;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Implements Company class
    /// </summary>
    public class Company
    {
        /// <summary> Company default language </summary>
        private string language;

        /// <summary> Company nif </summary>
        private string fiscalNumber;

        /// <summary>Compnay's default address</summary>
        private CompanyAddress defaultAddress;

        /// <summary>Initializes a new instance of the Company class</summary>
        public Company()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Company class.
        /// Company data is searched on database based in company identifier
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        public Company(int companyId)
        {
            this.Id = -1;
            this.MailContact = string.Empty;
            this.Web = string.Empty;
            this.SubscriptionEnd = DateTime.Now;
            this.SubscriptionStart = DateTime.Now;
            this.Name = string.Empty;
            this.language = "es";

            using (var cmd = new SqlCommand("Company_GetById"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                cmd.Parameters["@CompanyId"].Value = companyId;

                try
                {
                    cmd.Connection.Open();
                    var rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        this.Id = rdr.GetInt32(0);
                        this.Name = rdr[1].ToString();
                        this.MailContact = string.Empty;
                        this.Web = string.Empty;
                        this.SubscriptionStart = rdr.GetDateTime(2);
                        this.SubscriptionEnd = rdr.GetDateTime(3);
                        this.language = Convert.ToString(rdr[4], CultureInfo.GetCultureInfo("en-us"));
                        this.fiscalNumber = rdr[5].ToString();
                        this.Code = rdr[6].ToString();
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.InstalledUICulture, "cto::Company({0})", companyId));
                    this.Id = -1;
                    this.MailContact = string.Empty;
                    this.Web = string.Empty;
                    this.SubscriptionEnd = DateTime.Now;
                    this.SubscriptionStart = DateTime.Now;
                    this.Name = string.Empty;
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.InstalledUICulture, "cto::Company({0})", companyId));
                    this.Id = -1;
                    this.MailContact = string.Empty;
                    this.Web = string.Empty;
                    this.SubscriptionEnd = DateTime.Now;
                    this.SubscriptionStart = DateTime.Now;
                    this.Name = string.Empty;
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.InstalledUICulture, "cto::Company({0})", companyId));
                    this.Id = -1;
                    this.MailContact = string.Empty;
                    this.Web = string.Empty;
                    this.SubscriptionEnd = DateTime.Now;
                    this.SubscriptionStart = DateTime.Now;
                    this.Name = string.Empty;
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "cto::Company({0})", companyId));
                    this.Id = -1;
                    this.MailContact = string.Empty;
                    this.Web = string.Empty;
                    this.SubscriptionEnd = DateTime.Now;
                    this.SubscriptionStart = DateTime.Now;
                    this.Name = string.Empty;
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "cto::Company({0})", companyId));
                    this.Id = -1;
                    this.MailContact = string.Empty;
                    this.Web = string.Empty;
                    this.SubscriptionEnd = DateTime.Now;
                    this.SubscriptionStart = DateTime.Now;
                    this.Name = string.Empty;
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
        
        /// <summary>Gets an empty company</summary>
        public static Company Empty
        {
            get
            {
                return new Company
                {
                    Id = -1,
                    Code = string.Empty,
                    defaultAddress = CompanyAddress.Empty,
                    language = string.Empty,
                    MailContact = string.Empty,
                    Name = string.Empty,
                    fiscalNumber = string.Empty,
                    Web = string.Empty
                };
            }
        }

        /// <summary>Gets an empty company</summary>
        public static Company EmptySimple
        {
            get
            {
                return new Company
                {
                    Id = -1,
                    Code = string.Empty,
                    language = string.Empty,
                    MailContact = string.Empty,
                    Name = string.Empty,
                    fiscalNumber = string.Empty,
                    Web = string.Empty
                };
            }
        }

        #region Properties
        /// <summary>
        /// Gets a JSON key/value stucture of the company
        /// </summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0},""Value"":""{1}""}}", this.Id, SbrinnaCoreFramework.Tools.JsonCompliant(this.Name));
            }
        }

        /// <summary>
        /// Gets or sets the company's logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Gets or sets the company identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code of company
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name of company
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the date of starting subscription
        /// </summary>
        public DateTime SubscriptionStart { get; set; }

        /// <summary>
        /// Gets or sets the date of finishing subscription
        /// </summary>
        public DateTime SubscriptionEnd { get; set; }

        /// <summary>
        /// Gets or sets de email contacto of company
        /// </summary>
        public string MailContact { get; set; }

        /// <summary>
        /// Gets or sets de web address of company
        /// </summary>
        public string Web { get; set; }

        /// <summary>
        /// Gets or sets the default language of company
        /// </summary>
        public string Language
        {
            get
            {
                return this.language;
            }

            set
            {
                this.language = value;
            }
        }

        /// <summary>
        /// Gets or sets the NIF of company
        /// </summary>
        public string FiscalNumber
        {
            get
            {
                return this.fiscalNumber;
            }

            set
            {
                this.fiscalNumber = value;
            }
        }        

        /// <summary>
        /// Gets or sets the default address of company
        /// </summary>
        public CompanyAddress DefaultAddress
        {
            get
            {
                return this.defaultAddress;
            }

            set
            {
                this.defaultAddress = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets a descriptive text with the differences between two companies
        /// </summary>
        /// <param name="other">Second company to capmpare</param>
        /// <returns>The description of differences between two companies</returns>
        public  string Differences(Company other)
        {
            if (this == null || other == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            bool first = true;

            if (this.Name != other.Name)
            {
                res.Append("Name:").Append(other.Name);
                first = false;
            }

            if (this.fiscalNumber != other.fiscalNumber)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Nif:").Append(other.fiscalNumber);
                first = false;
            }

            if (this.language != other.language)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Language:").Append(other.language);
                first = false;
            }

            if (this.defaultAddress.Id != other.defaultAddress.Id)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("defaultAddress:").Append(other.defaultAddress.Address).Append(",").Append(other.defaultAddress.City);
            }

            return res.ToString();
        }

        /// <summary>Set the default address of a comapny</summary>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="addressId">Address identifier</param>
        /// <param name="userId">Identifier of user that peforms the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult SetDefaultAddress(int companyId, int addressId, int userId)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Company_SetDefaultAddress]
             * @CompanyId int,
             * @AddressId int,
             * @UserId int */
            using (var cmd = new SqlCommand("Company_SetDefaultAddress"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@AddressId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@AddressId"].Value = addressId;
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, "Company::SetDefaultAddress", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0},AddressId{1},UserId{2}", companyId, addressId, userId));
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, "Company::SetDefaultAddress", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0},AddressId{1},UserId{2}", companyId, addressId, userId));
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, "Company::SetDefaultAddress", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0},AddressId{1},UserId{2}", companyId, addressId, userId));
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

        /// <summary>Generates a JSON strcuture of compnay</summary>
        /// <param name="company">Compnay to extract data</param>
        /// <returns>JSON structure</returns>
        public string Json
        {
            get
            {
                if (this == null)
                {
                    return "{}";
                }

                var res = new StringBuilder("{").Append(Environment.NewLine);
                res.Append("\t\t\"Id\":").Append(this.Id).Append(",").Append(Environment.NewLine);
                res.Append("\t\t\"Name\":\"").Append(this.Name).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\"Nif\":\"").Append(this.fiscalNumber).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\"MailContact\":\"").Append(this.MailContact).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\"Web\":\"").Append(this.Web).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\"SubscriptionStart\":\"").Append(this.SubscriptionStart.ToShortDateString()).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\"SubscriptionEnd\":\"").Append(this.SubscriptionEnd.ToShortDateString()).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\"Language\":\"").Append(this.language).Append("\",").Append(Environment.NewLine);

                if (this.defaultAddress != null)
                {
                    res.Append(",").Append(Environment.NewLine).Append("\t\t\"DefaultAddress\":").Append(this.defaultAddress.Json);
                }

                return res.ToString();
            }
        }

        /// <summary>Get a company from data base by code</summary>
        /// <param name="code">Company's code</param>
        /// <returns>Company object</returns>
        public static int GetByCode(string code)
        {
            int res = 0;
            using (var cmd = new SqlCommand("Company_GetByCode"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.Text);
                cmd.Parameters["@CompanyCode"].Value = code;
                try
                {
                    cmd.Connection.Open();
                    var rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res = rdr.GetInt32(0);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, "Company::GetByCode", code);
                    res = 0;
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Company::GetByCode", code);
                    res = 0;
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, "Company::GetByCode", code);
                    res = 0;
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

        /// <summary>
        /// Gets log of company
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Filename of company's logo</returns>
        public static string GetLogoFileName(int companyId)
        {
            string res = "NoImage.jpg";
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\"))
            {
                path += string.Format(CultureInfo.InvariantCulture, @"\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}\images\Logos\", path);
            string pattern = string.Format(CultureInfo.InvariantCulture, "{0}.*", companyId);
            var last = new DateTime(1900, 1, 1);
            var files = Directory.GetFiles(path, pattern);
            foreach (string file in files)
            {
                var info = new FileInfo(file);
                var created = info.LastWriteTime;
                if (created > last)
                {
                    last = created;
                    res = file;
                }
            }

            res = Path.GetFileName(res);
            return res;
        }

        /// <summary>
        /// Update company in data base
        /// </summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            var res = ActionResult.NoAction;
            /* 
             * CREATE PROCEDURE Company_Update
             * @CompanyId int,
             * @Name nvarchar(50),
             * @Nif nvarchar(15),
             * @DefaultAddress int,
             * @Language nvarchar(2),
             * @UserId int
             */
            using (var cmd = new SqlCommand("Company_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@Name", SqlDbType.Text);
                    cmd.Parameters.Add("@Nif", SqlDbType.Text);
                    cmd.Parameters.Add("@DefaultAddress", SqlDbType.Int);
                    cmd.Parameters.Add("@Language", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@CompanyId"].Value = this.Id;
                    cmd.Parameters["@Name"].Value = this.Name;
                    cmd.Parameters["@Nif"].Value = this.fiscalNumber;
                    cmd.Parameters["@DefaultAddress"].Value = this.defaultAddress.Id;
                    cmd.Parameters["@Language"].Value = this.language;
                    cmd.Parameters["@UserId"].Value = userId;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.Success = true;
                    res.MessageError = string.Empty;
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex);
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex);
                }
                catch (NullReferenceException ex)
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