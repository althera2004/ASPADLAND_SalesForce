namespace AspadLandFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Colectivo : BaseItem
    {
        public Guid ProductoId { get; set; }
        public string ProductoName { get; set; }
        public bool Owner { get; set; }

        public override string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":""{0}"",""Description"":""{1}""}}",
                    this.Id,
                    SbrinnaCoreFramework.Tools.JsonCompliant(this.Description));
            }
        }

        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":""{0}"",""Description"":""{1}""}}",
                    this.Id,
                    SbrinnaCoreFramework.Tools.JsonCompliant(this.Description));
            }
        }

        public override string Link => throw new NotImplementedException();

        public static string JsonList(ReadOnlyCollection<Colectivo> list)
        {
            var res = new StringBuilder("[");
            var first = true;
            foreach(var colectivo in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(colectivo.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<Colectivo> AllASPAD
        {
            get
            {
                var res = new List<Colectivo>();
                var query = "SELECT Id,nombre_Compa_ia__c FROM Producto_ASPAD__c";
                var colectivos = Tools.SalesForcceQuery(query);
                foreach (var result in colectivos.records)
                {
                    var record = result as Producto_ASPAD__c;
                    var colectivo = new Colectivo
                    {
                         Id = record.Id,
                         Description = record.Nombre_Compa_ia__c
                    };

                    if (!res.Any(c => c.Description.Equals(colectivo.Description, StringComparison.OrdinalIgnoreCase)))
                    {
                        res.Add(colectivo);
                    }
                }

                return new ReadOnlyCollection<Colectivo>(res);
            }
        }

        public static ReadOnlyCollection<Colectivo> All
        {
            get
            {
                return AllASPAD;
                // weke
                var res = new List<Colectivo>();
                var user = HttpContext.Current.Session["User"] as ApplicationUser;
                var query = string.Format(
                    CultureInfo.InvariantCulture,
                    @"SELECT 
                        Producto_ASPAD__c
                        Centro_c
                    FROM Relaci_n_Productos_Centros__c
                    WHERE
                        Id IN (SELECT Acto__c FROM Actos_en_Centro__c WHERE Cuenta__r.Usuario_ASPADLand__c = '{0}')",
                    user.Id);
                var colectivos = Tools.SalesForcceQuery(query);
                foreach (var result in colectivos.records)
                {
                    var record = result as Relaci_n_Productos_Centros__c;
                    var colectivo = new Colectivo
                    {
                        ProductoName = record.Centro__c
                    };

                    if (!res.Any(c => c.ProductoName.Equals(colectivo.ProductoName, StringComparison.OrdinalIgnoreCase)))
                    {
                        res.Add(colectivo);
                    }
                }
                return AllASPAD;
            }
        }
    }
}