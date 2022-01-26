using System;
namespace TestApp.Validation
{
	public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
	{
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            switch (value)
            {
                case string strValue:
                    return !string.IsNullOrWhiteSpace(strValue);
                case T tValue:
                    return true;
                default:
                    return false;
            }
        }
    }
}

