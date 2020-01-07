// --------------------------------
// <copyright file="ApplicationGrant.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace AspadLandFramework
{
    /// <summary>Implements ApplicationGrant</summary>
    public sealed class ApplicationGrant
    {
        /// <summary>Gets or sets the code of item</summary>
        public int Code { get; set; }

        /// <summary>Gets or sets the description of item</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the page of item</summary>
        public string Page { get; set; }
    }
}