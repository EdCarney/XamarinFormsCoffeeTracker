using System;

namespace TestApp.Validation
{
    public class IsDateInPastRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value is DateTime dtValue)
            {
                return dtValue < DateTime.Now;
            }

            return false;
        }
    }
}