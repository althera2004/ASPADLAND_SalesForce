using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AspadLandFramework.Item
{
    public class DocumentosCentro : BaseItem
    {
        public override string Json => throw new NotImplementedException();

        public override string JsonKeyValue => throw new NotImplementedException();

        public override string Link => throw new NotImplementedException();

        public static string JsonList(ReadOnlyCollection<DocumentosCentro> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var documento in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(documento.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<DocumentosCentro> GetAll (Guid centroId)
        {
            var res = new List<DocumentosCentro>();

            return new ReadOnlyCollection<DocumentosCentro>(res);
        }
    }
}
