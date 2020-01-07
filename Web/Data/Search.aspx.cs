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
using SbrinnaCoreFramework.DAL;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using AspadLandFramework;

public partial class DataSearch : Page
{    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Response.Clear();
        var data = new StringBuilder("[");
        using(var cmd = new SqlCommand("ASPADLand_Admin_BusquedasPivot"))
        {
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    bool first = true;
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                data.Append(",");
                            }

                            data.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"{{""Colectivo"":""{0}"",""Centro"":""{1}"",""Busqueda"":""{2}""}}",
                                SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(0).Trim()),
                                SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(1).Trim()),
                                SbrinnaCoreFramework.Tools.JsonCompliant(rdr.GetString(2).Trim()));
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

        data.Append("]");

        this.Response.Write(data.ToString());
        this.Response.End();
    }
}