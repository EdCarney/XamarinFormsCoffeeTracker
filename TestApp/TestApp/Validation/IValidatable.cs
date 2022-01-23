using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TestApp.Validation
{
    public interface IValidatable<T> : INotifyPropertyChanged
    {
        List<IValidationRule<T>> Validations { get; set; }
        List<string> Errors { get; set; }
        bool IsValid { get; set; }

        bool Validate();
    }
}

