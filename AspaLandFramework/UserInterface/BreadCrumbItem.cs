// --------------------------------
// <copyright file="BreadCrumbItem.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace AspadLandFramework.UserInterface
{
    /// <summary>Implements then BreadCrumbItem class</summary>
    public class BreadcrumbItem
    {
        /// <summary>Link of breadcrumb item</summary>
        private string link;
        
        /// <summary>Label of breadcrumb item</summary>
        private string label;

        /// <summary>Indicates if item is leaf</summary>
        private bool leaf;

        /// <summary>Indicates if prevents text translation</summary>
        private bool invariant;

        /// <summary>Gets or sets a value indicating whether the link of breadcrumb item</summary>
        public string Link
        {
            get
            { 
                return this.link; 
            }

            set
            { 
                this.link = value; 
            }
        }

        /// <summary>Gets or sets a value indicating whether the label of breadcrumb item</summary>
        public string Label
        {
            get 
            {
                return this.label;
            }

            set 
            { 
                this.label = value; 
            }
        }

        /// <summary>Gets or sets a value indicating whether if item is leaf</summary>
        public bool Leaf
        {
            get 
            { 
                return this.leaf; 
            }

            set 
            { 
                this.leaf = value;
            }
        }

        /// <summary>Gets or sets a value indicating whether if prevents text translation</summary>
        public bool Invariant
        {
            get 
            {
                return this.invariant;
            }

            set 
            { 
                this.invariant = value;
            }
        }
    }
}