using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace emptime.Models
{
    public class RegistrationTable
    {
        [Required]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Only letters allowed")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Only letters allowed")]
        public string LastName { get; set; }
        [Required]
        [StringLength(5, ErrorMessage = "EmpID cannot be more than 5 characters.")]
        public string EmpID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Password too short.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}