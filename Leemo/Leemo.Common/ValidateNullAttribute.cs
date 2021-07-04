using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TPSS.Common
{
    public class  ValidateNullAttribute : ValidationAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            else if ((Guid)value == Guid.Empty)
            {
                return new ValidationResult("Error");
            }
            return ValidationResult.Success;
        }
    }
}
