using System;
using System.Web.UI;

public partial class LogOut : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();
        this.Response.Redirect("/Default.aspx");
    }
}