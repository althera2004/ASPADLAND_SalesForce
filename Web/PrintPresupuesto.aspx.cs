using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SbrinnaCoreFramework.DataAccess;

public partial class PrintPresupuesto : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var presupuestoId = Request.QueryString["presupuestoId"];
        var centro = HttpContext.Current.Session["user"] as ApplicationUser;
        var polizaId = Request.QueryString["polizaId"];
        var poliza = Poliza.ById(polizaId);
        var dictionary = ApplicationDictionary.Load("es");

        var path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        var fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}.pdf",
            dictionary["Item_Prespuesto"],
            Guid.NewGuid());

        // FONTS
        var pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
        }

        string type = string.Empty;
        string origin = string.Empty;
        string originSufix = string.Empty;
        string reporterType = string.Empty;
        string reporter = string.Empty;        

        string status = string.Empty;
        var document = new iTextSharp.text.Document(PageSize.A4, 30, 30, 65, 55);
        var writer = PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\Temp\\" + fileName, FileMode.Create));
        
        document.Open();

        var borderNone = Rectangle.NO_BORDER;
        var borderSides = Rectangle.RIGHT_BORDER + Rectangle.LEFT_BORDER;
        var borderAll = Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;
        var borderTBL = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderTBR = Rectangle.TOP_BORDER + Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;
        var borderBL = Rectangle.BOTTOM_BORDER + Rectangle.LEFT_BORDER;
        var borderBR = Rectangle.BOTTOM_BORDER + Rectangle.RIGHT_BORDER;

        document.Add(new Paragraph(centro.Nombre, FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD)));
        document.Add(new Paragraph("N.I.F: "+ centro.NIF, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
        document.Add(new Paragraph(centro.Nombre, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
        document.Add(new Paragraph(centro.CP + ", " + centro.Poblacion, FontFactory.GetFont(FontFactory.HELVETICA, 8)));
        document.Add(new Paragraph("Telf.: " + centro.Telefono1, FontFactory.GetFont(FontFactory.HELVETICA, 8)));

        var alignLeft = Element.ALIGN_LEFT;
        var alignRight = Element.ALIGN_RIGHT;

        var table = new PdfPTable(2)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 0
        };

        table.SetWidths(new float[] { 30f, 30f });
        var labelFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL);
        var totalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL);
        var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD);

        var presupuestoCodigo = string.Empty;
        var presupuestoFecha = string.Empty;

        var aseguradoNombre = poliza.AseguradoNombre;
        var aseguradoDireccion = string.Empty;
        var aseguradoCP = string.Empty;
        var aseguradoPoblacion = string.Empty;
        var aseguradoProvincia = string.Empty;
        var aseguradoNIF = poliza.AseguradoNIF;
        var observaciones = string.Empty;

        // Linea Total
        string impuestoText = dictionary["PDF_IVA"]+ " " + ConfigurationManager.AppSettings["IVA"].ToString() + "%";
        decimal impuesto = Convert.ToDecimal(ConfigurationManager.AppSettings["IVA"].ToString());

        if (centro.Provincia.IndexOf("tenerife", StringComparison.OrdinalIgnoreCase) != -1 || centro.Provincia.IndexOf("canaria", StringComparison.OrdinalIgnoreCase) != -1|| centro.Provincia.IndexOf("las palmas", StringComparison.OrdinalIgnoreCase) != -1)
        {
            impuestoText = dictionary["PDF_IGIC"] + " " + ConfigurationManager.AppSettings["IGIC"].ToString() + "%";
            impuesto = Convert.ToDecimal(ConfigurationManager.AppSettings["IGIC"].ToString());
        }

        if (centro.Provincia.IndexOf("melilla", StringComparison.OrdinalIgnoreCase) != -1)
        {
            impuestoText = dictionary["PDF_Melilla"] + " " + ConfigurationManager.AppSettings["Melilla"].ToString() + "%";
            impuesto = Convert.ToDecimal(ConfigurationManager.AppSettings["Melilla"].ToString());
        }

        if (centro.Provincia.IndexOf("ceuta", StringComparison.OrdinalIgnoreCase) != -1)
        {
            impuestoText = dictionary["PDF_Ceuta"] + " " + ConfigurationManager.AppSettings["Ceuta"].ToString() + "%";
            impuesto = Convert.ToDecimal(ConfigurationManager.AppSettings["Ceuta"].ToString());
        }

        var tableObservaciones = new PdfPTable(2)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 30f
        };

        var tableCostsAspad = new PdfPTable(6)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 20f,
            SpacingAfter = 10f
        };

        var tableCostsPropios = new PdfPTable(6)
        {
            WidthPercentage = 100,
            HorizontalAlignment = 1,
            SpacingBefore = 10f,
            SpacingAfter = 20f
        };

        tableObservaciones.SetWidths(new float[] { 1f, 3f });
        tableCostsAspad.SetWidths(new float[] { 25f, 23f, 10f, 10f, 10f ,15f });
        tableCostsPropios.SetWidths(new float[] { 25f, 23f, 10f, 10f, 10f, 15f });

        tableCostsAspad.AddCell(ToolsPdf.HeaderCell(dictionary["PDF_Especialidad"]));
        tableCostsAspad.AddCell(ToolsPdf.HeaderCell(dictionary["PDF_Concepto"]));
        tableCostsAspad.AddCell(ToolsPdf.HeaderCell(dictionary["PDF_Cantidad"]));
        tableCostsAspad.AddCell(ToolsPdf.HeaderCell(dictionary["PDF_Base"]));
        tableCostsAspad.AddCell(ToolsPdf.HeaderCell(dictionary["PDF_Dto"]));
        tableCostsAspad.AddCell(ToolsPdf.HeaderCell(dictionary["PDF_Importe"]));

        /* CREATE PROCEDURE ASPADLAND_GetPresupuestoPendienteByPresupuestoId
         *   @PresupuestoId uniqueidentifier,
         *   @CentroId nvarchar(50) */
        bool propios = false;
        bool hasObservaciones = false;
        decimal totalBaseCost = 0;
        decimal totalBaseCostASPAD = 0;
        using (var cmd = new SqlCommand("ASPADLAND_Salesforce_GetPresupuestoPendienteByPresupuestoId"))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.Parameters.Add(DataParameter.Input("@PresupuestoId", presupuestoId));
                cmd.Parameters.Add(DataParameter.Input("@CentroId", centro.Id));
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            observaciones = rdr.GetString(9);
                            presupuestoCodigo = rdr.GetString(1);
                            presupuestoFecha = string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", rdr.GetDateTime(7));

                            string amountText = string.Empty;
                            string totalRowText = string.Empty;
                            string actoObservaciones = rdr.GetString(10);
                            var especialidad = rdr.GetString(4);

                            decimal finalAmount = 0M;
                            if (!rdr.IsDBNull(5))
                            {
                                finalAmount = rdr.GetDecimal(5);

                                if (especialidad.Equals("propio", StringComparison.OrdinalIgnoreCase))
                                {
                                    finalAmount = finalAmount / (1 + impuesto / 100);
                                }

                                amountText = string.Format(
                                CultureInfo.InvariantCulture,
                                @"{0:#0.00} €",
                                finalAmount).Replace(",", ".");

                                totalRowText = string.Format(
                                    CultureInfo.InvariantCulture,
                                    @"{0:#0.00} €",
                                    finalAmount).Replace(",", ".");
                            }

                            var dto = string.Empty;
                            if (!rdr.IsDBNull(6))
                            {
                                decimal dtoAmount = rdr.GetDecimal(6);
                                finalAmount = finalAmount / (1 + impuesto / 100);

                                amountText = string.Format(
                                CultureInfo.InvariantCulture,
                                @"{0:#0.00} €",
                                finalAmount).Replace(",", ".");

                                finalAmount = finalAmount * (100 - dtoAmount) / 100;
                                dto = string.Format(
                                    CultureInfo.InvariantCulture,
                                    @"{0:#0.00} %",
                                    dtoAmount).Replace(",", ".");
                                totalRowText = string.Format(
                                    CultureInfo.InvariantCulture,
                                    @"{0:#0.00} €",
                                    finalAmount).Replace(",", ".");
                            }

                            if (especialidad.Equals("propio", StringComparison.OrdinalIgnoreCase))
                            {
                                totalBaseCost += finalAmount;
                            }
                            else
                            {
                                totalBaseCostASPAD += finalAmount;
                            }

                            if (especialidad.Equals("propio", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!propios)
                                {
                                    tableCostsPropios.AddCell(new PdfPCell(new Phrase(dictionary["PDF_Actospropios"], ToolsPdf.TimesBold))
                                    {
                                        Border = ToolsPdf.BorderBottom,
                                        Padding = 6f,
                                        PaddingTop = 6f,
                                        HorizontalAlignment = 0,
                                        Colspan = 6
                                    });
                                }

                                tableCostsPropios.AddCell(ToolsPdf.DataCell(especialidad, labelFont));
                                tableCostsPropios.AddCell(ToolsPdf.DataCell(rdr.GetString(3), labelFont));
                                tableCostsPropios.AddCell(ToolsPdf.DataCellRight(1, labelFont));
                                tableCostsPropios.AddCell(ToolsPdf.DataCellRight(amountText, labelFont));
                                tableCostsPropios.AddCell(ToolsPdf.DataCellRight(dto, labelFont));
                                tableCostsPropios.AddCell(ToolsPdf.DataCellRight(totalRowText, labelFont));
                                propios = true;

                                if (!string.IsNullOrEmpty(actoObservaciones))
                                {
                                    hasObservaciones = true;
                                    tableObservaciones.AddCell(new PdfPCell(new Phrase(rdr.GetString(3), labelFont))
                                    {
                                        Border = ToolsPdf.BorderNone,
                                        Padding = 6f,
                                        PaddingTop = 6f,
                                        HorizontalAlignment = 0
                                    });

                                    tableObservaciones.AddCell(new PdfPCell(new Phrase(actoObservaciones, labelFont))
                                    {
                                        Border = ToolsPdf.BorderNone,
                                        Padding = 6f,
                                        PaddingTop = 6f,
                                        HorizontalAlignment = 0
                                    });
                                }
                            }
                            else
                            {
                                tableCostsAspad.AddCell(ToolsPdf.DataCell(especialidad, labelFont));
                                tableCostsAspad.AddCell(ToolsPdf.DataCell(rdr.GetString(3), labelFont));
                                tableCostsAspad.AddCell(ToolsPdf.DataCellRight(1, labelFont));
                                tableCostsAspad.AddCell(ToolsPdf.DataCellRight(amountText, labelFont));
                                tableCostsAspad.AddCell(ToolsPdf.DataCellRight(dto, labelFont));
                                tableCostsAspad.AddCell(ToolsPdf.DataCellRight(totalRowText, labelFont));

                                if (!string.IsNullOrEmpty(actoObservaciones))
                                {
                                    hasObservaciones = true;
                                    tableObservaciones.AddCell(new PdfPCell(new Phrase(rdr.GetString(3), labelFont))
                                    {
                                        Border = ToolsPdf.BorderNone,
                                        Padding = 6f,
                                        PaddingTop = 6f,
                                        HorizontalAlignment = 0
                                    });

                                    tableObservaciones.AddCell(new PdfPCell(new Phrase(actoObservaciones, labelFont))
                                    {
                                        Border = ToolsPdf.BorderNone,
                                        Padding = 6f,
                                        PaddingTop = 6f,
                                        HorizontalAlignment = 0
                                    });
                                }
                            }
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

        var la1 = new Chunk(dictionary["PDF_Albaran"] + ": ", labelFont);
        var la2 = new Chunk(presupuestoCodigo + Environment.NewLine, FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD));
        var la3 = new Chunk(dictionary["PDF_Fecha"] + ": ", labelFont);
        var la4 = new Chunk(presupuestoFecha, dataFont);

        var p1 = new Paragraph
        {
            la1,
            la2,
            la3,
            la4
        };

        document.Add(new Paragraph(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine));

        var lb1 = new Chunk(aseguradoNombre + Environment.NewLine, dataFont);
        var lb2 = new Chunk(dictionary["PDF_Poliza"] + ": " , labelFont);
        var lb3 = new Chunk(poliza.Numero + Environment.NewLine, dataFont);

        var p2 = new Paragraph
        {
            lb1,
            lb2,
            lb3
        };

        var cell1 = new PdfPCell { Border = ToolsPdf.BorderNone };
        var cell2 = new PdfPCell { Border = ToolsPdf.BorderNone };

        cell1.AddElement(p1);
        cell2.AddElement(p2);


        table.AddCell(cell1);
        table.AddCell(cell2);

        document.Add(table);

        decimal baseIvaASPAD = totalBaseCostASPAD * impuesto / 100;
        string baseIvaTextASPAD = string.Format(CultureInfo.InvariantCulture, @"{0:#0.00} €", baseIvaASPAD).Replace(",", ".");
        string totalASPAD = string.Format(CultureInfo.InvariantCulture, @"{0:#0.00} €", totalBaseCostASPAD + baseIvaASPAD).Replace(",", ".");

        tableCostsAspad.AddCell(ToolsPdf.DataCellTotal(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCellTotal(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCellTotal(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCellTotal(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCellTotal(dictionary["PDF_Base"] + ":", labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCellTotal(string.Format(CultureInfo.InvariantCulture, @"{0:#0.00} €", totalBaseCostASPAD).Replace(",", "."), totalFont));

        tableCostsAspad.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCellRight(impuestoText + ":", labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCellRight(baseIvaTextASPAD, totalFont));

        tableCostsAspad.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCellRight(dictionary["PDF_Total"] + ":", labelFont));
        tableCostsAspad.AddCell(ToolsPdf.DataCellRight(totalASPAD, totalFont));
        document.Add(tableCostsAspad);

        if (propios)
        {
            decimal baseIva = totalBaseCost * impuesto / 100;
            string baseIvaText = string.Format(CultureInfo.InvariantCulture, @"{0:#0.00} €", baseIva).Replace(",", ".");
            string total = string.Format(CultureInfo.InvariantCulture, @"{0:#0.00} €", totalBaseCost + baseIva).Replace(",", ".");

            tableCostsPropios.AddCell(ToolsPdf.DataCellTotal(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCellTotal(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCellTotal(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCellTotal(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCellTotal(dictionary["PDF_Base"], labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCellTotal(string.Format(CultureInfo.InvariantCulture, @"{0:#0.00} €", totalBaseCost).Replace(",", "."), totalFont));

            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCellRight(impuestoText + ":", labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCellRight(baseIvaText, totalFont));

            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCellRight(dictionary["PDF_Total"] + ":", labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCellRight(total, totalFont));

            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));
            tableCostsPropios.AddCell(ToolsPdf.DataCell(string.Empty, labelFont));

            tableCostsPropios.AddCell(TotalCell(string.Empty));
            tableCostsPropios.AddCell(TotalCell(string.Empty));
            tableCostsPropios.AddCell(TotalCell(string.Empty));
            tableCostsPropios.AddCell(TotalCell(string.Empty));
            tableCostsPropios.AddCell(TotalCell(dictionary["PDF_Total"] + ":"));
            string superTotal = string.Format(CultureInfo.InvariantCulture, @"{0:#0.00} €", totalBaseCost + baseIva + totalBaseCostASPAD + baseIvaASPAD).Replace(",", ".");
            tableCostsPropios.AddCell(TotalCell(superTotal));

            document.Add(tableCostsPropios);
        }

        if (!string.IsNullOrEmpty(observaciones))
        {
            document.Add(new Paragraph("Observaciones" , ToolsPdf.TimesBold));
            document.Add(new Paragraph(observaciones, labelFont));
        }

        if (hasObservaciones)
        {
            document.Add(tableObservaciones);
        }

        document.Add(new Phrase(dictionary["Item_Presupuesto_Print_Caducity"], ToolsPdf.TimesLittle));

        document.Close();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "inline;filename=Incidencia.pdf");
        Response.ContentType = "application/pdf";
        Response.WriteFile(Request.PhysicalApplicationPath + "\\Temp\\" + fileName);
        Response.Flush();
        Response.Clear();
    }

    public static PdfPCell TotalCell(string label)
    {
        var finalLabel = string.Empty;
        if (!string.IsNullOrEmpty(label))
        {
            finalLabel = label;
        }

        return new PdfPCell(new Phrase(finalLabel.ToUpperInvariant(), new Font(ToolsPdf.Arial, 10, Font.BOLD, BaseColor.BLACK)))
        {
            Border = Rectangle.TOP_BORDER,
            HorizontalAlignment = Element.ALIGN_RIGHT,
            Padding = 8f,
            PaddingTop = 6f
        };
    }
}