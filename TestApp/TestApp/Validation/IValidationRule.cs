using System;
namespace TestApp.Validation
{
	public interface IValidationRule<T>
	{
        string ValidationMessage { get; set; }
        bool Check(T value);
    }
}

