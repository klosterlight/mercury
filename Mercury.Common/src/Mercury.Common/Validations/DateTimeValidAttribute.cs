using System;
using System.ComponentModel.DataAnnotations;

namespace Mercury.Common
{
    public class DateTimeValidAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} must be greater than {1}.";
        private DateTimeOffset _minDate;
        public DateTimeValidAttribute(string minDate = "")
        {
            if(string.IsNullOrEmpty(minDate))
            {
                _minDate = DateTimeOffset.Now;
            }
            else
            {
                _minDate = DateTimeOffset.Parse(minDate);
            }
        }

        public override bool IsValid(object value)
        {
            var currentDateTime = (DateTimeOffset)value;
            return currentDateTime > _minDate;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(DefaultErrorMessage, name, _minDate.ToString("yyyy-MM-dd"));
        }
    }
}