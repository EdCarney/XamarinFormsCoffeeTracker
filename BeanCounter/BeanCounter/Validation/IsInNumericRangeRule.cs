namespace BeanCounter.Validation
{
    public class IsInNumericRangeRule<T> : IValidationRule<T>
    {
        private double MinValue { get; }
        private double MaxValue { get; }
        public string ValidationMessage { get; set; }

        public IsInNumericRangeRule(double minValue, double maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public bool Check(T value)
        {
            if (value is string str && double.TryParse(str, out double num))
                return num <= MaxValue && num >= MinValue;
            return false;
        }
    }
}