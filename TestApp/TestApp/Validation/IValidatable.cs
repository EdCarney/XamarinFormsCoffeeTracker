using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TestApp.Validation
{
    public interface IValidatable<T> : IValidatable
    {
        List<IValidationRule<T>> Validations { get; set; }
    }

    public interface IValidatable : INotifyPropertyChanged
    {
        List<string> Errors { get; set; }
        bool IsValid { get; set; }
        bool Validate();
    }
}

