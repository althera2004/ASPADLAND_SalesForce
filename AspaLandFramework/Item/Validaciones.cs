// --------------------------------
// <copyright file="Validaciones.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace ShortcutFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using ShortcutFramework.Item.Binding;

    public class Validaciones
    {
        public Guid ColavalidacionId { get; set; }
        public Guid CentroId { get; set; }
        public string NumberOfEmployees { get; set; }
        public string CentroName { get; set; }
        public bool Urgente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public string Poliza { get; set; }
        public string Codigo { get; set; }
        public int Status { get; set; }
        public Guid? AseguradoId { get; set; }
        public string AseguradoIdName { get; set; }
        public Guid ColectivoId { get; set; }
        public string ColectivoIdName { get; set; }

        public string Json
        {
            get
            {
                var asegurado = "null";
                var ffin = "null";
                if (this.AseguradoId.HasValue)
                {
                    asegurado = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{{""Id"":""{0}"",""Name"":""{1}""}}",
                        this.AseguradoId,
                        SbrinnaCoreFramework.Tools.JsonCompliant(this.AseguradoIdName));
                }

                if (this.FechaFin.HasValue)
                {
                    ffin = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.FechaFin);
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""ColaValidacionId"":""{0}"",
                        ""Codigo"":""{1}"",
                        ""Urgente"":{2},
                        ""Colectivo"":{{""Id"":""{3}"",""Name"":""{4}""}},
                        ""FechaInicio"":""{5:dd/MM/yyyy}"",
                        ""FechaFin"":{6},
                        ""Asegurado"":{7},
                        ""Nombre"":""{8}"";
                        ""Dni"":""{9}"",
                        ""Poliza"":""{10}"",
                        ""Centro"":{{""Id"":""{11}"",""Name"":""{12}""}}}}",
                    this.ColavalidacionId,
                    this.Codigo,
                    this.Urgente ? "true" : "else",
                    this.ColectivoId,
                    SbrinnaCoreFramework.Tools.JsonCompliant(this.ColectivoIdName),
                    this.FechaInicio,
                    ffin,
                    asegurado,
                    SbrinnaCoreFramework.Tools.JsonCompliant(this.Nombre),
                    SbrinnaCoreFramework.Tools.JsonCompliant(this.Dni),
                    SbrinnaCoreFramework.Tools.JsonCompliant(this.Poliza),
                    this.CentroId,
                    SbrinnaCoreFramework.Tools.JsonCompliant(this.CentroName));
            }
        }

        public static string JsonList(ReadOnlyCollection<Validaciones> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var v in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(v.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<Validaciones> All
        {
            get
            {
                var res = new List<Validaciones>();
                using (var cmd = new SqlCommand("ASPADLand_Admin_Validaciones"))
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
                                while (rdr.Read())
                                {
                                    var validacion = new Validaciones
                                    {
                                        ColavalidacionId = rdr.GetGuid(ColumnsValidacionesGet.ColaValidacionId),
                                        Codigo = rdr.GetString(ColumnsValidacionesGet.Codigo),
                                        ColectivoId = rdr.GetGuid(ColumnsValidacionesGet.ColectivoId),
                                        ColectivoIdName = rdr.GetString(ColumnsValidacionesGet.ColectivoIdName),
                                        Urgente = rdr.GetBoolean(ColumnsValidacionesGet.Urgente),
                                        CentroId = rdr.GetGuid(ColumnsValidacionesGet.CentroId),
                                        CentroName = rdr.GetString(ColumnsValidacionesGet.CentroName),
                                        Dni = rdr.GetString(ColumnsValidacionesGet.Dni),
                                        Nombre = rdr.GetString(ColumnsValidacionesGet.Nombre),
                                        NumberOfEmployees = rdr.GetInt32(ColumnsValidacionesGet.NumberOfEmployees).ToString(),
                                        Status = rdr.GetInt32(ColumnsValidacionesGet.StatusCode)
                                    };

                                    if (!rdr.IsDBNull(ColumnsValidacionesGet.Poliza))
                                    {
                                        validacion.Poliza = rdr.GetString(ColumnsValidacionesGet.Poliza);
                                    }

                                    if (!rdr.IsDBNull(ColumnsValidacionesGet.Fechainicio))
                                    {
                                        validacion.FechaInicio = rdr.GetDateTime(ColumnsValidacionesGet.Fechainicio);
                                    }

                                    if (!rdr.IsDBNull(ColumnsValidacionesGet.FechaFin))
                                    {
                                        validacion.FechaFin = rdr.GetDateTime(ColumnsValidacionesGet.FechaFin);
                                    }

                                    if (!rdr.IsDBNull(ColumnsValidacionesGet.AseguradoId))
                                    {
                                        validacion.AseguradoId = rdr.GetGuid(ColumnsValidacionesGet.AseguradoId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsValidacionesGet.AseguradoIdName))
                                    {
                                        validacion.AseguradoIdName = rdr.GetString(ColumnsValidacionesGet.AseguradoIdName);
                                    }

                                    res.Add(validacion);
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

                return new ReadOnlyCollection<Validaciones>(res);
            }
        }
    }
}