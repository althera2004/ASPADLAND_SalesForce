// --------------------------------
// <copyright file="Data.aspx.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using AspadLandFramework;

/// <summary>Implements JavaScript generator for data page</summary>
public partial class js_Data : Page
{
    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime d1 = DateTime.Now;
        ApplicationUser user = Session["user"] as ApplicationUser;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.Response.Clear();
        this.Response.ClearHeaders();
        this.Response.ContentType = "text/javascript";

        this.Response.Write(string.Format(CultureInfo.InvariantCulture, @"var user = {0};", user.Json));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);

        string userCulture = "es-ES";

        this.Response.Write(string.Format(CultureInfo.InvariantCulture, @"var UserCulture = ""{0}"";", userCulture));

        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);        
        this.Response.Write(Environment.NewLine);
        this.Response.Write(Environment.NewLine);
        
        
        this.Response.Write("var Dictionary =" + Environment.NewLine);
        this.Response.Write("{" + Environment.NewLine);

        foreach (KeyValuePair<string, string> item in this.dictionary)
        {
            if (!item.Key.StartsWith("Help_") || true)
            {
                this.Response.Write(this.DictionaryItem(item.Key.Replace(' ', '_'), item.Value.Replace("\\", "\\\\").Replace("\"", "\\\"")));
            }
        }

        this.Response.Write("    \"-\": \"-\"" + Environment.NewLine);
        this.Response.Write("};");
    }

    /// <summary>Gets a entry for dictionary JSON</summary>
    /// <param name="key">Item key</param>
    /// <param name="value">Item value</param>
    /// <returns>JSON Code</returns>
    private string DictionaryItem(string key, string value)
    {
        return string.Format(CultureInfo.GetCultureInfo("es-es"), @"    ""{0}"": ""{1}"",{2}", key, value, Environment.NewLine);
    }
}