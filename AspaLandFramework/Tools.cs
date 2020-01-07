// --------------------------------
// <copyright file="Tools.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Globalization;
using System.Web;

namespace AspadLandFramework
{
    /// <summary>Implements Tools class.</summary>
    public static class Tools
    {
        public const int TracePresupuestoInsert = 1;
        public const int TraceSugerenciaInsert = 2;
        public const int TraceChangePassword = 3;
        public const int TracePresupuestoDiscard = 4;
        public const int TracePresupuestoRealizado = 5;
        public const int TraceResetPassword = 6;
        public const int TraceSearchAsegurado = 7;

        public static QueryResult SalesForcceQuery(string query)
        {
            var binding = HttpContext.Current.Session["SForceConnection"] as SforceService;
            return binding.query(query);
        }
    }
}