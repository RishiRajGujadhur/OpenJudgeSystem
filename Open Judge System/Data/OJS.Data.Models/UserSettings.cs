﻿namespace OJS.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using OJS.Common;

    [ComplexType]
    public class UserSettings
    {
        public UserSettings()
        {
            this.DateOfBirth = null;
        }

        [Column("FirstName")]
        [MinLength(GlobalConstants.NameMinLength)]
        [MaxLength(GlobalConstants.NameMaxLength)]
        public string FirstName { get; set; }

        [Column("LastName")]
        [MinLength(GlobalConstants.NameMinLength)]
        [MaxLength(GlobalConstants.NameMaxLength)]
        public string LastName { get; set; }

        [Column("City")]
        [MinLength(GlobalConstants.CityMinLength)]
        [MaxLength(GlobalConstants.CityMaxLength)]
        [RegularExpression(GlobalConstants.CityRegEx)]
        public string City { get; set; }

        [Column("EducationalInstitution")]
        public string EducationalInstitution { get; set; }

        [Column("FacultyNumber")]
        [MaxLength(30)]
        public string FacultyNumber { get; set; }

        [Column("DateOfBirth")]
        [DataType(DataType.Date)]
        //// TODO: [Column(TypeName = "Date")] temporally disabled because of SQL Compact database not having "date" type
        public DateTime? DateOfBirth { get; set; }

        [Column("Company")]
        [MaxLength(GlobalConstants.CompanyMaxLength)]
        [MinLength(GlobalConstants.CompanyMinLength)]
        [RegularExpression(GlobalConstants.CompanyRegEx)]
        public string Company { get; set; }

        [Column("JobTitle")]
        [MaxLength(GlobalConstants.JobTitleMaxLength)]
        [MinLength(GlobalConstants.JobTitleMinLength)]
        [RegularExpression(GlobalConstants.JobTitleRegEx)]
        public string JobTitle { get; set; }
        
        [NotMapped]
        public byte? Age
        {
            get
            {
                return Calculator.Age(this.DateOfBirth);
            }
        }
    }
}
