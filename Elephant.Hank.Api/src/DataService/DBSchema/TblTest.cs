﻿// ---------------------------------------------------------------------------------------------------
// <copyright file="TblTest.cs" company="Elephant Insurance Services, LLC">
//     Copyright (c) 2015 All Right Reserved
// </copyright>
// <author>Vyom Sharma</author>
// <date>2015-04-17</date>
// <summary>
//     The TblTest class
// </summary>
// ---------------------------------------------------------------------------------------------------

namespace Elephant.Hank.DataService.DBSchema
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Elephant.Hank.DataService.DBSchema.CustomIdentity;

    /// <summary>
    /// The TblTest class
    /// </summary>
    public class TblTest : BaseTable
    {
        /// <summary>
        /// Gets or sets the name of the test.
        /// </summary>
        [Required]
        public string TestName { get; set; }

        /// <summary>
        /// Gets or sets the website identifier.
        /// </summary>
        [Required]
        public long WebsiteId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        [ForeignKey("WebsiteId")]
        public virtual TblWebsite Website { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// Get or sets the TestCaseAccessStatus.
        /// </summary>
        public long TestCaseAccessStatus { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        [ForeignKey("CategoryId")]
        public TblTestCategories Category { get; set; }

        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        [ForeignKey("CreatedBy")]
        public virtual CustomUser CreatedByUser { get; set; }
    }
}
