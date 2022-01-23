﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TestApp.Validation
{
    public class ValidatableObject<T> : IValidatable<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IValidationRule<T>> Validations { get; set; }

        public List<string> Errors { get; set; }

        public bool IsValid { get; set; } = true;

        public bool CleanOnChange { get; set; } = true;

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                if (CleanOnChange)
                {
                    IsValid = true;
                }
            }
        }

        public bool Validate()
        {
            Errors.Clear();

            var errors = from v in Validations
                         where !v.Check(_value)
                         select v.ValidationMessage;

            Errors = errors.ToList();
            IsValid = !errors.Any();
            return IsValid;
        }
    }
}
