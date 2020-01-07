using SbrinnaCoreFramework.Graph;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;
using System.Web.Script.Services;
using System.Web.Services;
using SbrinnaCoreFramework.Activity;
using SbrinnaCoreFramework.DataAccess;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class Customer_ProductView : Page
{
    /// <summary>Master of page</summary>
    private Main master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    public DocumentosCentro Documento { get; private set; }
    

    public string ProductsJson
    {
        get
        {
            return DocumentosCentro.JsonList(DocumentosCentro.GetAll(new Guid()));
        }
    }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    private string WebBase;

    /// <summary>
    /// Gets the dictionary for interface texts
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    public string Filter
    {
        get
        {
            string filter = @"{""Owners"":true,""Others"":true}";

            if (Session["DashBoardFilter"] != null)
            {
                filter = Session["DashBoardFilter"].ToString();
            }

            return filter;
        }
    }

    public long ProductoId { get; private set; }
    public string ProductoName { get; private set; }
    public string Descripcion { get; private set; }
    public string Descripcion2 { get; private set; }
    public StringBuilder Images { get; private set; }
    public string Frontal { get; private set; }
    public string Relacionados { get; private set; }
    public string LineaProducto { get; private set; }
    public string Tallas { get; private set; }
    public string Colores { get; private set; }
    public string Calidad { get; private set; }
    public string NormasLavado { get; private set; }
    public string Destacados { get; private set; }
    public string Propiedades { get; private set; }
    public string Caracteristicas { get; private set; }
    public string ParaEl { get; private set; }
    public string ParaElla { get; private set; }
    public long CategoriaId { get; private set; }
    public string Estilo { get; private set; }
    public string Tejido { get; private set; }
    public string Composicion { get; private set; }
    public string Gramaje { get; private set; }
    public string Origen { get; private set; }
    public string Embalaje { get; private set; }
    public string Existencias { get; private set; }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.WebBase = ConfigurationManager.AppSettings["WebBase"].ToString();
        if (this.Session["User"] == null)
        {
            this.Response.Redirect("NoSession.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.Go();
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        this.user = (ApplicationUser)Session["User"];
        Session["User"] = this.user;
        this.master = this.Master as Main;
        this.company = Session["Company"] as Company;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.master.AddBreadCrumb("Item_Producto");
        this.master.TitleInvariant = true;
        this.ProductoId = Convert.ToInt64(this.Request.QueryString["id"].ToString());
        
    }

    [WebMethod]
    [ScriptMethod]
    public static ActionResult AddToCatalog(long productoId, long marcaClienteId)
    {
        var res = ActionResult.NoAction;
        try
        {
            /* CREATE PROCEDURE SmartlandCliente_ProductoAddMarcaCliente
             *   @ProductoId bigint,
             *   @MarcaClienteId bigint */
            using (SqlCommand cmd = new SqlCommand("SmartlandCliente_ProductoAddMarcaCliente"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Parameters.Add(DataParameter.Input("@ProductoId", productoId));
                    cmd.Parameters.Add(DataParameter.Input("@MarcaClienteId", marcaClienteId));
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch(Exception ex)
                    {
                        res.SetFail(ex);
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

                res.SetSuccess();
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }
}