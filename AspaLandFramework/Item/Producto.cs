using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AspadLandFramework.Item
{
    public class Producto : BaseItem
    {
        public Guid TarifaId { get; set; }
        public string TarifaName { get; set; }

        public override string Json => throw new NotImplementedException();

        public override string JsonKeyValue => throw new NotImplementedException();

        public override string Link => throw new NotImplementedException();

        public static ReadOnlyCollection<Producto> ByCentro(Guid centroId)
        {
            var res = new List<Producto>();
            /*string query = string.Format(
                CultureInfo.InvariantCulture,
                @"select 
	                    T.qes_ProductoId,
	                    T.qes_ProductoIdName,
	                    T.qes_TarifaId,
	                    T.qes_TarifaIdName
                    from qes_tarifascentro T WITH(NOLOCK)
                    WHERE 
	                    T.qes_ClienteId = '{0}'
                    AND	T.statecode = 0
					AND T.statuscode = 1",
                centroId);

            using(var cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            while(rdr.Read())
                            {
                                res.Add(new Producto
                                {
                                    Id = rdr.GetGuid(0),
                                    Description = rdr.GetString(1),
                                    TarifaId = rdr.GetGuid(2),
                                    TarifaName = rdr.GetString(3)
                                });
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
            }*/

            return new ReadOnlyCollection<Producto>(res);
        }
    }
}
