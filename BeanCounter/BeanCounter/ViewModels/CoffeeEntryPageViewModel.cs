using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BeanCounter.Annotations;
using BeanCounter.Models;
using BeanCounter.Validation;

namespace BeanCounter.ViewModels
{
    public class CoffeeEntryPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int ID { get; set; }
        public ValidatableObject<string> Company { get; } = new ValidatableObject<string>();
        public ValidatableObject<string> Name { get; } = new ValidatableObject<string>();
        public ValidatableObject<string> RoastStyle { get; } = new ValidatableObject<string>();
        public ValidatableObject<DateTime?> RoastDate { get; } = new ValidatableObject<DateTime?>();
        public string Notes { get; set; }
        
        public CoffeeEntryPageViewModel()
        {
            AddValidators(this);
        }

        public static CoffeeEntryPageViewModel Create(Coffee coffee)
        {
            var instance = new CoffeeEntryPageViewModel
            {
                ID = coffee.ID,
                Notes = coffee.Notes
            };

            instance.Company.Value = coffee.Company;
            instance.Name.Value = coffee.Name;
            instance.RoastStyle.Value = coffee.RoastStyle;
            instance.RoastDate.Value = coffee.RoastDate;

            return instance;
        }
       
        /// <summary>
        /// Generates instance of a <c>Coffee</c> from view model
        /// </summary>
        /// <param name="instance"></param>
        private static void AddValidators(CoffeeEntryPageViewModel instance)
        {
            instance.Company.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Company is required"});
            
            instance.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Name is required"});
            
            instance.RoastStyle.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Roast Style is required"} );
            
            instance.RoastDate.Validations.Add(new IsDateInPastRule<DateTime?> { ValidationMessage = "Roast Date must be in the past"});
        }

        public Coffee GenerateCoffee()
        {
            return new Coffee
            {
                ID = ID,
                Company = Company.Value,
                Name = Name.Value,
                RoastStyle = RoastStyle.Value,
                RoastDate = RoastDate.Value,
                Notes = Notes
            };
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool FieldsAreValid()
        {
            bool isCompanyValid = Company.Validate();
            bool isNameValid = Name.Validate();
            bool isRoastStyleValid = RoastStyle.Validate();
            bool isRoastDateValid = RoastDate.Validate();

            return isCompanyValid && isNameValid &&
                   isRoastStyleValid && isRoastDateValid;
        }
    }
}