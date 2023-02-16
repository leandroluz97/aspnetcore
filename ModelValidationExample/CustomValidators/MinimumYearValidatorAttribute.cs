using System.ComponentModel.DataAnnotations;

namespace ModelValidationExample.CustomValidators
{
    public class MinimumYearValidatorAttribute : ValidationAttribute
    {
        public int MinimumYear{ get; set; }
        public string DefaultErrorMessage { get; set; } = "Year should not be less than {0}";

        public MinimumYearValidatorAttribute()
        {

        }

        public MinimumYearValidatorAttribute(int minimumYear)
        {
            MinimumYear = minimumYear;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;

            DateTime date = (DateTime)value;
            if (date.Year <= MinimumYear)
            {
                return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, MinimumYear));
            }
            return ValidationResult.Success;
        }
    }
}
