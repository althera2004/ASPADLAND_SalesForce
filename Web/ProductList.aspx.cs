using SbrinnaCoreFramework.Graph;
using System;
using System.Collections.Generic;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.Item;

public partial class Customer_ProductList : Page
{
    /// <summary>Master of page</summary>
    private Main master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

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

    public string Tasks { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
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
        this.master.Titulo = "Item_Productos";
    }
}