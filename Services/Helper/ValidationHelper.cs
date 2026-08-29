using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Helper
{
    public class ValidationHelper
    {
        public static void ValidatorClass(object? obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            ValidationContext context = new ValidationContext(obj);

            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, context, validationResults, true);

            if (!isValid)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (ValidationResult validationResult in validationResults)
                {
                    stringBuilder.AppendLine(validationResult.ErrorMessage);
                }
                //return all errors
                throw new ArgumentException(stringBuilder.ToString());
            }

        }
    }
}
