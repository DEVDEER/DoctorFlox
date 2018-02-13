namespace devdeer.DoctorFlox.Logic.WpfSample.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Resources;

    using ViewModels;

    /// <summary>
    /// A wrapper for the data used in the <see cref="ChildViewModel" />.
    /// </summary>
    public class ChildDataModel : BaseDataModel
    {
        #region properties

        /// <summary>
        /// The age in years.
        /// </summary>
        [Range(18, 100, ErrorMessage = "This age is invalid!")]
        public int Age { get; set; }

        /// <summary>
        /// The first name.
        /// </summary>
        [Required(ErrorMessage = "The first name property is required.")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Default), ErrorMessageResourceName = "FirstNameTooLongError")]
        public string Firstname { get; set; }

        /// <summary>
        /// The associated group.
        /// </summary>
        public GroupDataModel Group { get; set; }

        /// <summary>
        /// The last name.
        /// </summary>
        [Required(ErrorMessage = "This last name is required.")]
        [MaxLength(20, ErrorMessage = "20 characters are allowed in max.")]
        public string Lastname { get; set; }

        /// <inheritdoc />
        protected override bool CollapseInnerDataErrors => false;

        #endregion
    }
}