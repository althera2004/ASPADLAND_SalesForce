// --------------------------------
// <copyright file="DataHeaderActions.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UIDataHeaderActions : UIDataHeaderItem
    {
        public int NumberOfActions { get; set; }

        public new string Render
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                     @"<th style=""width:{0}px;"">&nbsp;</th>",
                    this.NumberOfActions * 45);
            }
        }
    }
}
