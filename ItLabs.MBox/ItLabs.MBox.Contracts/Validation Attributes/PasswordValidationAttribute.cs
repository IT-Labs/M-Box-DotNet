using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ItLabs.MBox.Contracts.Data_Annotations
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public PasswordValidationAttribute()
        {
            Minimum = 0;
            Maximum = int.MaxValue;
        }
        public override bool IsValid(object value)
        {
            var strValue = value.ToString();
            return (strValue.Length <= Maximum && strValue.Length >= Minimum) && 
                (strValue.Any(char.IsDigit) || strValue.Any(char.IsPunctuation) || strValue.Any(char.IsSymbol));
        }
    }
}
