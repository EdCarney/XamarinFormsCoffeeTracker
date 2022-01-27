namespace TestApp.Validation
{
    public class IsWholeIntegerValueRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value is string strValue && double.TryParse(strValue, out double doubleVal))
            {
                double tol = 0.01;
                int wholeVal = (int) doubleVal;
                return doubleVal - wholeVal < 0.001;
            }

            return false;
        }
    }
}