
using SbrinnaCoreFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ShortcutFramework.Item
{
    public class Traza
    {
        public long Id { get; set; }
        public Guid CentroId { get; set; }
        public string CentroName { get; set; }
        public DateTime? Fecha { get; set; }
        public int Tipo { get; set; }
        public string Busqueda { get; set; }
        public Guid PresupuestoId { get; set; }
        public Guid MascotaId{get;set;}
        public string PresupuestoCode { get; set; }
        public string asegurado { get; set; }
        public string colectivo { get; set; }
        public string poliza { get; set; }
        public string mascotaName { get; set; }
        public string chip { get; set; }
        public string sexo { get; set; }
        public string tipo { get; set; }
        public string DNI { get; set; }

        public static Traza Empty
        {
            get
            {
                return new Traza
                {
                    Id = -1,
                    CentroId = Guid.Empty,
                    CentroName = string.Empty,
                    Fecha = null,
                    Tipo = -1,
                    Busqueda = string.Empty,
                    PresupuestoId = Guid.Empty,
                    MascotaId=Guid.Empty,
                    PresupuestoCode = string.Empty
                };
            }
        }

        public static string JsonList (ReadOnlyCollection<Traza> l)
        {
            var list = l.OrderBy(i2=>i2.CentroName).OrderByDescending(i => i.Fecha).ToList();
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var traza in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(traza.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public string Json
        {
            get
            {
                var centro = "null";
                if(this.CentroId != Guid.Empty)
                {
                    centro = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{{""I"":""{0}"",""N"":""{1}""}}",
                        this.CentroId,
                        Tools.JsonCompliant(this.CentroName));
                }

                var presupuesto = "null";
                if (this.PresupuestoId != Guid.Empty)
                {
                    presupuesto = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{{""I"":""{0}"",""C"":""{1}""}}",
                        this.PresupuestoId,
                        Tools.JsonCompliant(this.PresupuestoCode));
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""I"":{0},""T"":{1},""C"":{2},""D"":""{3:dd/MM/yyyy HH:mm}"",""B"":""{4}"",""P"":{5},""M"":""{6}"",""S"":""{7}"",""Ty"":""{8}"",""A"":""{9}"",""ch"":""{10}"",""co"":""{11}"",""PL"":""{12}""}}{13}",
                    this.Id,
                    this.Tipo,
                    centro,
                    this.Fecha,
                    Tools.JsonCompliant(this.Busqueda),
                    presupuesto,
                    Tools.JsonCompliant(mascotaName),
                    sexo,
                    tipo,
                    Tools.JsonCompliant( asegurado),
                    chip,
                    Tools.JsonCompliant(colectivo),
                    poliza,
                    Environment.NewLine);
            }
        }

        public static ReadOnlyCollection<Traza> All
        {
            get
            {
                var res = new List<Traza>();
                using(var cmd = new SqlCommand("ASPADLand_Admin_Traces_All"))
                {
                    using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newTraza = new Traza
                                    {
                                        Id = rdr.GetInt64(0),
                                        Tipo = rdr.GetInt32(1),
                                        Fecha = rdr.GetDateTime(4),
                                        CentroName = rdr.GetString(3),
                                        Busqueda = rdr.GetString(5),
                                        PresupuestoCode = rdr.GetString(7),
                                        CentroId = Guid.Empty,
                                        PresupuestoId = Guid.Empty,
                                        MascotaId = Guid.Empty,
                                        mascotaName = rdr.GetString(9),
                                        chip = rdr.GetString(10),
                                        sexo = rdr[11].ToString(),
                                        tipo = rdr[12].ToString(),
                                        poliza = rdr.GetString(13),
                                        asegurado = rdr.GetString(14),
                                        colectivo = rdr.GetString(15),
                                        DNI = rdr.GetString(16)
                                    };

                                    if (!rdr.IsDBNull(2))
                                    {
                                        newTraza.CentroId = rdr.GetGuid(2);
                                    }

                                    if (!rdr.IsDBNull(6))
                                    {
                                        newTraza.PresupuestoId = rdr.GetGuid(6);
                                    }

                                    if (!rdr.IsDBNull(8))
                                    {
                                        newTraza.PresupuestoId = rdr.GetGuid(8);
                                    }

                                    res.Add(newTraza);
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

                return new ReadOnlyCollection<Traza>(res);
            }
        }

        public static ReadOnlyCollection<Traza> AllPresupuestos
        {
            get
            {
                var res = new List<Traza>();
                using (var cmd = new SqlCommand("ASPADLand_Admin_TracesPresupuestos_All"))
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                var lastMascota = Guid.Empty;
                                var lastCentro = Guid.Empty;
                                while (rdr.Read())
                                {
                                    var mascota = rdr.GetGuid(8);
                                    var centro = rdr.GetGuid(2);

                                    if(lastMascota == mascota && lastCentro == centro)
                                    {
                                        continue;
                                    }

                                    var newTraza = new Traza
                                    {
                                        Id = 0,
                                        Tipo = rdr.GetInt32(1),
                                        Fecha = rdr.GetDateTime(4),
                                        CentroName = rdr.GetString(3),
                                        Busqueda = rdr.GetString(5),
                                        PresupuestoCode = rdr.GetString(7),
                                        CentroId = Guid.Empty,
                                        PresupuestoId = Guid.Empty,
                                        MascotaId = Guid.Empty,
                                        mascotaName = rdr.GetString(9),
                                        chip = rdr.GetString(10),
                                        sexo = rdr[11].ToString(),
                                        tipo = rdr[12].ToString(),
                                        poliza = rdr.GetString(13),
                                        asegurado = rdr.GetString(14),
                                        colectivo = rdr.GetString(15),
                                        DNI = rdr.GetString(16)
                                    };

                                    if (!rdr.IsDBNull(2))
                                    {
                                        newTraza.CentroId = rdr.GetGuid(2);
                                    }

                                    if (!rdr.IsDBNull(6))
                                    {
                                        newTraza.PresupuestoId = rdr.GetGuid(6);
                                    }

                                    if (!rdr.IsDBNull(8))
                                    {
                                        newTraza.MascotaId = rdr.GetGuid(8);
                                    }

                                    lastMascota = rdr.GetGuid(8);
                                    lastCentro = rdr.GetGuid(2);

                                    res.Add(newTraza);
                                }
                            }
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

                return new ReadOnlyCollection<Traza>(res);
            }
        }
    }
}
