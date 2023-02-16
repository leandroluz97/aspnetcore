using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationExample.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelValidationExample.Models
{
    public class Person : IValidatableObject
    {
        [Required(ErrorMessage = "{0} can't be empty or null")]
        [Display(Name = "Person Name")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} character long")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "{0} should contain only alphabets, space and dot")]
        public string? PersonName { get; set; }


        [Required(ErrorMessage = "{0} can't be empty or null")]
        [EmailAddress(ErrorMessage = "{0} should be a proper email address")]
        public string? Email { get; set; }


        [Phone(ErrorMessage = "{0} should contain 10 digits")]
        public string? Phone { get; set; }


        public string? Password { get; set; }


        [Required(ErrorMessage = "{0} can't be blank")]
        [Compare("Password", ErrorMessage = "{0} does not match {1}")]
        [Display(Name = "Re-enter Password")]
        public string? ConfirmPassword { get; set; }


        [Range(0, 999.999, ErrorMessage = "{0} should be between ${2} and ${1}")]
        public double? Price { get; set; }


        [MinimumYearValidator(2000, ErrorMessage = "Minimun year allowed is {0}")]
        [BindNever]
        public DateTime? DateOfBirth { get; set; }


        public DateTime? FromDate { get; set; }

        [DateRangeValidator("FromDate", ErrorMessage = "'From Date' should be older than or equal to 'To date'")]
        public DateTime? ToDate { get; set; }


        public int? Age { get; set; }

        public List<string?> Tags { get; set; } = new List<string?>();

        public override string ToString()
        {
            return $"Person object - Person name:{PersonName},Email: {Email}, Phone: {Phone}, Password:{Password}, Confirm Password:{ConfirmPassword}, Price:{Price}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!DateOfBirth.HasValue && !Age.HasValue)
            {
                yield return new ValidationResult($"Either Date of Birth or Age must be supplied", new[] {nameof(Age)});
            }
        }
    }
}
