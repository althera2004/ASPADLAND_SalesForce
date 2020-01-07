using SbrinnaCoreFramework.Activity;
using SbrinnaCoreFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ShortcutFramework.Item
{
    public class MailTrace
    {
        public long Id { get; set; }
        public Guid CentroId { get; set; }
        public string Sender { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SendDate { get; set; }

        public static MailTrace Empty
        {
            get
            {
                return new MailTrace
                {
                    Id = -1,
                    CentroId = Guid.Empty,
                    Sender = string.Empty,
                    To = string.Empty,
                    Subject = string.Empty,
                    Body = string.Empty,
                    SendDate = DateTime.Now
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":0, ""CentroId"":""{1}"", ""Sender"":""{2}"", ""To"":""{3}"", ""Subject"":""{4}"",""Body"":""{5}"",""SendDate"":""{6:dd/MM/yyyy}""}",
                    this.Id,
                    this.CentroId,
                    this.Sender,
                    this.To,
                    this.Subject,
                    this.Body,
                    this.SendDate);
            }
        }

        public static ReadOnlyCollection<MailTrace> ByCentroId(Guid centroId)
        {
            var res = new List<MailTrace>();
            /* CREATE PROCEDURE ASPADLAND_MailTrace_GetByCentroId
             *   @CentroId uniqueidentifier */
            using(var cmd = new SqlCommand("ASPADLAND_MailTrace_GetByCentroId"))
            {
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@CentroId", centroId));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            while(rdr.Read())
                            {
                                var newMailTrace = new MailTrace
                                {
                                    Id = rdr.GetInt64(0),
                                    CentroId = rdr.GetGuid(1),
                                    Sender = rdr.GetString(2),
                                    To = rdr.GetString(3),
                                    Subject = rdr.GetString(4),
                                    Body = rdr.GetString(5),
                                    SendDate = rdr.GetDateTime(6)
                                };

                                res.Add(newMailTrace);
                            }
                        }
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

            return new ReadOnlyCollection<MailTrace>(res);
        }

        public ActionResult Insert()
        {
            /* CREATE PROCEDURE [dbo].[ASPADLAND_MailTrace_Insert]
             *   @Id bigint output,
             *   @CentroId uniqueidentifier,
             *   @Sender nvarchar(100),
             *   @To nvarchar(100),
             *   @Subject nvarchar(150),
             *   @Body text	*/
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("ASPADLAND_MailTrace_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CentroId", this.CentroId));
                    cmd.Parameters.Add(DataParameter.Input("@Sender", this.Sender, 100));
                    cmd.Parameters.Add(DataParameter.Input("@To", this.To, 100));
                    cmd.Parameters.Add(DataParameter.Input("@Subject", this.Subject, 150));
                    cmd.Parameters.Add(DataParameter.Input("@Body", this.Body));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                        res.SetSuccess(this.Id);
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
    }
}
