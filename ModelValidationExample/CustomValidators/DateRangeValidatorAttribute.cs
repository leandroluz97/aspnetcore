using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ModelValidationExample.CustomValidators
{
    public class DateRangeValidatorAttribute : ValidationAttribute
    {
        public string OtherPropertyName { get; set; }

        public DateRangeValidatorAttribute(string otherPropertyName)
        {
            OtherPropertyName = otherPropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null) return null;

            //get to_date
            DateTime? to_date = Convert.ToDateTime(value);

            //get from_date
            PropertyInfo? otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);
            DateTime? from_date =  Convert.ToDateTime(otherProperty.GetValue(validationContext.ObjectInstance, null));

            if(otherProperty != null)
            {
                if (from_date > to_date)
                {
                    return new ValidationResult(ErrorMessage, new string[] { OtherPropertyName });
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
                
            return null;  
        }
    }
}
