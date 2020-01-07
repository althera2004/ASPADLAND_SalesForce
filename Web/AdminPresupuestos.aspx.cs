// --------------------------------
// <copyright file="AdminPresupuestos.aspx.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using AspadLandFramework;
using AspadLandFramework.Item;
using SbrinnaCoreFramework.Activity;
using SbrinnaCoreFramework.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;

public partial class AdminPresupuestos : Page
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
        this.master.AddBreadCrumbInvariant(this.Dictionary["Admin_Presupuestos"]);
        this.master.Titulo = this.Dictionary["Admin_Presupuestos"];
        this.master.TitleInvariant = true;
        this.RenderPendientes();
    }

    private void RenderPendientes()
    {
		//var realizados =  GetRealizados();
        var res = new StringBuilder();
        var count = 0;
        using(var cmd = new SqlCommand("ASPADLAND_GetPresupuestoRealizados"))
        {
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    using(var rdr = cmd.ExecuteReader())
                    {
                        var actual = string.Empty;
                        var fecha = DateTime.Now;
                        var centro = string.Empty;
                        var pol = string.Empty;
                        var acto = string.Empty;
                        var mascota = string.Empty;
                        var actos = string.Empty;
                        var colectivo = string.Empty;
                        while (rdr.Read())
                        {
                            /*P.PresupuestoId,
		P.Code,
		CONVERT(date,Fecha) AS Fecha,
		P.MascotaId,
		P.CentroId,
		POL.qes_name AS Poliza,
		C.Name AS Centro,
		P.ActoName,
		ISNULL(M.qes_name,'') + ' ' +  ISNULL(M.qes_NMicrochip,''),
		POL.qes_ColectivoIdName*/


                            var presupuesto = rdr.GetString(1);
                            if (!presupuesto.Equals(actual, StringComparison.OrdinalIgnoreCase))
                            {
                                if(actos.Length > 2)
                                {
                                    actos = actos.Substring(0, actos.Length - 2);
                                }

                                count++;
                                if (!string.IsNullOrEmpty(actual))
                                {
                                    res.AppendFormat(
                                        CultureInfo.InvariantCulture,
                                        @"<tr>
                                        <td style=""width:90px;"" rowspan=""2"">{0:dd/MM/yyyy}</td>
                                        <td>{1}</td>
                                        <td style=""width:150px;"">{2}</td>
                                        <td style=""width:220px;"">{3}<br /><strong>{6}</strong></td>
                                        <td style=""width:200px;text-align:center"">{4}</td>
                                        </tr>
                                      <tr><td colspan=""4""><i>{5}</i></td></tr>",
                                        fecha,
                                        centro,
                                        actual,
                                        pol,
                                        mascota,
                                        actos,
                                        colectivo);
                                }

                                actos = string.Empty;
                            }

							fecha = rdr.GetDateTime(2);
							centro = rdr.GetString(6);
							pol = rdr.GetString(5);
                            acto = rdr.GetString(7);
                            mascota = rdr.GetString(8);
                            actual = presupuesto;
                            colectivo = rdr.GetString(9);
                            actos += acto + ", ";
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

        this.LtBody.Text = res.ToString();
        this.LtBodyCount.Text = count.ToString();
    }
	
	private ReadOnlyCollection<Realizados> GetRealizados(){
		var res = new List<Realizados>();
		using(var cmd = new SqlCommand("ASPADLAND_GetPresupuestoRealizados"))
        {
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Connection.Open();
                    using(var rdr = cmd.ExecuteReader())
                    {
                        var actual = string.Empty;
                        while (rdr.Read())
                        {
                                res.Add(new Realizados{
									Poliza = rdr.GetString(5),
									CentroId = rdr.GetGuid(4),
									MascotaId = rdr.GetGuid(3),
									Fecha = rdr.GetDateTime(2)									
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
        }
			
			return  new ReadOnlyCollection<Realizados>(res);
	}
	
	public struct Realizados{
		public Guid CentroId {get;set;}
		public Guid MascotaId {get;set;}
		public DateTime Fecha {get;set;}
		public string Poliza{get;set;}
	}
}