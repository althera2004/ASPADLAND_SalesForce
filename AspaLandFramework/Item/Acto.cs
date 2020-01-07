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

namespace AspadLandFramework.Item
{
    public class Acto : BaseItem
    {
        public Guid EspecialidadId { get; set; }
        public string EspecialidadName { get; set; }
        public Guid ActoCentroId { get; set; }
        public Guid EspecialidadCentroId { get; set; }
        public bool Ofertado { get; set; }

        public override string Json => throw new NotImplementedException();

        public override string JsonKeyValue => throw new NotImplementedException();

        public override string Link => throw new NotImplementedException();

        public static ReadOnlyCollection<Acto> ByCentro(string centroId)
        {
            var res = new List<Acto>();

            var query = string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
                    Id,
                    Name,
                    C_digos__c,
                    Familia__c
                FROM Acto__c
                WHERE
                    Id not IN (SELECT Acto__c FROM Actos_en_Centro__c WHERE Cuenta__r.Usuario_ASPADLand__c = '{0}')",
                centroId);
            var bindingResult = Tools.SalesForcceQuery(query);
            if (bindingResult != null)
            {
                foreach (var record in bindingResult.records)
                {
                    var acto = record as Acto__c;
                    res.Add(new Acto
                    {
                        Id = acto.Id,
                        Active = true,
                        Description = acto.Name,
                        EspecialidadName = acto.Familia__c,
                        Ofertado = true
                    });
                }
            }

            return new ReadOnlyCollection<Acto>(res);
        }

        public static ReadOnlyCollection<Acto> All
        {
            get
            {
                var res = new List<Acto>();

                var query = "SELECT Name, C_digos__c, Familia__c FROM Acto__c";
                var bindingResult = Tools.SalesForcceQuery(query);
                if (bindingResult != null)
                {
                    foreach (var record in bindingResult.records)
                    {
                        var acto = record as Acto__c;
                        res.Add(new Acto
                        {
                            Id = acto.Id,
                            Active = true,
                            Description = acto.Name,
                            EspecialidadName = acto.Familia__c,
                            Ofertado = false
                        });
                    }
                }

                return new ReadOnlyCollection<Acto>(res);
            }
        }
    }
}
