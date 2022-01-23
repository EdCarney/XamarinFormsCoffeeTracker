using System;
namespace TestApp.Validation
{
	public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
	{
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            string strValue = value as string;
            return !string.IsNullOrWhiteSpace(strValue);
        }
    }
}

