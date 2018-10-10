using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Common.Attributes
{
    public class CurrentDateAttribute : PasswordValidationAttribute
    {
        public CurrentDateAttribute()
        {
        }
        public override bool IsValid(object value)
        {
            var date = (DateTime)value;
            if (date <= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}
