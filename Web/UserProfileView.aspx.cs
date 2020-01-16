// --------------------------------
// <copyright file="UserProfileView.aspx.cs" company="OpenFramework">
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
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class UserProfileView : Page
{
    /// <summary> Master of page</summary>
    private Main master;

    /// <summary>User logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    private FormFooter formFooter;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

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
            return this.formFooter.Render(this.dictionary);
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
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {        
        this.user = Session["User"] as ApplicationUser;
        this.company = Session["company"] as Company;
        //this.user = ApplicationUser.GetById(this.user.Id);

        if (user.HorarioLunes == null) { user.HorarioLunes = string.Empty; }
        if (user.HorarioMartes == null) { user.HorarioMartes = string.Empty; }
        if (user.HorarioMiercoles == null) { user.HorarioMiercoles = string.Empty; }
        if (user.HorarioJueves == null) { user.HorarioJueves = string.Empty; }
        if (user.HorarioViernes == null) { user.HorarioViernes = string.Empty; }
        if (user.HorarioSabado == null) { user.HorarioSabado = string.Empty; }
        if (user.HorarioDomingo == null) { user.HorarioDomingo = string.Empty; }

        if (!user.HorarioLunes.Contains("-")) { this.user.HorarioLunes += "-"; }
        if (!user.HorarioMartes.Contains("-")) { this.user.HorarioMartes += "-"; }
        if (!user.HorarioMiercoles.Contains("-")) { this.user.HorarioMiercoles += "-"; }
        if (!user.HorarioJueves.Contains("-")) { this.user.HorarioJueves += "-"; }
        if (!user.HorarioViernes.Contains("-")) { this.user.HorarioViernes += "-"; }
        if (!user.HorarioSabado.Contains("-")) { this.user.HorarioSabado += "-"; }
        if (!user.HorarioDomingo.Contains("-")) { this.user.HorarioDomingo += "-"; }

        //this.user.Employee = new Employee(this.user.Employee.Id, false);
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Main;
        this.master.AddBreadCrumbInvariant(this.Dictionary["Item_UserProfile_Breacrumb"]);
        this.master.Titulo = this.user.Nombre +  " - " + this.user.Code;
        this.master.TitleInvariant = true;

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Text = this.dictionary["Common_Accept"], Icon = "icon-ok", Action = "success" });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Text = this.dictionary["Common_Cancel"], Icon = "icon-undo" });
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult ChangePassword(int userId, string oldPassword, string newPassword, int companyId)
    {
        /*ActionResult res = ApplicationUser.ChangePassword(userId, oldPassword, newPassword, companyId);
        if (res.MessageError == "NOPASS")
        {
            Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            if (dictionary != null)
            {
                res.MessageError = dictionary["Common_Error_IncorrectPassword"];
            }
            else
            {
                res.MessageError = "Incorrect password";
            }
        }*/

        return ActionResult.NoAction;
    }
}