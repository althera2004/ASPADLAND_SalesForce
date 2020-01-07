// --------------------------------
// <copyright file="Configuracion.aspx.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using SbrinnaCoreFramework.Activity;
using SbrinnaCoreFramework.UI;
using ShortcutFramework.Item;

public partial class AdminTrazas : Page
{ 
    /// <summary> Master of page</summary>
    private AdminMaster master;

    /// <summary>User logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    private FormFooter formFooter;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public string Trazas { get; private set; }

    private int userId;

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.Dictionary);
        }
    }

    public ApplicationUser ApplicationUser
    {
        get
        {
            return this.user;
        }
    }

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public string ShowHelpChecked
    {
        get
        {
            return this.user.ShowHelp ? " checked=\"checked\"" : string.Empty;
        }
    }

    public string UserJson
    {
        get
        {
            return this.user.Json;
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.company = Session["company"] as Company;
        this.master = this.Master as AdminMaster;
        this.master.AddBreadCrumbInvariant(this.Dictionary["Admin_Traces"]);
        this.master.Titulo = this.Dictionary["Admin_Traces"];
        this.RenderPendientes();
        this.RenderSinPresupuesto();
        this.RenderDescartados();
    }

    private void RenderPendientes()
    {
        var trazas = Traza.AllPresupuestos;
        var actualDate = DateTime.Now;
        var actualMascota = Guid.Empty;

        var final = new List<Traza>();
        foreach (var traza in trazas)
        {
            if (traza.Fecha.Value.Date != actualDate && traza.MascotaId != actualMascota && traza.Tipo != 5)
            {
                final.Add(traza);
            }

            if (traza.Tipo == 4)
            {
                final.Add(traza);
            }

            actualDate = traza.Fecha.Value.Date;
            actualMascota = traza.MascotaId;
        }

        this.Trazas = Traza.JsonList(new ReadOnlyCollection<Traza>(final));
    }

    private void RenderSinPresupuesto()
    {
        var res = new StringBuilder();
        var query = @"
        SELECT DISTINCT
	        DATEADD(dd, 0, DATEDIFF(dd, 0, B.[Date])) AS FechaB,
	        A.Name,
	        A.NumberOfEmployees,
	        B.Busqueda,
	        P.Type,
	        ISNULL(C.name,'')

        FROM AspadLand_Traces B WITH(NOLOCK)
        LEFT JOIN AspadLand_Traces P WITH(NOLOCK)
        ON B.Type = 9
        AND P.Type = 5
        AND P.CentroId = B.CentroId
        AND P.Date >= B.Date
	        LEFT JOIN AspadLandPresuspuesto PR WITH(NOLOCK)
	        ON	PR.PresupuestoId = P.PresupuestoId
		        LEFT JOIN qes_poliza POL WITH(NOLOCK)
		        ON	POL.qes_polizaId = PR.PolizaId
		        AND POL.qes_dni = B.Busqueda
		
        INNER JOIN Account A WITH(NOLOCK)
        ON	A.AccountId = b.CentroId
        INNER JOIN Account C WITH(NOLOCK)
        ON	C.AccountId= B.ColectivoId

        WHERE P.CentroId is null
        AND B.Type = 9
        AND A.Name <> 'CENTRO VETERINARIO PILOTO ASPAD'

        ORDER BY DATEADD(dd, 0, DATEDIFF(dd, 0, B.[Date])) DESC";

        int x = 0;
        using(var cmd = new SqlCommand(query))
        {
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"<tr><td style=""width:450px;"">{0} - {1}</td><td style=""width:80px;"">{2:dd/MM/yyyy}</td><td style=""width:150px;"" title=""{3}""><div style=""width:150px;margin:0;padding:0;white-space:nowrap;overflow:hidden;text-overflow: ellipsis;"">{3}</div></td><td>{4}</td></tr>",
                                rdr.GetInt32(2),
                                rdr.GetString(1),
                                rdr.GetDateTime(0),
                                rdr.GetString(5),
                                rdr.GetString(3));
                            x++;
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

        this.TotalRecordsSinPresupuesto.Text = x.ToString();
        this.LtSinPresupuesto.Text = res.ToString();
    }

    private void RenderDescartados()
    {
        var res = new StringBuilder();
        int x = 0;
        using (var cmd = new SqlCommand("ASPADLand_Admin_TracesPresupuestos_Descartados"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    var acutalCentro = Guid.Empty;
                    var actualMascota = Guid.Empty;
                    var fecha = DateTime.Now;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if(actualMascota == rdr.GetGuid(6) && acutalCentro == rdr.GetGuid(10) && fecha.Date == rdr.GetDateTime(2).Date)
                            {
                                continue;
                            }

                            actualMascota = rdr.GetGuid(6);
                            acutalCentro = rdr.GetGuid(10);
                            fecha = rdr.GetDateTime(2).Date;

                            res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                @"<tr><td style=""width:450px;"">{0} - {1}</td><td style=""width:80px;"">{2:dd/MM/yyyy}</td><td style=""width:150px;"">{3}</td><td>{4}</td><td>{5} - {6}</td></tr>",
                                rdr.GetInt32(0),
                                rdr.GetString(1),
                                rdr.GetDateTime(2),
                                rdr.GetString(5),
                                rdr.GetString(8),
                                rdr.GetString(7),
                                rdr.GetString(9));
                            x++;
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


        this.LtDescartados.Text = res.ToString();
        this.TotalRecordsDescartados.Text = x.ToString();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult PendientesExcel()
    {
        var res = ActionResult.NoAction;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"Pendientes_{0:yyyyMMdd-hhmmss}.xls",
            DateTime.Now);

        var wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        var sh = (HSSFSheet)wb.CreateSheet("Presupuestos pendientes");

        var moneyCellStyle = wb.CreateCellStyle();
        var hssfDataFormat = wb.CreateDataFormat();
        moneyCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var headerCellStyle = wb.CreateCellStyle();
        var headerFont = wb.CreateFont();
        headerFont.Boldweight = (short)FontBoldWeight.Bold;
        headerCellStyle.SetFont(headerFont);
        headerCellStyle.BorderBottom = BorderStyle.Double;

        var totalCellStyle = wb.CreateCellStyle();
        var totalFont = wb.CreateFont();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalCellStyle.SetFont(headerFont);
        totalCellStyle.BorderTop = BorderStyle.Double;

        var totalValueCellStyle = wb.CreateCellStyle();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalValueCellStyle.SetFont(headerFont);
        totalValueCellStyle.BorderTop = BorderStyle.Double;
        totalValueCellStyle.BorderBottom = BorderStyle.None;
        totalValueCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var titleCellStyle = wb.CreateCellStyle();
        var titleFont = wb.CreateFont();
        titleFont.Boldweight = (short)FontBoldWeight.Bold;
        titleFont.FontHeight = 400;
        titleCellStyle.SetFont(titleFont);

        var decimalFormat = wb.CreateCellStyle();
        decimalFormat.DataFormat = wb.CreateDataFormat().GetFormat("#.00");

        var integerformat = wb.CreateCellStyle();
        integerformat.DataFormat = wb.CreateDataFormat().GetFormat("#0");

        var cra = new CellRangeAddress(0, 1, 0, 4);
        sh.AddMergedRegion(cra);
        if (sh.GetRow(0) == null) { sh.CreateRow(0); }
        sh.GetRow(0).CreateCell(0);
        sh.GetRow(0).GetCell(0).SetCellValue("Presupuestos pendientes");
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;

        var dataFormatCustom = wb.CreateDataFormat();


        // Crear Cabecera
        var headers = new List<string>() {
            "Centro",
            "Fecha",
            "Presupuesto",
            "Asegurado",
            "DNI",
            "Colectivo",
            "Poliza",
            "Mascota",
            "Chip",
            "Tipo",
            "Sexo"
        };

        int countColumns = 0;
        foreach (string headerLabel in headers)
        {
            if (sh.GetRow(3) == null) { sh.CreateRow(3); }

            if (sh.GetRow(3).GetCell(countColumns) == null)
            {
                sh.GetRow(3).CreateCell(countColumns);
            }

            sh.GetRow(3).GetCell(countColumns).SetCellValue(headerLabel);
            sh.GetRow(3).GetCell(countColumns).CellStyle = headerCellStyle;
            countColumns++;
        }

        var trazas = Traza.AllPresupuestos;
        var actualDate = DateTime.Now;
        var actualMascota = Guid.Empty;

        var final = new List<Traza>();
        foreach (var traza in trazas)
        {
            if (traza.Fecha.Value.Date != actualDate && traza.MascotaId != actualMascota && traza.Tipo != 5)
            {
                final.Add(traza);
            }

            if (traza.Tipo == 4)
            {
                final.Add(traza);
            }

            actualDate = traza.Fecha.Value.Date;
            actualMascota = traza.MascotaId;
        }

        int countRow = 4;
        foreach (var t in final)
        {
            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }

            // Centro
            if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
            sh.GetRow(countRow).GetCell(0).SetCellValue(t.CentroName);

            // Fecha
            if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
            sh.GetRow(countRow).GetCell(1).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
            sh.GetRow(countRow).GetCell(1).SetCellValue(t.Fecha.Value);

            // Presupuesto
            if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
            sh.GetRow(countRow).GetCell(2).SetCellValue(t.PresupuestoCode);

            // Asegurado
            if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
            sh.GetRow(countRow).GetCell(3).SetCellValue(t.asegurado);

            // DNI
            if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
            sh.GetRow(countRow).GetCell(4).SetCellValue(t.DNI);

            // Colectivo
            if (sh.GetRow(countRow).GetCell(5) == null) { sh.GetRow(countRow).CreateCell(5); }
            sh.GetRow(countRow).GetCell(5).SetCellValue(t.colectivo);

            // Poliza
            if (sh.GetRow(countRow).GetCell(6) == null) { sh.GetRow(countRow).CreateCell(6); }
            sh.GetRow(countRow).GetCell(6).SetCellValue(t.poliza);

            // Mascota
            if (sh.GetRow(countRow).GetCell(7) == null) { sh.GetRow(countRow).CreateCell(7); }
            sh.GetRow(countRow).GetCell(7).SetCellValue(t.mascotaName);

            // Sexo
            var sexo = "";
            if (t.sexo == "100000000") { sexo = "macho"; }
            if (t.sexo == "100000001") { sexo = "hembra"; }
            if (sh.GetRow(countRow).GetCell(8) == null) { sh.GetRow(countRow).CreateCell(8); }
            sh.GetRow(countRow).GetCell(8).SetCellValue(sexo);

            // Tipo
            var tipo = "";
            if (t.tipo == "100000000") { tipo = "perro"; }
            if (t.tipo == "100000001") { tipo = "gato"; }
            if (sh.GetRow(countRow).GetCell(9) == null) { sh.GetRow(countRow).CreateCell(9); }
            sh.GetRow(countRow).GetCell(9).SetCellValue(tipo);

            // Microchip
            if (sh.GetRow(countRow).GetCell(10) == null) { sh.GetRow(countRow).CreateCell(10); }
            sh.GetRow(countRow).GetCell(10).SetCellValue(t.chip);

            countRow++;
        }

        sh.SetColumnWidth(0, 12000);
        sh.SetColumnWidth(1, 3000);
        sh.SetColumnWidth(2, 4000);
        sh.SetColumnWidth(3, 20000);
        sh.SetColumnWidth(4, 4000);
        sh.SetColumnWidth(5, 6000);
        sh.SetColumnWidth(6, 6000);
        sh.SetColumnWidth(7, 6000);
        sh.SetColumnWidth(8, 4000);
        sh.SetColumnWidth(9, 4000);

        if (!path.EndsWith("\\"))
        {
            path += "\\Temp\\";
        }
        else
        {
            path += "Temp\\";
        }

        using (var fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create, FileAccess.Write))
        {
            wb.Write(fs);
        }

        res.SetSuccess(string.Format("/Temp/{0}", fileName));
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult DescartadosExcel()
    {
        var res = ActionResult.NoAction;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"Descartados_{0:yyyyMMdd-hhmmss}.xls",
            DateTime.Now);

        var wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        var sh = (HSSFSheet)wb.CreateSheet("Presupuestos descartados");
        
        var moneyCellStyle = wb.CreateCellStyle();
        var hssfDataFormat = wb.CreateDataFormat();
        moneyCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var headerCellStyle = wb.CreateCellStyle();
        var headerFont = wb.CreateFont();
        headerFont.Boldweight = (short)FontBoldWeight.Bold;
        headerCellStyle.SetFont(headerFont);
        headerCellStyle.BorderBottom = BorderStyle.Double;

        var totalCellStyle = wb.CreateCellStyle();
        var totalFont = wb.CreateFont();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalCellStyle.SetFont(headerFont);
        totalCellStyle.BorderTop = BorderStyle.Double;

        var totalValueCellStyle = wb.CreateCellStyle();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalValueCellStyle.SetFont(headerFont);
        totalValueCellStyle.BorderTop = BorderStyle.Double;
        totalValueCellStyle.BorderBottom = BorderStyle.None;
        totalValueCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var titleCellStyle = wb.CreateCellStyle();
        var titleFont = wb.CreateFont();
        titleFont.Boldweight = (short)FontBoldWeight.Bold;
        titleFont.FontHeight = 400;
        titleCellStyle.SetFont(titleFont);

        var decimalFormat = wb.CreateCellStyle();
        decimalFormat.DataFormat = wb.CreateDataFormat().GetFormat("#.00");

        var integerformat = wb.CreateCellStyle();
        integerformat.DataFormat = wb.CreateDataFormat().GetFormat("#0");

        var cra = new CellRangeAddress(0, 1, 0, 4);
        sh.AddMergedRegion(cra);
        if (sh.GetRow(0) == null) { sh.CreateRow(0); }
        sh.GetRow(0).CreateCell(0);
        sh.GetRow(0).GetCell(0).SetCellValue("Presupuestos descartados");
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;

        var dataFormatCustom = wb.CreateDataFormat();        


        // Crear Cabecera
        var headers = new List<string>() {
            "Centro",
            "Fecha",
            "Presupuesto",
            "Asegurado",
            "DNI/NIF",
            "Poliza",
            "Colectivo",
            "Mascota",
            "Chip",
            "Tipo",
            "Sexo"
        };

        int countColumns = 0;
        foreach (string headerLabel in headers)
        {
            if (sh.GetRow(3) == null) { sh.CreateRow(3); }

            if (sh.GetRow(3).GetCell(countColumns) == null)
            {
                sh.GetRow(3).CreateCell(countColumns);
            }

            sh.GetRow(3).GetCell(countColumns).SetCellValue(headerLabel);
            sh.GetRow(3).GetCell(countColumns).CellStyle = headerCellStyle;
            countColumns++;
        }

        int countRow = 4;
        using (var cmd = new SqlCommand("ASPADLand_Admin_TracesPresupuestos_Descartados"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    var acutalCentro = Guid.Empty;
                    var actualMascota = Guid.Empty;
                    var fecha = DateTime.Now;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (actualMascota == rdr.GetGuid(6) && acutalCentro == rdr.GetGuid(10) && fecha.Date == rdr.GetDateTime(2).Date)
                            {
                                continue;
                            }

                            actualMascota = rdr.GetGuid(6);
                            acutalCentro = rdr.GetGuid(10);
                            fecha = rdr.GetDateTime(2).Date;

                            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }

                            // Centro
                            if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
                            sh.GetRow(countRow).GetCell(0).SetCellValue(rdr[0].ToString() + " - " + rdr.GetString(1));

                            // Fecha
                            if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
                            sh.GetRow(countRow).GetCell(1).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
                            sh.GetRow(countRow).GetCell(1).SetCellValue(rdr.GetDateTime(2));

                            // Presupuesto
                            if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
                            sh.GetRow(countRow).GetCell(2).SetCellValue(rdr.GetString(5));

                            // Asegurado
                            if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
                            sh.GetRow(countRow).GetCell(3).SetCellValue(rdr.GetString(15));

                            // DNI
                            if (sh.GetRow(countRow).GetCell(4) == null) { sh.GetRow(countRow).CreateCell(4); }
                            sh.GetRow(countRow).GetCell(4).SetCellValue(rdr.GetString(9));

                            // Poliza
                            if (sh.GetRow(countRow).GetCell(5) == null) { sh.GetRow(countRow).CreateCell(5); }
                            sh.GetRow(countRow).GetCell(5).SetCellValue(rdr.GetString(7));

                            // Poliza
                            if (sh.GetRow(countRow).GetCell(6) == null) { sh.GetRow(countRow).CreateCell(6); }
                            sh.GetRow(countRow).GetCell(6).SetCellValue(rdr.GetString(8));

                            // Poliza
                            if (sh.GetRow(countRow).GetCell(7) == null) { sh.GetRow(countRow).CreateCell(7); }
                            sh.GetRow(countRow).GetCell(7).SetCellValue(rdr.GetString(11));

                            // Poliza
                            var sexo = "";
                            if (rdr.GetString(12) == "100000000") { sexo = "macho"; }
                            if (rdr.GetString(12) == "100000001") { sexo = "hembra"; }
                            if (sh.GetRow(countRow).GetCell(8) == null) { sh.GetRow(countRow).CreateCell(8); }
                            sh.GetRow(countRow).GetCell(8).SetCellValue(sexo);

                            // Poliza
                            var tipo = "";
                            if (rdr.GetString(13) == "100000000") { tipo = "perro"; }
                            if (rdr.GetString(13) == "100000001") { tipo = "gato"; }
                            if (sh.GetRow(countRow).GetCell(9) == null) { sh.GetRow(countRow).CreateCell(9); }
                            sh.GetRow(countRow).GetCell(9).SetCellValue(tipo);

                            // Poliza
                            if (sh.GetRow(countRow).GetCell(10) == null) { sh.GetRow(countRow).CreateCell(10); }
                            sh.GetRow(countRow).GetCell(10).SetCellValue(rdr.GetString(14));

                            countRow++;
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

        sh.SetColumnWidth(0, 12000);
        sh.SetColumnWidth(1, 3000);
        sh.SetColumnWidth(2, 4000);
        sh.SetColumnWidth(3, 20000);
        sh.SetColumnWidth(4, 4000);
        sh.SetColumnWidth(5, 6000);
        sh.SetColumnWidth(6, 6000);
        sh.SetColumnWidth(7, 6000);
        sh.SetColumnWidth(8, 4000);
        sh.SetColumnWidth(9, 4000);

        if (!path.EndsWith("\\"))
        {
            path += "\\Temp\\";
        }
        else
        {
            path += "Temp\\";
        }

        using (var fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create, FileAccess.Write))
        {
            wb.Write(fs);
        }

        res.SetSuccess(string.Format("/Temp/{0}", fileName));
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult SinPresupuestoExcel()
    {
        var res = ActionResult.NoAction;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"SinPresupuesto_{0:yyyyMMdd-hhmmss}.xls",
            DateTime.Now);

        var wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
        var sh = (HSSFSheet)wb.CreateSheet("Presupuestos descartados");

        var moneyCellStyle = wb.CreateCellStyle();
        var hssfDataFormat = wb.CreateDataFormat();
        moneyCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var headerCellStyle = wb.CreateCellStyle();
        var headerFont = wb.CreateFont();
        headerFont.Boldweight = (short)FontBoldWeight.Bold;
        headerCellStyle.SetFont(headerFont);
        headerCellStyle.BorderBottom = BorderStyle.Double;

        var totalCellStyle = wb.CreateCellStyle();
        var totalFont = wb.CreateFont();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalCellStyle.SetFont(headerFont);
        totalCellStyle.BorderTop = BorderStyle.Double;

        var totalValueCellStyle = wb.CreateCellStyle();
        totalFont.Boldweight = (short)FontBoldWeight.Bold;
        totalValueCellStyle.SetFont(headerFont);
        totalValueCellStyle.BorderTop = BorderStyle.Double;
        totalValueCellStyle.BorderBottom = BorderStyle.None;
        totalValueCellStyle.DataFormat = hssfDataFormat.GetFormat("#,##0.00");

        var titleCellStyle = wb.CreateCellStyle();
        var titleFont = wb.CreateFont();
        titleFont.Boldweight = (short)FontBoldWeight.Bold;
        titleFont.FontHeight = 400;
        titleCellStyle.SetFont(titleFont);

        var decimalFormat = wb.CreateCellStyle();
        decimalFormat.DataFormat = wb.CreateDataFormat().GetFormat("#.00");

        var integerformat = wb.CreateCellStyle();
        integerformat.DataFormat = wb.CreateDataFormat().GetFormat("#0");

        var cra = new CellRangeAddress(0, 1, 0, 4);
        sh.AddMergedRegion(cra);
        if (sh.GetRow(0) == null) { sh.CreateRow(0); }
        sh.GetRow(0).CreateCell(0);
        sh.GetRow(0).GetCell(0).SetCellValue("Busquedas sin presupuestos");
        sh.GetRow(0).GetCell(0).CellStyle = titleCellStyle;

        var dataFormatCustom = wb.CreateDataFormat();


        // Crear Cabecera
        var headers = new List<string>() {
            "Centro",
            "Fecha",
            "Búsqueda",
            "Colectivo"
        };

        int countColumns = 0;
        foreach (string headerLabel in headers)
        {
            if (sh.GetRow(3) == null) { sh.CreateRow(3); }

            if (sh.GetRow(3).GetCell(countColumns) == null)
            {
                sh.GetRow(3).CreateCell(countColumns);
            }

            sh.GetRow(3).GetCell(countColumns).SetCellValue(headerLabel);
            sh.GetRow(3).GetCell(countColumns).CellStyle = headerCellStyle;
            countColumns++;
        }

        int countRow = 4;
        var query = @"
        SELECT DISTINCT
	        A.NumberOfEmployees,
		    ISNULL(A.Name,''),
		    DATEADD(dd, 0, DATEDIFF(dd, 0, B.[Date])) AS Fecha,
		    C.Name,
		    ISNULL(B.Busqueda,'')

        FROM AspadLand_Traces B WITH(NOLOCK)
        LEFT JOIN AspadLand_Traces P WITH(NOLOCK)
        ON B.Type = 9
        AND P.Type = 5
        AND P.CentroId = B.CentroId
        AND P.Date >= B.Date
	        LEFT JOIN AspadLandPresuspuesto PR WITH(NOLOCK)
	        ON	PR.PresupuestoId = P.PresupuestoId
		        LEFT JOIN qes_poliza POL WITH(NOLOCK)
		        ON	POL.qes_polizaId = PR.PolizaId
		        AND POL.qes_dni = B.Busqueda
		
        INNER JOIN Account A WITH(NOLOCK)
        ON	A.AccountId = b.CentroId
        INNER JOIN Account C WITH(NOLOCK)
        ON	C.AccountId= B.ColectivoId

        WHERE P.CentroId is null
        AND B.Type = 9
        AND A.Name <> 'CENTRO VETERINARIO PILOTO ASPAD'

        ORDER BY DATEADD(dd, 0, DATEDIFF(dd, 0, B.[Date])) DESC";
        using (var cmd = new SqlCommand(query))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Connection.Open();
                    var acutalCentro = Guid.Empty;
                    var actualMascota = Guid.Empty;
                    var fecha = DateTime.Now;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (sh.GetRow(countRow) == null) { sh.CreateRow(countRow); }

                            // Centro
                            if (sh.GetRow(countRow).GetCell(0) == null) { sh.GetRow(countRow).CreateCell(0); }
                            sh.GetRow(countRow).GetCell(0).SetCellValue(rdr[0].ToString() + " - " + rdr.GetString(1));

                            // Fecha
                            if (sh.GetRow(countRow).GetCell(1) == null) { sh.GetRow(countRow).CreateCell(1); }
                            sh.GetRow(countRow).GetCell(1).CellStyle.DataFormat = dataFormatCustom.GetFormat("dd/MM/yyyy");
                            sh.GetRow(countRow).GetCell(1).SetCellValue(rdr.GetDateTime(2));

                            // Colectivo
                            if (sh.GetRow(countRow).GetCell(2) == null) { sh.GetRow(countRow).CreateCell(2); }
                            sh.GetRow(countRow).GetCell(2).SetCellValue(rdr.GetString(3));

                            // Búsqueda
                            if (sh.GetRow(countRow).GetCell(3) == null) { sh.GetRow(countRow).CreateCell(3); }
                            sh.GetRow(countRow).GetCell(3).SetCellValue(rdr.GetString(4));

                            countRow++;
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

        sh.SetColumnWidth(0, 12000);
        sh.SetColumnWidth(1, 3000);
        sh.SetColumnWidth(2, 8000);

        if (!path.EndsWith("\\"))
        {
            path += "\\Temp\\";
        }
        else
        {
            path += "Temp\\";
        }

        using (var fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create, FileAccess.Write))
        {
            wb.Write(fs);
        }

        res.SetSuccess(string.Format("/Temp/{0}", fileName));
        return res;
    }
}