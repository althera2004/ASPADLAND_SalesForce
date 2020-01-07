// --------------------------------
// <copyright file="AdminMaster.master.cs" company="OpenFramework">
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
using System.Text;
using System.Web.UI;
using AspadLandFramework;
using AspadLandFramework.UserInterface;
using SbrinnaCoreFramework.UI;

/// <summary>Implements the master page of application</summary>
public partial class AdminMaster : MasterPage
{
    /// <summary>List of navigation history urls</summary>
    private List<string> navigation;

    public string Version
    {
        get
        {
            return ConfigurationManager.AppSettings["Version"];
        }
    }

    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public int PendentTasks { get; set; }

    public string IssusVersion
    {
        get
        {
            return ConfigurationManager.AppSettings["issusversion"];
        }
    }

    public string BK
    {
        get
        {
            return Session["BK"] as string;
        }
    }

    /// <summary>Gets the navigation history</summary>    
    public string NavigationHistory
    {
        get
        {
            var res = new StringBuilder(Environment.NewLine).Append("<!-- Havigation history -->").Append(Environment.NewLine);
            /*foreach (string link in this.navigation)
            {
                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "    {0}{1}<br />", link, Environment.NewLine));
            }*/

            res.Append("<!-- -->").Append(Environment.NewLine);
            return res.ToString();
        }
    }

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Indicates if the title of page is translatable</summary>
    private bool titleInvariant;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    private Collection<BreadcrumbItem> breadCrumb;

    public string ItemCode { get; set; }

    /// <summary>Gets or sets a JSON list of searcheable items</summary>
    public string SearcheableItems { get; set; }

    /// <summary>Gets the url of referrer page</summary>
    public string Referrer
    {
        get
        {
            if(Session["Navigation"] == null)
            {
                this.navigation = new List<string>();
            }
            else
            {
                this.navigation = Session["Navigation"] as List<string>;
            }

            string actual = this.Request.RawUrl;
            string res = string.Empty;

            if (actual.ToUpperInvariant().IndexOf("AdminDefault.ASPX", StringComparison.OrdinalIgnoreCase) != -1)
            {
                var newNavigation = new List<string>
                {
                    "AdminDefault.aspx"
                };
                Session["Navigation"] = newNavigation;
                return "AdminDefault.aspx";
            }

            string last1 = string.Empty;
            string last2 = string.Empty;
            string last3 = string.Empty;

            if (this.navigation.Count > 2)
            {
                last1 = this.navigation[this.navigation.Count - 1];
                last2 = this.navigation[this.navigation.Count - 2];
                last3 = this.navigation[this.navigation.Count - 3];
            }
            else if (this.navigation.Count > 1)
            {
                last1 = this.navigation[this.navigation.Count - 1];
                last2 = this.navigation[this.navigation.Count - 2];
            }
            else if (this.navigation.Count == 1)
            {
                last1 = this.navigation[0];
            }

            if (actual == last1)
            {
                this.navigation.RemoveAt(this.navigation.Count - 1);
                res = last2;
            }
            else if (actual == last2)
            {
                this.navigation.RemoveAt(this.navigation.Count - 1);
                this.navigation.RemoveAt(this.navigation.Count - 1);
                res = last3;
            }
            else if (last1.IndexOf(".aspx?id=-1") != -1)
            {
                this.navigation.RemoveAt(this.navigation.Count - 1);
                res = last2;
            }
            else
            {
                res = last1;
            }

            if (actual != res)
            {
                this.navigation.Add(actual);
            }

            Session["Navigation"] = this.navigation;


            return res;
        }
    }

    /// <summary>Gets or sets a value indicating whether if is an administration pagesummary>
    public bool AdminPage { get; set; }

    /// <summary>Gets de breadcrumb elements</summary>
    public Collection<BreadcrumbItem> BreadCrumb
    {
        get
        {
            return this.breadCrumb;
        }
    }

    /// <summary>Gets a value indicating whether if the text of title is translatable</summary>
    public bool TitleInvariant
    {
        get
        {
            return this.titleInvariant;
        }

        set
        {
            this.titleInvariant = value;
        }
    }

    public ApplicationUser ApplicationUser
    {
        get
        {
            return this.user;
        }
    }

    /// <summary>Gets the HTML code for breadcrumb object</summary>
    public string RenderBreadCrumb
    {
        get
        {
            var res = new StringBuilder("<li><i class=\"fas fa-home\"></i><a href=\"DashBoard.aspx\">");
            res.Append(this.Dictionary["Common_Home"]);
            res.Append("</a></li>");

            foreach (var item in this.breadCrumb)
            {
                string label = item.Label;
                if (item.Leaf)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"<li class=""active"">{0}</li>",
                        label);
                }
                else
                {
                    string link = "#";
                    if (!string.IsNullOrEmpty(item.Link))
                    {
                        link = item.Link;
                    }

                    res.Append("<li><a href=\"").Append(link).Append("\" title=\"").Append(label).Append("\">").Append(this.dictionary[item.Label]).Append("</a></li>");
                }
            }

            return res.ToString();
        }
    }

    public string UserId
    {
        get
        {
            return this.user.Id.ToString();
        }
    }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    public string Titulo { get; set; }

    public UIButton ButtonNewItem { get; set; }

    public string ButtonNewItemHtml
    {
        get
        {
            if (this.ButtonNewItem != null)
            {
                return this.ButtonNewItem.Render;
            }

            return string.Empty;
        }
    }

    public void AddBreadCrumb(string label)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem() { Link = "#", Label = label, Leaf = true };
        this.breadCrumb.Add(newBreadCrumb);
    }

    public void AddBreadCrumbInvariant(string label)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem { Link = "#", Label = label, Leaf = true, Invariant = true };
        this.breadCrumb.Add(newBreadCrumb);
    }

    public void AddBreadCrumb(string label, bool leaf)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem { Link = "#", Label = label, Leaf = leaf };
        this.breadCrumb.Add(newBreadCrumb);
    }

    public void AddBreadCrumb(string label, string link, bool leaf)
    {
        if (this.breadCrumb == null)
        {
            this.breadCrumb = new Collection<BreadcrumbItem>();
        }

        var newBreadCrumb = new BreadcrumbItem { Link = link, Label = label, Leaf = leaf };
        this.breadCrumb.Add(newBreadCrumb);
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.LtBuild.Text = ConfigurationManager.AppSettings["Version"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

        // Renew session
        //// ---------------------------------
        this.Session["Dictionary"] = this.dictionary;
        //// ---------------------------------
    }
}