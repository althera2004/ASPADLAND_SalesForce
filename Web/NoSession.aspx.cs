using System;
using System.Web.UI;

public partial class Customer_NoSession : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Response.Redirect("/Default.aspx");
    }
}