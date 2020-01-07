// --------------------------------
// <copyright file="InitSession.aspx.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using AspadLandFramework;

/// <summary>Implements InitSession page</summary>
public partial class Customer_InitSession : Page
{
    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        var dictionay = ApplicationDictionary.Load("es");
        this.Session["Navigation"] = new List<string>();
        this.Session["Dictionary"] = dictionay;
        Response.Redirect("/DashBoard.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
}